using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using HtmlAgilityPack;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace WolGetLanguages
{
    public partial class MainPage : PhoneApplicationPage
    {
        //Preferences page on Wol web page.
        String stringWolLink = "http://m.wol.jw.org/en/wol/si/r1/lp-e?url=/{locale}/wol/pref/r1/lp-e";

        //The HtmlWeb class is a utility class to get the HTML over HTTP.
        HtmlWeb htmlWebLanguages = new HtmlWeb();

        //The Dictionary to store list of both url and language's name.
        Dictionary<string, string> dictionaryUrlLang = new Dictionary<string, string>();

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            //Add event handler when load completed.
            htmlWebLanguages.LoadCompleted += new EventHandler<HtmlDocumentLoadCompleted>(htmlWeb_LoadCompleted);

            // Creates an HtmlDocument object from an URL.
            htmlWebLanguages.LoadAsync(stringWolLink);
        }

        void htmlWeb_LoadCompleted(object sender, HtmlDocumentLoadCompleted e)
        {
            HtmlAgilityPack.HtmlDocument document = e.Document;

            #region XPath Parsing with open source Html Agility Pack library.
            //http://olussier.net/2010/03/30/easily-parse-html-documents-in-csharp/

            //HtmlNodeCollection nodeCollectionDataLangs = document.DocumentNode.SelectNodes("//*[@id='contentOnly']/div[2]/ul/li");
            //// If there is no node with that Id, someNode will be null
            //if (nodeCollectionDataLangs != null)
            //{
            //    // Outputs the href for external links
            //    foreach (HtmlNode link in nodeCollectionDataLangs)
            //        Debug.WriteLine(link.Attributes["data-lang"].Value);
            //}

            HtmlNodeCollection nodeCollectionHrefs = document.DocumentNode.SelectNodes("//*[@id='contentOnly']/div[2]/ul/li/a");
            // If there is no node with that Id, someNode will be null
            if (nodeCollectionHrefs != null)
            {
                // Outputs the href for external links
                foreach (HtmlNode link in nodeCollectionHrefs)
                {
                    Debug.WriteLine(link.Attributes["href"].Value + " - " + link.InnerText);

                    string stringWholeHrefUrl = link.Attributes["href"].Value;
                    //Here we call Regex.Match() function.
                    //Match match = Regex.Match(stringWholeUrl, "url=.*$", RegexOptions.IgnoreCase);
                    //http://www.codeproject.com/Articles/9099/The-30-Minute-Regex-Tutorial
                    //TODO: Improve regex on /zu/wol/pref/r1/lp-e?newlocale=zu&url=/zu/wol/pref/r1/lp-e link to get only the language variable. Fix: match.Groups[1].Value.

                    String regexPattern = @"/(.*?)/"; //@".*?wol";

                    Match match = Regex.Match(stringWholeHrefUrl, regexPattern, RegexOptions.IgnoreCase);

                    Debug.WriteLine("Result of Regex.Match(): " + Regex.Match(stringWholeHrefUrl, regexPattern).Groups[0].Value);
                    Debug.WriteLine("Result of Regex.Match(): " + Regex.Match(stringWholeHrefUrl, regexPattern).Groups[1].Value);
                    Debug.WriteLine("Result of Regex.Match(): " + Regex.Match(stringWholeHrefUrl, regexPattern).Groups[3].Value);
                    Debug.WriteLine("Count of Regex.Match(): " + Regex.Match(stringWholeHrefUrl, regexPattern).Groups.Count);

                    // Here we check the Match instance.
                    if (match.Success)
                    {
                        // Finally, we get the Group value and display it.
                        string key = match.Groups[1].Value;
                        //Debug.WriteLine(key);
                    }
                    else
                    {
                        Debug.WriteLine("Regex match failed.");
                    }

                    //Write the results in the dictionary.
                    dictionaryUrlLang.Add(match.Groups[0].Value, link.InnerText);
                }

                listBox1.ItemsSource = dictionaryUrlLang;
            }
            #endregion

            #region Basic Parsing with open source Html Agility Pack library.
            //http://olussier.net/2010/03/30/easily-parse-html-documents-in-csharp/

            //// Targets a specific node
            //HtmlNode someNode = document.GetElementbyId("contentOnly");
            //Debug.WriteLine(someNode.InnerHtml);

            //// If there is no node with that Id, someNode will be null
            //if (someNode != null)
            //{
            //    // Extracts all links within that node
            //    IEnumerable<HtmlNode> allLinks = someNode.Descendants("a");

            //    // Outputs the href for external links
            //    foreach (HtmlNode link in allLinks)
            //    {
            //        // Checks whether the link contains an HREF attribute
            //        if (link.Attributes.Contains("href"))
            //        {
            //            // Simple check: if the href begins with "http://", prints it out
            //            if (link.Attributes["href"].Value.StartsWith("http://"))
            //                Debug.WriteLine(link.Attributes["href"].Value);
            //        }
            //    }
            //}
            #endregion
        }

        private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MessageBox.Show("Selected: " + listBox1.SelectedItem.ToString());
        }
    }
}