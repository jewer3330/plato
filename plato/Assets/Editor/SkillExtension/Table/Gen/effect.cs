/* !!auto gen do not change
 
 */

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Table
{
    public partial class effect
    {
	    
        public int id;
        public string effectname;
        public int effecttype;
        public string filename;
        public bool isloop;
        public float scale;

        static int memberCount = 6 ; 
        public effect()
        {
        }

        public effect( int id, string effectname, int effecttype, string filename, bool isloop, float scale)
        {
            this.id = id;
            this.effectname = effectname;
            this.effecttype = effecttype;
            this.filename = filename;
            this.isloop = isloop;
            this.scale = scale;

        }
        public static Dictionary<int, effect> _datas = new Dictionary<int, effect>();
		public static bool loaded = false;
		public static  Dictionary<int, effect> datas
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
                
                effect dataeffect = new effect();
                dataeffect.id = br.ReadInt32();
                dataeffect.effectname = br.ReadString();
                dataeffect.effecttype = br.ReadInt32();
                dataeffect.filename = br.ReadString();
                dataeffect.isloop = br.ReadBoolean();
                dataeffect.scale = br.ReadSingle();
                if (_datas.ContainsKey(dataeffect.id))
                {
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误,主键重复:" + dataeffect.id);
                }
                _datas.Add(dataeffect.id,dataeffect);
                
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
                    Debug.LogError("effect严重错误，表头和表数据长度不一样");
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("effect严重错误，表头和表数据长度不一样");
                }
                effect dataeffect = new effect();
                if(!int.TryParse(values[0],out dataeffect.id))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[0] + " to int");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[0] + " to int" + " 第"+ i + "行,第0列");
                 
                }
                dataeffect.effectname = values[1];
                if(!int.TryParse(values[2],out dataeffect.effecttype))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[2] + " to int");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[2] + " to int" + " 第"+ i + "行,第2列");
                 
                }
                dataeffect.filename = values[3];
		 int isloop = 0; 
		 if(!int.TryParse(values[4], out isloop)) 
		  { 
#if UNITY_EDITOR  
		  Debug.LogError("数据有误:" + values[4] + " to int"); 
			 UnityEditor.EditorApplication.isPaused = true; 
#endif 
			 throw new ArgumentException("数据有误:" + values[4] + " to int" + " 第"+ i + "行,第4列"); 
		 } 
		 dataeffect.isloop = isloop == 1;

                if(!float.TryParse(values[5],out dataeffect.scale))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[5] + " to float");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[5] + " to float" + " 第"+ i + "行,第5列");
                 
                }
                if (datas.ContainsKey(dataeffect.id))
                {
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误,主键重复:" + dataeffect.id);
                }
                datas.Add(dataeffect.id,dataeffect);
            }

        }

        public static void LoadFromResources()
        {           
			Clear();
			string path = "";
			TextAsset data = null;
            
				
					path = "Table/effect"; 
				
 				
           
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
                effect.LoadFromString(text);
        }

        public static void LoadBinFromResources()
        {           
			Clear();
			loaded = true;
			string path = "";
			TextAsset data = null;
            
				
					path = "TableBin/effect"; 
				 
            
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
                effect.LoadFromBinanry(text);
        }
/*
        public static void LoadFromStreaming()
        {
            try
            {
                string url = "Table/effect";
                string content = FileUtils.ReadStringFromStreaming(url);

                LoadFromString(content);
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format("表effect数据有误! ({0})",ex.Message));
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


        public static effect Get(int id)
        {
#if UNITY_EDITOR
            if (!Contains(id))
            {

                Debug.LogError("表effect没有元素" + id + ",检测一下Excel表");
                #if UNITY_EDITOR
                      UnityEditor.EditorApplication.isPaused = true;
                #endif
                return null;
            }
#endif
            return datas[id];
        }


        public static string Geteffectname(int id)
        {
            return Get(id).effectname;
        }
        public static int Geteffecttype(int id)
        {
            return Get(id).effecttype;
        }
        public static string Getfilename(int id)
        {
            return Get(id).filename;
        }
        public static bool Getisloop(int id)
        {
            return Get(id).isloop;
        }
        public static float Getscale(int id)
        {
            return Get(id).scale;
        }

    }
}