using NUnit.Framework;
using TextEffects.Formatters;

namespace TextEffects.Editor.Tests
{
    public class XmlUnescapeHelperTest
    {
        #region EscapeXml Tests

        [Test]
        public void EscapeLessThan()
        {
            var text = "<test>";
            var expected = "&lt;test&gt;";
            var result = XmlEscapeUtility.EscapeXml(text);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void EscapeAmpersand()
        {
            var text = "Test & Test";
            var expected = "Test &amp; Test";
            var result = XmlEscapeUtility.EscapeXml(text);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void EscapeQuotes()
        {
            var text = "\"Hello\" and 'World'";
            var expected = "&quot;Hello&quot; and &#39;World&#39;";
            var result = XmlEscapeUtility.EscapeXml(text);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void EscapeNonBreakingSpace()
        {
            var text = "Test\u00A0Space";
            var expected = "Test&nbsp;Space";
            var result = XmlEscapeUtility.EscapeXml(text);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void EscapeAllSpecialCharacters()
        {
            var text = "<>&\"'\u00A0";
            var expected = "&lt;&gt;&amp;&quot;&#39;&nbsp;";
            var result = XmlEscapeUtility.EscapeXml(text);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void EscapeWithNormalText()
        {
            var text = "Normal text with <special> characters";
            var expected = "Normal text with &lt;special&gt; characters";
            var result = XmlEscapeUtility.EscapeXml(text);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void EscapeEmptyString()
        {
            var text = "";
            var expected = "";
            var result = XmlEscapeUtility.EscapeXml(text);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void EscapeNull()
        {
            string text = null;
            var result = XmlEscapeUtility.EscapeXml(text);
            Assert.IsNull(result);
        }

        [Test]
        public void EscapeAndUnescapeRoundTrip()
        {
            var text = "Test <tag attr=\"value\"> & 'quote' \u00A0";
            var escaped = XmlEscapeUtility.EscapeXml(text);
            var unescaped = XmlEscapeUtility.UnescapeXml(escaped);
            Assert.AreEqual(text, unescaped);
        }

        #endregion

        #region UnescapeXml Tests

        [Test]
        public void UnescapeLessThan()
        {
            var text = "&lt;test&gt;";
            var expected = "<test>";
            var result = XmlEscapeUtility.UnescapeXml(text);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void UnescapeAmpersand()
        {
            var text = "Test &amp; Test";
            var expected = "Test & Test";
            var result = XmlEscapeUtility.UnescapeXml(text);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void UnescapeQuotes()
        {
            var text = "&quot;Hello&quot; and &#39;World&#39;";
            var expected = "\"Hello\" and 'World'";
            var result = XmlEscapeUtility.UnescapeXml(text);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void UnescapeNonBreakingSpace()
        {
            var text = "Test&nbsp;Space";
            var expected = "Test\u00A0Space";
            var result = XmlEscapeUtility.UnescapeXml(text);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void UnescapeMultiple()
        {
            var text = "&lt;tag attr=&quot;value&quot;&gt;Content&lt;/tag&gt;";
            var expected = "<tag attr=\"value\">Content</tag>";
            var result = XmlEscapeUtility.UnescapeXml(text);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void UnescapeAllEntities()
        {
            var text = "&lt;&gt;&amp;&quot;&#39;&nbsp;";
            var expected = "<>&\"'\u00A0";
            var result = XmlEscapeUtility.UnescapeXml(text);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void UnescapeWithNormalText()
        {
            var text = "Normal text with &lt;escaped&gt; content";
            var expected = "Normal text with <escaped> content";
            var result = XmlEscapeUtility.UnescapeXml(text);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void UnescapeEmptyString()
        {
            var text = "";
            var expected = "";
            var result = XmlEscapeUtility.UnescapeXml(text);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void UnescapeNull()
        {
            string text = null;
            var result = XmlEscapeUtility.UnescapeXml(text);
            Assert.IsNull(result);
        }

        [Test]
        public void UnescapeInvalidEntity()
        {
            var text = "Test &invalid; entity";
            var expected = "Test &invalid; entity";
            var result = XmlEscapeUtility.UnescapeXml(text);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void UnescapePartialEntity()
        {
            var text = "Test &lt";
            var expected = "Test &lt";
            var result = XmlEscapeUtility.UnescapeXml(text);
            Assert.AreEqual(expected, result);
        }

        #endregion
    }
}
