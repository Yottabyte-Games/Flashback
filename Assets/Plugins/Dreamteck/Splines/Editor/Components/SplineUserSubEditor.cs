using Plugins.Dreamteck.Splines.Components;
using UnityEditor;

namespace Plugins.Dreamteck.Splines.Editor.Components
{
    public class SplineUserSubEditor
    {
        protected string title = "";
        protected SplineUser user;
        protected SplineUserEditor editor = null;
        public bool alwaysOpen = false;

        public bool isOpen
        {
            get { return foldout || alwaysOpen; }
        }
        bool foldout = false;

        public SplineUserSubEditor(SplineUser user, SplineUserEditor editor)
        {
            this.editor = editor;
            this.user = user;
        }

        public virtual void DrawInspector()
        {
            if (!alwaysOpen)
            {
                foldout = EditorGUILayout.Foldout(foldout, title);
            }
        }

        public virtual void DrawScene()
        {

        }
    }
}
