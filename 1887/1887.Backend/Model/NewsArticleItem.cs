using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1887.Backend.Model
{
    public class NewsArticleItem : INotifyPropertyChanged
    {
        string title;
        string url;
        DateTime date;
        int id;
        string source;

        public string Source
        {
            get { return source; }
            set { source = value; }
        }

        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        public string DateTimeString
        {
            get { return date.ToString("dd/MM") + "-1887"; }
        }

        public string SourceDateString
        {
            get { return "Source: " + Source + " - " + DateTimeString; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        public string InternalBrowserUrl
        {
            get 
            {
                if(!string.IsNullOrEmpty(this.url))
                { 
                    return "/Browser.xaml?urlToVisit="+url;
                }
                return null;
            }
        }

        public NewsArticleItem(string title, string url, DateTime date, int id, string source)
        {
            this.title = title;
            this.url = url;
            this.date = date;
            this.id = id;
            this.source = source;
        }

        public NewsArticleItem()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
