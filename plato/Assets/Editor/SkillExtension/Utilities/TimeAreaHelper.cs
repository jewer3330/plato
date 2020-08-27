using System;
using System.Reflection;
using UnityEngine;
using Object = System.Object;

namespace Skill
{
    class TimeAreaHelper
    {
        private bool v;
        internal bool hRangeLocked
        {
            set
            {
                var info = t.GetProperty("hRangeLocked");
                info.SetValue(timeArea, value);
            }
        }
        internal bool vRangeLocked
        {
            set
            {
                var info = t.GetProperty("vRangeLocked");
                info.SetValue(timeArea, value);
            }
        }
        internal int margin
        {
            set
            {
                var info = t.GetProperty("margin");
                info.SetValue(timeArea, value);
            }
        }
        internal bool scaleWithWindow
        {
            set
            {
                var info = t.GetProperty("scaleWithWindow");
                info.SetValue(timeArea, value);
            }
        }
        internal bool hSlider
        {
            set
            {
                var info = t.GetProperty("hSlider");
                info.SetValue(timeArea, value);
            }
        }
        internal bool vSlider
        {
            set
            {
                var info = t.GetProperty("vSlider");
                info.SetValue(timeArea, value);
            }
        }
        internal float hBaseRangeMin
        {
            set
            {
                var info = t.GetProperty("hBaseRangeMin");
                info.SetValue(timeArea, value);
            }
        }
        internal int hBaseRangeMax
        {
            set
            {
                var info = t.GetProperty("hBaseRangeMax");
                info.SetValue(timeArea, value);
            }
        }
        internal float hRangeMin
        {
            set
            {
                var info = t.GetProperty("hRangeMin");
                info.SetValue(timeArea, value);
            }
        }
        internal float hScaleMax
        {
            set
            {
                var info = t.GetProperty("hScaleMax");
                info.SetValue(timeArea, value);
            }
        }
        internal Rect rect
        {
            set
            {
                var info = t.GetProperty("rect");
                info.SetValue(timeArea, value);
            }
        }

        private object timeArea;

        public Vector2 translation
        {
            get
            {
                var info = t.GetProperty("translation");
                return (Vector2)info.GetValue(timeArea);
            }

        }
        public Vector2 scale
        {
            get
            {
                var info = t.GetProperty("scale");
                return (Vector2)info.GetValue(timeArea);
            }
        }

        internal int hRangeMax
        {
            set
            {
                var info = t.GetProperty("hRangeMax");
                info.SetValue(timeArea, value);
            }
        }


        private Type t;
        private Type tbase;

        public TimeAreaHelper(bool v)
        {
            this.v = v;
            Assembly asm = Assembly.Load("UnityEditor");
            t = asm.GetType("UnityEditor.TimeArea");

            tbase = asm.GetType("UnityEditor.ZoomableArea");
            ConstructorInfo con = t.GetConstructor(new Type[] { typeof(bool) });
            timeArea = con.Invoke(new Object[] { v });
        }

        internal void BeginViewGUI()
        {
            MethodInfo oMethod = t.GetMethod("BeginViewGUI", BindingFlags.Public | BindingFlags.Instance);
            oMethod.Invoke(timeArea, new Object[] { });
        }
        internal void TimeRuler(Rect position, float frameRate, bool labels, bool useEntireHeight, float alpha, int timeFormat)
        {
            MethodInfo oMethod = t.GetMethod("TimeRuler", new Type[] { typeof(Rect), typeof(float) });
            oMethod.Invoke(timeArea, new Object[] { position, frameRate });
        }

        //internal void TimeRuler(Rect position, float frameRate, bool labels, bool useEntireHeight, float alpha, TimeFormat timeFormat)
        //{ 

        //}


        internal void DrawTimeOnSlider(float time, Color c, float duration, float kDurationGuiThickness)
        {
            MethodInfo oMethod = t.GetMethod("DrawTimeOnSlider", BindingFlags.Instance | BindingFlags.Public);
            oMethod.Invoke(timeArea, new Object[] { time, c, duration, kDurationGuiThickness });
        }

        internal void DrawMajorTicks(Rect timeTickRect, int frame)
        {
            MethodInfo oMethod = t.GetMethod("DrawMajorTicks", BindingFlags.Instance | BindingFlags.Public);
            oMethod.Invoke(timeArea, new Object[] { timeTickRect, frame });
        }

        internal void EndViewGUI()
        {
            MethodInfo oMethod = t.GetMethod("EndViewGUI", BindingFlags.Instance | BindingFlags.Public);
            oMethod.Invoke(timeArea, new Object[] { });
        }

        internal float TimeToPixel(float time, Rect timeAreaRect)
        {
            MethodInfo oMethod = t.GetMethod("TimeToPixel", BindingFlags.Instance | BindingFlags.Public);
            return (float)oMethod.Invoke(timeArea, new Object[] { time, timeAreaRect });
        }
    }

}