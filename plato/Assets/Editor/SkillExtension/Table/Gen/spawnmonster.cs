/* !!auto gen do not change
 
 */

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Table
{
    public partial class spawnmonster
    {
	    
        public int id;
        public int mapid;
        public int stage;
        public int arrindex;
        public int baseid;
        public float spawnx;
        public float spawny;
        public float spawnz;
        public string RelativePosition;

        static int memberCount = 9 ; 
        public spawnmonster()
        {
        }

        public spawnmonster( int id, int mapid, int stage, int arrindex, int baseid, float spawnx, float spawny, float spawnz, string RelativePosition)
        {
            this.id = id;
            this.mapid = mapid;
            this.stage = stage;
            this.arrindex = arrindex;
            this.baseid = baseid;
            this.spawnx = spawnx;
            this.spawny = spawny;
            this.spawnz = spawnz;
            this.RelativePosition = RelativePosition;

        }
        public static Dictionary<int, spawnmonster> _datas = new Dictionary<int, spawnmonster>();
		public static bool loaded = false;
		public static  Dictionary<int, spawnmonster> datas
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
                
                spawnmonster dataspawnmonster = new spawnmonster();
                dataspawnmonster.id = br.ReadInt32();
                dataspawnmonster.mapid = br.ReadInt32();
                dataspawnmonster.stage = br.ReadInt32();
                dataspawnmonster.arrindex = br.ReadInt32();
                dataspawnmonster.baseid = br.ReadInt32();
                dataspawnmonster.spawnx = br.ReadSingle();
                dataspawnmonster.spawny = br.ReadSingle();
                dataspawnmonster.spawnz = br.ReadSingle();
                dataspawnmonster.RelativePosition = br.ReadString();
                if (_datas.ContainsKey(dataspawnmonster.id))
                {
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误,主键重复:" + dataspawnmonster.id);
                }
                _datas.Add(dataspawnmonster.id,dataspawnmonster);
                
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
                    Debug.LogError("spawnmonster严重错误，表头和表数据长度不一样");
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("spawnmonster严重错误，表头和表数据长度不一样");
                }
                spawnmonster dataspawnmonster = new spawnmonster();
                if(!int.TryParse(values[0],out dataspawnmonster.id))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[0] + " to int");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[0] + " to int" + " 第"+ i + "行,第0列");
                 
                }
                if(!int.TryParse(values[1],out dataspawnmonster.mapid))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[1] + " to int");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[1] + " to int" + " 第"+ i + "行,第1列");
                 
                }
                if(!int.TryParse(values[2],out dataspawnmonster.stage))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[2] + " to int");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[2] + " to int" + " 第"+ i + "行,第2列");
                 
                }
                if(!int.TryParse(values[3],out dataspawnmonster.arrindex))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[3] + " to int");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[3] + " to int" + " 第"+ i + "行,第3列");
                 
                }
                if(!int.TryParse(values[4],out dataspawnmonster.baseid))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[4] + " to int");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[4] + " to int" + " 第"+ i + "行,第4列");
                 
                }
                if(!float.TryParse(values[5],out dataspawnmonster.spawnx))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[5] + " to float");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[5] + " to float" + " 第"+ i + "行,第5列");
                 
                }
                if(!float.TryParse(values[6],out dataspawnmonster.spawny))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[6] + " to float");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[6] + " to float" + " 第"+ i + "行,第6列");
                 
                }
                if(!float.TryParse(values[7],out dataspawnmonster.spawnz))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[7] + " to float");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[7] + " to float" + " 第"+ i + "行,第7列");
                 
                }
                dataspawnmonster.RelativePosition = values[8];
                if (datas.ContainsKey(dataspawnmonster.id))
                {
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误,主键重复:" + dataspawnmonster.id);
                }
                datas.Add(dataspawnmonster.id,dataspawnmonster);
            }

        }

        public static void LoadFromResources()
        {           
			Clear();
			string path = "";
			TextAsset data = null;
            
				
					path = "Table/spawnmonster"; 
				
 				
           
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
                spawnmonster.LoadFromString(text);
        }

        public static void LoadBinFromResources()
        {           
			Clear();
			loaded = true;
			string path = "";
			TextAsset data = null;
            
				
					path = "TableBin/spawnmonster"; 
				 
            
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
                spawnmonster.LoadFromBinanry(text);
        }
/*
        public static void LoadFromStreaming()
        {
            try
            {
                string url = "Table/spawnmonster";
                string content = FileUtils.ReadStringFromStreaming(url);

                LoadFromString(content);
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format("表spawnmonster数据有误! ({0})",ex.Message));
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


        public static spawnmonster Get(int id)
        {
#if UNITY_EDITOR
            if (!Contains(id))
            {

                Debug.LogError("表spawnmonster没有元素" + id + ",检测一下Excel表");
                #if UNITY_EDITOR
                      UnityEditor.EditorApplication.isPaused = true;
                #endif
                return null;
            }
#endif
            return datas[id];
        }


        public static int Getmapid(int id)
        {
            return Get(id).mapid;
        }
        public static int Getstage(int id)
        {
            return Get(id).stage;
        }
        public static int Getarrindex(int id)
        {
            return Get(id).arrindex;
        }
        public static int Getbaseid(int id)
        {
            return Get(id).baseid;
        }
        public static float Getspawnx(int id)
        {
            return Get(id).spawnx;
        }
        public static float Getspawny(int id)
        {
            return Get(id).spawny;
        }
        public static float Getspawnz(int id)
        {
            return Get(id).spawnz;
        }
        public static string GetRelativePosition(int id)
        {
            return Get(id).RelativePosition;
        }

    }
}