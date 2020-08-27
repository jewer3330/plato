using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Timeline;
using UnityEditor.VersionControl;
using UnityEngine;
using Object = System.Object;
namespace Skill
{
   
    class SkillEditorWindow : EditorWindow
    {

        public TimeAreaHelper timeArea;
        public WindowState state;
        public readonly Rect LeftTopSize = new Rect(0, 0, 480, 44);
        public readonly Rect LeftContentHeaderSize = new Rect(0, 44, 480, 40);
        public readonly Rect RightContentHeaderSize = new Rect(500, 20, 800, 24);
        public readonly Rect RightTopSize = new Rect(500, 0, 800, 20);
        readonly List<Manipulator> m_PostManipulators = new List<Manipulator>();


        [MenuItem("Tools/Skill/Editor")]
        public static void ShowWindow()
        {
            //Show existing window instance. If one doesn't exist, make one.
            Table.TableLoad.Clear();
            Table.TableLoad.LoadBinFromResources();
            var window = (SkillEditorWindow)EditorWindow.GetWindow(typeof(SkillEditorWindow), false, "SkillEditor", true);
            window.minSize = new Vector2(800, 600);
            
        }

        private void Init()
        {
            if(state == null)
                state = new WindowState(this);
            
            if (playHead == null)
            {
                playHead = new TimeAreaItem(TimelineFuncHelper.timeCursor, state, OnTrackHeadDrag, false);
                playHead.drawHead = true;
                playHead.drawLine = true;
            }
            if (timeArea == null)
            {
                
                timeArea = new TimeAreaHelper(false)
                {
                    hRangeLocked = false,
                    vRangeLocked = true,
                    margin = -2,
                    scaleWithWindow = true,
                    hSlider = true,
                    vSlider = false,
                    hBaseRangeMin = 0,
                    hBaseRangeMax = 20,
                    hRangeMin = 0,
                    hRangeMax = 100,
                    hScaleMax = 1000f,
                    rect = TimeContent,
                };
                m_PostManipulators.Add(new NewEventMenuManipulator(state));
            }
        }


        public Rect TimeHeaderRect => new Rect(RightContentHeaderSize.x, RightContentHeaderSize.y, position.width - RightContentHeaderSize.x, RightContentHeaderSize.height);

        public Rect TimeTickRect => new Rect(RightContentHeaderSize.x, RightContentHeaderSize.y + RightContentHeaderSize.height, position.width - RightContentHeaderSize.x, position.height - RightContentHeaderSize.height);


        public Rect TimeContent => new Rect(RightContentHeaderSize.x, RightContentHeaderSize.y, position.width - LeftContentHeaderSize.width, position.height - RightContentHeaderSize.y);

        public Rect TimeContentWithOffset => new Rect(RightContentHeaderSize.x - 5, RightContentHeaderSize.y, position.width - LeftContentHeaderSize.width, position.height - RightContentHeaderSize.y);


        public Rect TimeContentWithOutTile => new Rect(RightContentHeaderSize.x, RightContentHeaderSize.y + RightContentHeaderSize.y, position.width - LeftContentHeaderSize.width, position.height - RightContentHeaderSize.y - RightContentHeaderSize.height);

        public int frame = 30;

        public TimeAreaItem playHead;

        private GameObject select_player;
        /// <summary>
        /// 编辑中可以预览的玩家对象
        /// </summary>
        public GameObject player;


        private GameObject select_enemy;

        /// <summary>
        /// 编辑器中可以预览的敌人对象
        /// </summary>
        public GameObject enemy;

        private string filePath = string.Empty;
        private string fileName = string.Empty;

        

        void OnPlayerChange(GameObject go)
        {
            if (!go) return;
            if (player)
            {
                GameObject.DestroyImmediate(player);
            }
            player = GameObject.Instantiate(go);
            player.transform.position = Vector3.zero;
        }

        void OnEnmeyChange(GameObject go)
        {
            if (!go) return;
            if (enemy)
            {
                GameObject.DestroyImmediate(enemy);
            }
            enemy = GameObject.Instantiate(go);
            enemy.transform.position = new Vector3(0, 0, 10);
            enemy.transform.forward = Vector3.back;
        }

        private void OnDestroy()
        {
            if (player)
                GameObject.DestroyImmediate(player);
            if (enemy)
                GameObject.DestroyImmediate(enemy);
        }

