using System.Collections.Generic;
using UnityEngine;

namespace TextEffects.Data
{
    public readonly partial struct TagInfo
    {
        public Dictionary<string, string> CloneAttributes()
        {
            return new Dictionary<string, string>(_handle.Item.Attributes);
        }

        public float GetFloat(string key, float defaultValue = 0f)
        {
            Attributes.TryGetValue(key, out var value);
            return value != null && float.TryParse(value, out var result) ? result : defaultValue;
        }

        public bool TryGetFloat(string key, out float value)
        {
            Attributes.TryGetValue(key, out var stringValue);
            return float.TryParse(stringValue, out value);
        }

        public int GetInt(string key, int defaultValue = 0)
        {
            Attributes.TryGetValue(key, out var value);
            return value != null && int.TryParse(value, out var result) ? result : defaultValue;
        }

        public bool TryGetInt(string key, out int value)
        {
            Attributes.TryGetValue(key, out var stringValue);
            return int.TryParse(stringValue, out value);
        }

        public bool GetBool(string key, bool defaultValue = false)
        {
            Attributes.TryGetValue(key, out var value);
            return value != null && bool.TryParse(value, out var result) ? result : defaultValue;
        }

        public bool TryGetBool(string key, out bool value)
        {
            Attributes.TryGetValue(key, out var stringValue);
            return bool.TryParse(stringValue, out value);
        }

        public string GetString(string key, string defaultValue = "")
        {
            Attributes.TryGetValue(key, out var value);
            return value ?? defaultValue;
        }

        public bool TryGetString(string key, out string value)
        {
            return Attributes.TryGetValue(key, out value);
        }

        public Color32 GetColor(string key, Color32 defaultValue = default)
        {
            Attributes.TryGetValue(key, out var value);
            return value != null && ColorUtility.TryParseHtmlString(value, out var result) ? result : defaultValue;
        }

        public bool TryGetColor(string key, out Color value)
        {
            Attributes.TryGetValue(key, out var stringValue);
            return ColorUtility.TryParseHtmlString(stringValue, out value);
        }
    }
}