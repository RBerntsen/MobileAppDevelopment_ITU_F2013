using _1887.Backend.Helpers;
using _1887.Backend.Model;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using _1887.Backend.ExtensionMethods;

namespace _1887.Backend.WebScraper
{
    public class WebScraper
    {
        HttpClient client = new HttpClient();
        public async Task<List<NewsArticleItem>> ScrapeHoldNytNewsArticles(string webAddress)
        {
            try	
            {
                string responseBody = await client.GetStringAsync(@"http:\\" + webAddress);
                List<NewsArticleItem> filteredResult = FilterNewsItems(responseBody, 0);               
                return filteredResult;
            }  
            catch(HttpRequestException e)
            {
                Debug.WriteLine("ScrapeHoldNytNewsArticles");
                return null;
            }
        }

        private List<NewsArticleItem> FilterNewsItems(string httpResult, int noOfItemsToGet)
        {
            HtmlDocument htmlSnippet = new HtmlDocument();
            htmlSnippet.LoadHtml(httpResult);

            List<NewsArticleItem> listOfNewsToReturn = new List<NewsArticleItem>();

            List<HtmlNode> newsHolder = htmlSnippet.DocumentNode.Descendants("div").FirstOrDefault(div => div.Id.Equals("newsHolder")).Descendants("div").Where(div => div.GetAttributeValue("class", "").Contains("news_container")).ToList();

            foreach (HtmlNode node in newsHolder)//.Descendants("div").Where(div => div.GetAttributeValue("class", "").Contains("news_container")))
            {
                try
                {
                    //Harvest needed information: date, title, url, id
                    string dateRaw = node.Descendants("div").Where(div => div.GetAttributeValue("class", "").Contains("news_date")).FirstOrDefault().InnerText.ToString().Trim();
                    DateTime date = HoldNytHelpers.FormatDateTime(dateRaw);

                    string titleRaw = node.Descendants("div").Where(div => div.GetAttributeValue("class", "").Contains("news_headline")).FirstOrDefault().Descendants("a").FirstOrDefault().InnerText.ToString().Trim();
                    string title = HoldNytHelpers.FormatTitle(titleRaw);

                    string urlRaw = node.Descendants("div").Where(div => div.GetAttributeValue("class", "").Contains("news_headline")).FirstOrDefault().Descendants("a").FirstOrDefault().GetAttributeValue("href", "");
                    string url = HoldNytHelpers.FormatUrl(urlRaw);
                        
                    int id = HoldNytHelpers.FormatID(urlRaw);

                    //string sourceRaw = node.Descendants("div").Where(div => div.GetAttributeValue("class", "").Contains("news_media")).FirstOrDefault().Descendants("span").FirstOrDefault().GetAttributeValue("class", string.Empty);
                    string sourceRaw = node.Descendants("div").Where(div => div.GetAttributeValue("class", "").Contains("news_media")).FirstOrDefault().Descendants("a").FirstOrDefault().GetAttributeValue("title", string.Empty);
                    string source = HoldNytHelpers.SourceFormat(sourceRaw);

                    NewsArticleItem item = new NewsArticleItem(title, url, date, id, source);

                    listOfNewsToReturn.Add(item);
                }
                catch
                {
                    Debug.WriteLine("FilterNewsItems");
                }
            }
            if(listOfNewsToReturn.Count > noOfItemsToGet && noOfItemsToGet != 0)
            {
                return listOfNewsToReturn.Take(noOfItemsToGet).ToList();
            };
            return listOfNewsToReturn;
        }

        public async Task<List<MatchItem>> ScrapeBoldMatchSchedule(string webAddress)
        {
            try
            {
                string responseBody = await client.GetStringAsync(@"http:\\" + webAddress);
                List<MatchItem> filteredResult = FilterMatchItems(responseBody, 99);

                //foreach(NewsArticleItem item in filteredResult)
                //{
                //    string url = await followedUrl(item.Url);
                //}


                return filteredResult;
            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine("ScrapeBoldMatchSchedule");
                return null;
            }
        }

