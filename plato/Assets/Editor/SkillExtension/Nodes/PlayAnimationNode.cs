using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Animations;
using UnityEngine;

namespace Skill
{
    [System.Serializable]
    public class PlayAnimationNode : EventNode
    {
        public AnimatorController control;
        //public int triggleIndex;
        public string triggleName;
        public float duration;
        public override float timeEnd => base.timeEnd + duration;
        public override string Name => "播放动画节点";
        public PlayAnimationNode(string lua, float time):base(lua,time)
        {
        }

        public override void ToLua(StreamWriter stream)
        {
            luaConfig = string.Format("{{ Event = 'PlayAnimation',Name = '{0}' }}", triggleName);
            base.ToLua(stream);
        }

    }
}