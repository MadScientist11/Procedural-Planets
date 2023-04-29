using System;
using System.Reflection;

namespace Planets.Editor
{
    public class EditorExtensions
    {
        public static Type GetPropertyDrawer(Type classType)
        {
            var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
            var scriptAttributeUtility = assembly.CreateInstance("UnityEditor.ScriptAttributeUtility");
            var scriptAttributeUtilityType = scriptAttributeUtility.GetType();
 
 
            var bindingFlags = BindingFlags.NonPublic | BindingFlags.Static;
            var getDrawerTypeForType = scriptAttributeUtilityType.GetMethod("GetDrawerTypeForType", bindingFlags);
 
            return (Type)getDrawerTypeForType.Invoke(scriptAttributeUtility, new object[] { classType });
        }
    }
}