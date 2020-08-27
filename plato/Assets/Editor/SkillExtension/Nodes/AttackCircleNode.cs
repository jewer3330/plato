using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Skill
{
    /// <summary>
    /// 直接伤害
    /// </summary>
    [System.Serializable]
    public class AttackCircleNode : EventNode
    {
        public enum UseType
        { 
            Enemy,
            Player
        }

        public UseType type;
        public float radius = 2;
        public override string Name => "圆形攻击节点";
        public AttackCircleNode(string lua, float time) : base(lua, time)
        {
        }

        public override void ToLua(StreamWriter stream)
        {
            luaConfig = string.Format("{{ Event = 'AttackCircle', UseType = '{0}',Radius = {1}}}", type.ToString(), radius);
            base.ToLua(stream);
        }

    }
}