        private List<MatchItem> FilterMatchItems(string httpResult, int noOfItemsToGet, string teamToSearchFor="OB")
        {
            HtmlDocument htmlSnippet = new HtmlDocument();
            htmlSnippet.LoadHtml(httpResult);

            List<MatchItem> listOfNewsToReturn = new List<MatchItem>();

            HtmlNode newsHolder = htmlSnippet.DocumentNode.Descendants("div").FirstOrDefault(div => div.GetAttributeValue("class", "").Contains("col1")).Descendants("table").FirstOrDefault();

            foreach (HtmlNode node in newsHolder.Descendants("tr").Where(div => div.InnerHtml.Contains(teamToSearchFor)))
            {
                if (listOfNewsToReturn.Count < noOfItemsToGet)
                {
                    try
                    {
                        //Harvest needed information: date, title, url, id
                        string dateRaw = node.Descendants("td").Where(td => !string.IsNullOrEmpty(td.InnerText.ToString())).FirstOrDefault().InnerText;
                        DateTime date = BoldHelpers.FormatDateTime(dateRaw, false);

                        string titleRaw = node.Descendants("a").Where(a => a.GetAttributeValue("class", "").Contains("tekst")).Where(a => a.InnerText.Contains(teamToSearchFor)).FirstOrDefault().InnerText;
                        string homeTeam = BoldHelpers.FormatHomeTeam(titleRaw);
                        string awayTeam = BoldHelpers.FormatAwayTeam(titleRaw);

                        bool isHomeMatch = BoldHelpers.isHomeTeam(teamToSearchFor, homeTeam);

                        string matchRaw = node.Descendants("a").Where(a => a.GetAttributeValue("class", "").Contains("tekstsmall")).FirstOrDefault().InnerText;
                        //string url = HoldNytHelpers.FormatUrl(urlRaw);

                        string idRaw = node.Descendants("a").Where(a => a.GetAttributeValue("class", "").Contains("tekst")).FirstOrDefault().GetAttributeValue("href", "");

                        //string sourceRaw = node.Descendants("div").Where(div => div.GetAttributeValue("class", "").Contains("news_media")).FirstOrDefault().Descendants("span").FirstOrDefault().GetAttributeValue("class", string.Empty);
                        //string source = HoldNytHelpers.SourceFormat(sourceRaw);

                        //MatchItem item = new MatchItem(title, url, date, id, source);
                        MatchItem item = new MatchItem(homeTeam, awayTeam, date, isHomeMatch, idRaw, string.Empty);

                        listOfNewsToReturn.Add(item);
                    }
                    catch
                    {
                        //Fail silently.... :'(
                        Debug.WriteLine("FilterMatchItems");
                    }
                }
            }
            return listOfNewsToReturn;
        }

        public async Task<List<MatchItem>> ScrapeBoldSuperligaMatchSchedule(string webAddress, bool useProperNames)
        {
            try
            {
                var response = await client.GetByteArrayAsync(@"http:\\" + webAddress);
                Encoding enc = new CustomEncoder();
                string responseBody = enc.GetString(response, 0, response.Length - 1);
                List<MatchItem> filteredResult = FilterMatchItemsSuperliga(responseBody, 0, "OB", useProperNames);

                return filteredResult;
            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine("ScrapeBoldSuperligaMatchSchedule: " + e);
                return null;
            }
        }

        private List<MatchItem> FilterMatchItemsSuperliga(string httpResult, int noOfItemsToGet, string teamToSearchFor, bool useProperNames)
        {
            HtmlDocument htmlSnippet = new HtmlDocument();
            Encoding enc = new CustomEncoder();

            htmlSnippet.LoadHtml(httpResult);

            List<MatchItem> listOfNewsToReturn = new List<MatchItem>();

            List<HtmlNode> newsHolder = htmlSnippet.DocumentNode.Descendants("div").
                                        FirstOrDefault(div => div.GetAttributeValue("class", "").Contains("col2"))
                                        .Descendants()
                                        .Where(html => html.OriginalName == "div" && html.GetAttributeValue("class", "").Contains("note") ||
                                                      html.OriginalName == "table" && html.GetAttributeValue("class", "").Contains("matchprogramtable"))
                                        .ToList();
            int i = 0;
            while (i <= newsHolder.Count() - 2)
            {
                if (i % 2 == 0)
                {
                    try
                    {
                        //get node
                        HtmlNode note = newsHolder[i];
                        HtmlNode matchprogramtable = newsHolder[i + 1];

                        //Harvest needed information: date, title, url, id
                        string dateRaw = note.FirstChild.InnerText.Trim();
                        DateTime date = BoldHelpers.FormatDateTime(dateRaw.ToLower().Replace("den ", "").Replace(" i", string.Empty).Trim(), true);

                        string isSuperliga = note.Descendants("a").FirstOrDefault().InnerText.Trim();

                        string titleRaw = matchprogramtable.Descendants("td").Where(td => td.GetAttributeValue("class", "").Contains("col1")).FirstOrDefault().Descendants("a").FirstOrDefault().InnerText.Replace("Vs.", " - ").RemoveNewLineTag().RemoveTabTag();
                        string score = string.Empty;
                        if (matchprogramtable.Descendants("td").Where(td => td.GetAttributeValue("class", "").Contains("col2")).FirstOrDefault().Descendants("a").FirstOrDefault() != null)
                        {
                            score = matchprogramtable.Descendants("td").Where(td => td.GetAttributeValue("class", "").Contains("col2")).FirstOrDefault().Descendants("a").FirstOrDefault().InnerText.RemoveNewLineTag().RemoveTabTag();
                        }
                        string lol = score;
                        string homeTeam = BoldHelpers.FormatHomeTeam(titleRaw);
                        string awayTeam = BoldHelpers.FormatAwayTeam(titleRaw);
                        homeTeam = BoldHelpers.FormatClubName(homeTeam, useProperNames);
                        awayTeam = BoldHelpers.FormatClubName(awayTeam, useProperNames);

                        bool isHomeMatch = BoldHelpers.isHomeTeam(teamToSearchFor, homeTeam);

                        //string urlRaw = matchprogramtable.Descendants("a").FirstOrDefault().GetAttributeValue("href", "123456");
                        //string url = HoldNytHelpers.FormatUrl(urlRaw);

                        string idRaw = matchprogramtable.Descendants("a").FirstOrDefault().GetAttributeValue("href", "123456");

                        ////string sourceRaw = node.Descendants("div").Where(div => div.GetAttributeValue("class", "").Contains("news_media")).FirstOrDefault().Descendants("span").FirstOrDefault().GetAttributeValue("class", string.Empty);
                        ////string source = HoldNytHelpers.SourceFormat(sourceRaw);

                        ////MatchItem item = new MatchItem(title, url, date, id, source);
                        MatchItem item = new MatchItem(homeTeam, awayTeam, date, isHomeMatch, idRaw, score);

                        listOfNewsToReturn.Add(item);
                    }
                    catch(Exception e)
                    {
                        Debug.WriteLine("FilterMatchItemsSuperliga: " + e);
                    }
                }
                
                i = i + 2;
            }
            return listOfNewsToReturn;
        }



