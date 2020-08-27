using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class TimelineFuncHelper
{
    static TimelineFuncHelper()
    {
        Assembly asm = Assembly.Load("Unity.Timeline.Editor");
        t = asm.GetType("UnityEditor.Timeline.DirectorStyles");
        var info = t.GetProperty("Instance");
        obj = info.GetValue(null);
    }

    public static Type t;
    public static object obj;

    public static string fullName(this Transform t)
    {
        string ret = t.name;
        var current = t;
        while (current.parent)
        {       
            ret = current.parent.name + "/" + ret;
            current = current.parent;
        }
        return ret;
          
    }

    public static MethodInfo PermanentControlIDMethod
    {
        get
        {
            if (_GetPermanentControlIDMethod == null)
            {
                var t = typeof(GUIUtility);
                var infos = t.GetRuntimeMethods().ToList();
                _GetPermanentControlIDMethod = infos.Find(r => r.Name.Contains("GetPermanentControlID")); //TODO hard code by zj
            }
            return _GetPermanentControlIDMethod;
        }
    }

    private static MethodInfo _GetPermanentControlIDMethod;
    public static int GetPermanentControlID()
    {

        return (int)PermanentControlIDMethod.Invoke(null, null);
    }

    public static GUIStyle displayBackground
    {
        get
        {
            var field = t.GetField("displayBackground");
            return (GUIStyle)field.GetValue(obj);
        }
    }

    public static GUIStyle tinyFont
    {
        get
        {
            //return DirectorStyles.Instance.tinyFont;
            var field = t.GetField("tinyFont");
            return (GUIStyle)field.GetValue(obj);
        }
    }

    public static float kDurationGuiThickness = 5.0f;

    public static GUIStyle timeCursor
    {
        get
        {
            //return DirectorStyles.Instance.timeCursor;
            var field = t.GetField("timeCursor");
            return (GUIStyle)field.GetValue(obj);
        }
    }
}
