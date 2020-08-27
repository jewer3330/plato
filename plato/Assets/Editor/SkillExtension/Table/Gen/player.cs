/* !!auto gen do not change
 
 */

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Table
{
    public partial class player
    {
	    
        public int id;
        public string name;
        public int level;
        public int rolesex;
        public int Professiontag;
        public string model;
        public string head;
        public string learnedskilllist;
        public int movespeed;
        public string idle;
        public string run;
        public string death;
        public string hit;
        public int maxhp;
        public int maxmp;
        public int basedamage;
        public int basedefense;
        public float BodyModify;
        public float hpbarheight;
        public float nameheight;

        static int memberCount = 20 ; 
        public player()
        {
        }

        public player( int id, string name, int level, int rolesex, int Professiontag, string model, string head, string learnedskilllist, int movespeed, string idle, string run, string death, string hit, int maxhp, int maxmp, int basedamage, int basedefense, float BodyModify, float hpbarheight, float nameheight)
        {
            this.id = id;
            this.name = name;
            this.level = level;
            this.rolesex = rolesex;
            this.Professiontag = Professiontag;
            this.model = model;
            this.head = head;
            this.learnedskilllist = learnedskilllist;
            this.movespeed = movespeed;
            this.idle = idle;
            this.run = run;
            this.death = death;
            this.hit = hit;
            this.maxhp = maxhp;
            this.maxmp = maxmp;
            this.basedamage = basedamage;
            this.basedefense = basedefense;
            this.BodyModify = BodyModify;
            this.hpbarheight = hpbarheight;
            this.nameheight = nameheight;

        }
        public static Dictionary<int, player> _datas = new Dictionary<int, player>();
		public static bool loaded = false;
		public static  Dictionary<int, player> datas
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
                
                player dataplayer = new player();
                dataplayer.id = br.ReadInt32();
                dataplayer.name = br.ReadString();
                dataplayer.level = br.ReadInt32();
                dataplayer.rolesex = br.ReadInt32();
                dataplayer.Professiontag = br.ReadInt32();
                dataplayer.model = br.ReadString();
                dataplayer.head = br.ReadString();
                dataplayer.learnedskilllist = br.ReadString();
                dataplayer.movespeed = br.ReadInt32();
                dataplayer.idle = br.ReadString();
                dataplayer.run = br.ReadString();
                dataplayer.death = br.ReadString();
                dataplayer.hit = br.ReadString();
                dataplayer.maxhp = br.ReadInt32();
                dataplayer.maxmp = br.ReadInt32();
                dataplayer.basedamage = br.ReadInt32();
                dataplayer.basedefense = br.ReadInt32();
                dataplayer.BodyModify = br.ReadSingle();
                dataplayer.hpbarheight = br.ReadSingle();
                dataplayer.nameheight = br.ReadSingle();
                if (_datas.ContainsKey(dataplayer.id))
                {
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误,主键重复:" + dataplayer.id);
                }
                _datas.Add(dataplayer.id,dataplayer);
                
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
                    Debug.LogError("player严重错误，表头和表数据长度不一样");
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("player严重错误，表头和表数据长度不一样");
                }
                player dataplayer = new player();
                if(!int.TryParse(values[0],out dataplayer.id))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[0] + " to int");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[0] + " to int" + " 第"+ i + "行,第0列");
                 
                }
                dataplayer.name = values[1];
                if(!int.TryParse(values[2],out dataplayer.level))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[2] + " to int");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[2] + " to int" + " 第"+ i + "行,第2列");
                 
                }
                if(!int.TryParse(values[3],out dataplayer.rolesex))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[3] + " to int");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[3] + " to int" + " 第"+ i + "行,第3列");
                 
                }
                if(!int.TryParse(values[4],out dataplayer.Professiontag))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[4] + " to int");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[4] + " to int" + " 第"+ i + "行,第4列");
                 
                }
                dataplayer.model = values[5];
                dataplayer.head = values[6];
                dataplayer.learnedskilllist = values[7];
                if(!int.TryParse(values[8],out dataplayer.movespeed))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[8] + " to int");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[8] + " to int" + " 第"+ i + "行,第8列");
                 
                }
                dataplayer.idle = values[9];
                dataplayer.run = values[10];
                dataplayer.death = values[11];
                dataplayer.hit = values[12];
                if(!int.TryParse(values[13],out dataplayer.maxhp))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[13] + " to int");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[13] + " to int" + " 第"+ i + "行,第13列");
                 
                }
                if(!int.TryParse(values[14],out dataplayer.maxmp))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[14] + " to int");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[14] + " to int" + " 第"+ i + "行,第14列");
                 
                }
                if(!int.TryParse(values[15],out dataplayer.basedamage))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[15] + " to int");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[15] + " to int" + " 第"+ i + "行,第15列");
                 
                }
                if(!int.TryParse(values[16],out dataplayer.basedefense))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[16] + " to int");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[16] + " to int" + " 第"+ i + "行,第16列");
                 
                }
                if(!float.TryParse(values[17],out dataplayer.BodyModify))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[17] + " to float");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[17] + " to float" + " 第"+ i + "行,第17列");
                 
                }
                if(!float.TryParse(values[18],out dataplayer.hpbarheight))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[18] + " to float");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[18] + " to float" + " 第"+ i + "行,第18列");
                 
                }
                if(!float.TryParse(values[19],out dataplayer.nameheight))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[19] + " to float");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[19] + " to float" + " 第"+ i + "行,第19列");
                 
                }
                if (datas.ContainsKey(dataplayer.id))
                {
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误,主键重复:" + dataplayer.id);
                }
                datas.Add(dataplayer.id,dataplayer);
            }

        }

        public static void LoadFromResources()
        {           
			Clear();
			string path = "";
			TextAsset data = null;
            
				
					path = "Table/player"; 
				
 				
           
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
                player.LoadFromString(text);
        }

        public static void LoadBinFromResources()
        {           
			Clear();
			loaded = true;
			string path = "";
			TextAsset data = null;
            
				
					path = "TableBin/player"; 
				 
            
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
                player.LoadFromBinanry(text);
        }
