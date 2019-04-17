using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTemplates.Classes
{
    class Changelog // TODO: Update old HTML to HTML5 and use full HTML markup.
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

            //TODO: Decide whether to make this one entry or not.
            var notesList = changelogEntries
                .Where(s => s.Section == TableSection.Notes)
                .Select(s => s.Item)
                .ToList();
            if (notesList.Count > 0)
            { 
                sb.AppendLine("<br>");
                sb.AppendLine(GetNotesSection(notesList));
            }

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
                case TableSection.NewFeaturesImprovements:
                    sectionName = "New Features / Improvements";
                    color = "#A5FAC0";
                    break;
                case TableSection.BugFixes:
                    sectionName = "Bug Fixes";
                    color = "#FAA5A5";
                    break;
                case TableSection.KnownIssuesLimitations:
                    sectionName = "Known Issues / Limitations";
                    color = "#F1F58E";
                    break;
            }

            var sb = new StringBuilder();

            sb.AppendLine("<table width=\"442\" cellpadding=\"10\" cellspacing=\"0\" style=\"border: 1px dashed black;\">");
            sb.AppendLine("<tr>");
            sb.AppendLine($"<th bgcolor=\"{color}\" style=\"text-align: left;\">{sectionName}</th>");
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td style=\"padding-top: 15px; padding-bottom: 0px;\"><ul>");
            if (items?.Count > 0)
            {
                items.ForEach(i => sb.AppendLine($"<li>{i}</li>"));
            }
            else
            {
                sb.AppendLine("<li>None.</li>");
            }
            sb.AppendLine("</ul></td>");
            sb.AppendLine("</tr>");
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

            sb.AppendLine("<table width=\"442\" cellpadding=\"10\" cellspacing=\"0\" style=\"border: 1px dashed black;\">");
            sb.AppendLine("<tr>");
            sb.AppendLine($"<th bgcolor=\"#DCE1E5\" style=\"text-align: left;\">Notes</th>");
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td style=\"padding-top: 15px; padding-bottom: 15px;\">");
            if (items?.Count > 0)
            {
                items.ForEach(i => sb.AppendLine($"<p>{i}</p>"));
            }
            else
            {
                sb.AppendLine("<p>None.</p>");
            }
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("</table>");

            return sb.ToString();
        }
    }
}
