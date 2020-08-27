using System;
using UnityEngine;

namespace Skill
{
    class Scrub : Manipulator
    {
        readonly Func<Event, WindowState, bool> m_OnMouseDown;
        readonly Action<double> m_OnMouseDrag;
        readonly Action m_OnMouseUp;

        bool m_IsCaptured;

        public Scrub(Func<Event, WindowState, bool> onMouseDown, Action<double> onMouseDrag, Action onMouseUp)
        {
            m_OnMouseDown = onMouseDown;
            m_OnMouseDrag = onMouseDrag;
            m_OnMouseUp = onMouseUp;
        }

        protected override bool MouseDown(Event evt, WindowState state)
        {
            if (evt.button != 0)
                return false;

            if (!m_OnMouseDown(evt, state))
                return false;

            state.AddCaptured(this);
            m_IsCaptured = true;

            return true;
        }

        protected override bool MouseUp(Event evt, WindowState state)
        {
            if (!m_IsCaptured)
                return false;

            m_IsCaptured = false;
            state.RemoveCaptured(this);

            m_OnMouseUp();

            return true;
        }

        protected override bool MouseDrag(Event evt, WindowState state)
        {
            if (!m_IsCaptured)
                return false;

            m_OnMouseDrag(state.GetSnappedTimeAtMousePosition(evt.mousePosition));

            return true;
        }
    }


}