        void RightTopHelper()
        {
            GUILayout.Space(50);
            bool isSelectChange = false;
            var typeChange = (PrefabModel.TableType)EditorGUILayout.EnumPopup(PrefabModel.currentType);
            if (typeChange != PrefabModel.currentType)
            {
                PrefabModel.currentModelIndex = 0;
                PrefabModel.currentSkillIDIndex = 0;
                PrefabModel.currentType = typeChange;
                isSelectChange = true;
            }
            var idx = PrefabModel.GetIDs(PrefabModel.currentType);
            var display = idx.Select(r => r.ToString()).ToArray();
           

            var modelChange = EditorGUILayout.Popup(PrefabModel.currentModelIndex, display);
            
            if (modelChange != PrefabModel.currentModelIndex)
            {
                PrefabModel.currentModelIndex = modelChange;
                PrefabModel.currentSkillIDIndex = 0;
                isSelectChange = true;
            }

            PrefabModel.currentID = idx[PrefabModel.currentModelIndex];
            var displaySkillList = PrefabModel.GetSkillList(PrefabModel.currentType, PrefabModel.currentID);

            var skillChange = EditorGUILayout.Popup(PrefabModel.currentSkillIDIndex, displaySkillList);
            if (skillChange != PrefabModel.currentSkillIDIndex)
            {
                PrefabModel.currentSkillIDIndex = skillChange;
                isSelectChange = true;
            }

            

            PrefabModel.currentSkillID = int.Parse(displaySkillList[PrefabModel.currentSkillIDIndex]);
            EditorGUILayout.LabelField(PrefabModel.GetSkillName(PrefabModel.currentSkillID));

            if (isSelectChange)
            {
                FindPlayer();
                FindEnemy();
                FindConfig("Skill");
            }

            if (GUILayout.Button("Find Player"))
            {
                FindPlayer();
                

            }
            if (GUILayout.Button("Find Enemy"))
            {
                FindEnemy();

            }
            if (GUILayout.Button("Find Skill"))
            {
                FindConfig("Skill");
            }
            //if (GUILayout.Button("Find Defend"))
            //{
            //    FindConfig("Defend");
            //}
            if (GUILayout.Button("New"))
            {
                New();
            }
        }


        void FindConfig(string defend = "Skill")
        {
            if (PrefabModel.currentSkillID == 0)
            {
                var tempSkillList = PrefabModel.GetSkillList(PrefabModel.currentType, PrefabModel.currentID);
                PrefabModel.currentSkillID = int.Parse(tempSkillList[PrefabModel.currentSkillIDIndex]);
            }
            var path = "Assets/Editor/SkillExtension/Config/"+ defend + PrefabModel.currentSkillID + ".asset";
            var obj = AssetDatabase.LoadAssetAtPath<EventNodeScriptableObject>(path);
            if (obj)
                Load(path);
            else
                MessageBox(string.Format($"{defend} config not find id:{PrefabModel.currentSkillID}"));
        }

        void FindPlayer()
        {
            var newSelect = PrefabModel.GetPrefab(PrefabModel.currentType, PrefabModel.currentID);
            if (newSelect != select_player)
            {
                select_player = newSelect;
                OnPlayerChange(select_player);
            }
            
        }

        void FindEnemy()
        {
            var newSelect = PrefabModel.GetPrefab(PrefabModel.currentType, PrefabModel.currentID);
            if (newSelect != select_enemy)
            {
                select_enemy = newSelect;
                OnEnmeyChange(select_enemy);
            }
        }

