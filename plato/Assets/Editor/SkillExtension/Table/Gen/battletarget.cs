/* !!auto gen do not change
 
 */

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Table
{
    public partial class battletarget
    {
	    
        public int id;
        public int columntype;
        public string weight1;
        public string weight2;
        public string weight3;
        public string weight4;
        public string weight5;

        static int memberCount = 7 ; 
        public battletarget()
        {
        }

        public battletarget( int id, int columntype, string weight1, string weight2, string weight3, string weight4, string weight5)
        {
            this.id = id;
            this.columntype = columntype;
            this.weight1 = weight1;
            this.weight2 = weight2;
            this.weight3 = weight3;
            this.weight4 = weight4;
            this.weight5 = weight5;

        }
        public static Dictionary<int, battletarget> _datas = new Dictionary<int, battletarget>();
		public static bool loaded = false;
		public static  Dictionary<int, battletarget> datas
        {
            get
            {
				if(!loaded)
				{
				LoadBinFromResources();
				}
                return _datas;
            }
			
			set
			{
				_datas = value;
			}
        }
        
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
                
                battletarget databattletarget = new battletarget();
                databattletarget.id = br.ReadInt32();
                databattletarget.columntype = br.ReadInt32();
                databattletarget.weight1 = br.ReadString();
                databattletarget.weight2 = br.ReadString();
                databattletarget.weight3 = br.ReadString();
                databattletarget.weight4 = br.ReadString();
                databattletarget.weight5 = br.ReadString();
                if (_datas.ContainsKey(databattletarget.id))
                {
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误,主键重复:" + databattletarget.id);
                }
                _datas.Add(databattletarget.id,databattletarget);
                
            }
            br.Close();
            ms.Close();
        }
		
        public static void LoadFromString(string data)
        {

            string content = data;
            string[] lines = content.Split('\n');
       

            for (int i = 3; i < lines.Length; i++)
            {
                string line = lines[i];
                line = line.Replace("\r", "");
                if(string.IsNullOrEmpty(line)) continue;
                string[] values = line.Split('\t');
                if(values.Length != memberCount)
                {
                    Debug.LogError("battletarget严重错误，表头和表数据长度不一样");
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("battletarget严重错误，表头和表数据长度不一样");
                }
                battletarget databattletarget = new battletarget();
                if(!int.TryParse(values[0],out databattletarget.id))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[0] + " to int");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[0] + " to int" + " 第"+ i + "行,第0列");
                 
                }
                if(!int.TryParse(values[1],out databattletarget.columntype))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[1] + " to int");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[1] + " to int" + " 第"+ i + "行,第1列");
                 
                }
                databattletarget.weight1 = values[2];
                databattletarget.weight2 = values[3];
                databattletarget.weight3 = values[4];
                databattletarget.weight4 = values[5];
                databattletarget.weight5 = values[6];
                if (datas.ContainsKey(databattletarget.id))
                {
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误,主键重复:" + databattletarget.id);
                }
                datas.Add(databattletarget.id,databattletarget);
            }

        }

        public static void LoadFromResources()
        {           
			Clear();
			string path = "";
			TextAsset data = null;
            
				
					path = "Table/battletarget"; 
				
 				
           
                data = Resources.Load(path) as TextAsset;
				if(data == null)
				{
				    Debug.LogError(path + " 不存在！！！！");
					#if UNITY_EDITOR
                    	UnityEditor.EditorApplication.isPaused = true;
					#endif
					return;
				}
                string text = data.text;
				if(string.IsNullOrEmpty(text))
				{
					
				    Debug.LogError(path + " 没有内容");
					#if UNITY_EDITOR
                    	UnityEditor.EditorApplication.isPaused = true;
					#endif
					return;
				}
                battletarget.LoadFromString(text);
        }

        public static void LoadBinFromResources()
        {           
			Clear();
			loaded = true;
			string path = "";
			TextAsset data = null;
            
				
					path = "TableBin/battletarget"; 
				 
            
                data = Resources.Load(path) as TextAsset;
				if(data == null)
				{
				    Debug.LogError(path + " 不存在！！！！");
					#if UNITY_EDITOR
                    	UnityEditor.EditorApplication.isPaused = true;
					#endif
					return;
				}
                byte [] text = data.bytes;
				if(text == null || text.Length == 0)
				{
					
				    Debug.LogError(path + " 没有内容");
					#if UNITY_EDITOR
                    	UnityEditor.EditorApplication.isPaused = true;
					#endif
					return;
				}
                battletarget.LoadFromBinanry(text);
        }
/*
        public static void LoadFromStreaming()
        {
            try
            {
                string url = "Table/battletarget";
                string content = FileUtils.ReadStringFromStreaming(url);

                LoadFromString(content);
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format("表battletarget数据有误! ({0})",ex.Message));
            }
        }
*/

		public static void UnLoad()
		{
			Clear();
		}
        public static void Clear()
        {
        	if(_datas != null && _datas.Count != 0)
            	_datas.Clear();
        }

        public static bool Contains(int id)
        {    
            return datas.ContainsKey(id);
        }


        public static battletarget Get(int id)
        {
#if UNITY_EDITOR
            if (!Contains(id))
            {

                Debug.LogError("表battletarget没有元素" + id + ",检测一下Excel表");
                #if UNITY_EDITOR
                      UnityEditor.EditorApplication.isPaused = true;
                #endif
                return null;
            }
#endif
            return datas[id];
        }


        public static int Getcolumntype(int id)
        {
            return Get(id).columntype;
        }
        public static string Getweight1(int id)
        {
            return Get(id).weight1;
        }
        public static string Getweight2(int id)
        {
            return Get(id).weight2;
        }
        public static string Getweight3(int id)
        {
            return Get(id).weight3;
        }
        public static string Getweight4(int id)
        {
            return Get(id).weight4;
        }
        public static string Getweight5(int id)
        {
            return Get(id).weight5;
        }

    }
}