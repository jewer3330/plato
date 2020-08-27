using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Skill
{
    class DeleteEventMenuManipulator : NewEventMenuManipulator
    {
        
        NodeControl control;
        public DeleteEventMenuManipulator(WindowState state, NodeControl control):base(state)
        {
            
           
            this.control = control;
        }

        void OnClickDeleteEvent()
        {
            state.RemoveNodeControl(control);
        }
       
        protected override bool ContextClick(Event evt, WindowState state)
        {
            if (control.rightSize.Contains(evt.mousePosition))
            {
                menu = state.GetMenu();
                menu.AddItem(new GUIContent("Delete"), false, OnClickDeleteEvent);
                menu.ShowAsContext();
                return true;
            }
            return false;
        }
    }

}
