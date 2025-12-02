using NUnit.Framework;
using System.Text.RegularExpressions;
using TextEffects.Formatters;
using UnityEngine;

namespace TextEffects.Editor.Tests.Tests.Editor
{
    public class RubyTextHelperTest
    {
        [Test]
        public void RubyRegex_DoubleQuotedValue_ShouldMatch()
        {
            var regex = RubyTextHelper.RubyRegex;
            var text = @"<r=""ふりがな"">漢字</r>";
            var match = regex.Match(text);

            Assert.IsTrue(match.Success);
            Assert.AreEqual("ふりがな", GetRubyText(match));
            Assert.AreEqual("漢字", match.Groups["base"].Value);
        }

        [Test]
        public void RubyRegex_SingleQuotedValue_ShouldMatch()
        {
            var regex = RubyTextHelper.RubyRegex;
            var text = @"<r='ふりがな'>漢字</r>";
            var match = regex.Match(text);

            Assert.IsTrue(match.Success);
            Assert.AreEqual("ふりがな", GetRubyText(match));
            Assert.AreEqual("漢字", match.Groups["base"].Value);
        }

        [Test]
        public void RubyRegex_UnquotedValue_ShouldMatch()
        {
            var regex = RubyTextHelper.RubyRegex;
            var text = @"<r=ふりがな>漢字</r>";
            var match = regex.Match(text);

            Assert.IsTrue(match.Success);
            Assert.AreEqual("ふりがな", GetRubyText(match));
            Assert.AreEqual("漢字", match.Groups["base"].Value);
        }

        [Test]
        public void RubyRegex_ValueWithClosingBracket_ShouldMatch()
        {
            var regex = RubyTextHelper.RubyRegex;
            var text = @"<r=""ruby>with>brackets"">漢字</r>";
            var match = regex.Match(text);

            Assert.IsTrue(match.Success);
            Assert.AreEqual("ruby>with>brackets", GetRubyText(match));
            Assert.AreEqual("漢字", match.Groups["base"].Value);
        }

        [Test]
        public void RubyRegex_EscapedQuotes_ShouldMatch()
        {
            var regex = RubyTextHelper.RubyRegex;
            var text = @"<r=""ruby\""with\""quotes"">漢字</r>";
            var match = regex.Match(text);

            Assert.IsTrue(match.Success);
            Assert.AreEqual(@"ruby\""with\""quotes", GetRubyText(match));
            Assert.AreEqual("漢字", match.Groups["base"].Value);
        }

        [Test]
        public void RubyRegex_EscapedBackslash_ShouldMatch()
        {
            var regex = RubyTextHelper.RubyRegex;
            var text = @"<r=""ruby\\with\\backslash"">漢字</r>";
            var match = regex.Match(text);

            Assert.IsTrue(match.Success);
            Assert.AreEqual(@"ruby\\with\\backslash", GetRubyText(match));
            Assert.AreEqual("漢字", match.Groups["base"].Value);
        }

        [Test]
        public void FormatRubyText_WithClosingBracketInRuby_ShouldFormat()
        {
            var input = @"<r=""ruby>text"">漢字</r>";
            var result = RubyTextHelper.FormatRubyText(
                input,
                0.5f,
                1.0f,
                text => new Vector2(text.Length * 10f, 20f)
            );

            // 結果に「ruby>text」が含まれることを確認
            Assert.IsTrue(result.Contains("ruby>text"));
            Assert.IsTrue(result.Contains("漢字"));
        }

        [Test]
        public void FormatRubyText_WithEscapedQuotes_ShouldUnescapeAndFormat()
        {
            var input = @"<r=""ruby\""text"">漢字</r>";
            var result = RubyTextHelper.FormatRubyText(
                input,
                0.5f,
                1.0f,
                text => new Vector2(text.Length * 10f, 20f)
            );

            // エスケープが解除されて「ruby"text」になることを確認
            Assert.IsTrue(result.Contains(@"ruby""text"));
            Assert.IsTrue(result.Contains("漢字"));
        }

        private static string GetRubyText(Match match)
        {
            return match.Groups["ruby1"].Success ? match.Groups["ruby1"].Value :
                   match.Groups["ruby2"].Success ? match.Groups["ruby2"].Value :
                   match.Groups["ruby3"].Value;
        }
    }
}
