using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Skill
{
    [System.Serializable]
    public class FireBulletNode : EventNode
    {
        public GameObject effect;
        public float lifeTime = 2f;
        public float speed = 10;
        public Vector3 dir = Vector3.forward;
        public Vector3 offset = Vector3.zero;
        public bool alwaysAttack = false;

        public Vector3 localScale = Vector3.one;
        public Vector3 localEulerAngles;
        
        public Transform transform;
        public string transformPath;
        public int transformIndex;

        public Transform transformTarget;
        public string transformPathTarget;
        public int transformIndexTarget;

        public override string Name => "发射子弹节点";
        public override float timeEnd => base.timeEnd + lifeTime;
        public FireBulletNode(string lua, float time) : base(lua, time)
        {
        }

        public override void ToLua(StreamWriter stream)
        {
            luaConfig = string.Format("{{Event='FireBullet',Res = '{0}',LifeTime ={1},Offset = {2},Speed = {3},Dir = {4}, AlwaysAttack = {5},Path = '{6}',Scale = {7},Euler = {8},TargetPath = '{9}'}}",
                AssetDatabase.GetAssetPath(effect), lifeTime, Tolua(offset),speed,Tolua(dir),alwaysAttack ? 1 : 0,
                transformPath,
                Tolua(localScale),
                Tolua(localEulerAngles),
                transformPathTarget
                );
            base.ToLua(stream);
        }

        public string Tolua(Vector3 vector3)
        {
            return string.Format("CS.UnityEngine.Vector3({0},{1},{2})", vector3.x, vector3.y, vector3.z);
        }

    }
}