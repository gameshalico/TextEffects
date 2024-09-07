using System;
using System.Collections.Generic;
using TextEffects.Core;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace TextEffects.Editor
{
    public class EffectorFeatureAdvancedDropdown : AdvancedDropdown
    {
        private Dictionary<int, Type> _types;

        public EffectorFeatureAdvancedDropdown(AdvancedDropdownState state) : base(state)
        {
            minimumSize = new Vector2(200, 300);
        }

        public event Action<Type> OnSelectItem;

        protected override void ItemSelected(AdvancedDropdownItem item)
        {
            OnSelectItem?.Invoke(_types[item.id]);
        }

        protected override AdvancedDropdownItem BuildRoot()
        {
            var attributeTypes = TypeCache.GetTypesWithAttribute<AddEffectorFeatureMenuAttribute>();

            var root = new AdvancedDropdownItem("Feature");
            _types = new Dictionary<int, Type>();

            foreach (var type in attributeTypes)
                if (type.GetCustomAttributes(typeof(AddEffectorFeatureMenuAttribute), false) is
                    AddEffectorFeatureMenuAttribute[] attributes)
                    foreach (var attribute in attributes)
                    {
                        var path = attribute.MenuPath.Split('/');
                        var parent = root;
                        for (var i = 0; i < path.Length; i++)
                        {
                            var pathItem = path[i];

                            var found = false;
                            foreach (var child in parent.children)
                                if (child.name == pathItem)
                                {
                                    parent = child;
                                    found = true;
                                    break;
                                }

                            if (!found)
                            {
                                var item = new AdvancedDropdownItem(pathItem);
                                parent.AddChild(item);
                                parent = item;

                                if (i == path.Length - 1)
                                {
                                    item.id = type.GetHashCode();
                                    var monoScript =
                                        MonoScript.FromScriptableObject(ScriptableObject.CreateInstance(type));
                                    var icon = EditorGUIUtility.GetIconForObject(monoScript);

                                    item.icon = icon;
                                    _types.TryAdd(item.id, type);
                                }
                            }
                        }
                    }

            return root;
        }
    }
}