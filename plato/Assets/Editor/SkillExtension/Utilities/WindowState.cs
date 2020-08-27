using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using Object = System.Object;
namespace Skill
{
    class WindowState : IDisposable
    {
        public List<NodeControl> controls = new List<NodeControl>();

        public float maxTime => controls.Select(r => r.node).Max(r => r.timeEnd);

        public int skillID => PrefabModel.currentSkillID;
        public GameObject player
        {
            get
            {
                return window.player;
            }
        }

        public GameObject enemy
        {
            get
            {
                return window.enemy;
            }
        }

        public void SetData(IList<EventNode> datas,EventNodeScriptableObject eventNodeScriptable)
        {
            foreach (var k in datas)
            {
                if (k is PlayAnimationNode)
                {
                    eventNodeScriptable.animationNodes.Add(k as PlayAnimationNode);
                }
                else if (k is PlayEffectNode)
                {
                    eventNodeScriptable.playEffectNodes.Add(k as PlayEffectNode);
                }
                else if (k is FireBulletNode)
                {
                    eventNodeScriptable.fireBulletNodes.Add(k as FireBulletNode);
                }
                else if (k is TriggerAnimationNode)
                {
                    eventNodeScriptable.triggerAnimationNodes.Add(k as TriggerAnimationNode);
                }
                else if (k is TriggerEffectNode)
                {
                    eventNodeScriptable.triggerEventNodes.Add(k as TriggerEffectNode);
                }
                else if (k is AttackNode)
                {
                    eventNodeScriptable.attackNodes.Add(k as AttackNode);
                }
                else if (k is AttackCircleNode)
                {
                    eventNodeScriptable.attackCircleNodes.Add(k as AttackCircleNode);
                }
                else
                {
                    eventNodeScriptable.eventNodes.Add(k);
                }
            }
        }

