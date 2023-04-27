using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Planets.Editor
{
    [CustomPropertyDrawer(typeof(PlanetSettings))]
    public class PlanetSettingsPropertyDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();

            BuildUI(root, property);
            return root;
        }

        private void BuildUI(VisualElement root, SerializedProperty property)
        {
            PropertyField propertyField = new PropertyField(property, "Settings");
            VisualElement fieldsContainer = new();
            root.Add(propertyField);
            root.Add(fieldsContainer);

            propertyField.RegisterCallback<ChangeEvent<Object>>(OnPropertyFieldChanged);

            void OnPropertyFieldChanged(ChangeEvent<Object> changeEvent)
            {
                fieldsContainer.Clear();

                if (property.objectReferenceValue == null)
                    return;

                var planetSettings = property.objectReferenceValue as PlanetSettings;
                planetSettings.RaiseChangedEvent();

                fieldsContainer.Add(DrawSettingsFields(property));

                root.Add(fieldsContainer);
                root.Bind(new SerializedObject(property.objectReferenceValue));
            }
        }

        public static IEnumerable<FieldInfo> GetFieldsWithAttribute<T>(PlanetSettings settings) where T : Attribute
        {
            return settings.GetType()
                .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(f => f.GetCustomAttribute<T>() != null);
        }

        private Dictionary<string, Foldout> _foldouts = new();

        private VisualElement DrawSettingsFields(SerializedProperty property)
        {
            var assetObject = new SerializedObject(property.objectReferenceValue);

            Box box = new Box();
            VisualElement foldoutsContainer = new VisualElement();
            box.Add(foldoutsContainer);

            FieldInfo[] fields = typeof(PlanetSettings).GetFields(BindingFlags.Public | BindingFlags.Instance);

            foreach (var field in fields)
            {
                SerializedProperty fieldProp = assetObject.FindProperty(field.Name);

                if (HasAttribute(field, out FoldoutAttribute foldoutAttribute))
                {
                    Foldout foldout = GetOrCreateFoldout(foldoutsContainer, foldoutAttribute);
                    SerializedProperty foldoutMemberProp = assetObject.FindProperty(field.Name);
                    foldout.Add(new PropertyField(foldoutMemberProp, foldoutMemberProp.displayName));
                    continue;
                }

                box.Add(new PropertyField(fieldProp, fieldProp.displayName));
            }

            return box;
        }

        private Foldout GetOrCreateFoldout(VisualElement container, FoldoutAttribute foldoutAttribute)
        {
            Foldout foldout = _foldouts.GetOrCreate(foldoutAttribute.Name);
            if (string.IsNullOrEmpty(foldout.text))
            {
                container.Add(foldout);
                foldout.text = foldoutAttribute.Name;
            }

            return foldout;
        }

        private bool HasAttribute(FieldInfo field, out FoldoutAttribute foldoutAttribute)
        {
            foldoutAttribute = field.GetCustomAttribute(typeof(FoldoutAttribute)) as FoldoutAttribute;
            return foldoutAttribute != null;
        }
    }
}