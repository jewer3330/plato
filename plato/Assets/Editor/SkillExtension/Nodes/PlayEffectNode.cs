using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Skill
{
    [System.Serializable]
    public class PlayEffectNode : EventNode
    {
        public GameObject effect;
        public float lifeTime = 1f;
        public Transform transform;
        
        public string transformPath;
        public int transformIndex;


        public Vector3 localPosition;
        public Vector3 localScale = Vector3.one;
        public Vector3 localEulerAngles;

        public bool isWorld;
        public override float timeEnd => base.timeEnd + lifeTime;

        public override string Name => "播放特效节点";
        public PlayEffectNode(string lua, float time) : base(lua, time)
        {
        }

        public override void ToLua(StreamWriter stream)
        {
            luaConfig = string.Format("{{Event='PlayEffect',Res = '{0}',LifeTime ={1},Path = '{2}',Speed = {3},Offset = {4},Scale = {5},Euler = {6},IsWorld = {7}}}",
                AssetDatabase.GetAssetPath(effect),lifeTime,transformPath,0
                ,Tolua(localPosition),Tolua(localScale),Tolua(localEulerAngles)
                ,isWorld.ToString().ToLower());
            base.ToLua(stream);
        }

        public string Tolua(Vector3 vector3)
        {
            return string.Format("CS.UnityEngine.Vector3({0},{1},{2})", vector3.x, vector3.y, vector3.z);
        }
    }
}