        public static AnimationClip GetAnimatorClip(string triggleName, AnimatorController controller)
        {
            if (!controller)
                return null;
            var states = controller.layers[0].stateMachine.states.ToList();
            //AnimatorState destState = null;
            //bool find = false;
            //foreach (var k in states)
            //{
            //    var transitions = k.state.transitions;
            //    foreach (var v in transitions)
            //    {
            //        var conditions = v.conditions;
            //        foreach (var z in conditions)
            //        {
            //            if (z.parameter == triggleName)
            //            {
            //                destState = v.destinationState;
            //                find = true;
            //                break;
            //            }
            //        }
            //        if (find)
            //            break;
            //    }
            //    if (find)
            //        break;
            //}
            

            var find = states.Exists(r => r.state.name == triggleName);
            if (find)
            {
                var r = states.Find(t => t.state.name == triggleName);
                if (r.state.motion)
                {
                    return r.state.motion as AnimationClip;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }

        }


        public List<EventNode> GetData(EventNodeScriptableObject eventNodeScriptable)
        {
            List<EventNode> rets = new List<EventNode>();
            if (eventNodeScriptable)
            {
                rets.AddRange(eventNodeScriptable.eventNodes);
                rets.AddRange(eventNodeScriptable.animationNodes);
                rets.AddRange(eventNodeScriptable.playEffectNodes);
                rets.AddRange(eventNodeScriptable.fireBulletNodes);
                rets.AddRange(eventNodeScriptable.attackNodes);
                rets.AddRange(eventNodeScriptable.attackCircleNodes);
                rets.AddRange(eventNodeScriptable.triggerAnimationNodes);
                rets.AddRange(eventNodeScriptable.triggerEventNodes);
            }
            return rets;
        }

        private Dictionary<Type, Type> dics = new Dictionary<Type, Type>()
        {
            { typeof(PlayAnimationNode),typeof(PlayAnimationControl) },
            { typeof(PlayEffectNode),typeof(PlayEffectControl) },
            { typeof(FireBulletNode),typeof(FireBulletControl) },
            { typeof(AttackNode),typeof(AttackControl) },
            { typeof(TriggerEffectNode),typeof(TriggerEffectControl) },
            { typeof(TriggerAnimationNode),typeof(TriggerAnimationControl) },
            { typeof(AttackCircleNode),typeof(AttackCircleControl) },
            { typeof(EventNode),typeof(NodeControl) },
        };

        public void AddNodeControl(EventNode node)
        {
            var t = node.GetType();
            var r = dics[t];
            
            ConstructorInfo con = r.GetConstructor(new Type[] { typeof(WindowState) });
            var controller = con.Invoke(new Object[] { this });

            FieldInfo f = r.GetField("node");
            f.SetValue(controller, node);

            controls.Add((NodeControl)controller);
            //if (node is PlayAnimationNode)
            //    controls.Add(new PlayAnimationControl(this) { node = node });
            //else if (node is PlayEffectNode)
            //    controls.Add(new PlayEffectControl(this) { node = node });
            //else if (node is FireBulletNode)
            //    controls.Add(new FireBulletControl(this) { node = node });
            //else if (node is AttackNode)
            //    controls.Add(new AttackControl(this) { node = node });
            //else if (node is TriggerEffectNode)
            //    controls.Add(new TriggerEffectControl(this) { node = node });
            //else if (node is TriggerAnimationNode)
            //    controls.Add(new TriggerAnimationControl(this) { node = node });
            //else if(node is EventNode)
            //    controls.Add(new NodeControl(this) { node = node });

        }

        public bool HasPlayAnimationControl()
        {
            return null != controls.Find((r) =>
            {
                if (r is PlayAnimationControl)
                {
                    return true;
                }
                return false;
            });
        }

        public GenericMenu GetMenu()
        {
            var menu = new GenericMenu();
            if (HasPlayAnimationControl())
            {
                //menu.AddItem(new GUIContent("New Event"), false, OnAddEvent);
                //menu.AddItem(new GUIContent("New PlayAnimation"), false, OnAddPlayAniamtionEvent);
                menu.AddDisabledItem(new GUIContent("Play/Animation"), false);
                menu.AddItem(new GUIContent("Play/Effect"), false, OnAddPlayEffectEvent);
                menu.AddItem(new GUIContent("Play/FireBullet"), false, OnAddFireBulletEvent);
                menu.AddItem(new GUIContent("Play/Attack"), false, OnAddAttackEvent);
                menu.AddItem(new GUIContent("Play/AttackCircle"), false, OnAddAttackCircleEvent);
                menu.AddItem(new GUIContent("Trigger/Animation"), false, OnAddTriggerAnimationEvent);
                menu.AddItem(new GUIContent("Trigger/Effect"), false, OnAddTriggerEffectEvent);
            }
            else
            {
                //menu.AddItem(new GUIContent("New Event"), false, OnAddEvent);
                menu.AddItem(new GUIContent("Play/Animation"), false, OnAddPlayAniamtionEvent);
                menu.AddDisabledItem(new GUIContent("Play/Effect"), false);
                menu.AddDisabledItem(new GUIContent("Play/FireBullet"), false);
                menu.AddDisabledItem(new GUIContent("Play/Attack"), false);
                menu.AddDisabledItem(new GUIContent("Play/AttackCircle"), false);
                menu.AddItem(new GUIContent("Trigger/Animation"), false, OnAddTriggerAnimationEvent);
                menu.AddItem(new GUIContent("Trigger/Effect"), false, OnAddTriggerEffectEvent);
            }
            return menu;
        }

        protected void OnAddAttackCircleEvent()
        {
            AddNodeControl(new AttackCircleNode("{Event = 'TriggleAnimation',Name = 'Take Damage'}", (float)time));

        }

        protected void OnAddTriggerEffectEvent()
        {
            AddNodeControl(new TriggerEffectNode("{Event = 'TriggleAnimation',Name = 'Take Damage'}", (float)time));

        }

        protected void OnAddAttackEvent()
        {
            AddNodeControl(new AttackNode("{Event = 'TriggleAnimation',Name = 'Take Damage'}", (float)time));

        }

        protected void OnAddTriggerAnimationEvent()
        {
            AddNodeControl(new TriggerAnimationNode("{Event = 'TriggleAnimation',Name = 'Take Damage'}", (float)time));

        }

        protected void OnAddFireBulletEvent()
        {
            AddNodeControl(new FireBulletNode("{Event = 'TriggleAnimation',Name = 'Take Damage'}", (float)time));

        }

        protected void OnAddEvent()
        {
            AddNodeControl(new EventNode("{Event = 'TriggleAnimation',Name = 'Take Damage'}", (float)time));

        }

        protected void OnAddPlayAniamtionEvent()
        {
            AddNodeControl(new PlayAnimationNode("{Event = 'PlayAnimation',Name = 'Take Damage'}", (float)time));

        }

        protected void OnAddPlayEffectEvent()
        {
            AddNodeControl(new PlayEffectNode("{Event = 'PlayAnimation',Name = 'Take Damage'}", (float)time));

        }

        public void RemoveNodeControl(EventNode node)
        {
            var control = this.controls.Find(r => r.node == node);
            this.controls.Remove(control);
        }

        public void RemoveNodeControl(NodeControl control)
        {
            controls.Remove(control);
        }

        public SkillEditorWindow window;

        public WindowState(SkillEditorWindow window)
        {
            this.window = window;
            
        }

        public double time;

        public List<Manipulator> captured { get; } = new List<Manipulator>();

        public void AddCaptured(Manipulator manipulator)
        {
            if (!captured.Contains(manipulator))
                captured.Add(manipulator);
        }

        public void RemoveCaptured(Manipulator manipulator)
        {
            captured.Remove(manipulator);
        }

        public double GetSnappedTimeAtMousePosition(Vector2 mousePosition)
        {
            return SnapToFrameIfRequired(ScreenSpacePixelToTimeAreaTime(mousePosition.x));
        }

        public float ScreenSpacePixelToTimeAreaTime(float p)
        {
            // transform into track space by offsetting the pixel by the screen-space offset of the time area
            p -= timeAreaRect.x;
            return TrackSpacePixelToTimeAreaTime(p);
        }

        public Vector2 timeAreaScale
        {
            get { return window.timeArea.scale; }
        }

        public Vector2 timeAreaTranslation
        {
            get { return window.timeArea.translation; }
        }
        public float TrackSpacePixelToTimeAreaTime(float p)
        {
            p -= timeAreaTranslation.x;

            if (timeAreaScale.x > 0.0f)
                return p / timeAreaScale.x;

            return p;
        }
        public double SnapToFrameIfRequired(double currentTime)
        {
            return currentTime;
        }

        internal float TimeToPixel(double time)
        {
            return window.timeArea.TimeToPixel((float)time, timeAreaRect);
        }

        public void Dispose()
        {
            controls.ForEach(r => Dispose());
        }

        internal Rect timeAreaRect => window.TimeContent;

      
    }



}