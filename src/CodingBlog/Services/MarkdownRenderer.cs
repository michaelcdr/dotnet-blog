using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace CodingBlog.Services;

public static partial class MarkdownRenderer
{
    public static string ToHtml(string? markdown)
    {
        if (string.IsNullOrWhiteSpace(markdown))
            return string.Empty;

        var normalized = markdown.Replace("\r\n", "\n").Trim();
        var blocks = normalized.Split("\n\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var builder = new StringBuilder();

        foreach (var block in blocks)
        {
            var lines = block.Split('\n', StringSplitOptions.TrimEntries);

            if (lines.All(line => line.StartsWith("- ") || line.StartsWith("* ")))
            {
                builder.Append("<ul>");
                foreach (var line in lines)
                {
                    builder.Append("<li>");
                    builder.Append(FormatInline(line[2..]));
                    builder.Append("</li>");
                }

                builder.Append("</ul>");
                continue;
            }

            var headingMatch = HeadingRegex().Match(lines[0]);
            if (headingMatch.Success)
            {
                var level = Math.Min(6, headingMatch.Groups["hashes"].Value.Length);
                builder.Append($"<h{level}>");
                builder.Append(FormatInline(headingMatch.Groups["text"].Value));
                builder.Append($"</h{level}>");
                continue;
            }

            builder.Append("<p>");
            builder.Append(string.Join("<br />", lines.Select(FormatInline)));
            builder.Append("</p>");
        }

        return builder.ToString();
    }

    private static string FormatInline(string text)
    {
        var html = WebUtility.HtmlEncode(text);
        html = Regex.Replace(html, @"\*\*(.+?)\*\*", "<strong>$1</strong>");
        html = Regex.Replace(html, @"\*(.+?)\*", "<em>$1</em>");
        html = Regex.Replace(html, @"`(.+?)`", "<code>$1</code>");
        html = Regex.Replace(html, @"\[(.+?)\]\((https?://.+?)\)", "<a href=\"$2\" target=\"_blank\" rel=\"noopener noreferrer\">$1</a>");
        return html;
    }

    [GeneratedRegex("^(?<hashes>#{1,6})\\s+(?<text>.+)$")]
    private static partial Regex HeadingRegex();
}
