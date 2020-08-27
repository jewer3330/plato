using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Skill
{
    class TriggerEffectControl : NodeControl<TriggerEffectNode>
    {

        protected Vector3 hitPoint;
        protected TimeAreaItem timeEndControl;
        protected GameObject preview;
        protected GameObject previewInstance;
        protected bool triggered;
        public TriggerEffectControl(WindowState state) : base(state)
        {
            timeStartControl.drawHead = false;
            timeEndControl = new TimeAreaItem(TimelineFuncHelper.timeCursor, state, OnDragEnd, true);
            timeEndControl.drawHead = true;
            timeEndControl.drawLine = true;
            timeEndControl.lineColor = color;
            timeEndControl.headColor = color;
        }
        public override Color color => Color.cyan * 0.5f;

        void OnDragEnd(double newTime)
        {
            var endTime = Math.Max(0.0, newTime);
            Node.lifeTime = (float)(endTime - Node.time);
        }

        public override void OnLeftGUI()
        {
            GUILayout.BeginArea(leftSize);

            node.active =GUILayout.Toggle(node.active, "开启");
            EditorGUILayout.BeginHorizontal();

            GUILayout.Label(Node.Name);
            Node.effect = (GameObject)EditorGUILayout.ObjectField(Node.effect, typeof(GameObject), false);
            EditorGUILayout.EndHorizontal();
            GUILayout.EndArea();
        }


        public override void OnRightGUI()
        {
            DrawTimeRect(0, state.maxTime, color);
            timeEndControl.Draw(rightSize, state, Node.time + Node.lifeTime);

        }

        public override void OnEventTimeChange(double time)
        {
            if (time > Node.time && time < (Node.lifeTime + Node.time) && triggered)
            {
                if (Node.effect)
                {
                    //切换选择
                    if (preview != Node.effect)
                    {
                        preview = Node.effect;
                        if (previewInstance)
                            GameObject.DestroyImmediate(previewInstance);
                        previewInstance = GameObject.Instantiate(Node.effect);
                    }

                    //超出被删除了
                    if (!previewInstance)
                    {
                        previewInstance = GameObject.Instantiate(Node.effect);
                    }

                    var timeCurrent = (float)time - Node.time;
                    previewInstance.transform.position = hitPoint;

                    var particleSystem = previewInstance.GetComponentsInChildren<ParticleSystem>().ToList();
                    particleSystem?.ForEach(r => r.Simulate(timeCurrent));
                }
            }
            else
            {
                if (previewInstance)
                {
                    GameObject.DestroyImmediate(previewInstance);
                }
            }
        }



        public override void Dispose()
        {
            if (previewInstance)
            {
                GameObject.DestroyImmediate(previewInstance);
            }
        }
        public override bool HandleManipulatorsEvents(WindowState state)
        {
            if (!timeEndControl.HandleManipulatorsEvents(state))
            {
                return base.HandleManipulatorsEvents(state);
            }
            return false;
        }

        public override void OnTrigger(HitInfo info)
        {
            hitPoint = info.point;
            Node.time = (float)state.time;
            triggered = true;
        }
    }
}