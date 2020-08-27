using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace TableTool
{
    [CreateAssetMenu(fileName = "table.asset", menuName = "Table/Config")]
    public class Config : ScriptableObject
    {
        public string sourcePath = "Resources/Table";
        public string tableCSPath = "Scripts/Table";
        public string tableluaPath = "LuaScripts/src/Table";
        static string key = "table";
        public string pluginPath = "";
        public char SplitChar = '\t';
        public bool showGroup;
        public int offset = 1;
        public static void Save(Config data)
        {
            //var asset = (Config)ScriptableObject.CreateInstance<Config>();
            //asset.showGroup = data.showGroup;
            //asset.sourcePath = data.sourcePath;
            //asset.tableCSPath = data.tableCSPath;
            //asset.tableluaPath = data.tableluaPath;
            //asset.pluginPath = data.pluginPath;
            //asset.SplitChar = data.SplitChar;
            //AssetDatabase.CreateAsset(asset, asset.pluginPath + "/" + key + ".asset");
            EditorUtility.SetDirty(data);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public static Config Load()
        {
            var path = "Assets/Editor/Table/" + key + ".asset";
            var asset = (Config)AssetDatabase.LoadAssetAtPath(path, typeof(Config));
            if (!asset)
            {
                asset = CreateInstance<Config>();
                asset.pluginPath = Application.dataPath + "/Editor/Table/";
                AssetDatabase.CreateAsset(asset, path);
            }
            asset.pluginPath = Application.dataPath + "/Editor/Table/";
            return asset;
        }
    }
}

