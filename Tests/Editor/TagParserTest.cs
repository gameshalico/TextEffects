using System.Collections.Generic;
using NUnit.Framework;
using TextEffects.Core;
using TextEffects.Data;

namespace TextEffects.Editor.Tests.Tests.Editor
{
    public class TagParserTest
    {
        [Test]
        public void EmptyTagTest()
        {
            var text = "012345<test-empty attr=0>6789";

            var expectedText = "0123456789";

            var expectedTags = new[]
            {
                TagInfo.Create(
                    "test-empty",
                    new Dictionary<string, string> { ["attr"] = "0" },
                    6, 10, true
                )
            };

            TestParse(text, expectedText, expectedTags);
        }

        [Test]
        public void ContainerTagTest()
        {
            var text = "012345<test>67</test>89";

            var expectedText = "0123456789";

            var expectedTags = new[]
            {
                TagInfo.Create("test", new Dictionary<string, string>(), 6, 8, false)
            };

            TestParse(text, expectedText, expectedTags);
        }

        [Test]
        public void BrCountTest()
        {
            var text = "<test1 attr=red>Red</test1><br><test2=20>Size 20</test2>";
            var expectedText = "Red<br>Size 20";
            var expectedTags = new[]
            {
                TagInfo.Create("test1", new Dictionary<string, string> { ["attr"] = "red" }, 0, 3, false),
                TagInfo.Create("test2", new Dictionary<string, string> { [""] = "20" }, 4, 11, false)
            };

            TestParse(text, expectedText, expectedTags);
        }

        [Test]
        public void DoubleQuotedAttributeTest()
        {
            var text = "<test attr=\"value with spaces\">Content</test>";
            var expectedText = "Content";
            var expectedTags = new[]
            {
                TagInfo.Create("test", new Dictionary<string, string> { ["attr"] = "value with spaces" }, 0, 7, false)
            };

            TestParse(text, expectedText, expectedTags);
        }

        [Test]
        public void SingleQuotedAttributeTest()
        {
            var text = "<test attr='value with spaces'>Content</test>";
            var expectedText = "Content";
            var expectedTags = new[]
            {
                TagInfo.Create("test", new Dictionary<string, string> { ["attr"] = "value with spaces" }, 0, 7, false)
            };

            TestParse(text, expectedText, expectedTags);
        }

        [Test]
        public void AttributeWithClosingBracketTest()
        {
            var text = "<test attr=\"value>with>brackets\">Content</test>";
            var expectedText = "Content";
            var expectedTags = new[]
            {
                TagInfo.Create("test", new Dictionary<string, string> { ["attr"] = "value>with>brackets" }, 0, 7, false)
            };

            TestParse(text, expectedText, expectedTags);
        }

        [Test]
        public void EscapedQuotesTest()
        {
            var text = "<test attr=\"value\\\"with\\\"quotes\">Content</test>";
            var expectedText = "Content";
            var expectedTags = new[]
            {
                TagInfo.Create("test", new Dictionary<string, string> { ["attr"] = "value\"with\"quotes" }, 0, 7, false)
            };

            TestParse(text, expectedText, expectedTags);
        }

        [Test]
        public void EscapedBackslashTest()
        {
            var text = "<test attr=\"value\\\\with\\\\backslash\">Content</test>";
            var expectedText = "Content";
            var expectedTags = new[]
            {
                TagInfo.Create("test", new Dictionary<string, string> { ["attr"] = "value\\with\\backslash" }, 0, 7, false)
            };

            TestParse(text, expectedText, expectedTags);
        }

        [Test]
        public void BackwardCompatibilityTest()
        {
            // 従来の引用符なし形式も引き続き動作することを確認
            var text = "<test attr=simplevalue>Content</test>";
            var expectedText = "Content";
            var expectedTags = new[]
            {
                TagInfo.Create("test", new Dictionary<string, string> { ["attr"] = "simplevalue" }, 0, 7, false)
            };

            TestParse(text, expectedText, expectedTags);
        }

        [Test]
        public void XmlUnescapeTest()
        {
            var text = "012&lt;test&gt;345&amp;678";
            var expectedText = "012<\u200Btest>\u200B345&678";

            var result = TagParser.Parse(text, unescapeXml: true);

            Assert.AreEqual(expectedText, result.Text);
        }

