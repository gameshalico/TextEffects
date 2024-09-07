using System;

namespace TextEffects.Core
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AddEffectorFeatureMenuAttribute : Attribute
    {
        public AddEffectorFeatureMenuAttribute(string menuPath)
        {
            MenuPath = menuPath;
        }

        public string MenuPath { get; }
    }
}