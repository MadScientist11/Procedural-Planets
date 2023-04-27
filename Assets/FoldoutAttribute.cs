using System;

namespace Planets
{
    public class FoldoutAttribute : Attribute
    {
        private readonly string _name;

        public string Name => _name;

        public FoldoutAttribute(string name)
        {
            _name = name;
        }
    }
}