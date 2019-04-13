using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTemplates.Classes
{
    class Changelog
    {
        private enum TableSection { FeatureImprovement, BugFix, KnownIssue, Note, Other }

        private struct ChangelogEntry
        {
            public TableSection Section;
            public string Item;
        }

        /// <summary>
        /// Converts a text-based changelog to a specially formatted HTML version for display.
        /// </summary>
        /// <param name="releaseVersion">Version of release</param>
        /// <param name="changelogText">Raw text changelog</param>
        /// <returns>Changelog in HTML format</returns>
        public string GetHTMLVersion(string releaseVersion, string changelogText)
        {
            var sb = new StringBuilder();
            var changelogEntries = ParseChangelog(changelogText);

            sb.AppendLine(GetTableHeader(releaseVersion));
            sb.AppendLine("<br>");

            // TODO: Maybe hide sections with 0 entries.
            sb.AppendLine(GetTableSection(TableSection.FeatureImprovement,
                changelogEntries.Where(s => s.Section == TableSection.FeatureImprovement).Select(s => s.Item).ToList()));
            sb.AppendLine("<br>");

            sb.AppendLine(GetTableSection(TableSection.BugFix,
                changelogEntries.Where(s => s.Section == TableSection.BugFix).Select(s => s.Item).ToList()));
            sb.AppendLine("<br>");

            sb.AppendLine(GetTableSection(TableSection.KnownIssue,
                changelogEntries.Where(s => s.Section == TableSection.KnownIssue).Select(s => s.Item).ToList()));

            return sb.ToString();
        }

        /// <summary>
        /// Parses a raw text-based changelog to a category-based list.
        /// </summary>
        /// <param name="changelogText">Raw text-based changelog</param>
        /// <returns>Categorized list of changelog entries</returns>
        private List<ChangelogEntry> ParseChangelog(string changelogText)
        {
            var changelogEntries = new List<ChangelogEntry>();

            using (var reader = new StringReader(changelogText.Trim()))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    ChangelogEntry changeEntry;
                    if (line.Contains(":") == false)
                    {
                        continue;
                    }
                    var parsedLine = line.Split(new[] { ':' }, 2);
                    switch (parsedLine[0].Trim())
                    {
                        case "ADDED":
                        case "UPDATED":
                        case "IMPROVED":
                        case "CHANGED":
                            changeEntry.Section = TableSection.FeatureImprovement;
                            break;
                        case "FIXED":
                            changeEntry.Section = TableSection.BugFix;
                            break;
                        case "KNOWN ISSUE":
                            changeEntry.Section = TableSection.KnownIssue;
                            break;
                        case "NOTE":
                            changeEntry.Section = TableSection.Note;
                            break;
                        default:
                            changeEntry.Section = TableSection.Other;
                            break;
                    }
                    changeEntry.Item = parsedLine[1].Trim();
                    changelogEntries.Add(changeEntry);
                }
            }

            return changelogEntries;
        }

        /// <summary>
        /// Generates the header for the HTML changelog.
        /// </summary>
        /// <param name="releaseVersion">Version of release</param>
        /// <returns>HTML header</returns>
        private string GetTableHeader(string releaseVersion)
        {
            var sb = new StringBuilder();

            sb.AppendLine("<table width=\"442\" cellpadding=\"10\" cellspacing=\"0\">");
            sb.AppendLine("<tr>");
            sb.AppendLine($"<th bgcolor=\"#A5EAFA\" style=\"text-align: left\">Version {releaseVersion} Changelog</th>");
            sb.AppendLine("</tr>");
            sb.AppendLine("</table>");

            return sb.ToString();
        }

        /// <summary>
        /// Generates the color-coded table groups for the HTML changelog.
        /// </summary>
        /// <param name="section">Section name for the needed table group</param>
        /// <param name="items">Items associated with section</param>
        /// <returns>HTML color-coded table group</returns>
        private string GetTableSection(TableSection section, List<string> items)
        {
            string sectionName = "";
            string color = "";

            switch (section)
            {
                case TableSection.FeatureImprovement:
                    sectionName = "New Features / Improvements";
                    color = "#A5FAC0";
                    break;
                case TableSection.BugFix:
                    sectionName = "Bug Fixes";
                    color = "#FAA5A5";
                    break;
                case TableSection.KnownIssue:
                    sectionName = "Known Issues";
                    color = "#F1F58E";
                    break;
            }

            var sb = new StringBuilder();

            sb.AppendLine("<table width=\"442\" cellpadding=\"10\" cellspacing=\"0\" style=\"border: 1px dashed black;\">");
            sb.AppendLine("<tr>");
            sb.AppendLine($"<th bgcolor=\"{color}\" style=\"text-align: left\">{sectionName}</th>");
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td style=\"padding-top: 15px; padding-bottom: 0px\"><ul>");
            if (items?.Count > 0)
            {
                items.ForEach(i => sb.AppendLine($"<li>{i}</li>"));
            }
            else
            {
                sb.AppendLine("<li>None.</li>");
            }
            sb.AppendLine("</ul> </td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("</table>");

            return sb.ToString();
        }
    }
}
