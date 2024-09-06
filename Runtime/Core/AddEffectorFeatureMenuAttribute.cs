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

        public AddEffectorFeatureMenuAttribute(string menuPath, int order)
        {
            MenuPath = menuPath;
            Order = order;
        }

        public string MenuPath { get; }
        public int Order { get; }
    }
}