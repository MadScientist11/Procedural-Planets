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

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement container = new VisualElement();
            FoldoutAttribute foldoutAttribute = (FoldoutAttribute)attribute;
            _foldout ??= new Foldout
            {
                text = foldoutAttribute.Name
            };

            Type propertyDrawer = EditorExtensions.GetPropertyDrawer(foldoutAttribute.EditorType);
            if(propertyDrawer != null)
            {
                PropertyDrawer editor = (PropertyDrawer)Activator.CreateInstance(propertyDrawer);
                container.Add(editor.CreatePropertyGUI(property));
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