        public async Task<List<LeagueTableItem>> ScrapeBoldLeagueTable(string webAddress, bool useProperNames)
        {
            try
            {
                //string responseBody = await client.GetStringAsync(@"http:\\" + webAddress);
                var response = await client.GetByteArrayAsync(@"http:\\" + webAddress);
                Encoding enc = new CustomEncoder();
                string responseBody = enc.GetString(response, 0, response.Length - 1);

                List<LeagueTableItem> filteredResult = FilterLeagueTableItems(responseBody, "OB", useProperNames);

                return filteredResult;
            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine("ScrapeBoldLeagueTable");
                return null;
            }
        }

        private List<LeagueTableItem> FilterLeagueTableItems(string httpResult, string teamToSearchFor, bool useProperNames)
        {
            HtmlDocument htmlSnippet = new HtmlDocument();
            //htmlSnippet.Load(httpResult.EndRead());
            htmlSnippet.LoadHtml(httpResult);

            List<LeagueTableItem> leagueTableToReturn = new List<LeagueTableItem>();

            HtmlNode newsHolder = htmlSnippet.DocumentNode.Descendants("table").Where(table => table.GetAttributeValue("class", "").Contains("ligatable")).FirstOrDefault();

            foreach (HtmlNode node in newsHolder.Descendants("tr").Where(tr => tr.Descendants("td").First().GetAttributeValue("class", "").Contains("rank")))
            {
                    try
                    {
                        //Harvest needed information: date, title, url, id
                        string rankRaw = node.Descendants("td").Where(td => td.GetAttributeValue("class", "").Contains("rank")).FirstOrDefault().InnerText;
                        int rank = int.Parse(rankRaw);
                        
                        string clubRaw = node.Descendants("td").Where(td => td.GetAttributeValue("class", "").Contains("club")).FirstOrDefault().Descendants("a").FirstOrDefault().InnerText;

                        string club = BoldHelpers.FormatClubName(clubRaw, useProperNames);
                        
                        string PlayedRaw = node.Descendants("td").Where(td => td.GetAttributeValue("class", "").Contains("played")).FirstOrDefault().InnerText;
                        int playedMatches = int.Parse(PlayedRaw);

                        string wonRaw = node.Descendants("td").Where(td => td.GetAttributeValue("class", "").Contains("match")).ElementAtOrDefault(0).InnerText;
                        int wonMatches = int.Parse(wonRaw);
                        
                        string drawRaw = node.Descendants("td").Where(td => td.GetAttributeValue("class", "").Contains("match")).ElementAtOrDefault(1).InnerText;
                        int drawMatches = int.Parse(drawRaw);
                        
                        string lostRaw = node.Descendants("td").Where(td => td.GetAttributeValue("class", "").Contains("match")).ElementAtOrDefault(2).InnerText;
                        int lostMatches = int.Parse(lostRaw);
                        
                        string goalsRaw = node.Descendants("td").Where(td => td.GetAttributeValue("class", "").Contains("goals")).FirstOrDefault().InnerText;
                        int goalsScored = BoldHelpers.FormatGoalsScored(goalsRaw);
                        int goalsConceded = BoldHelpers.FormatGoalsConceded(goalsRaw);
                        
                        string pointsRaw = node.Descendants("td").Where(td => td.GetAttributeValue("class", "").Contains("points")).FirstOrDefault().InnerText;
                        int points = int.Parse(pointsRaw);

                        LeagueTableItem lti = new LeagueTableItem(club, rank, playedMatches, wonMatches, drawMatches, lostMatches, goalsScored, goalsConceded, points);

                        leagueTableToReturn.Add(lti);

                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("FilterLeagueTableItems");
                        //Fail silently.... :'(
                    }
            }
            return leagueTableToReturn;
        }



        public async Task<string> followedUrl(string webAddress)
        {
            HttpResponseMessage response = await client.GetAsync(webAddress);
            response.EnsureSuccessStatusCode();
            string responseUri = response.RequestMessage.RequestUri.ToString();
            return responseUri;
        }
    }
}
