using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TextEffects.Data;

namespace TextEffects.Core
{
    public static class TagParser
    {
        // 引用符内の>を許容するため、属性部分のパターンを変更
        private const string TagPattern = @"<(?<Closing>\/)?(?<TagName>[a-zA-Z0-9-_@~#]+)(?<Attributes>(?:[^<>""'\r\n]|""(?:[^""\\]|\\.)*""|'(?:[^'\\]|\\.)*')*?)>";
        // 文字リテラル対応: ""または''で囲まれた値、またはスペースと>以外の文字列
        private const string AttributePattern = @"(?<Name>[a-zA-Z0-9-_@~#]*)=(?:(?<Value>""(?:[^""\\]|\\.)*"")|(?<Value>'(?:[^'\\]|\\.)*')|(?<Value>[^\s>]+))";
        private static readonly Regex TagRegex = new(TagPattern);
        private static readonly Regex AttributeRegex = new(AttributePattern);

        private static readonly HashSet<string> TMPTags = new()
        {
            "a", "align", "allcaps", "alpha", "b", "color", "cspace", "font", "font-weight", "gradient", "i", "indent",
            "line-height", "line-indent", "link", "lowercase", "margin", "margin-left", "margin-right", "mark",
            "mspace", "noparse", "pos", "rotate",
            "s", "size", "smallcaps", "space", "sprite", "strikethrough", "style", "sub", "sup", "u", "uppercase",
            "voffset", "width", "br", "nobr", "page"
        };


        public static (string tmpText, TagInfo[] tags) Parse(string input)
        {
            var tags = new LinkedList<TagInfoBuffer>();
            var stack = new Stack<TagInfoBuffer>();
            var tempStack = new Stack<TagInfoBuffer>();

            var tmpTextBuilder = new StringBuilder();
            var lastAddedIndex = 0;

            var isNoParse = false;
            var ignoreTagLength = 0;

            foreach (Match match in TagRegex.Matches(input))
            {
                var tagName = match.Groups["TagName"].Value;
                var isClosingTag = match.Groups["Closing"].Success;

                if (tagName == "noparse")
                {
                    if (!isNoParse && !isClosingTag)
                    {
                        isNoParse = true;
                        ignoreTagLength += match.Length;
                    }

                    if (isNoParse && isClosingTag)
                    {
                        isNoParse = false;
                    }
                }

                if (isNoParse)
                {
                    continue;
                }

                var tagNextCharIndex = match.Index - ignoreTagLength;
                ignoreTagLength += match.Length;

                if (tagName == "br")
                {
                    ignoreTagLength -= 1;
                    continue;
                }

                if (TMPTags.Contains(tagName))
                {
                    continue;
                }

                tmpTextBuilder.Append(input.Substring(lastAddedIndex, match.Index - lastAddedIndex));
                lastAddedIndex = match.Index + match.Length;

                if (isClosingTag) // Closing tag
                {
                    while (stack.Count > 0)
                    {
                        if (stack.Peek().TagName == tagName)
                        {
                            // Opening tag found
                            var openTag = stack.Pop();
                            openTag.EndIndex = tagNextCharIndex;
                            openTag.IsEmptyTag = false;
                            tags.AddLast(openTag);
                            break;
                        }

                        tempStack.Push(stack.Pop());
                    }

                    while (tempStack.Count > 0) stack.Push(tempStack.Pop());
                }
                else // Opening tag
                {
                    var attributesString = match.Groups["Attributes"].Value.Trim();
                    var tagBuffer = TagInfoBuffer.Rent();
                    ParseAttributes(attributesString, tagBuffer.Attributes);

                    var tagInfo = tagBuffer.Set(
                        tagName,
                        tagNextCharIndex,
                        tagNextCharIndex,
                        true
                    );
                    stack.Push(tagInfo);
                }
            }

            foreach (var tagInfo in stack)
            {
                tags.AddLast(tagInfo);
            }

            tmpTextBuilder.Append(input.Substring(lastAddedIndex));

            var lastIndex = input.Length - ignoreTagLength;
            foreach (var tagInfo in tags)
            {
                if (tagInfo.IsEmptyTag)
                {
                    tagInfo.EndIndex = lastIndex;
                }
            }

            var buffers = tags.ToArray();
            var tagInfos = new TagInfo[buffers.Length];
            for (var i = 0; i < buffers.Length; i++)
            {
                tagInfos[i] = new TagInfo(buffers[i]);
            }

            return (tmpTextBuilder.ToString(), tagInfos);
        }

        // Parse attributes from string
        private static void ParseAttributes(string attributesString, Dictionary<string, string> attributes)
        {
            foreach (Match match in AttributeRegex.Matches(attributesString))
            {
                var name = match.Groups["Name"].Value;
                var value = match.Groups["Value"].Value;
                attributes[name] = UnescapeAttributeValue(value);
            }
        }

        /// <summary>
        /// 属性値から引用符を除去してエスケープシーケンスを解除します
        /// </summary>
        private static string UnescapeAttributeValue(string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            // ダブルクォートまたはシングルクォートで囲まれている場合
            if ((value.StartsWith("\"") && value.EndsWith("\"")) ||
                (value.StartsWith("'") && value.EndsWith("'")))
            {
                // 引用符を除去
                value = value.Substring(1, value.Length - 2);

                // エスケープシーケンスを解除
                var result = new StringBuilder(value.Length);
                for (int i = 0; i < value.Length; i++)
                {
                    if (value[i] == '\\' && i + 1 < value.Length)
                    {
                        // エスケープされた文字
                        i++;
                        switch (value[i])
                        {
                            case 'n':
                                result.Append('\n');
                                break;
                            case 'r':
                                result.Append('\r');
                                break;
                            case 't':
                                result.Append('\t');
                                break;
                            case '\\':
                            case '"':
                            case '\'':
                            case '>':
                                result.Append(value[i]);
                                break;
                            default:
                                // 未知のエスケープシーケンスはそのまま
                                result.Append('\\');
                                result.Append(value[i]);
                                break;
                        }
                    }
                    else
                    {
                        result.Append(value[i]);
                    }
                }
                return result.ToString();
            }

            // 引用符なしの場合はそのまま返す(後方互換性)
            return value;
        }
    }
}