/*
        public static void LoadFromStreaming()
        {
            try
            {
                string url = "Table/player";
                string content = FileUtils.ReadStringFromStreaming(url);

                LoadFromString(content);
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format("表player数据有误! ({0})",ex.Message));
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


        public static player Get(int id)
        {
#if UNITY_EDITOR
            if (!Contains(id))
            {

                Debug.LogError("表player没有元素" + id + ",检测一下Excel表");
                #if UNITY_EDITOR
                      UnityEditor.EditorApplication.isPaused = true;
                #endif
                return null;
            }
#endif
            return datas[id];
        }


        public static string Getname(int id)
        {
            return Get(id).name;
        }
        public static int Getlevel(int id)
        {
            return Get(id).level;
        }
        public static int Getrolesex(int id)
        {
            return Get(id).rolesex;
        }
        public static int GetProfessiontag(int id)
        {
            return Get(id).Professiontag;
        }
        public static string Getmodel(int id)
        {
            return Get(id).model;
        }
        public static string Gethead(int id)
        {
            return Get(id).head;
        }
        public static string Getlearnedskilllist(int id)
        {
            return Get(id).learnedskilllist;
        }
        public static int Getmovespeed(int id)
        {
            return Get(id).movespeed;
        }
        public static string Getidle(int id)
        {
            return Get(id).idle;
        }
        public static string Getrun(int id)
        {
            return Get(id).run;
        }
        public static string Getdeath(int id)
        {
            return Get(id).death;
        }
        public static string Gethit(int id)
        {
            return Get(id).hit;
        }
        public static int Getmaxhp(int id)
        {
            return Get(id).maxhp;
        }
        public static int Getmaxmp(int id)
        {
            return Get(id).maxmp;
        }
        public static int Getbasedamage(int id)
        {
            return Get(id).basedamage;
        }
        public static int Getbasedefense(int id)
        {
            return Get(id).basedefense;
        }
        public static float GetBodyModify(int id)
        {
            return Get(id).BodyModify;
        }
        public static float Gethpbarheight(int id)
        {
            return Get(id).hpbarheight;
        }
        public static float Getnameheight(int id)
        {
            return Get(id).nameheight;
        }

    }
}