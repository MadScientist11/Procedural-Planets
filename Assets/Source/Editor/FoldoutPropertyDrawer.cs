using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Planets.Editor
{
    [CustomPropertyDrawer(typeof(FoldoutAttribute))]
    public class FoldoutPropertyDrawer : PropertyDrawer
    {
        private Foldout _foldout;
        private PropertyDrawer _editor;

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement container = new VisualElement();
            FoldoutAttribute foldoutAttribute = (FoldoutAttribute)attribute;
            _foldout ??= new Foldout
            {
                text = foldoutAttribute.Name
            };
            _foldout.Clear();

            Type propertyDrawer = EditorHelpers.GetPropertyDrawer(property.objectReferenceValue.GetType());
            if (propertyDrawer != null)
            {
                if (_editor == null)
                    _editor = (PropertyDrawer)Activator.CreateInstance(propertyDrawer);
                container.Add(_editor.CreatePropertyGUI(property));
            }
            else
            {
                container.Add(new PropertyField(property));
            }

            _foldout.Add(container);
            return _foldout;
        }
    }
}