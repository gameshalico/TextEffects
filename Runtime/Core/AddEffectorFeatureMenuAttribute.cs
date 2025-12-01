using System;

namespace TextEffects.Core
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class AddEffectorFeatureMenuAttribute : Attribute
    {
        public AddEffectorFeatureMenuAttribute(string menuPath)
        {
            MenuPath = menuPath;
        }

        public string MenuPath { get; }
    }
}