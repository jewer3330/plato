using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Skill
{
    class AttackCircleControl : NodeControl<AttackCircleNode>
    {
        protected bool triggered = false;
        public AttackCircleControl(WindowState state) : base(state)
        {

        }

        public override void OnLeftGUI()
        {
            GUILayout.BeginArea(leftSize);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(Node.Name);
            Node.type = (AttackCircleNode.UseType)EditorGUILayout.EnumPopup(Node.type);
            Node.radius = EditorGUILayout.FloatField("Radius", Node.radius);
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
                    var go = Node.type == AttackCircleNode.UseType.Enemy ? state.enemy : state.player;
                    info.gameObject = go;
                    info.transform = go.transform;
                    info.point = go.transform.position;
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