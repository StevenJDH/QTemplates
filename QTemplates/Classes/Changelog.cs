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
        private enum TableSection { NewFeaturesImprovements, BugFixes, KnownIssuesLimitations, Notes, Other }

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

            sb.AppendLine("<!DOCTYPE html><html lang=\"en\"><head><meta charset=\"utf-8\">");
            sb.AppendLine("<title>Changelog</title>");
            sb.AppendLine("<style>");
            sb.AppendLine("table { width: 442px; border-collapse: collapse; }");
            sb.AppendLine("table tr th { text-align: left; padding: 10px; }");
            sb.AppendLine("table tr td { padding: 0px; }");
            sb.AppendLine("table tr td ul { padding-right: 15px; margin-top: 15px; }");
            sb.AppendLine("table tr td p { padding: 10px; }");
            sb.AppendLine(".dashed-border { border: 1px dashed black; }");
            sb.AppendLine(".blue-title { background-color: #A5EAFA; }");
            sb.AppendLine(".green-title { background-color: #A5FAC0; }");
            sb.AppendLine(".red-title { background-color: #FAA5A5; }");
            sb.AppendLine(".yellow-title { background-color: #F1F58E; }");
            sb.AppendLine(".gray-title { background-color: #DCE1E5; }");
            sb.AppendLine("</style>");
            sb.AppendLine("</head><body>");

            sb.AppendLine(GetTableHeader(releaseVersion));

            var featuresList = changelogEntries
                .Where(s => s.Section == TableSection.NewFeaturesImprovements)
                .Select(s => s.Item)
                .ToList();
            if (featuresList.Count > 0)
            {
                sb.AppendLine("<br>");
                sb.AppendLine(GetTableSection(TableSection.NewFeaturesImprovements, featuresList));
            }

            var bugsList = changelogEntries
                .Where(s => s.Section == TableSection.BugFixes)
                .Select(s => s.Item)
                .ToList();
            if (bugsList.Count > 0)
            {
                sb.AppendLine("<br>");
                sb.AppendLine(GetTableSection(TableSection.BugFixes, bugsList));
            }

            var issuesList = changelogEntries
                .Where(s => s.Section == TableSection.KnownIssuesLimitations)
                .Select(s => s.Item)
                .ToList();
            if (issuesList.Count > 0)
            {
                sb.AppendLine("<br>");
                sb.AppendLine(GetTableSection(TableSection.KnownIssuesLimitations, issuesList));
            }

            var notesList = changelogEntries
                .Where(s => s.Section == TableSection.Notes)
                .Select(s => s.Item)
                .ToList();
            if (notesList.Count > 0)
            { 
                sb.AppendLine("<br>");
                sb.AppendLine(GetNotesSection(notesList));
            }

            sb.AppendLine("</body></html>");

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
            TableSection changelogGroup = TableSection.Other;

            using (var reader = new StringReader(changelogText.Trim()))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string parsedLine = line.Replace("*", "").Trim(); // Removes markdown.

                    // Controls group associations for changelog entries.
                    if (parsedLine.EndsWith(":"))
                    {
                        switch (parsedLine.ToLower())
                        {
                            case "new features / improvements:":
                                changelogGroup = TableSection.NewFeaturesImprovements;
                                break;
                            case "bug fixes:":
                                changelogGroup = TableSection.BugFixes;
                                break;
                            case "known issues / limitations:":
                                changelogGroup = TableSection.KnownIssuesLimitations;
                                break;
                            case "notes:":
                                changelogGroup = TableSection.Notes;
                                break;
                            default:
                                changelogGroup = TableSection.Other;
                                break;
                        }
                        continue; // Start adding entries for group with below.
                    }

                    // Adds changelog entries into current group.
                    if (String.IsNullOrWhiteSpace(parsedLine) == false)
                    {
                        ChangelogEntry changeEntry;
                        changeEntry.Section = changelogGroup;
                        changeEntry.Item = parsedLine;
                        changelogEntries.Add(changeEntry);
                    }
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

            sb.AppendLine("<table>");
            sb.AppendLine($"<tr><th class=\"blue-title\">Version {releaseVersion} Changelog</th></tr>");
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
            string classColor = "";

            switch (section)
            {
                case TableSection.NewFeaturesImprovements:
                    sectionName = "New Features / Improvements";
                    classColor = "green-title";
                    break;
                case TableSection.BugFixes:
                    sectionName = "Bug Fixes";
                    classColor = "red-title";
                    break;
                case TableSection.KnownIssuesLimitations:
                    sectionName = "Known Issues / Limitations";
                    classColor = "yellow-title";
                    break;
            }

            var sb = new StringBuilder();

            sb.AppendLine("<table class=\"dashed-border\">");
            sb.AppendLine($"<tr><th class=\"{classColor}\">{sectionName}</th></tr>");
            sb.AppendLine("<tr><td><ul>");
            if (items?.Count > 0)
            {
                items.ForEach(i => sb.AppendLine($"<li>{i}</li>"));
            }
            else
            {
                sb.AppendLine("<li>None.</li>");
            }
            sb.AppendLine("</ul></td></tr>");
            sb.AppendLine("</table>");

            return sb.ToString();
        }

        /// <summary>
        /// Generates the notes group for the HTML changelog.
        /// </summary>
        /// <param name="items">Items associated with notes section</param>
        /// <returns>HTML notes group</returns>
        private string GetNotesSection(List<string> items)
        {
            var sb = new StringBuilder();

            sb.AppendLine("<table class=\"dashed-border\">");
            sb.AppendLine("<tr><th class=\"gray-title\">Notes</th></tr>");
            sb.AppendLine("<tr><td>");
            if (items?.Count > 0)
            {
                items.ForEach(i => sb.AppendLine($"<p>{i}</p>"));
            }
            else
            {
                sb.AppendLine("<p>None.</p>");
            }
            sb.AppendLine("</td></tr>");
            sb.AppendLine("</table>");

            return sb.ToString();
        }

        /// <summary>
        /// Generates a loading view to be used as a placeholder while the changelog generates and loads.
        /// </summary>
        /// <returns>Loading HTML view</returns>
        public string GetLoadingHTML()
        {
            var sb = new StringBuilder();

            sb.AppendLine("<!DOCTYPE html><html lang=\"en\"><head><meta charset=\"utf-8\">");
            sb.AppendLine("<title>Loading</title>");
            sb.AppendLine("<style>");
            sb.AppendLine("body { font-size: 36px; text-align: center;	}");
            sb.AppendLine(".medium-text { font-size: 48px; }");
            sb.AppendLine(".large-text { font-size: 58px; }");
            sb.AppendLine("</style>");
            sb.AppendLine("</head><body>");
            sb.AppendLine("<br>");
            sb.AppendLine("<p>Lo<span class=\"medium-text\">a</span>ding.<span class=\"large-text\">..</span></p>");
            sb.AppendLine("</body></html>");

            return sb.ToString();
        }
    }
}
 