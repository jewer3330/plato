using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Skill
{

    class FireBulletControl : NodeControl<FireBulletNode>
    {

        protected TimeAreaItem timeEndControl;
        protected bool toggle;
        protected bool toggleTarget;
        protected string search;
        protected string searchTarget;
        public FireBulletControl(WindowState state) : base(state)
        {
            timeEndControl = new TimeAreaItem(TimelineFuncHelper.timeCursor, state, OnDragEnd, true);
            timeEndControl.drawHead = true;
            timeEndControl.drawLine = true;
            timeEndControl.lineColor = color;
            timeEndControl.headColor = color;
        }
        public override Color color => Color.red * 0.5f;


        void OnDragEnd(double newTime)
        {
            var endTime = Math.Max(0.0, newTime);
            Node.lifeTime = (float)endTime - Node.time;
        }



        public override float height
        {
            get
            {
                if (toggle && toggleTarget)
                {
                    return 390;
                }
                else if (toggle && !toggleTarget)
                {
                    return 255;
                }
                else if (!toggle && toggleTarget)
                {
                    return 255;
                }
                else
                {
                    return 95;
                }
            }
        }

        public override void OnLeftGUI()
        {
            GUILayout.BeginArea(leftSize);
            EditorGUILayout.BeginHorizontal();
            node.active =GUILayout.Toggle(node.active,"开启");
           
            EditorGUILayout.EndHorizontal();

            Node.alwaysAttack = GUILayout.Toggle(Node.alwaysAttack, "必中子弹");

            EditorGUILayout.BeginHorizontal();

            GUILayout.Label(Node.Name);
            Node.effect = (GameObject)EditorGUILayout.ObjectField(Node.effect, typeof(GameObject), false);


            Node.speed = EditorGUILayout.FloatField(Node.speed);
            //if (GUILayout.Button("计算偏差"))
            //{
            //    if (state.player && previewInstance)
            //        Node.offset = previewInstance.transform.position - state.player.transform.position;
            //}
            EditorGUILayout.EndHorizontal();

            //EditorGUILayout.BeginHorizontal();
            //Node.offset = EditorGUILayout.Vector3Field("offset",Node.offset);
            //Node.dir = EditorGUILayout.Vector3Field("dir",Node.dir);
            //EditorGUILayout.EndHorizontal();


            if (state.player)
            {
                OnTransformField(state.player.transform);
               
            }
            if (state.enemy && Node.alwaysAttack)
            {
                OnTransformTargetField(state.enemy.transform);
            }
            GUILayout.EndArea();
        }

        public void OnTransformField(Transform owner)
        {


            toggle = EditorGUILayout.BeginFoldoutHeaderGroup( toggle, "出生挂点");
            if (toggle)
            {
                if (owner)
                {
                    EditorGUILayout.BeginHorizontal();
                    //bool searchChange = false;
                    var s = EditorGUILayout.TextField(search);
                    if (s != search )
                    {
                        //searchChange = true;
                        search = s;
                    }
                    var transforms = owner.GetComponentsInChildren<Transform>().Select(r => r.fullName()).Select(r => r.Replace("/", "$")).ToList();
                    if (!string.IsNullOrEmpty(search))
                    {
                        transforms = transforms.FindAll(r => Regex.IsMatch(r, search, RegexOptions.IgnoreCase));
                    }

                    if (transforms.Count > 0)
                    {
                        if (Node.transformPath == null) Node.transformPath = string.Empty;

                        //if (searchChange)
                        //{
                        var findIndex = transforms.FindIndex(r => r.Contains(Node.transformPath.Replace("/", "$")));
                            Node.transformIndex = findIndex == -1 ? 0 : findIndex;
                        //}

                        var index = EditorGUILayout.Popup(Node.transformIndex, transforms.ToArray());
                        var root = transforms[Node.transformIndex];

                        if (Node.transformIndex != index)
                        {
                            root = transforms[index];
                        }
                        if (root.Contains("$"))
                        {
                            root = root.Substring(root.IndexOf("$") + 1);
                            Node.transformPath = root.Replace("$", "/");
                            Node.transform = owner.transform.Find(Node.transformPath);
                        }
                        else
                        {
                            Node.transformPath = "";
                            Node.transform = owner.transform;
                        }
                    }
                    else
                    {
                        EditorGUILayout.LabelField("查找失败");
                        //Node.transformPath = "";
                        //Node.transform = owner.transform;
                    }
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.ObjectField(Node.transform, typeof(Transform), false);
                Node.offset = EditorGUILayout.Vector3Field("offset", Node.offset);
                Node.localEulerAngles = EditorGUILayout.Vector3Field("eulur", Node.localEulerAngles);
                Node.localScale = EditorGUILayout.Vector3Field("scale", Node.localScale);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

        }


        public void OnTransformTargetField(Transform owner)
        {


            toggleTarget = EditorGUILayout.BeginFoldoutHeaderGroup(toggleTarget, "目标挂点");
            if (toggleTarget)
            {
                if (owner)
                {
                    EditorGUILayout.BeginHorizontal();
                    //bool searchChange = false;
                    var s = EditorGUILayout.TextField(searchTarget);
                    if (s != searchTarget)
                    {
                        //searchChange = true;
                        searchTarget = s;
                    }
                    var transforms = owner.GetComponentsInChildren<Transform>().Select(r => r.fullName()).Select(r => r.Replace("/", "$")).ToList();
                    if (!string.IsNullOrEmpty(searchTarget))
                    {
                        transforms = transforms.FindAll(r => Regex.IsMatch(r, searchTarget, RegexOptions.IgnoreCase));
                    }

                    if (transforms.Count > 0)
                    {
                        //if (searchChange)
                        //{
                        if (Node.transformPathTarget == null) Node.transformPathTarget = string.Empty;

                        var findIndex = transforms.FindIndex(r => r.Contains(Node.transformPathTarget.Replace("/", "$")));
                            Node.transformIndexTarget = findIndex == -1 ? 0 : findIndex;
                        //}

                        var index = EditorGUILayout.Popup(Node.transformIndexTarget, transforms.ToArray());
                        var root = transforms[Node.transformIndexTarget];

                        if (Node.transformIndexTarget != index)
                        {
                            root = transforms[index];
                        }

                        if (root.Contains("$"))
                        {
                            root = root.Substring(root.IndexOf("$") + 1);
                            Node.transformPathTarget = root.Replace("$", "/");
                            Node.transformTarget = owner.transform.Find(Node.transformPathTarget);
                        }
                        else
                        {
                            Node.transformPathTarget = "";
                            Node.transformTarget = owner.transform;
                        }
                    }
                    else
                    {
                        EditorGUILayout.LabelField("查找失败");
                        //Node.transformPathTarget = "";
                        //Node.transformTarget = owner.transform;
                    }
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.ObjectField(Node.transformTarget, typeof(Transform), false);
                
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

        }


        public override void OnRightGUI()
        {

            //if (Node.effect)
            //{
                timeEndControl.Draw(rightSize, state, Node.time + Node.lifeTime);
            //}
            DrawTimeRect(Node.time, Node.time + Node.lifeTime, color);

        }

        public override bool HandleManipulatorsEvents(WindowState state)
        {
            if (!timeEndControl.HandleManipulatorsEvents(state))
            {
                return base.HandleManipulatorsEvents(state);
            }
            return false;
        }

        protected GameObject preview;
        protected GameObject previewInstance;
        protected Vector3 lastPosition;
        protected Vector3 currentPosition;
        protected GameObject cube;
        public override void OnEventTimeChange(double time)
        {
            if (time > Node.time && time < (Node.lifeTime + Node.time))
            {
                var timeCurrent = (float)time - Node.time;
                lastPosition = currentPosition;
                currentPosition = CalStartPosition() + Node.dir * timeCurrent * Node.speed;
                if (!Node.effect)
                {
                    cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.hideFlags = HideFlags.HideAndDontSave;
                    Node.effect = cube;
                    Node.effect.transform.localScale = Vector3.one * 0.1f;
                }
                if (Node.effect)
                {
                    //选择了另外一个
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

                    //previewInstance.transform.parent = Node.transform;
                    previewInstance.transform.position = currentPosition;
                    previewInstance.transform.localScale = Node.localScale;
                    preview.transform.localEulerAngles = Node.localEulerAngles;
                    var particleSystem = previewInstance.GetComponentsInChildren<ParticleSystem>().ToList();
                    //particleSystem?.ForEach(r => r.Simulate(timeCurrent));
                    particleSystem?.ForEach(r => r.Play(true));
                    var objs = Selection.objects.ToList();
                    objs.Add(previewInstance);
                    Selection.objects = objs.ToArray();
                }

                Raycast(currentPosition, lastPosition);


            }
            else
            {
                cal = false;
                OnDispose();
            }
        }

        private bool cal = false;
        private Vector3 worldPosition;
        protected Vector3 CalStartPosition()
        {
            if (!cal)
            {
                cal = true;
                worldPosition = Node.offset;
                if (Node.transform)
                {
                    worldPosition = Node.transform.TransformPoint(Node.offset);
                }

                
            }
            return worldPosition;
        }

        protected void Raycast(Vector3 currentPosition, Vector3 lastPosition)
        {
            RaycastHit info;
            var dir = (currentPosition - lastPosition).normalized;
            var length = (currentPosition - lastPosition).magnitude;

            var ret = Physics.Raycast(lastPosition, dir, out info, length);
            if (ret && state.player && info.transform != state.player.transform)
            {
                trigger = ret;
                state.controls.ForEach(r => r.OnTrigger(new HitInfo() { gameObject = info.transform.gameObject, time = (float)state.time, point = info.point, transform = info.transform }));
            }
            if (trigger && !ret)
            {
                trigger = ret;
                state.controls.ForEach(r => r.OnTriggerEnd());

            }
        }

        protected bool trigger;

        protected void OnDispose()
        {
            if (previewInstance)
            {
                var objs = Selection.objects.ToList();
                var obj = objs.Find(r => r == previewInstance);
                objs.Remove(obj);
                Selection.objects = objs.ToArray();
                GameObject.DestroyImmediate(previewInstance);
            }
            if (cube)
            { 
                GameObject.DestroyImmediate(cube);
            }
        }

        public override void Dispose()
        {
            OnDispose();
        }


    }
}
