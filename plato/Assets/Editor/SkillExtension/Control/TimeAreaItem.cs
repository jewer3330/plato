using System;
using UnityEditor;
using UnityEngine;

namespace Skill
{
    class TimeAreaItem : Control
    {
        private WindowState state;
        public Color headColor { get; set; }
        public Color lineColor { get; set; }
        public bool drawLine { get; set; }
        public bool drawHead { get; set; }
        public bool canMoveHead { get; set; }
        public string tooltip { get; set; }
        public Vector2 boundOffset { get; set; }

        readonly GUIContent m_HeaderContent = new GUIContent();
        readonly GUIStyle m_Style;
        readonly Tooltip m_Tooltip;

        Rect m_BoundingRect;

        float widgetHeight { get { return m_Style.fixedHeight; } }
        float widgetWidth { get { return m_Style.fixedWidth; } }

        public Rect bounds
        {
            get
            {
                //Rect r = m_BoundingRect;
                //r.y = state.window.TimeHeaderRect.yMax - widgetHeight;
                //r.position += boundOffset;
                return m_BoundingRect;
            }
        }

        public GUIStyle style
        {
            get { return m_Style; }
        }


        public bool showTooltip { get; set; }

        // is this the first frame the drag callback is being invoked
        public bool firstDrag { get; private set; }

        public bool alwaysShowTooltip = false;
        public TimeAreaItem(GUIStyle style, WindowState state, Action<double> onDrag, bool alwaysShowTooltip = false)
        {
            this.state = state;
            m_Style = style;
            headColor = Color.white;
            this.alwaysShowTooltip = alwaysShowTooltip;
            var scrub = new Scrub(
                (evt, st) =>
                {
                    firstDrag = true;
                    return st.window.TimeContentWithOffset.Contains(evt.mousePosition) && bounds.Contains(evt.mousePosition);
                },
                (d) =>
                {
                    if (onDrag != null)
                        onDrag(d);
                    firstDrag = false;
                },
                () =>
                {
                    showTooltip = false;
                    firstDrag = false;
                }
            );
            AddManipulator(scrub);
            lineColor = m_Style.normal.textColor;
            drawLine = true;
            drawHead = true;
            canMoveHead = false;
            tooltip = string.Empty;
            boundOffset = Vector2.zero;
            m_Tooltip = new Tooltip(TimelineFuncHelper.displayBackground, TimelineFuncHelper.tinyFont);
        }
        protected Rect contentSize;
        public void Draw(Rect rect, WindowState state, double time)
        {
            contentSize = rect;
            var clipRect = new Rect(0.0f, 0.0f, state.window.position.width, state.window.position.height);
            clipRect.xMin += state.window.LeftContentHeaderSize.x;

            using (new GUIViewportScope(clipRect))
            {
                Vector2 windowCoordinate = rect.min;
                //windowCoordinate.y += 4.0f;
                windowCoordinate.x = state.TimeToPixel(time);

                m_BoundingRect = new Rect((windowCoordinate.x - widgetWidth / 2.0f), windowCoordinate.y, widgetWidth, widgetHeight);

                // Do not paint if the time cursor goes outside the timeline bounds...
                if (Event.current.type == EventType.Repaint)
                {
                    if (m_BoundingRect.xMax < state.window.TimeContent.xMin)
                        return;
                    if (m_BoundingRect.xMin > state.window.TimeContent.xMax)
                        return;
                }

                var top = new Vector3(windowCoordinate.x, rect.y );
                var bottom = new Vector3(windowCoordinate.x, rect.yMax);

                if (drawLine)
                {
                    Rect lineRect = Rect.MinMaxRect(top.x - 0.5f, top.y, bottom.x + 0.5f, bottom.y);
                    EditorGUI.DrawRect(lineRect, lineColor);
                }

                if (drawHead)
                {
                    Color c = GUI.color;
                    GUI.color = headColor;
                    GUI.Box(bounds, m_HeaderContent, m_Style);
                    GUI.color = c;

                    if (canMoveHead)
                        EditorGUIUtility.AddCursorRect(bounds, MouseCursor.MoveArrow);
                }

                if (alwaysShowTooltip || showTooltip)
                {
                    m_Tooltip.text = string.Format("{0:F2}", time);

                    Vector2 position = bounds.position;
                    //position.y = state.window.TimeContent.y;
                    position.y -= m_Tooltip.bounds.height;
                    position.x -= Mathf.Abs(m_Tooltip.bounds.width - bounds.width) / 2.0f;

                    Rect tooltipBounds = bounds;
                    tooltipBounds.position = position;
                    m_Tooltip.bounds = tooltipBounds;

                    m_Tooltip.Draw();
                }
            }
        }
    }

}
