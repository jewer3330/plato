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
    public class AttackNode : EventNode
    {
        public override string Name => "直接攻击节点";
        public AttackNode(string lua, float time) : base(lua, time)
        {
        }

        public override void ToLua(StreamWriter stream)
        {
            luaConfig = string.Format("{{ Event = 'Attack'}}");
            base.ToLua(stream);
        }

    }
}