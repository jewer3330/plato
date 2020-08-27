using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Animations;
using UnityEngine;

namespace Skill
{
    [System.Serializable]
    public class TriggerAnimationNode : TriggerNode
    {
        public AnimatorController control;
        public int triggleIndex;
        public string triggleName;
        public TriggerAnimationNode(string lua, float time) : base(lua, time)
        {
        }
        public override string Name => "触发动画节点";
        public override void ToLua(StreamWriter stream)
        {
            luaConfig = string.Format("{{ Event = 'TriggerAnimation',Name = '{0}',IsTrigger = true }}", triggleName);
            base.ToLua(stream);
        }

    }
}