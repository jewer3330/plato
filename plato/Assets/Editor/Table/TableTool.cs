using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System;

using System.Threading;

namespace TableTool
{

    public class TableConfigEditorWindow : EditorWindow
    {

        [MenuItem("Tools/Table")]
        public static void ShowWindow()
        {
            TableConfigEditorWindow w = (TableConfigEditorWindow)EditorWindow.GetWindow(typeof(TableConfigEditorWindow));
            w.minSize = new Vector2(400, 300);
            w.ShowPopup();

        }
       
        public Config data;
        void OnGUI()
        {
            if (!data)
                data = Config.Load();
            data.showGroup = EditorGUILayout.BeginToggleGroup("配置", data.showGroup);
            EditorGUILayout.LabelField("插件路径", data.pluginPath);
            data.sourcePath = EditorGUILayout.TextField("csv资源存放路径", data.sourcePath);
            data.tableCSPath = EditorGUILayout.TextField("cs生成路径", data.tableCSPath);
            data.tableluaPath = EditorGUILayout.TextField("lua生成路径", data.tableluaPath);
            data.offset = EditorGUILayout.IntField("行偏移", data.offset);
            var input = EditorGUILayout.TextField("分隔符", data.SplitChar.ToString());
            EditorGUILayout.EndToggleGroup();
            if (!char.TryParse(input, out data.SplitChar))
            {
                data.SplitChar = '\t';
            }
            if (GUILayout.Button("Save"))
            {
                Config.Save(data);
            }
            if (GUILayout.Button("csv转c#"))
            {
                TableToolEditor.GlobalTable();
            }
            if (GUILayout.Button("csv转lua"))
            {
                Csv2Lua.Run(data);
            }
        }

        private void OnDisable()
        {
            Config.Save(data);
        }


    }

    public enum TableType
    {
        NONE,
        INT8,
        UINT8,
        INT16,
        UINT16,
        INT32,
        UINT32,
        INT64,
        UINT64,
        BOOL,
        FLOAT,
        DOUBLE,
        STRING
    }

    public class TableToolEditor
    {

        public static Config config;
        public static string resPath;

        //[MenuItem("Tools/Table/表一键处理")]
        public static void GlobalTable()
        {
            try
            {
                config = Config.Load();
                resPath = string.Format("{0}/{1}", Application.dataPath, config.sourcePath);

                Debug.ClearDeveloperConsole();




                GenBinaryTable(resPath);


                AutoGenTableResAndScript();

                EditorUtility.DisplayProgressBar("Table", "Table", 1f);

                EditorUtility.ClearProgressBar();
                AssetDatabase.Refresh();
                AssetDatabase.SaveAssets();

            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                EditorUtility.ClearProgressBar();

            }

        }

