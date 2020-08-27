using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Skill
{
    class PlayEffectControl : NodeControl<PlayEffectNode>
    {
        protected TimeAreaItem timeEndControl;
        protected GameObject preview;
        protected GameObject previewInstance;
        protected string search;
        protected bool toggle;
        public PlayEffectControl(WindowState state) : base(state)
        {
            timeEndControl = new TimeAreaItem(TimelineFuncHelper.timeCursor, state, OnDragEnd, true);
            timeEndControl.drawHead = true;
            timeEndControl.drawLine = true;
            timeEndControl.lineColor = color;
            timeEndControl.headColor = color;
        }
        public override Color color => Color.yellow * 0.5f;

       

        void OnDragEnd(double newTime)
        {
            var endTime = Math.Max(0.0, newTime);
            Node.lifeTime = (float)endTime - Node.time;
        }

        public void OnTransformField(Transform owner)
        {


            toggle = EditorGUILayout.BeginFoldoutHeaderGroup(toggle, "出生挂点");
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
                        //if (searchChange)
                        //{
                        if (Node.transformPath == null) Node.transformPath = string.Empty;
                        
                            var findIndex = transforms.FindIndex(r => r.Contains(Node.transformPath.Replace("/", "$")));
                            Node.transformIndex = findIndex == -1 ? 0 : findIndex;
                        
                        //}

                       var index = EditorGUILayout.Popup(Node.transformIndex, transforms.ToArray());
                        var root = transforms[Node.transformIndex];

                        if (index != Node.transformIndex)
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
                Node.isWorld = EditorGUILayout.Toggle("世界", Node.isWorld);
                Node.localPosition = EditorGUILayout.Vector3Field("offset", Node.localPosition);
                Node.localEulerAngles = EditorGUILayout.Vector3Field("eulur", Node.localEulerAngles);
                Node.localScale = EditorGUILayout.Vector3Field("scale", Node.localScale);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();


        }

        public override float height => toggle ? 235 : base.height + 15; 

        public override void OnLeftGUI()
        {
            GUILayout.BeginArea(leftSize);
            node.active = GUILayout.Toggle(node.active, "开启");
            EditorGUILayout.BeginHorizontal();

            GUILayout.Label(Node.Name);
            Node.effect = (GameObject)EditorGUILayout.ObjectField(Node.effect, typeof(GameObject), false);
            //Node.lifeTime = EditorGUILayout.FloatField("Time:",Node.lifeTime);
            EditorGUILayout.EndHorizontal();
            if(state.player)
                OnTransformField(state.player.transform);
            

            
            GUILayout.EndArea();
        }

     
        public override void OnRightGUI()
        {
            if (Node.effect)
            {
                timeEndControl.Draw(rightSize, state, Node.time + Node.lifeTime);
            }
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

        

        public override void OnEventTimeChange(double time)
        {
            if (time > Node.time && time < (Node.lifeTime + Node.time))
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
                    if (state.player)
                    {
                        previewInstance.transform.SetParent(state.player.transform.Find(Node.transformPath), false);
                        previewInstance.transform.localPosition = Node.localPosition;
                        previewInstance.transform.localRotation = Quaternion.Euler(Node.localEulerAngles);
                        previewInstance.transform.localScale = Node.localScale; 
                    }
                   
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

    }
}