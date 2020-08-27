using UnityEditor;
using UnityEngine;
using UnityEditor.Animations;
using System.Linq;

namespace Skill
{
    class PlayAnimationControl : NodeControl<PlayAnimationNode>
    {

        protected TimeAreaItem endTime;
       
        public AnimationClip motion;


        public PlayAnimationControl(WindowState state) : base(state)
        {
            endTime = new TimeAreaItem(TimelineFuncHelper.timeCursor, state, null, true);
            endTime.drawHead = false;
            endTime.drawLine = true;
            endTime.lineColor = color;
            endTime.headColor = color;

        }

        public override Color color => Color.green * 0.5f;


    
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
                }
            }

            var control = (AnimatorController)EditorGUILayout.ObjectField(Node.control, typeof(AnimatorController), false);

            if (Node.control != control)
            {
                //Node.triggleIndex = 0;
                Node.triggleName = string.Empty;
                Node.control = control;

            }
            if (Node.control)
            {
                var parameters = Node.control.parameters.Select(k => k.name).ToList();
                if (parameters != null && parameters.Count != 0)
                {
                    var ret = parameters.Find(r => r == Table.skill.Get(state.skillID).attackaction);
                    if (!string.IsNullOrEmpty(ret))
                    {
                        //Node.triggleIndex = EditorGUILayout.Popup(Node.triggleIndex, parameters);
                        EditorGUILayout.LabelField(ret);
                        //Node.triggleName = parameters[Node.triggleIndex];
                        
                        Node.triggleName = ret;
                    }
                    else
                    {
                        using (new GUIColorOverride(Color.red))
                        {
                            EditorGUILayout.LabelField($"没有找到对应动作{Table.skill.Get(state.skillID).attackaction}");
                        }
                    }
                }
                else
                {
                    //Node.triggleIndex = 0;
                    Node.triggleName = string.Empty;
                }
                motion = WindowState.GetAnimatorClip(Node.triggleName, Node.control);
                EditorGUILayout.ObjectField(motion, typeof(AnimationClip), true);
                if (motion)
                {
                    Node.duration = motion.length;
                }
            }
            else
            {
                using (new GUIColorOverride(Color.red))
                {
                    EditorGUILayout.LabelField("请检查角色是否有Animator组件");
                }
            }

            EditorGUILayout.EndHorizontal();
            GUILayout.EndArea();

        }

        public override void OnRightGUI()
        {
            //TODO draw duration
            if (Node.control)
            {
                endTime.Draw(rightSize, state, node.time + Node.duration);
            }
            DrawTimeRect(node.time, node.time + Node.duration, color);
        }

        public override void OnEventTimeChange(double time)
        {
            if (time > Node.time && time < (Node.duration + Node.time))
            {
                if (Node.control && motion)
                {
                    if (motion is AnimationClip && state.window.player)//todo other motion
                    {
                        var clip = motion as AnimationClip;
                        clip.SampleAnimation(state.window.player, ((float)time - Node.time));
                    }
                }
            }
        }
    }
}