        void Play()
        {
            play = !play;

            if (play)
            {
                startTime = Time.realtimeSinceStartup;
                EditorApplication.update += TimeUpdate;
            }
            else
            { 
                EditorApplication.update -= TimeUpdate;
                startTime = 0;
            }
        }
        private float startTime = 0;
        void TimeUpdate()
        {
            OnTrackHeadDrag(Time.realtimeSinceStartup - startTime);
        }
        private bool play = false;
        void OnGUI()
        {
            Init();
            GUILayout.BeginArea(LeftTopSize);
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            //var old = GUI.backgroundColor;
            //GUI.backgroundColor = !play ? Color.green : Color.red;
            //if (GUILayout.Button("Play", GUILayout.Width(80)))
            //{
            //    Play();
            //}
            //GUI.backgroundColor = old;
           
            if (GUILayout.Button("Save", GUILayout.Width(80)))
            {
                Save();
            }
            if (GUILayout.Button("Load", GUILayout.Width(80)))
            {
                Load();
            }
            if (GUILayout.Button("SaveEvent"))
            {
                var nodes = state.controls.Select(arg => arg.node).ToList();
                SaveAnimationEvents(nodes, PrefabModel.currentSkillID);
            }
            if (GUILayout.Button("ExportAll", GUILayout.Width(80)))
            {
                ExportLua();
            }
            if (GUILayout.Button("ExportAllEvents"))
            {
                ExportAllEvents();
            }


            //var options = new string[] { "Skill", "Defend" };
            //PrefabModel.currentDefendIndex = EditorGUILayout.Popup(PrefabModel.currentDefendIndex, options);
            PrefabModel.currentDefendIndex = 0;
            PrefabModel.isDefendSkill = PrefabModel.currentDefendIndex == 1;
            EditorGUILayout.LabelField(Path.GetFileName(filePath));

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            var newSelect = (GameObject)EditorGUILayout.ObjectField("player", select_player, typeof(GameObject), true);
            var select = (GameObject)EditorGUILayout.ObjectField("enemy", select_enemy, typeof(GameObject), true);
            GUILayout.EndHorizontal();

            if (select != select_enemy)
            {
                select_enemy = select;
                OnEnmeyChange(select);
            }

            if (newSelect != select_player)
            {
                select_player = newSelect;
                OnPlayerChange(newSelect);
            }
            
            GUILayout.EndVertical();
            GUILayout.EndArea();

            GUILayout.BeginArea(RightTopSize);
            GUILayout.BeginHorizontal();
            
            RightTopHelper();
            
            GUILayout.EndHorizontal();
            GUILayout.EndArea();


            GUILayout.BeginHorizontal();
            
            

            timeArea.BeginViewGUI();
            timeArea.TimeRuler(TimeHeaderRect, frame, true, false, 1.0f, 1);
            timeArea.DrawMajorTicks(TimeTickRect, frame);
            timeArea.EndViewGUI();
            GUILayout.EndHorizontal();
            var clipRect = new Rect(0.0f, 0.0f, position.width, position.height);
            clipRect.xMin += state.window.TimeHeaderRect.width;


            OnGUILeftContent();
            playHead.Draw(TimeContent, state, state.time);
            OnHandlePlayHead();

            if (state.captured.Count > 0)
            {
                using (new GUIViewportScope(clipRect))
                {
                    foreach (var o in state.captured)
                    {
                        o.Overlay(Event.current, state);
                    }
                    Repaint();
                }
            }

        }

        void OnHandlePlayHead()
        {
            playHead.HandleManipulatorsEvents(state);

            for (int i = 0; i < state.controls.Count; i++)
            {
                var k = state.controls[i];
                k.HandleManipulatorsEvents(state);
            }

            HandleManipulatorsPostEvents(state);
        }

        public bool HandleManipulatorsPostEvents(WindowState state)
        {
            var isHandled = false;

            foreach (var manipulator in m_PostManipulators)
            {
                isHandled = manipulator.HandleEvent(state);
                if (isHandled)
                    break;
            }

            return isHandled;
        }

        void OnTrackHeadDrag(double newTime)
        {
            state.time = Math.Max(0.0, newTime);
            playHead.showTooltip = true;
            OnEventTimeChange(newTime);
        }

        void OnEventTimeChange(double time)
        {
            state.controls.ForEach((r) =>
            {
                if(r.node.active)
                    r.OnEventTimeChange(time);
            });
        }

        void OnGUILeftContent()
        {
            float offsetHeight = 0;
            for (int i = 0; i < state.controls.Count; i++)
            {
                var k = state.controls[i];
                k.Draw(new Vector2(10, LeftContentHeaderSize.y + offsetHeight + 10));
                offsetHeight += state.controls[i].height + 20;
            }
        }


        void SetEvent(SerializedProperty events, int index, AnimationEvent animationEvent)
        {
            //Debug.Log("set event");
            if (events != null && events.isArray)
            {
                if (index < events.arraySize)
                {
                    events.GetArrayElementAtIndex(index).FindPropertyRelative("floatParameter").floatValue = animationEvent.floatParameter;
                    events.GetArrayElementAtIndex(index).FindPropertyRelative("functionName").stringValue = animationEvent.functionName;
                    events.GetArrayElementAtIndex(index).FindPropertyRelative("intParameter").intValue = animationEvent.intParameter;
                    events.GetArrayElementAtIndex(index).FindPropertyRelative("objectReferenceParameter").objectReferenceValue = animationEvent.objectReferenceParameter;
                    events.GetArrayElementAtIndex(index).FindPropertyRelative("data").stringValue = animationEvent.stringParameter;
                    events.GetArrayElementAtIndex(index).FindPropertyRelative("time").floatValue = animationEvent.time;
                }
                else
                {
                    Debug.LogWarning("Invalid Event Index");
                }
            }
        }


        public ModelImporterClipAnimation AssgnEvent(ModelImporterClipAnimation animation, IList<AnimationEvent> list)
        {
            animation.events = list.ToArray();
            return animation;
        }

