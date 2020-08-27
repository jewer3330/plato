using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skill
{
    [System.Serializable]
    public class EventNode 
    {

        public virtual string Name => "通用事件节点";
        public string luaConfig;
        public float time;
        public bool active = true;

        public virtual float timeEnd => time;
        public EventNode()
        {
        }

        public EventNode(string lua, float time)
        {
            this.luaConfig = lua;
            this.time = time;
        }


        public virtual void ToLua(System.IO.StreamWriter stream)
        {
            stream.WriteLine("{");
            stream.WriteLine("\t config =" + luaConfig);
            stream.Write(",");
            stream.WriteLine("\t time = " + time);
            stream.WriteLine("},");
        }

        
    }

  

}
