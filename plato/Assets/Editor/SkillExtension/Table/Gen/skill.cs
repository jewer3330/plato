/* !!auto gen do not change
 
 */

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Table
{
    public partial class skill
    {
	    
        public int id;
        public string skillname;
        public int level;
        public string image;
        public int targettype;
        public int damage;
        public int skillpriority;
        public int manacost;
        public float castrange;
        public int casttime;
        public int colddown;
        public int cancelCastIfTargetDied;
        public int skilltype;
        public int AreaCenterType;
        public int TargetNum;
        public int TargetRangeType;
        public int TargetRange;
        public int Angle;
        public int TargetChooseType;
        public int SelfMoveType;
        public int SelfMoveDistance;
        public int SelfMoveSpeed;
        public int bullet;
        public int bulletspeed;
        public string attackaction;
        public int InstantSkillAction;
        public int ContinuousSkillAction;
        public int AttackEffect;
        public int TargetEffect;
        public float EffectModify;

        static int memberCount = 30 ; 
        public skill()
        {
        }

        public skill( int id, string skillname, int level, string image, int targettype, int damage, int skillpriority, int manacost, float castrange, int casttime, int colddown, int cancelCastIfTargetDied, int skilltype, int AreaCenterType, int TargetNum, int TargetRangeType, int TargetRange, int Angle, int TargetChooseType, int SelfMoveType, int SelfMoveDistance, int SelfMoveSpeed, int bullet, int bulletspeed, string attackaction, int InstantSkillAction, int ContinuousSkillAction, int AttackEffect, int TargetEffect, float EffectModify)
        {
            this.id = id;
            this.skillname = skillname;
            this.level = level;
            this.image = image;
            this.targettype = targettype;
            this.damage = damage;
            this.skillpriority = skillpriority;
            this.manacost = manacost;
            this.castrange = castrange;
            this.casttime = casttime;
            this.colddown = colddown;
            this.cancelCastIfTargetDied = cancelCastIfTargetDied;
            this.skilltype = skilltype;
            this.AreaCenterType = AreaCenterType;
            this.TargetNum = TargetNum;
            this.TargetRangeType = TargetRangeType;
            this.TargetRange = TargetRange;
            this.Angle = Angle;
            this.TargetChooseType = TargetChooseType;
            this.SelfMoveType = SelfMoveType;
            this.SelfMoveDistance = SelfMoveDistance;
            this.SelfMoveSpeed = SelfMoveSpeed;
            this.bullet = bullet;
            this.bulletspeed = bulletspeed;
            this.attackaction = attackaction;
            this.InstantSkillAction = InstantSkillAction;
            this.ContinuousSkillAction = ContinuousSkillAction;
            this.AttackEffect = AttackEffect;
            this.TargetEffect = TargetEffect;
            this.EffectModify = EffectModify;

        }
        public static Dictionary<int, skill> _datas = new Dictionary<int, skill>();
		public static bool loaded = false;
		public static  Dictionary<int, skill> datas
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
                
                skill dataskill = new skill();
                dataskill.id = br.ReadInt32();
                dataskill.skillname = br.ReadString();
                dataskill.level = br.ReadInt32();
                dataskill.image = br.ReadString();
                dataskill.targettype = br.ReadInt32();
                dataskill.damage = br.ReadInt32();
                dataskill.skillpriority = br.ReadInt32();
                dataskill.manacost = br.ReadInt32();
                dataskill.castrange = br.ReadSingle();
                dataskill.casttime = br.ReadInt32();
                dataskill.colddown = br.ReadInt32();
                dataskill.cancelCastIfTargetDied = br.ReadInt32();
                dataskill.skilltype = br.ReadInt32();
                dataskill.AreaCenterType = br.ReadInt32();
                dataskill.TargetNum = br.ReadInt32();
                dataskill.TargetRangeType = br.ReadInt32();
                dataskill.TargetRange = br.ReadInt32();
                dataskill.Angle = br.ReadInt32();
                dataskill.TargetChooseType = br.ReadInt32();
                dataskill.SelfMoveType = br.ReadInt32();
                dataskill.SelfMoveDistance = br.ReadInt32();
                dataskill.SelfMoveSpeed = br.ReadInt32();
                dataskill.bullet = br.ReadInt32();
                dataskill.bulletspeed = br.ReadInt32();
                dataskill.attackaction = br.ReadString();
                dataskill.InstantSkillAction = br.ReadInt32();
                dataskill.ContinuousSkillAction = br.ReadInt32();
                dataskill.AttackEffect = br.ReadInt32();
                dataskill.TargetEffect = br.ReadInt32();
                dataskill.EffectModify = br.ReadSingle();
                if (_datas.ContainsKey(dataskill.id))
                {
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误,主键重复:" + dataskill.id);
                }
                _datas.Add(dataskill.id,dataskill);
                
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
                    Debug.LogError("skill严重错误，表头和表数据长度不一样");
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("skill严重错误，表头和表数据长度不一样");
                }
                skill dataskill = new skill();
                if(!int.TryParse(values[0],out dataskill.id))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[0] + " to int");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[0] + " to int" + " 第"+ i + "行,第0列");
                 
                }
                dataskill.skillname = values[1];
                if(!int.TryParse(values[2],out dataskill.level))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[2] + " to int");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[2] + " to int" + " 第"+ i + "行,第2列");
                 
                }
                dataskill.image = values[3];
                if(!int.TryParse(values[4],out dataskill.targettype))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[4] + " to int");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[4] + " to int" + " 第"+ i + "行,第4列");
                 
                }
                if(!int.TryParse(values[5],out dataskill.damage))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[5] + " to int");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[5] + " to int" + " 第"+ i + "行,第5列");
                 
                }
                if(!int.TryParse(values[6],out dataskill.skillpriority))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[6] + " to int");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[6] + " to int" + " 第"+ i + "行,第6列");
                 
                }
                if(!int.TryParse(values[7],out dataskill.manacost))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[7] + " to int");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[7] + " to int" + " 第"+ i + "行,第7列");
                 
                }
                if(!float.TryParse(values[8],out dataskill.castrange))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[8] + " to float");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[8] + " to float" + " 第"+ i + "行,第8列");
                 
                }
                if(!int.TryParse(values[9],out dataskill.casttime))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[9] + " to int");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[9] + " to int" + " 第"+ i + "行,第9列");
                 
                }
                if(!int.TryParse(values[10],out dataskill.colddown))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[10] + " to int");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[10] + " to int" + " 第"+ i + "行,第10列");
                 
                }
                if(!int.TryParse(values[11],out dataskill.cancelCastIfTargetDied))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[11] + " to int");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[11] + " to int" + " 第"+ i + "行,第11列");
                 
                }
                if(!int.TryParse(values[12],out dataskill.skilltype))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[12] + " to int");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[12] + " to int" + " 第"+ i + "行,第12列");
                 
                }
                if(!int.TryParse(values[13],out dataskill.AreaCenterType))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[13] + " to int");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[13] + " to int" + " 第"+ i + "行,第13列");
                 
                }
                if(!int.TryParse(values[14],out dataskill.TargetNum))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[14] + " to int");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[14] + " to int" + " 第"+ i + "行,第14列");
                 
                }
                if(!int.TryParse(values[15],out dataskill.TargetRangeType))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[15] + " to int");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[15] + " to int" + " 第"+ i + "行,第15列");
                 
                }
                if(!int.TryParse(values[16],out dataskill.TargetRange))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[16] + " to int");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[16] + " to int" + " 第"+ i + "行,第16列");
                 
                }
                if(!int.TryParse(values[17],out dataskill.Angle))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[17] + " to int");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[17] + " to int" + " 第"+ i + "行,第17列");
                 
                }
                if(!int.TryParse(values[18],out dataskill.TargetChooseType))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[18] + " to int");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[18] + " to int" + " 第"+ i + "行,第18列");
                 
                }
                if(!int.TryParse(values[19],out dataskill.SelfMoveType))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[19] + " to int");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[19] + " to int" + " 第"+ i + "行,第19列");
                 
                }
                if(!int.TryParse(values[20],out dataskill.SelfMoveDistance))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[20] + " to int");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[20] + " to int" + " 第"+ i + "行,第20列");
                 
                }
                if(!int.TryParse(values[21],out dataskill.SelfMoveSpeed))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[21] + " to int");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[21] + " to int" + " 第"+ i + "行,第21列");
                 
                }
                if(!int.TryParse(values[22],out dataskill.bullet))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[22] + " to int");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[22] + " to int" + " 第"+ i + "行,第22列");
                 
                }
                if(!int.TryParse(values[23],out dataskill.bulletspeed))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[23] + " to int");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[23] + " to int" + " 第"+ i + "行,第23列");
                 
                }
                dataskill.attackaction = values[24];
                if(!int.TryParse(values[25],out dataskill.InstantSkillAction))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[25] + " to int");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[25] + " to int" + " 第"+ i + "行,第25列");
                 
                }
                if(!int.TryParse(values[26],out dataskill.ContinuousSkillAction))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[26] + " to int");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[26] + " to int" + " 第"+ i + "行,第26列");
                 
                }
                if(!int.TryParse(values[27],out dataskill.AttackEffect))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[27] + " to int");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[27] + " to int" + " 第"+ i + "行,第27列");
                 
                }
                if(!int.TryParse(values[28],out dataskill.TargetEffect))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[28] + " to int");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[28] + " to int" + " 第"+ i + "行,第28列");
                 
                }
                if(!float.TryParse(values[29],out dataskill.EffectModify))
                {

#if UNITY_EDITOR
                    Debug.LogError("数据有误:" + values[29] + " to float");
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误:" + values[29] + " to float" + " 第"+ i + "行,第29列");
                 
                }
                if (datas.ContainsKey(dataskill.id))
                {
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                    throw new ArgumentException("数据有误,主键重复:" + dataskill.id);
                }
                datas.Add(dataskill.id,dataskill);
            }

        }

        public static void LoadFromResources()
        {           
			Clear();
			string path = "";
			TextAsset data = null;
            
				
					path = "Table/skill"; 
				
 				
           
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
                skill.LoadFromString(text);
        }

        public static void LoadBinFromResources()
        {           
			Clear();
			loaded = true;
			string path = "";
			TextAsset data = null;
            
				
					path = "TableBin/skill"; 
				 
            
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
                skill.LoadFromBinanry(text);
        }
