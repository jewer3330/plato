/* !!auto gen do not change
 
 */

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Table
{
    public partial class spawnplayer
    {
	    
        public int id;
        public int mapid;
        public int stage;
        public string idlist;
        public int numberofplayer;
        public int condition;
        public string spawncenter;
        public string offsetlist;
        public string RelativePositionList;

        static int memberCount = 9 ; 
        public spawnplayer()
        {
        }

        public spawnplayer( int id, int mapid, int stage, string idlist, int numberofplayer, int condition, string spawncenter, string offsetlist, string RelativePositionList)
        {
            this.id = id;
            this.mapid = mapid;
            this.stage = stage;
            this.idlist = idlist;
            this.numberofplayer = numberofplayer;
            this.condition = condition;
            this.spawncenter = spawncenter;
            this.offsetlist = offsetlist;
            this.RelativePositionList = RelativePositionList;

        }
        public static Dictionary<int, spawnplayer> _datas = new Dictionary<int, spawnplayer>();
		public static bool loaded = false;
		public static  Dictionary<int, spawnplayer> datas
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
                
                spawnplayer dataspawnplayer = new spawnplayer();
                dataspawnplayer.id = br.ReadInt32();
                dataspawnplayer.mapid = br.ReadInt32();
                dataspawnplayer.stage = br.ReadInt32();
                dataspawnplayer.idlist = br.ReadString();
                dataspawnplayer.numberofplayer = br.ReadInt32();
                dataspawnplayer.condition = br.ReadInt32();
                dataspawnplayer.spawncenter = br.ReadString();
                dataspawnplayer.offsetlist = br.ReadString();
                dataspawnplayer.RelativePositionList = br.ReadString();
                if (_datas.ContainsKey(dataspawnplayer.id))
                {
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误,主键重复:" + dataspawnplayer.id);
                }
                _datas.Add(dataspawnplayer.id,dataspawnplayer);
                
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
                    Debug.LogError("spawnplayer严重错误，表头和表数据长度不一样");
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("spawnplayer严重错误，表头和表数据长度不一样");
                }
                spawnplayer dataspawnplayer = new spawnplayer();
                if(!int.TryParse(values[0],out dataspawnplayer.id))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[0] + " to int");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[0] + " to int" + " 第"+ i + "行,第0列");
                 
                }
                if(!int.TryParse(values[1],out dataspawnplayer.mapid))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[1] + " to int");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[1] + " to int" + " 第"+ i + "行,第1列");
                 
                }
                if(!int.TryParse(values[2],out dataspawnplayer.stage))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[2] + " to int");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[2] + " to int" + " 第"+ i + "行,第2列");
                 
                }
                dataspawnplayer.idlist = values[3];
                if(!int.TryParse(values[4],out dataspawnplayer.numberofplayer))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[4] + " to int");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[4] + " to int" + " 第"+ i + "行,第4列");
                 
                }
                if(!int.TryParse(values[5],out dataspawnplayer.condition))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[5] + " to int");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[5] + " to int" + " 第"+ i + "行,第5列");
                 
                }
                dataspawnplayer.spawncenter = values[6];
                dataspawnplayer.offsetlist = values[7];
                dataspawnplayer.RelativePositionList = values[8];
                if (datas.ContainsKey(dataspawnplayer.id))
                {
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误,主键重复:" + dataspawnplayer.id);
                }
                datas.Add(dataspawnplayer.id,dataspawnplayer);
            }

        }

        public static void LoadFromResources()
        {           
			Clear();
			string path = "";
			TextAsset data = null;
            
				
					path = "Table/spawnplayer"; 
				
 				
           
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
                spawnplayer.LoadFromString(text);
        }

        public static void LoadBinFromResources()
        {           
			Clear();
			loaded = true;
			string path = "";
			TextAsset data = null;
            
				
					path = "TableBin/spawnplayer"; 
				 
            
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
                spawnplayer.LoadFromBinanry(text);
        }
/*
        public static void LoadFromStreaming()
        {
            try
            {
                string url = "Table/spawnplayer";
                string content = FileUtils.ReadStringFromStreaming(url);

                LoadFromString(content);
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format("表spawnplayer数据有误! ({0})",ex.Message));
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


        public static spawnplayer Get(int id)
        {
#if UNITY_EDITOR
            if (!Contains(id))
            {

                Debug.LogError("表spawnplayer没有元素" + id + ",检测一下Excel表");
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
        public static string Getidlist(int id)
        {
            return Get(id).idlist;
        }
        public static int Getnumberofplayer(int id)
        {
            return Get(id).numberofplayer;
        }
        public static int Getcondition(int id)
        {
            return Get(id).condition;
        }
        public static string Getspawncenter(int id)
        {
            return Get(id).spawncenter;
        }
        public static string Getoffsetlist(int id)
        {
            return Get(id).offsetlist;
        }
        public static string GetRelativePositionList(int id)
        {
            return Get(id).RelativePositionList;
        }

    }
}