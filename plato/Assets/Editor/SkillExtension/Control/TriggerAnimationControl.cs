using UnityEditor;
using UnityEngine;
using UnityEditor.Animations;
using System.Linq;

namespace Skill
{
    class TriggerAnimationControl : NodeControl<TriggerAnimationNode>
    {

        protected Motion motion;
        protected GameObject enemy;
        protected Vector3 position;
        protected Vector3 forward;
        protected float duration;
        protected TimeAreaItem timeEndControl;
        public TriggerAnimationControl(WindowState state) : base(state)
        {
            timeStartControl.drawHead = false;

            timeEndControl = new TimeAreaItem(TimelineFuncHelper.timeCursor, state, null, true);
            timeEndControl.drawHead = false;
            timeEndControl.drawLine = true;
            timeEndControl.lineColor = color;
            timeEndControl.headColor = color;
        }


        public override Color color => Color.magenta * 0.5f;

    
        public override void OnLeftGUI()
        {
            GUILayout.BeginArea(leftSize);
            node.active = GUILayout.Toggle(node.active, "开启");
            EditorGUILayout.BeginHorizontal();

            GUILayout.Label(Node.Name);
            if (state.player)
            {
                if (Node.control == null)
                {
                    var asset = state.player.GetComponent<Animator>().runtimeAnimatorController;
                    var path = AssetDatabase.GetAssetPath(asset);
                    Node.control = (AnimatorController)AssetDatabase.LoadAssetAtPath(path, typeof(AnimatorController));
                    var parameters = Node.control.parameters.Select(k => k.name).ToList();
                    if (parameters != null)
                    {
                        var takedamage = parameters.Find(r => r == "Take Damage");
                        if (!string.IsNullOrEmpty(takedamage))
                        {
                            var index = parameters.IndexOf(takedamage);
                            Node.triggleIndex = index;
                            Node.triggleName = takedamage;
                        }
                    }
                }
            }

            var control = (AnimatorController)EditorGUILayout.ObjectField(Node.control, typeof(AnimatorController), false);

            if (Node.control != control)
            {
                Node.triggleIndex = 0;
                Node.triggleName = string.Empty;
                Node.control = control;

            }
            if (Node.control)
            {
                var parameters = Node.control.parameters.Select(k => k.name).ToArray();
                if (parameters != null && parameters.Length != 0)
                {
                    if (Node.triggleIndex >= parameters.Length || Node.triggleIndex <= 0)
                        Node.triggleIndex = 0;
                    Node.triggleIndex = EditorGUILayout.Popup(Node.triggleIndex, parameters);
                    Node.triggleName = parameters[Node.triggleIndex];
                }
                else
                {
                    Node.triggleIndex = 0;
                    Node.triggleName = string.Empty;
                }
                GetAnimationStateClipLength(Node.triggleName, Node.control);
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.EndArea();

        }

        protected void GetAnimationStateClipLength(string triggleName, AnimatorController controller)
        {
            if (!controller)
                return;
            var states = controller.layers[0].stateMachine.states.ToList();

            var find = states.Exists(r => r.state.name == Node.triggleName);
            if (find)
            {
                var r = states.Find(t => t.state.name == Node.triggleName);
                motion = r.state.motion;
                if (motion)
                    duration = r.state.motion.averageDuration;
                else
                    duration = 0;
            }
            else
            {
               
                motion = null;
                duration = 0;
            }

        }

        public override void OnRightGUI()
        {
            DrawTimeRect(0,state.maxTime, color);
            timeEndControl.Draw(rightSize, state, Node.time + duration);
        }

        public override void OnTrigger(HitInfo go)
        {
            enemy = go.transform.gameObject;
            Node.time = (float)state.time;
            position = enemy.transform.position;
            forward = enemy.transform.forward;
        }

        public override void OnEventTimeChange(double time)
        {
            if (time > Node.time && time < (Node.time + duration))
            {
                if (enemy && motion)
                {
                    if (motion is AnimationClip)//todo other motion
                    {
                        var clip = motion as AnimationClip;
                        clip.SampleAnimation(enemy, (float)(time - Node.time));
                        enemy.transform.position = position;
                        enemy.transform.forward = forward;
                    }
                }
            }
        }

      
    }
}
