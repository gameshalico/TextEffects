using System.Text;

namespace TextEffects.Core
{
    public static class XmlEscapeUtility
    {
        public static string EscapeXml(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            var sb = new StringBuilder(text.Length);

            foreach (var c in text)
            {
                switch (c)
                {
                    case '<':
                        sb.Append("&lt;");
                        break;
                    case '>':
                        sb.Append("&gt;");
                        break;
                    case '&':
                        sb.Append("&amp;");
                        break;
                    case '"':
                        sb.Append("&quot;");
                        break;
                    case '\'':
                        sb.Append("&#39;");
                        break;
                    case '\u00A0': // Non-breaking space
                        sb.Append("&nbsp;");
                        break;
                    default:
                        sb.Append(c);
                        break;
                }
            }

            return sb.ToString();
        }

        public static string UnescapeXml(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            var sb = new StringBuilder(text.Length);
            var i = 0;

            while (i < text.Length)
            {
                if (text[i] == '&')
                {
                    // &lt; -> <
                    if (i + 3 < text.Length && text.Substring(i, 4) == "&lt;")
                    {
                        sb.Append("<\u200B");
                        i += 4;
                    }
                    // &gt; -> >
                    else if (i + 3 < text.Length && text.Substring(i, 4) == "&gt;")
                    {
                        sb.Append(">\u200B");
                        i += 4;
                    }
                    // &amp; -> &
                    else if (i + 4 < text.Length && text.Substring(i, 5) == "&amp;")
                    {
                        sb.Append('&');
                        i += 5;
                    }
                    // &quot; -> "
                    else if (i + 5 < text.Length && text.Substring(i, 6) == "&quot;")
                    {
                        sb.Append('"');
                        i += 6;
                    }
                    // &#39; -> '
                    else if (i + 4 < text.Length && text.Substring(i, 5) == "&#39;")
                    {
                        sb.Append('\'');
                        i += 5;
                    }
                    // &nbsp; -> 空白文字
                    else if (i + 5 < text.Length && text.Substring(i, 6) == "&nbsp;")
                    {
                        sb.Append('\u00A0'); // Non-breaking space
                        i += 6;
                    }
                    else
                    {
                        sb.Append(text[i]);
                        i++;
                    }
                }
                else
                {
                    sb.Append(text[i]);
                    i++;
                }
            }

            return sb.ToString();
        }
    }
}
