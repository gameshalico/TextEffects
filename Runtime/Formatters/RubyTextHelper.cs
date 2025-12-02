using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace TextEffects.Formatters
{
    /// <summary>
    /// Ruby text formatting helper for TextEffects
    /// </summary>
    public static class RubyTextHelper
    {
        private static Regex _rubyRegex;

        /// <summary>
        /// Delegate for getting preferred text size
        /// </summary>
        /// <param name="text">Text to measure</param>
        /// <returns>Preferred width and height</returns>
        public delegate Vector2 GetPreferredValuesDelegate(string text);

        /// <summary>
        /// Gets or creates the cached regex for ruby text matching
        /// Pattern: &lt;r="rubyText"&gt;baseText&lt;/r&gt;, &lt;r='rubyText'&gt;baseText&lt;/r&gt;, or &lt;r=rubyText&gt;baseText&lt;/r&gt;
        /// </summary>
        public static Regex RubyRegex
        {
            get
            {
                if (_rubyRegex == null)
                {
                    // 文字リテラル対応: ""または''で囲まれた値、またはスペースと>以外の文字列
                    // 引用符内の>を許容するように修正
                    _rubyRegex = new Regex(@"<r=(?:""(?<ruby1>(?:[^""\\]|\\.)*)""|'(?<ruby2>(?:[^'\\]|\\.)*)'|(?<ruby3>[^\s>]+))>(?<base>[^<]+)</r>", RegexOptions.Compiled);
                }
                return _rubyRegex;
            }
        }

        /// <summary>
        /// Formats input text by converting ruby tags to TextMeshPro markup
        /// </summary>
        /// <param name="input">Input text with ruby tags</param>
        /// <param name="rubyScale">Ruby text scale (0-1)</param>
        /// <param name="rubyVerticalOffset">Vertical offset in em units</param>
        /// <param name="getPreferredValues">Function to get preferred text size in pixels</param>
        /// <param name="rubyPrefixTag">Tag to add before ruby text (optional)</param>
        /// <param name="rubySuffixTag">Tag to add after ruby text (optional)</param>
        /// <param name="unescapeXml">If true, unescapes XML entities before calculating width</param>
        /// <returns>Formatted text with TMP markup</returns>
        public static string FormatRubyText(string input, float rubyScale, float rubyVerticalOffset, GetPreferredValuesDelegate getPreferredValues, string rubyPrefixTag = "", string rubySuffixTag = "", bool unescapeXml = false)
        {
            return RubyRegex.Replace(input, match =>
            {
                // 3つの名前付きグループのいずれかからルビテキストを取得
                var rubyText = match.Groups["ruby1"].Success ? match.Groups["ruby1"].Value :
                               match.Groups["ruby2"].Success ? match.Groups["ruby2"].Value :
                               match.Groups["ruby3"].Value;

                // エスケープシーケンスを解除
                rubyText = UnescapeValue(rubyText);

                var baseText = match.Groups["base"].Value;
                return CreateRubyText(baseText, rubyText, rubyScale, rubyVerticalOffset, getPreferredValues, rubyPrefixTag, rubySuffixTag, unescapeXml);
            });
        }

        /// <summary>
        /// Creates ruby text markup for TextMeshPro
        /// </summary>
        /// <param name="baseText">Base text (kanji)</param>
        /// <param name="rubyText">Ruby text (furigana)</param>
        /// <param name="rubyScale">Ruby text scale (0-1)</param>
        /// <param name="rubyVerticalOffset">Vertical offset in em units</param>
        /// <param name="getPreferredValues">Function to get preferred text size in pixels</param>
        /// <param name="rubyPrefixTag">Tag to add before ruby text (optional)</param>
        /// <param name="rubySuffixTag">Tag to add after ruby text (optional)</param>
        /// <param name="unescapeXml">If true, unescapes XML entities before calculating width</param>
        /// <returns>Formatted ruby text markup</returns>
        public static string CreateRubyText(string baseText, string rubyText, float rubyScale, float rubyVerticalOffset, GetPreferredValuesDelegate getPreferredValues, string rubyPrefixTag = "", string rubySuffixTag = "", bool unescapeXml = false)
        {
            var rubyScalePercent = rubyScale * 100f;

            // XMLエスケープを解除して実際の表示幅を計算
            var baseTextForWidth = unescapeXml ? XmlEscapeUtility.UnescapeXml(baseText) : baseText;
            var rubyTextForWidth = unescapeXml ? XmlEscapeUtility.UnescapeXml(rubyText) : rubyText;

            // Get actual text widths in pixels from TextMeshPro
            var baseSize = getPreferredValues(baseTextForWidth);
            var rubySize = getPreferredValues(rubyTextForWidth);

            float baseWidth = baseSize.x;
            float rubyWidth = rubySize.x * rubyScale;

            // Layout strategy: Center both base and ruby within the total width
            float actualRubyWidth = rubyWidth;
            string formattedRubyText;

            // When ruby is shorter than base, use cspace to distribute ruby characters evenly
            // 文字数の計算は変換後のテキストを使用
            int rubyCharCount = rubyTextForWidth.Length;
            if (rubyWidth < baseWidth && rubyCharCount > 1)
            {
                // Calculate character spacing to make ruby fill the base width
                // totalExtraSpace: the space we need to add between characters
                var totalExtraSpace = baseWidth - rubyWidth;

                // Distribute the extra space evenly between character gaps
                var cspacePerGap = totalExtraSpace / (rubyCharCount - 1);

                formattedRubyText = $"<cspace={cspacePerGap}px>{rubyText}</cspace>";
                actualRubyWidth = baseWidth;
            }
            else
            {
                formattedRubyText = rubyText;
            }

            // Calculate offsets for centering (in pixels)
            var baseOffset = Mathf.Max(0f, (actualRubyWidth - baseWidth) * 0.5f);
            var rubyOffset = Mathf.Max(0f, (baseWidth - actualRubyWidth) * 0.5f);
            var totalWidth = Mathf.Max(baseWidth, actualRubyWidth);

            // After rendering ruby, cursor is at: rubyOffset + actualRubyWidth
            var finalSpace = totalWidth - rubyOffset - actualRubyWidth;
            var backSpace = -(baseOffset + baseWidth);

            // Build the markup using pixel units with optional prefix/suffix tags
            return $"<nobr><space={baseOffset}px>{baseText}<space={backSpace}px><space={rubyOffset}px><voffset={rubyVerticalOffset}em><size={rubyScalePercent}%>{rubyPrefixTag}{formattedRubyText}{rubySuffixTag}</size></voffset><space={finalSpace}px></nobr>";
        }

        /// <summary>
        /// エスケープシーケンスを解除します
        /// </summary>
        private static string UnescapeValue(string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

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
    }
}
