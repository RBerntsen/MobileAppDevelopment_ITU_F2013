using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1887.Backend.Helpers
{
    static class BoldHelpers
    {
        public static DateTime FormatDateTime(string dateTime, bool dateWithYear)
        {
            try
            {
                //DateTime String received looks like: "29/10&nbsp;16:00"
                //DateTime String received could also look like: "23/11-2013 kl. 17:00"
                dateTime = dateTime.Replace("&nbsp;", " ").Trim().Replace(" kl. ", " ");
                dateTime = dateTime.Replace(" i", string.Empty).Trim();

                int year = int.Parse(DateTime.Today.Year.ToString());

                if (dateWithYear)
                {
                    year = int.Parse(dateTime.Substring(6, 4));
                    dateTime = dateTime.Remove(5, 5);
                }

                
                int day = int.Parse(dateTime.Substring(0, 2));
                int month = int.Parse(dateTime.Substring(3, 2));
                int hour = int.Parse(dateTime.Substring(6, 2));
                int minute = int.Parse(dateTime.Substring(9, 2));

                DateTime dt = new DateTime(year, month, day, hour, minute, DateTime.Now.Second);
                return dt;
            }
            catch
            {
                DateTime dt = new DateTime(1887, 01, 01, 00, 00, 00);
                return dt;
            }
        }

        public static string FormatHomeTeam(string matchString)
        {
            //eks Fredericia Reserver - OB Reserver
            return matchString.Substring(0, matchString.IndexOf(" - ")).Replace(" - ", string.Empty).Trim();
        }

        public static string FormatAwayTeam(string matchString)
        {
            //eks Fredericia Reserver - OB Reserver
            return matchString.Substring(matchString.IndexOf(" - ")).Replace(" - ", string.Empty).Trim();
        }

        public static bool isHomeTeam(string teamToMatch, string homeTeam)
        {
            return homeTeam.Contains(teamToMatch);
        }

        public static int FormatGoalsScored(string scored)
        {
            return int.Parse(scored.Substring(0, scored.IndexOf("-")).Replace("-", string.Empty).Trim());
        }

        public static int FormatGoalsConceded(string conceded)
        {
            return int.Parse(conceded.Substring(conceded.IndexOf("-")).Replace("-", string.Empty).Trim());
        }

        public static string FormatClubName(string club, bool useProperNames)
        {
            if(!useProperNames)
            { 
                if(club.ToLower().Contains("ndby"))
                {
                    return "Vestegnen IF";
                }
                if(club.ToLower().Contains("nordsj"))
                {
                    return "FC Rødvin";
                }
                if(club.ToLower().Contains("benhavn"))
                {
                    return "FC Caffe Latte";
                }
                if (club.ToLower().Contains("nderjyske"))
                {
                    return "NordtyskE";
                }
                if (club.ToLower().Contains("vestsj"))
                {
                    return "FC Harboe";
                }
                if(club.ToLower().Contains("ob"))
                {
                    return "ODENSE";
                }
                if(club.ToLower().Contains("midtjylland"))
                {
                    return "FC Møgjylland";
                }
                if(club.ToLower().Contains("randers"))
                {
                    return "FC Hestene";
                }
                else return club;
            }
            else if (club.ToLower().Contains("ob"))
            {
                return "ODENSE";
            }
            return club;
        }
    }
}