        void SerilizeEvents(ModelImporter modelImporter, IList<AnimationEvent> events,int id)
        {

            //var clipAnimations = modelImporter.defaultClipAnimations;
            //var m_clip = clipAnimations[0];

            //m_clip.events = events.ToArray();
            //modelImporter.SaveAndReimport();
            //AssetDatabase.Refresh();

            //ModelImporter importer = modelImporter;
            //if (importer != null)
            //{
            //    List<ModelImporterClipAnimation> animList = new List<ModelImporterClipAnimation>();
                
            //    for (int i = 0; i < importer.clipAnimations.Length; i++)
            //    {
            //        animList.Add(AssgnEvent(importer.clipAnimations[i],events));
            //    }

            //    importer.clipAnimations = animList.ToArray();

            //}
            //SerializedObject serializedObject = new SerializedObject(importer);
            //serializedObject.ApplyModifiedProperties();
            //importer.SaveAndReimport();
            //AssetDatabase.Refresh();

            SerializedObject serializedObject = new SerializedObject(modelImporter);

            if (modelImporter.clipAnimations == null || modelImporter.clipAnimations.Length == 0)
            {
                modelImporter.clipAnimations = modelImporter.defaultClipAnimations;
            }
            
            SerializedProperty clipAnimations = serializedObject.FindProperty("m_ClipAnimations");
            if (clipAnimations == null || clipAnimations.arraySize == 0)
            {
                MessageBox(string.Format($" m_ClipAnimations = null，clip:{modelImporter.assetPath} skillID: {id}"));
            }
            else
            {
                SerializedProperty m_clip = clipAnimations.GetArrayElementAtIndex(0);
                SerializedProperty m_events = m_clip.FindPropertyRelative("events");
                if (modelImporter.clipAnimations.Length == 1)
                {
                    m_events.ClearArray();
                    foreach (AnimationEvent evt in events)
                    {
                        m_events.InsertArrayElementAtIndex(m_events.arraySize);
                        SetEvent(m_events, m_events.arraySize - 1, evt);
                    }

                    serializedObject.ApplyModifiedProperties();
                    modelImporter.SaveAndReimport();
                    AssetDatabase.Refresh();
                }
                else
                {
                    MessageBox(string.Format($" clipAnimations != 1，技能ID:{id}"));

                }
            }

        }
        public void MessageBox(string msg)
        { 
            EditorUtility.DisplayDialog("Skill Editor", msg, "确认");
            Debug.LogWarning(msg);
        }
        void SaveAnimationEvents(List<EventNode> nodes,int id)
        {
            if (nodes == null || nodes.Count == 0)
            { 
                MessageBox(string.Format($"技能ID:{id}空配置，建议删除"));
            }
            var find = nodes.Find(r => r is PlayAnimationNode);
            if (find != null)
            {
                var playAnimationNode = find as PlayAnimationNode;
                if (playAnimationNode != null && playAnimationNode.control)
                {
                    var clip = WindowState.GetAnimatorClip(playAnimationNode.triggleName, playAnimationNode.control);
                    if (clip)
                    {
                        List<AnimationEvent> events = new List<AnimationEvent>();
                        foreach (var k in nodes)
                        {
                            if (!(k is TriggerNode || k is PlayAnimationNode))
                            {
                                events.Add(new AnimationEvent() { functionName = "NewEvent", stringParameter = k.luaConfig, time = Mathf.Clamp01((k.time - playAnimationNode.time) / clip.length) });
                            }
                        }
                        var import = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(clip)) as ModelImporter;
                        if (import)
                            SerilizeEvents(import, events,id);
                        else
                            MessageBox(string.Format($" ModelImporter not find，技能ID:{id}"));
                    }
                    else
                    {
                        MessageBox(string.Format($" AnimationClip not find，技能ID:{id}"));
                    }
                }
                else
                {
                    MessageBox(string.Format($" Aniamtion Controller not find，技能ID:{id}"));
                }
            }
            else
            {
                MessageBox(string.Format($" PlayAnimationNode not find，技能ID:{id}"));
            }
        }

        void ExportLua()
        {
            List<string> skillID = new List<string>();
            var monsterIDs = PrefabModel.GetMonsterIDs();
            foreach (var k in monsterIDs)
            { 
                skillID.AddRange(PrefabModel.GetMonsterSkillList(k));
            }
            var playerIDS = PrefabModel.GetPlayerIDs();
            foreach (var k in playerIDS)
            {
                skillID.AddRange(PrefabModel.GetPlayerSkillList(k));
            }
            foreach (var id in skillID)
            {
                var asset = AssetDatabase.LoadAssetAtPath<EventNodeScriptableObject>("Assets/Editor/SkillExtension/Config/Skill" + id + ".asset");
                var events = state.GetData(asset);
                ExportLua(id, events);
            }
        }

