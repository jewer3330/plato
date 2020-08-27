using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Table;
using System.Linq;
using UnityEditor;
using System.IO;

public class PrefabModel 
{
    public static int currentID;
    public static int currentModelIndex;
    public enum TableType
    { 
        Player,
        Monster,
    }

    public static TableType currentType;
    public static int currentSkillID;
    public static int currentSkillIDIndex;
    public static bool isDefendSkill;
    public static int currentDefendIndex;
    public static int[] GetIDs(TableType type)
    {
        return type == TableType.Player ? GetPlayerIDs() : GetMonsterIDs();
    }

    public static int [] GetPlayerIDs()
    {
        return player.datas.Select(r => r.Key).ToArray();
    }

    public static int[] GetMonsterIDs()
    {
        return monster.datas.Select(r => r.Key).ToArray();
    }

    public static string GetPlayerPrefabName(int id)
    {
        return player.datas[id].model;
    }

    public static string GetMonsterPrefabName(int id)
    {
        return monster.datas[id].model;
    }

    public static string[] GetSkillList(TableType type, int id)
    {
        return type == TableType.Monster ? GetMonsterSkillList(id) : GetPlayerSkillList(id);
    }

    public static string[] GetPlayerSkillList(int id)
    {
        return player.datas[id].learnedskilllist.Split(';');
    }

    public static string[] GetMonsterSkillList(int id)
    {
        return monster.datas[id].learnedskilllist.Split(';');
    }

    public static GameObject GetPrefab(TableType type, int id)
    {
        var fileName = type == TableType.Monster ? GetMonsterPrefabName(id) : GetPlayerPrefabName(id);
        var path = "Assets/DemoRes/Prefabs/NPC/{0}.prefab";
        var go = (GameObject) AssetDatabase.LoadAssetAtPath(string.Format(path,fileName),typeof(GameObject));
        return go;
    }

    public static string GetSkillName(int id)
    {
        return skill.datas[id].skillname;
    }
}
