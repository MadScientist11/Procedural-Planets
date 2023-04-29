using System;
using UnityEngine;

namespace Planets
{
    public class FoldoutAttribute : PropertyAttribute
    {
        private readonly string _name;
        private readonly Type _editorType;

        public string Name => _name;
        public Type EditorType => _editorType;

        
        public FoldoutAttribute(string name)
        {
            _name = name;
            _editorType = null;
        }
        
        public FoldoutAttribute(string name, Type editorType)
        {
            _name = name;
            _editorType = editorType;
        }
    }
}