        void ExportAllEvents()
        {
            List<string> skillID = new List<string>();
            var monsterIDs = PrefabModel.GetMonsterIDs();
            foreach (var k in monsterIDs)
            {
                skillID.AddRange(PrefabModel.GetMonsterSkillList(k));
            }
            var playerIDS = PrefabModel.GetPlayerIDs();
            foreach (var k in playerIDS)
            {
                skillID.AddRange(PrefabModel.GetPlayerSkillList(k));
            }
            foreach (var id in skillID)
            {
                var asset = AssetDatabase.LoadAssetAtPath<EventNodeScriptableObject>("Assets/Editor/SkillExtension/Config/Skill" + id + ".asset");
                var events = state.GetData(asset);
                SaveAnimationEvents(events, int.Parse(id));
            }
        }

        void ExportLua(string id,List<EventNode> nodes)
        {
           
            SaveAnimationEvents(nodes, int.Parse(id));
           
            string luapath = Application.dataPath + "/LuaScripts/src/Battle/Skill/Config/Skill" + id + ".lua";
            using (FileStream fs = new FileStream(luapath, System.IO.FileMode.Create))
            {

                System.IO.StreamWriter writer = new System.IO.StreamWriter(fs);
                writer.WriteLine("local Skill = {");
                foreach (var k in nodes)
                {
                    k.ToLua(writer);
                }
                float maxTime = (nodes == null || nodes.Count == 0) ? 0 : nodes.Max(r => r.timeEnd);
                writer.WriteLine(string.Format("duration = {0}", maxTime));
                writer.WriteLine("}");
                writer.WriteLine("return Skill");
                writer.Flush();
                writer.Close();
            }
        }

        void Save()
        {
            fileName = (PrefabModel.isDefendSkill ? "Defend" :"Skill") + PrefabModel.currentSkillID.ToString();
            var path = EditorUtility.SaveFilePanel("Save Skill", Application.dataPath + "/Editor/SkillExtension/Config/" , fileName, "asset");
            if (string.IsNullOrEmpty(path))
                return;
            filePath = path;
            var nodes = state.controls.Select(arg => arg.node).ToList();
            SaveAnimationEvents(nodes, PrefabModel.currentSkillID);
            var obj = CreateInstance<EventNodeScriptableObject>();
            state.SetData(nodes,obj);
            obj.skillOwner = select_player;
            string luapath = Application.dataPath + "/LuaScripts/src/Battle/Skill/Config/" + Path.GetFileNameWithoutExtension(fileName) + ".lua";
            Debug.Log(luapath);
            using (FileStream fs = new FileStream(luapath, System.IO.FileMode.Create))
            {

                System.IO.StreamWriter writer = new System.IO.StreamWriter(fs);
                writer.WriteLine("local Skill = {");
                foreach (var k in nodes)
                {
                    k.ToLua(writer);
                }
                writer.WriteLine(string.Format("duration = {0}",state.maxTime));
                writer.WriteLine("}");
                writer.WriteLine("return Skill");
                writer.Flush();
                writer.Close();
            }

            var p = filePath.Substring(filePath.IndexOf("Assets"));
            AssetDatabase.CreateAsset(GameObject.Instantiate(obj), p);
            AssetDatabase.Refresh();
        }

        void New()
        {
            state.controls.ForEach(r => r.Dispose());
            state.controls.Clear();
        }

        void Load(string path)
        {
            if (string.IsNullOrEmpty(path))
                return;
            string assetpath = path.Substring(path.IndexOf("Assets/"));
            filePath = assetpath;
            fileName = Path.GetFileName(filePath);
            var nodes = UnityEngine.Object.Instantiate(AssetDatabase.LoadAssetAtPath<EventNodeScriptableObject>(assetpath));
            state.controls.ForEach(r => r.Dispose());
            state.controls.Clear();
            var data = state.GetData(nodes);
            foreach (var k in data)
            {
                state.AddNodeControl(k);
            }
            if (nodes.skillOwner != select_player)
            {
                select_player = nodes.skillOwner;
                OnPlayerChange(select_player);
            }
        }
        void Load()
        {
            var path = EditorUtility.OpenFilePanel("Load Skill", Application.dataPath+ "/Editor/SkillExtension/Config/", "asset");
            Load(path);
        }

       
    }
}