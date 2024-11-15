using Plugins.Dreamteck.Splines.Core.Primitives;
using Plugins.Dreamteck.Splines.Editor.SplineEditor.DS_Editor;
using UnityEditor;

namespace Plugins.Dreamteck.Splines.Editor.Primitives
{
    public class RoundedRectangleEditor : PrimitiveEditor
    {
        public override string GetName()
        {
            return "Rounded Rect";
        }

        public override void Open(DreamteckSplinesEditor editor)
        {
            base.Open(editor);
            primitive = new RoundedRectangle();
            primitive.offset = origin;
        }

        protected override void OnGUI()
        {
            base.OnGUI();
            RoundedRectangle rect = (RoundedRectangle)primitive;
            rect.size = EditorGUILayout.Vector2Field("Size", rect.size);
            rect.xRadius = EditorGUILayout.FloatField("X Radius", rect.xRadius);
            rect.yRadius = EditorGUILayout.FloatField("Y Radius", rect.yRadius);
        }
    }
}
