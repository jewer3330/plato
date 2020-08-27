using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skill
{
    public class TriggerNode : EventNode
    {
        public override string Name => "普通触发节点";
        public TriggerNode()
        {
        }

        public TriggerNode(string lua, float time)
        {
            this.luaConfig = lua;
            this.time = time;
        }
    }

}