using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Planets.Editor
{
    [CustomEditor(typeof(Planet))]
    public class PlanetEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var container = new VisualElement();
            InspectorElement.FillDefaultInspector(container, serializedObject, this);
            return container;
        }
    }
}

