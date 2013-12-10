using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1887.Backend.Helpers
{
    static class HoldNytHelpers
    {
        public static DateTime FormatDateTime(string dateTime)
        {
            try
            {
                //DateTime received looks like: 29/10 15:07
                int day = int.Parse(dateTime.Substring(0, 2));
                int month = int.Parse(dateTime.Substring(3, 2));
                int hour = int.Parse(dateTime.Substring(6, 2));
                int minute = int.Parse(dateTime.Substring(9, 2));

                DateTime dt = new DateTime(DateTime.Today.Year, month, day, hour, minute, DateTime.Now.Second);
                return dt;
            }
            catch
            {
                DateTime dt = new DateTime(1887, 01, 01, 00, 00, 00);
                return dt;
            }
        }

        public static int FormatID(string urlWithId)
        {
            try
            {
                if (urlWithId.Count() > 0)
                {
                    //Try to get id:
                    string urlId = urlWithId.Remove(0, 16);
                    int id = int.Parse(urlId);
                    return id;
                }
                else
                {
                    throw Exception();
                }
            }
            catch
            {
                return 1887;
            }
        }

        public static string FormatTitle(string title)
        {
            try
            {
                if(title.Count() > 0)
                {
                    if(title.Count() > 35)
                    {
                        return title.Substring(0, 30) + "...";
                    }

                    return title;
                }
                else
                {
                    throw Exception();
                }
            }
            catch
            {
                return "Error getting title";
            }
        }

        public static string FormatUrl(string urlRaw)
        {
            //To do: Follow url and get complete url!
            //HttpClient client = new HttpClient();
            //HttpResponseMessage response = await client.GetAsync("http://www.yahoo.com/");
            //response.EnsureSuccessStatusCode();
            //string responseUri = response.RequestMessage.RequestUri.ToString();
            //Console.Out.WriteLine(responseUri);

            try
            {
                if(urlRaw.Count() > 0)
                { 
                    string url = "http://www.holdnyt.dk/" + urlRaw;
                    return url;
                }
                else
                {
                    throw Exception();
                }
            }
            catch
            {
                return "http://www.ob.dk";
            }
        }

        public static string SourceFormat(string sourceRaw)
        {
            if(!string.IsNullOrEmpty(sourceRaw))
            {
                if(sourceRaw.Contains("media_logo"))
                { 
                    return sourceRaw.Replace("media_logo ", "");
                }
                if (sourceRaw.ToLower().Contains("klik og læs nyhed på "))
                {
                    return sourceRaw.ToLower().Replace("klik og læs nyhed på ", "");
                }
            }
            return "unkown";
        }

        private static Exception Exception()
        {
            throw new NotImplementedException();
        }
    }
}
