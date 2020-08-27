using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skill
{
    class Control : IDisposable
    {
        readonly List<Manipulator> m_Manipulators = new List<Manipulator>();

        public virtual bool HandleManipulatorsEvents(WindowState state)
        {
            var isHandled = false;

            foreach (var manipulator in m_Manipulators)
            {
                isHandled = manipulator.HandleEvent(state);
                if (isHandled)
                    break;
            }

            return isHandled;
        }

        public void AddManipulator(Manipulator m)
        {
            m_Manipulators.Add(m);
        }

        public virtual void Dispose()
        {
        }
    }

}