/*
        public static void LoadFromStreaming()
        {
            try
            {
                string url = "Table/skill";
                string content = FileUtils.ReadStringFromStreaming(url);

                LoadFromString(content);
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format("表skill数据有误! ({0})",ex.Message));
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


        public static skill Get(int id)
        {
#if UNITY_EDITOR
            if (!Contains(id))
            {

                Debug.LogError("表skill没有元素" + id + ",检测一下Excel表");
                #if UNITY_EDITOR
                      UnityEditor.EditorApplication.isPaused = true;
                #endif
                return null;
            }
#endif
            return datas[id];
        }


        public static string Getskillname(int id)
        {
            return Get(id).skillname;
        }
        public static int Getlevel(int id)
        {
            return Get(id).level;
        }
        public static string Getimage(int id)
        {
            return Get(id).image;
        }
        public static int Gettargettype(int id)
        {
            return Get(id).targettype;
        }
        public static int Getdamage(int id)
        {
            return Get(id).damage;
        }
        public static int Getskillpriority(int id)
        {
            return Get(id).skillpriority;
        }
        public static int Getmanacost(int id)
        {
            return Get(id).manacost;
        }
        public static float Getcastrange(int id)
        {
            return Get(id).castrange;
        }
        public static int Getcasttime(int id)
        {
            return Get(id).casttime;
        }
        public static int Getcolddown(int id)
        {
            return Get(id).colddown;
        }
        public static int GetcancelCastIfTargetDied(int id)
        {
            return Get(id).cancelCastIfTargetDied;
        }
        public static int Getskilltype(int id)
        {
            return Get(id).skilltype;
        }
        public static int GetAreaCenterType(int id)
        {
            return Get(id).AreaCenterType;
        }
        public static int GetTargetNum(int id)
        {
            return Get(id).TargetNum;
        }
        public static int GetTargetRangeType(int id)
        {
            return Get(id).TargetRangeType;
        }
        public static int GetTargetRange(int id)
        {
            return Get(id).TargetRange;
        }
        public static int GetAngle(int id)
        {
            return Get(id).Angle;
        }
        public static int GetTargetChooseType(int id)
        {
            return Get(id).TargetChooseType;
        }
        public static int GetSelfMoveType(int id)
        {
            return Get(id).SelfMoveType;
        }
        public static int GetSelfMoveDistance(int id)
        {
            return Get(id).SelfMoveDistance;
        }
        public static int GetSelfMoveSpeed(int id)
        {
            return Get(id).SelfMoveSpeed;
        }
        public static int Getbullet(int id)
        {
            return Get(id).bullet;
        }
        public static int Getbulletspeed(int id)
        {
            return Get(id).bulletspeed;
        }
        public static string Getattackaction(int id)
        {
            return Get(id).attackaction;
        }
        public static int GetInstantSkillAction(int id)
        {
            return Get(id).InstantSkillAction;
        }
        public static int GetContinuousSkillAction(int id)
        {
            return Get(id).ContinuousSkillAction;
        }
        public static int GetAttackEffect(int id)
        {
            return Get(id).AttackEffect;
        }
        public static int GetTargetEffect(int id)
        {
            return Get(id).TargetEffect;
        }
        public static float GetEffectModify(int id)
        {
            return Get(id).EffectModify;
        }

    }
}