using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Skill
{
    class AttackControl : NodeControl<AttackNode>
    {
        protected bool triggered = false;
        public AttackControl(WindowState state) : base(state)
        {

        }

        public override void OnLeftGUI()
        {
            GUILayout.BeginArea(leftSize);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(Node.Name);
            EditorGUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        public override Color color => Color.yellow;

        public override void OnEventTimeChange(double time)
        {
            if (time > Node.time)
            {
                var ret = !triggered;
                if (ret && state.player && state.enemy)
                {
                    var info = new HitInfo();
                    info.gameObject = state.enemy;
                    info.transform = state.enemy.transform;
                    info.point = state.enemy.transform.position;
                    triggered = true;
                    state.controls.ForEach(r => r.OnTrigger(info));
                    state.controls.ForEach(r => r.OnTriggerEnd());
                }
            }
            else
            {
                triggered = false;
            }
        }
    }

}