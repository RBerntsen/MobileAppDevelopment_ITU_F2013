using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1887.App.ViewModels
{
    public class NewsArticleViewItem : INotifyPropertyChanged
    {
        string title;
        string url;
        DateTime date;
        int id;

        public DateTime Date
        {
            get { return date; }
            set { date = value; }
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

        public NewsArticleViewItem(string title, string url, DateTime date, int id)
        {
            this.title = title;
            this.url = url;
            this.date = date;
            this.id = id;
        }

        public NewsArticleViewItem()
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