        public static void GenBinaryTable(string path)
        {
            var bindir = path.Replace("Table", "TableBin");
            if (Directory.Exists(bindir))
                Directory.Delete(bindir, true);
            string[] files = Directory.GetFiles(path, "*.csv");
            int fileCount = 0;
            try
            {
                foreach (var file in files)
                {
                    string filename = System.IO.Path.GetFileNameWithoutExtension(file);
                    string content = File.ReadAllText(file);
                    content = content.Replace("\r\n", "\n");
                    string[] lines = content.Split(new char[] { '\n' },StringSplitOptions.RemoveEmptyEntries);
                    string[] headers = lines[config.offset + 1].Split(config.SplitChar);
                    string[] comments = lines[config.offset + 0].Split(config.SplitChar);

                    EditorUtility.DisplayProgressBar("Table", filename, (float)fileCount++ / (float)files.Length);

                    string newfile = file.Replace("Table", "TableBin").Replace(".csv", ".bytes");

                    var dir = Path.GetDirectoryName(newfile);
                    if(!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);

                    FileStream fs = new FileStream(newfile, FileMode.Create);
                    BinaryWriter bw = new BinaryWriter(fs);
                    int length = headers.Length;
                    bw.Write(length);

                    foreach (var s in headers)
                    {
                        byte data = (byte)GetType(s);
                        bw.Write(data);
                    }

                    int looplength = lines.Length - 2 - config.offset;
                    bw.Write(looplength);
                    for (int i = config.offset + 2; i < lines.Length; i++)
                    {
                        if (string.IsNullOrEmpty(lines[i]))
                            continue;
                        string[] datas = lines[i].Split(config.SplitChar);
                        for (int j = 0; j < datas.Length; j++)
                        {
                            string type = headers[j];
                            TableType t = GetType(type);

                            #region write

                            try
                            {
                                switch (t)
                                {
                                    case TableType.NONE:
                                        break;

                                    case TableType.INT8:
                                        {
                                            sbyte value = sbyte.Parse(datas[j]);
                                            bw.Write(value);
                                            break;
                                        }
                                    case TableType.UINT8:
                                        {
                                            byte value = byte.Parse(datas[j]);
                                            bw.Write(value);
                                            break;
                                        }
                                    case TableType.INT16:
                                        {
                                            short value = short.Parse(datas[j]);
                                            bw.Write(value);
                                            break;
                                        }
                                    case TableType.UINT16:
                                        {
                                            ushort value = ushort.Parse(datas[j]);
                                            bw.Write(value);
                                            break;
                                        }
                                    case TableType.INT32:
                                        {
                                            int value = int.Parse(datas[j]);
                                            bw.Write(value);
                                            break;
                                        }
                                    case TableType.UINT32:
                                        {
                                            uint value = uint.Parse(datas[j]);
                                            bw.Write(value);
                                            break;
                                        }
                                    case TableType.INT64:
                                        {
                                            long value = long.Parse(datas[j]);
                                            bw.Write(value);
                                            break;
                                        }
                                    case TableType.UINT64:
                                        {
                                            ulong value = ulong.Parse(datas[j]);
                                            bw.Write(value);
                                            break;
                                        }
                                    case TableType.BOOL:
                                        {
                                            bool value = int.Parse(datas[j]) != 0;
                                            bw.Write(value);
                                            break;
                                        }
                                    case TableType.FLOAT:
                                        {
                                            float value = float.Parse(datas[j]);
                                            bw.Write(value);
                                            break;
                                        }
                                    case TableType.DOUBLE:
                                        {
                                            double value = double.Parse(datas[j]);
                                            bw.Write(value);
                                            break;
                                        }
                                    case TableType.STRING:
                                        {
                                            string value = datas[j];
                                            bw.Write(value);
                                            break;
                                        }
                                }
                            }
                            catch (System.Exception exception)
                            {
                                Debug.LogErrorFormat("{0},{1},{2},{3}", filename, i, j, exception);
                                bw.Flush();
                                bw.Close();
                                fs.Close();
                                EditorUtility.ClearProgressBar();
                                AssetDatabase.Refresh();
                                return;
                            }

                            #endregion

                        }
                    }
                    bw.Flush();
                    bw.Close();
                    fs.Close();
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }
            finally
            {
                EditorUtility.ClearProgressBar();
                AssetDatabase.Refresh();
            }
        }


        public static TableType GetType(string header)
        {
            TableType t = TableType.NONE;
            switch (header)
            {
                case "INT8":
                case "SBYTE":
                    t = TableType.INT8;
                    break;
                case "UINT8":
                case "BYTE":
                    t = TableType.UINT8;
                    break;
                case "INT16":
                    t = TableType.INT16;
                    break;
                case "UINT16":
                    t = TableType.UINT16;
                    break;
                case "INT32":
                case "INT":
                    t = TableType.INT32;
                    break;
                case "UINT32":
                    t = TableType.UINT32;
                    break;
                case "INT64":
                    t = TableType.INT64;
                    break;
                case "UINT64":
                    t = TableType.UINT64;
                    break;
                case "BOOL":
                case "BOOLEAN":
                    t = TableType.BOOL;
                    break;
                case "FLOAT":
                    t = TableType.FLOAT;
                    break;
                case "DOUBLE":
                    t = TableType.DOUBLE;
                    break;
                case "STRING":
                    t = TableType.STRING;
                    break;
                default:
                    Debug.LogErrorFormat("can't find type {0}", header);
                    break;
            }

            return t;
        }





        public static void AutoGenTableResAndScript()
        {
            

            TableToolEditor tool = new TableToolEditor();

            string[] arrFileName = Directory.GetFiles(resPath);
            int fileCount = 0;
            foreach (string fileName in arrFileName)
            {
                EditorUtility.DisplayProgressBar("Table", fileName, (float)fileCount++ / (float)arrFileName.Length);

                tool.GenerateCSharpClass(fileName);

                tool.GenerateCSharpClass_Handle(fileName);


            }

            tool.GenerateTableLoad(arrFileName);

            

        }

        public void GenerateCSharpClass(string filePath)
        {
            if (!filePath.EndsWith(".csv")) return;
            filePath = filePath.Replace("\\", "/");
            string fileName = filePath.Substring(filePath.LastIndexOf("/") + 1);
            fileName = fileName.Substring(0, fileName.LastIndexOf('.'));

            string text = File.ReadAllText(filePath);

            string textTemplate = GetCSharpClassTemplate();

            string[] lines = text.Split('\n');

            if (string.IsNullOrEmpty(lines[lines.Length - 1]))
            {
                string[] newlines = new string[lines.Length - 1];
                for (int i = 0; i < lines.Length - 1; i++)
                {
                    newlines[i] = lines[i];
                }
                lines = newlines;
            }

            string[] members = lines[config.offset + 0].Split(config.SplitChar);
            string[] types = lines[config.offset + 1].Split(config.SplitChar);
            for (int i = 0; i < types.Length; i++)
            {
                types[i] = types[i].Replace("\r", "");
            }

            for (int i = 0; i < members.Length; i++)
            {
                members[i] = members[i].Replace("\r", "");
            }

            #region members

            string Name = fileName;
            string Struct_Name = Name;

            string templateGet = @"        public static {Struct_Name} Get({Key_Type} {Key_Name})
        {
#if UNITY_EDITOR
            if (!Contains({Key_Name}))
            {

                Debug.LogError(""表{Struct_Name}没有元素"" + {Key_Name} + "",检测一下Excel表"");
                #if UNITY_EDITOR
                      UnityEditor.EditorApplication.isPaused = true;
                #endif
                return null;
            }
#endif
            return datas[{Key_Name}];
        }
";

            string get = templateGet.Replace("{Struct_Name}", Struct_Name);
            string Key_Type = ConvertType(types[0]);
            string Key_Name = members[0];
            get = get.Replace("{Key_Type}", Key_Type);
            get = get.Replace("{Key_Name}", Key_Name);

            string templateContains = @"        public static bool Contains({Key_Type} {Key_Name})
        {    
            return datas.ContainsKey({Key_Name});
        }
";

            string contains = templateContains.Replace("{Struct_Name}", Struct_Name);

            contains = contains.Replace("{Key_Type}", Key_Type);
            contains = contains.Replace("{Key_Name}", Key_Name);

            string templateField = "        public {Type} {Name};\n";
            string templateStructParams = " {Type} {Name}";
            string templateStructAssign = "            this.{Name} = {Name};\n";
            string Struct_Field = "";
            string Struct_Params = "";
            string Struct_Assign = "";

            string templateGetField = @"        public static {Field_Type} Get{Field_Name}({Key_Type} {Key_Name})
        {
            return Get({Key_Name}).{Field_Name};
        }
";
            string Get_Field = "";
            for (int i = 0; i < members.Length; i++)
            {
                string member = members[i];
                member = member.Replace("\n", "");
                member = member.Replace("\r", "");
                string type = ConvertType(types[i]);

                string memberLower = member;
                memberLower = memberLower.Replace("\n", "");

                string field = templateField;
                field = field.Replace("{Type}", type);
                field = field.Replace("{Name}", memberLower);
                Struct_Field += field;

                string structParams = templateStructParams;
                structParams = structParams.Replace("{Type}", type);
                structParams = structParams.Replace("{Name}", memberLower);
                Struct_Params += structParams;
                if (i != members.Length - 1)
                {
                    Struct_Params += ",";
                }

                string structAssign = templateStructAssign;
                structAssign = structAssign.Replace("{Type}", type);
                structAssign = structAssign.Replace("{Name}", memberLower);
                Struct_Assign += structAssign;

                if (i != 0)
                {
                    string getField = templateGetField.Replace("{Field_Name}", memberLower);
                    getField = getField.Replace("{Field_Type}", type);
                    getField = getField.Replace("{Key_Type}", Key_Type);
                    getField = getField.Replace("{Key_Name}", Key_Name);
                    Get_Field += getField;
                }
            }

            textTemplate = textTemplate.Replace("{Name}", Name);
            textTemplate = textTemplate.Replace("{Struct_Name}", Struct_Name);
            textTemplate = textTemplate.Replace("{Struct_Field}", Struct_Field);

            textTemplate = textTemplate.Replace("{Struct_Construct_Params}", Struct_Params);
            textTemplate = textTemplate.Replace("{Struct_Construct_Assign}", Struct_Assign);

            textTemplate = textTemplate.Replace("{Key_Type}", Key_Type);

            textTemplate = textTemplate.Replace("{Get}", get);
            textTemplate = textTemplate.Replace("{Get_Field}", Get_Field);
            textTemplate = textTemplate.Replace("{Contains}", contains);
            #endregion

            #region LoadFromMemory
            //LoadFromMemory
//            string templateLoadFromMemory = @"        public static void LoadFromMemory()
//        {
//{data}
//        }
//";
            //string datas = "";
            //string templateData = "            datas.Add({Key}, new {Struct_Name}({data}));\n";
            //templateData = templateData.Replace("{Struct_Name}", Struct_Name);
            //for (int i = 2; i < lines.Length; i++)
            //{
            //    string data = templateData;
            //    string line = lines[i];
            //    string[] values = line.Split(',');
            //    Debug.Log(types.Length + "   " + values.Length);
            //    for (int j = 0; j < types.Length; j++)
            //    {
            //        values[j] = values[j].Replace("\r", "");
            //        if (types[j] == "STRING")
            //        {
            //            values[j] = '"' + values[j] + '"';
            //        }
            //        else if (true)
            //        {
            //            if (values[j] == "" || values[j] == " ")
            //                values[j] = "0";
            //        }
            //        values[j] = values[j].Replace("\r", "");
            //        values[j] = values[j].Replace("\n", "");
            //    }
            //    //Debug.Log(values[0]);
            //    data = data.Replace("{Key}", values[0]);
            //    data = data.Replace("{data}", string.Join(", ", values));
            //    datas += data; 
            //    Thread.Sleep(100);
            //}

            //templateLoadFromMemory = templateLoadFromMemory.Replace("{data}", datas);
            //textTemplate = textTemplate.Replace("{LoadFromMemory}", templateLoadFromMemory);
            #endregion

            #region LoadFromString
            string templateLoadFromString = @"        public static void LoadFromString(string data)
        {

            string content = data;
            string[] lines = content.Split('\n');
       

            for (int i = 3; i < lines.Length; i++)
            {
                string line = lines[i];
                line = line.Replace(""\r"", """");
                if(string.IsNullOrEmpty(line)) continue;
                string[] values = line.Split('\t');
                if(values.Length != memberCount)
                {
                    Debug.LogError(""{Struct_Name}严重错误，表头和表数据长度不一样"");
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException(""{Struct_Name}严重错误，表头和表数据长度不一样"");
                }
                {Struct_Name} data{Struct_Name} = new {Struct_Name}();
{Data_Field}
                if (datas.ContainsKey(data{Struct_Name}.{Key_Name}))
                {
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException(""数据有误,主键重复:"" + data{Struct_Name}.{Key_Name});
                }
                datas.Add(data{Struct_Name}.{Key_Name},data{Struct_Name});
            }

        }
";

            templateLoadFromString = templateLoadFromString.Replace("{Struct_Name}", Struct_Name);
            string dataParse = "";
            string[] dataParseList = new string[types.Length];
            for (int i = 0; i < types.Length; i++)
            {
                //Debug.Log(string.Format("data{0}.{1}", Struct_Name, members[i]));
                //Debug.Log(string.Format(string.Format("values[{0:d}]",i)));
                dataParseList[i] = Parse(types[i], string.Format("data{0}.{1}", Struct_Name, members[i]), string.Format("values[{0:d}]", i));
                //if (types[i] == "STRING")
                //{
                //    dataParseList[i] = string.Format("                data{0}.{1} = values[{2:d}];", Struct_Name, members[i], i);
                //}
                //else
                //{
                //    //dataParseList[i] = string.Format("{0}.Parse(values[{1}]);\n", ConvertType(types[i]), i);
                //    dataParseList[i] = string.Format("                {0}.TryParse(values[{1:d}],out data{2}.{3});", ConvertType(types[i]), i, Struct_Name, members[i]);
                //}

                //if (i == 0)
                //{
                //    templateLoadFromString = templateLoadFromString.Replace("{Key}", dataParseList[0]);
                //}
            }
            dataParse = string.Join("\n", dataParseList);
            templateLoadFromString = templateLoadFromString.Replace("{Struct_Name}", Struct_Name);
            templateLoadFromString = templateLoadFromString.Replace("{Key_Type}", Key_Type);
            templateLoadFromString = templateLoadFromString.Replace("{Key_Name}", Key_Name);
            templateLoadFromString = templateLoadFromString.Replace("{Data_Field}", dataParse);

            textTemplate = textTemplate.Replace("{LoadFromString}", templateLoadFromString);
            #endregion

            #region LoadBin
            string templateLoadFromBinary = @"        
        public static void LoadFromBinanry(byte[] bytes)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);
            System.IO.BinaryReader br = new System.IO.BinaryReader(ms);
            int length = br.ReadInt32();
            
            for (int i = 0; i < length; i++)
            {
                br.ReadByte();
            }

            int looplength = br.ReadInt32();
            for (int i = 0; i < looplength; i++)
            {
                
                {Struct_Name} data{Struct_Name} = new {Struct_Name}();
{Data_Field}
                if (_datas.ContainsKey(data{Struct_Name}.{Key_Name}))
                {
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException(""数据有误,主键重复:"" + data{Struct_Name}.{Key_Name});
                }
                _datas.Add(data{Struct_Name}.{Key_Name},data{Struct_Name});
                
            }
            br.Close();
            ms.Close();
        }
";
            string dataBinParse = "";
            string[] dataBinParseList = new string[types.Length];
            for (int i = 0; i < types.Length; i++)
            {
                string content = null;
                switch (types[i])
                {
                    case "STRING":
                        content = "                data{Struct_Name}.{Meberrs_I_Name} = br.ReadString();";
                        break;
                    case "UINT32":
                        content = "                data{Struct_Name}.{Meberrs_I_Name} = br.ReadUInt32();";
                        break;
                    case "INT":
                        content = "                data{Struct_Name}.{Meberrs_I_Name} = br.ReadInt32();";
                        break;
                    case "UINT":
                        content = "                data{Struct_Name}.{Meberrs_I_Name} = br.ReadUInt32();";
                        break;
                    case "INT32":
                        content = "                data{Struct_Name}.{Meberrs_I_Name} = br.ReadInt32();";
                        break;
                    case "SHORT":
                        content = "                data{Struct_Name}.{Meberrs_I_Name} = br.ReadInt16();";
                        break;
                    case "INT16":
                        content = "                data{Struct_Name}.{Meberrs_I_Name} = br.ReadInt16();";
                        break;
                    case "UINT16":
                        content = "                data{Struct_Name}.{Meberrs_I_Name} = br.ReadUInt16();";
                        break;
                    case "INT8":
                        content = "                data{Struct_Name}.{Meberrs_I_Name} = br.ReadSByte();";
                        break;
                    case "UINT8":
                        content = "                data{Struct_Name}.{Meberrs_I_Name} = br.ReadByte();";
                        break;
                    case "BOOL":
                        content = "                data{Struct_Name}.{Meberrs_I_Name} = br.ReadBoolean();";
                        break;
                    case "FLOAT":
                        content = "                data{Struct_Name}.{Meberrs_I_Name} = br.ReadSingle();";
                        break;
                    case "DOUBLE":
                        content = "                data{Struct_Name}.{Meberrs_I_Name} = br.ReadDouble();";
                        break;
                    default:
                        content = "";
                        break;
                }
                content = content.Replace("{Meberrs_I_Name}", members[i]);
                content = content.Replace("{Struct_Name}", Struct_Name);
                dataBinParseList[i] = content;

            }
            dataBinParse = string.Join("\n", dataBinParseList);
            templateLoadFromBinary = templateLoadFromBinary.Replace("{Struct_Name}", Struct_Name);
            templateLoadFromBinary = templateLoadFromBinary.Replace("{Key_Type}", Key_Type);
            templateLoadFromBinary = templateLoadFromBinary.Replace("{Key_Name}", Key_Name);
            templateLoadFromBinary = templateLoadFromBinary.Replace("{Data_Field}", dataBinParse);

            textTemplate = textTemplate.Replace("{LoadFromBinanry}", templateLoadFromBinary);


            #endregion

            string path = "Table/" + fileName;
            //string iosPath = "Table_IOS/" + fileName;
            //string iosAuditPath = "Table_IOS_Audit" + "/" + fileName;
            //string iosLocalizationPath = "Table_" + "\" + SysEnv.region + \"_IOS/" + fileName;
            //string pathLocalizationPath = "Table_" + "\" + SysEnv.region + \"/" + fileName;


            textTemplate = textTemplate.Replace("{Path}", path);
            //textTemplate = textTemplate.Replace("{Path_IOS}", iosPath);
            //textTemplate = textTemplate.Replace("{Path_IOS_Audit}", iosAuditPath);
            //textTemplate = textTemplate.Replace("{Path_IOS_Localization}", iosLocalizationPath);
            //textTemplate = textTemplate.Replace("{Path_Localization}", pathLocalizationPath);

            string pathBin = "TableBin/" + fileName;
            //string iosPathBin = "TableBin_IOS/" + fileName;
            //string iosAuditPathBin = "TableBin_IOS_Audit" + "/" + fileName;
            //string iosLocalizationPathBin = "TableBin_" + "\" + SysEnv.region + \"_IOS/" + fileName;
            //string pathLocalizationPathBin = "TableBin_" + "\" + SysEnv.region + \"/" + fileName;


            textTemplate = textTemplate.Replace("{Path_Bin}", pathBin);
            //textTemplate = textTemplate.Replace("{Path_IOS_Bin}", iosPathBin);
            //textTemplate = textTemplate.Replace("{Path_IOS_Audit_Bin}", iosAuditPathBin);
            //textTemplate = textTemplate.Replace("{Path_IOS_Localization_Bin}", iosLocalizationPathBin);
            //textTemplate = textTemplate.Replace("{Path_Localization_Bin}", pathLocalizationPathBin);


            textTemplate = textTemplate.Replace("{Member_Count}", "        static int memberCount = " + members.Length + " ; ");

            string savePath = filePath.Replace(config.sourcePath, config.tableCSPath + "/Gen");
            savePath = savePath.Replace(".csv", ".cs");
            if (!Directory.Exists(Application.dataPath + "/" + config.tableCSPath + "/Gen")) Directory.CreateDirectory(Application.dataPath + "/" + config.tableCSPath + "/Gen");
            FileStream fs = new FileStream(savePath, FileMode.Create);
            byte[] tdata = System.Text.Encoding.UTF8.GetBytes(textTemplate);
            fs.Write(tdata, 0, tdata.Length);
            fs.Flush();
            fs.Close();

            //AssetDatabase.SaveAssets();
            //AssetDatabase.Refresh();
        }

        public void GenerateCSharpClass_Handle(string filePath)
        {
            if (!filePath.EndsWith(".csv")) return;
            filePath = filePath.Replace("\\", "/");
            string fileName = filePath.Substring(filePath.LastIndexOf("/") + 1);
            fileName = fileName.Substring(0, fileName.LastIndexOf('.'));

            string textTemplate = GetCSharpClassTemplateHandle();

            string Name = fileName;

            textTemplate = textTemplate.Replace("{Name}", Name);
            string dir = filePath.Replace(config.sourcePath, config.tableCSPath + "/Handle");
            if (!Directory.Exists(Path.GetDirectoryName(dir)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(dir));
            }

            string savePath = filePath.Replace(config.sourcePath, config.tableCSPath + "/Handle");
            savePath = savePath.Replace(".csv", ".cs");
            if (!File.Exists(savePath))
            {
                StreamWriter sw = new StreamWriter(savePath, false, System.Text.Encoding.UTF8);
                sw.Write(textTemplate);
                sw.Flush();
                sw.Close();
            }

        }

        public void GenerateTableLoad(string[] arrFileName)
        {
            string textTemplate = GetCSharpTableLoadTemplate();

            string loadFromMemory = "";
            string loadFromResource = "";
            string loadFromWWW = "";
            string loadBinFromResource = "";
            string clear = "";
            string unload = "";
            string[] files = Directory.GetFiles(Application.dataPath + "/" + config.sourcePath);

            foreach (string file in files)
            {
                if (!file.EndsWith(".csv")) continue;
                string fileName = file.Replace("\\", "/");
                fileName = fileName.Substring(fileName.LastIndexOf("/") + 1);
                fileName = fileName.Substring(0, fileName.LastIndexOf('.'));

                loadFromMemory += string.Format("            {0}.LoadFromMemory();\n", fileName);
                loadFromResource += string.Format("            {0}.LoadFromResources();\n", fileName);
                loadBinFromResource += string.Format("            {0}.LoadBinFromResources();\n", fileName);
                loadFromWWW += string.Format("            {0}.LoadFromStreaming();\n", fileName);
                unload += string.Format("            {0}.UnLoad();\n", fileName);
                clear += string.Format("            {0}.Clear();\n", fileName);
            }

            textTemplate = textTemplate.Replace("{LoadBinFromResource}", loadBinFromResource);
            textTemplate = textTemplate.Replace("{LoadFromMemory}", loadFromMemory);
            textTemplate = textTemplate.Replace("{LoadFromResource}", loadFromResource);
            textTemplate = textTemplate.Replace("{LoadFromStreaming}", loadFromWWW);
            textTemplate = textTemplate.Replace("{UnLoad}", unload);
            textTemplate = textTemplate.Replace("{Clear}", clear);

            string savePath = Application.dataPath + "/" + config.tableCSPath + "/TableLoad.cs";

            FileStream fs = new FileStream(savePath, FileMode.Create);
            byte[] tdata = System.Text.Encoding.UTF8.GetBytes(textTemplate);
            fs.Write(tdata, 0, tdata.Length);
            fs.Flush();
            fs.Close();

        }

        public string GetCSharpTableLoadTemplate()
        {
            string filePath = string.Format("{0}/{1}", config.pluginPath, "TableLoad.txt");
            return File.ReadAllText(filePath);
        }

        public string GetCSharpClassTemplate()
        {
            string filePath = string.Format("{0}/{1}", config.pluginPath, "Table.txt");
            return File.ReadAllText(filePath);
        }

        public string GetCSharpClassTemplateHandle()
        {
            string filePath = string.Format("{0}/{1}", config.pluginPath, "TableHandle.txt");
            return File.ReadAllText(filePath);
        }

        public string ConvertType(string type)
        {
            string value = type;
            switch (type)
            {
                case "DOUBLE":
                    value = "double";
                    break;
                case "UINT32":
                    value = "uint";
                    break;
                case "INT":
                    value = "int";
                    break;
                case "UINT":
                    value = "uint";
                    break;
                case "INT32":
                    value = "int";
                    break;
                case "STRING":
                    value = "string";
                    break;
                case "SHORT":
                    value = "short";
                    break;
                case "INT16":
                    value = "short";
                    break;
                case "UINT16":
                    value = "ushort";
                    break;
                case "INT8":
                    value = "sbyte";
                    break;
                case "UINT8":
                    value = "byte";
                    break;
                case "BOOLEAN":
                case "BOOL":
                    value = "bool";
                    break;
                case "FLOAT":
                    value = "float";
                    break;
                
                default:
                    value = null;
                    break;
            }
            return value;
        }

        public string Parse(string type, string var, string val)
        {
            string value = null;
            switch (type)
            {
                case "STRING":
                    value = string.Format("                {0} = {1};", var, val);
                    break;
                case "DOUBLE":
                case "FLOAT":
                case "UINT32":
                case "INT":
                case "UINT":
                case "INT32":
                case "SHORT":
                case "INT16":
                case "UINT16":
                case "INT8":
                case "UINT8":
                    int start = val.IndexOf("[") + 1;
                    int end = val.IndexOf("]");
                    string lineNum = val.Substring(start, end - start);
                    value = string.Format(
    @"                if(!{0}.TryParse({1},out {2}))
                {{

#if UNITY_EDITOR
                    Debug.LogError(""数据有误:"" + {1} + "" to {0}"");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException(""数据有误:"" + {1} + "" to {0}"" + "" 第""+ i + ""行,第{3}列"");
                 
                }}"
                    , ConvertType(type), val, var, lineNum);
                    break;
                case "BOOLEAN":
                case "BOOL":
                    int s = val.IndexOf("[") + 1;
                    int e = val.IndexOf("]");
                    string num = val.Substring(s, e - s);
                    string one = val;
                    string two = var;
                    string three = var.Substring(var.IndexOf('.') + 1);
                    value =
                    "\t\t int " + three + " = 0; \r\n"
    + "\t\t if(!int.TryParse(" + one + ", out " + three + ")) \r\n"
    + "\t\t  { \r\n"
    + "#if UNITY_EDITOR  \r\n"
    + "\t\t  Debug.LogError(\"数据有误:\" + " + one + " + \" to int\"); \r\n"
    + "\t\t\t UnityEditor.EditorApplication.isPaused = true; \r\n"
    + "#endif \r\n"
    + "\t\t\t throw new ArgumentException(\"数据有误:\" + " + one + " + \" to int\" + \" 第\"+ i + \"行,第" + num + "列\"); \r\n"
    + "\t\t } \r\n"
    + "\t\t " + two + " = " + three + " == 1;\r\n";
                    //Debug.Log(value);
                    break;
                default:
                    value = string.Format("                {0} = {1};", var, val);
                    break;
            }
            return value;
        }
    }

}