using UnityEditor;
using UnityEngine;

namespace Skill
{
    class NewEventMenuManipulator : Manipulator
    {
        protected GenericMenu menu;
        protected WindowState state;
        public NewEventMenuManipulator(WindowState state)
        {
            this.state = state;
           
        }

        protected override bool ContextClick(Event evt, WindowState state)
        {
            if (state.window.TimeContentWithOutTile.Contains(evt.mousePosition))
            {
                menu = state.GetMenu();
                menu.ShowAsContext();
                return true;
            }
            return false;
        }
    }


}