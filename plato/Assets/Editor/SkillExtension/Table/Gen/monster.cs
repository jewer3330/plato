/* !!auto gen do not change
 
 */

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Table
{
    public partial class monster
    {
	    
        public int id;
        public string name;
        public int level;
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
        public float AnimationSpeed;
        public int IsBoss;
        public float hpbarheight;
        public float nameheight;
        public float ringradius;

        static int memberCount = 21 ; 
        public monster()
        {
        }

        public monster( int id, string name, int level, string model, string head, string learnedskilllist, int movespeed, string idle, string run, string death, string hit, int maxhp, int maxmp, int basedamage, int basedefense, float BodyModify, float AnimationSpeed, int IsBoss, float hpbarheight, float nameheight, float ringradius)
        {
            this.id = id;
            this.name = name;
            this.level = level;
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
            this.AnimationSpeed = AnimationSpeed;
            this.IsBoss = IsBoss;
            this.hpbarheight = hpbarheight;
            this.nameheight = nameheight;
            this.ringradius = ringradius;

        }
        public static Dictionary<int, monster> _datas = new Dictionary<int, monster>();
		public static bool loaded = false;
		public static  Dictionary<int, monster> datas
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
                
                monster datamonster = new monster();
                datamonster.id = br.ReadInt32();
                datamonster.name = br.ReadString();
                datamonster.level = br.ReadInt32();
                datamonster.model = br.ReadString();
                datamonster.head = br.ReadString();
                datamonster.learnedskilllist = br.ReadString();
                datamonster.movespeed = br.ReadInt32();
                datamonster.idle = br.ReadString();
                datamonster.run = br.ReadString();
                datamonster.death = br.ReadString();
                datamonster.hit = br.ReadString();
                datamonster.maxhp = br.ReadInt32();
                datamonster.maxmp = br.ReadInt32();
                datamonster.basedamage = br.ReadInt32();
                datamonster.basedefense = br.ReadInt32();
                datamonster.BodyModify = br.ReadSingle();
                datamonster.AnimationSpeed = br.ReadSingle();
                datamonster.IsBoss = br.ReadInt32();
                datamonster.hpbarheight = br.ReadSingle();
                datamonster.nameheight = br.ReadSingle();
                datamonster.ringradius = br.ReadSingle();
                if (_datas.ContainsKey(datamonster.id))
                {
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误,主键重复:" + datamonster.id);
                }
                _datas.Add(datamonster.id,datamonster);
                
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
                    Debug.LogError("monster严重错误，表头和表数据长度不一样");
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("monster严重错误，表头和表数据长度不一样");
                }
                monster datamonster = new monster();
                if(!int.TryParse(values[0],out datamonster.id))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[0] + " to int");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[0] + " to int" + " 第"+ i + "行,第0列");
                 
                }
                datamonster.name = values[1];
                if(!int.TryParse(values[2],out datamonster.level))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[2] + " to int");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[2] + " to int" + " 第"+ i + "行,第2列");
                 
                }
                datamonster.model = values[3];
                datamonster.head = values[4];
                datamonster.learnedskilllist = values[5];
                if(!int.TryParse(values[6],out datamonster.movespeed))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[6] + " to int");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[6] + " to int" + " 第"+ i + "行,第6列");
                 
                }
                datamonster.idle = values[7];
                datamonster.run = values[8];
                datamonster.death = values[9];
                datamonster.hit = values[10];
                if(!int.TryParse(values[11],out datamonster.maxhp))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[11] + " to int");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[11] + " to int" + " 第"+ i + "行,第11列");
                 
                }
                if(!int.TryParse(values[12],out datamonster.maxmp))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[12] + " to int");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[12] + " to int" + " 第"+ i + "行,第12列");
                 
                }
                if(!int.TryParse(values[13],out datamonster.basedamage))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[13] + " to int");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[13] + " to int" + " 第"+ i + "行,第13列");
                 
                }
                if(!int.TryParse(values[14],out datamonster.basedefense))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[14] + " to int");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[14] + " to int" + " 第"+ i + "行,第14列");
                 
                }
                if(!float.TryParse(values[15],out datamonster.BodyModify))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[15] + " to float");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[15] + " to float" + " 第"+ i + "行,第15列");
                 
                }
                if(!float.TryParse(values[16],out datamonster.AnimationSpeed))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[16] + " to float");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[16] + " to float" + " 第"+ i + "行,第16列");
                 
                }
                if(!int.TryParse(values[17],out datamonster.IsBoss))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[17] + " to int");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[17] + " to int" + " 第"+ i + "行,第17列");
                 
                }
                if(!float.TryParse(values[18],out datamonster.hpbarheight))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[18] + " to float");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[18] + " to float" + " 第"+ i + "行,第18列");
                 
                }
                if(!float.TryParse(values[19],out datamonster.nameheight))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[19] + " to float");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[19] + " to float" + " 第"+ i + "行,第19列");
                 
                }
                if(!float.TryParse(values[20],out datamonster.ringradius))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[20] + " to float");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[20] + " to float" + " 第"+ i + "行,第20列");
                 
                }
                if (datas.ContainsKey(datamonster.id))
                {
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误,主键重复:" + datamonster.id);
                }
                datas.Add(datamonster.id,datamonster);
            }

        }

        public static void LoadFromResources()
        {           
			Clear();
			string path = "";
			TextAsset data = null;
            
				
					path = "Table/monster"; 
				
 				
           
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
                monster.LoadFromString(text);
        }

        public static void LoadBinFromResources()
        {           
			Clear();
			loaded = true;
			string path = "";
			TextAsset data = null;
            
				
					path = "TableBin/monster"; 
				 
            
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
                monster.LoadFromBinanry(text);
        }
/*
        public static void LoadFromStreaming()
        {
            try
            {
                string url = "Table/monster";
                string content = FileUtils.ReadStringFromStreaming(url);

                LoadFromString(content);
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format("表monster数据有误! ({0})",ex.Message));
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


        public static monster Get(int id)
        {
#if UNITY_EDITOR
            if (!Contains(id))
            {

                Debug.LogError("表monster没有元素" + id + ",检测一下Excel表");
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
        public static float GetAnimationSpeed(int id)
        {
            return Get(id).AnimationSpeed;
        }
        public static int GetIsBoss(int id)
        {
            return Get(id).IsBoss;
        }
        public static float Gethpbarheight(int id)
        {
            return Get(id).hpbarheight;
        }
        public static float Getnameheight(int id)
        {
            return Get(id).nameheight;
        }
        public static float Getringradius(int id)
        {
            return Get(id).ringradius;
        }

    }
}