        [Test]
        public void XmlUnescapeWithTagsTest()
        {
            var text = "&lt;bold&gt;<test>Hello&amp;World</test>&lt;/bold&gt;";
            var expectedText = "<\u200Bbold>\u200BHello&World<\u200B/bold>\u200B";

            var expectedTags = new[]
            {
                TagInfo.Create("test", new Dictionary<string, string>(), 8, 19, false)
            };

            var result = TagParser.Parse(text, unescapeXml: true);

            Assert.AreEqual(expectedText, result.Text);
            Assert.AreEqual(expectedTags.Length, result.Tags.Length);
            Assert.AreEqual(expectedTags[0].TagName, result.Tags[0].TagName);
            Assert.AreEqual(expectedTags[0].StartIndex, result.Tags[0].StartIndex);
            Assert.AreEqual(expectedTags[0].EndIndex, result.Tags[0].EndIndex);
        }

        [Test]
        public void XmlUnescapeDisabledTest()
        {
            var text = "012&lt;test&gt;345&amp;678";
            var expectedText = "012&lt;test&gt;345&amp;678";

            var result = TagParser.Parse(text, unescapeXml: false);

            Assert.AreEqual(expectedText, result.Text);
        }

        [Test]
        public void XmlUnescapeAllEntitiesTest()
        {
            var text = "&lt;&gt;&amp;&quot;&#39;&nbsp;";
            var expectedText = "<\u200B>\u200B&\"'\u00A0";

            var result = TagParser.Parse(text, unescapeXml: true);

            Assert.AreEqual(expectedText, result.Text);
        }

        private static void TestParse(string text, string expectedText, TagInfo[] expectedTags)
        {
            var result = TagParser.Parse(text);

            Assert.AreEqual(expectedText, result.Text);
            Assert.AreEqual(expectedTags, result.Tags);
        }

        [Test]
        public void GetPlainText_RemovesAllTags()
        {
            var text = "Hello<color=red>World</color>";
            var expected = "HelloWorld";

            var result = TagParser.GetPlainText(text);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetPlainText_RemovesZeroWidthSpace()
        {
            var text = "Hello\u200BWorld";
            var expected = "HelloWorld";

            var result = TagParser.GetPlainText(text);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetPlainText_RemovesTMPAndCustomTags()
        {
            var text = "<custom>Hello<color=red>World</color></custom>";
            var expected = "HelloWorld";

            var result = TagParser.GetPlainText(text);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetPlainText_HandlesQuotedAttributes()
        {
            var text = "<tag attr=\"value>with>brackets\">Content</tag>";
            var expected = "Content";

            var result = TagParser.GetPlainText(text);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetPlainText_RemovesZeroWidthSpaceWithTags()
        {
            var text = "<\u200BHello>\u200B<color=red>World\u200B</color>";
            var expected = "<Hello>World";

            var result = TagParser.GetPlainText(text);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void RemoveCustomTags_KeepsTMPTags()
        {
            var text = "Hello<color=red>World</color>";
            var expected = "Hello<color=red>World</color>";

            var result = TagParser.RemoveCustomTags(text);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void RemoveCustomTags_RemovesCustomTags()
        {
            var text = "<custom>Hello</custom><color=red>World</color>";
            var expected = "Hello<color=red>World</color>";

            var result = TagParser.RemoveCustomTags(text);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void RemoveCustomTags_HandlesNestedTags()
        {
            var text = "<custom><color=red>Hello</color></custom>World";
            var expected = "<color=red>Hello</color>World";

            var result = TagParser.RemoveCustomTags(text);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void RemoveCustomTags_HandlesQuotedAttributes()
        {
            var text = "<custom attr=\"value>with>brackets\">Content</custom><size=20>Text</size>";
            var expected = "Content<size=20>Text</size>";

            var result = TagParser.RemoveCustomTags(text);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void RemoveCustomTags_PreservesAllTMPTags()
        {
            // <e>はカスタムタグなので除去される
            var text = "<custom>Test</custom><b>Bold</b><i>Italic</i><size=20>Size</size><color=#FF0000>Color</color>";
            var expected = "Test<b>Bold</b><i>Italic</i><size=20>Size</size><color=#FF0000>Color</color>";

            var result = TagParser.RemoveCustomTags(text);

            Assert.AreEqual(expected, result);
        }
    }
}