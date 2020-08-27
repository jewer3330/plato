using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Skill
{
    [System.Serializable]
    public class TriggerEffectNode : TriggerNode
    {
        public GameObject effect;
        public float lifeTime = 1f;

        public override string Name => "触发特效节点";

        public TriggerEffectNode(string lua, float time) : base(lua, time)
        {
        }

        public override void ToLua(StreamWriter stream)
        {
            luaConfig = string.Format("{{Event='TriggerEffect',Res = '{0}',LifeTime ={1},IsTrigger = true}}", AssetDatabase.GetAssetPath(effect), lifeTime);
            base.ToLua(stream);
        }

    }
}