using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using _1887.App.Resources;
using System.Diagnostics;
using System.Threading.Tasks;
using _1887.Backend.Model;
using System.Collections.Generic;
using System.Linq;
using _1887.Backend.Settings;
using _1887.Backend.Constants;

namespace _1887.App.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            this.NewsItems = new ObservableCollection<NewsArticleItem>();
            this.NewsItemsLongList = new ObservableCollection<NewsArticleItem>();
            this.LeagueTableItems = new ObservableCollection<LeagueTableItem>();
            this.MatchItemsLongList = new ObservableCollection<MatchItem>();
            this.MatchItems = new ObservableCollection<MatchItem>();
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<NewsArticleItem> NewsItems { get; private set; }
        public ObservableCollection<NewsArticleItem> NewsItemsLongList { get; private set; }
        public ObservableCollection<LeagueTableItem> LeagueTableItems { get; private set; }
        public ObservableCollection<MatchItem> MatchItemsLongList { get; private set; }
        public ObservableCollection<MatchItem> MatchItems { get; private set; }

        static readonly IsolatedStorageProperty<bool> settingUseProperNames = new IsolatedStorageProperty<bool>(LocalSettings.settingUseProperNames, false);
        static readonly IsolatedStorageProperty<int> settingNewsToShowList = new IsolatedStorageProperty<int>(LocalSettings.settingNewsToShowList, 3);


        private bool _sampleProperty = false;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding
        /// </summary>
        /// <returns></returns>
        public bool IsLoadingData
        {
            get
            {
                return _sampleProperty;
            }
            set
            {
                if (value != _sampleProperty)
                {
                    _sampleProperty = value;
                    NotifyPropertyChanged("isLoadingData");
                }
            }
        }

        /// <summary>
        /// Sample property that returns a localized string
        /// </summary>
        public string LocalizedSampleProperty
        {
            get
            {
                return AppResources.SampleProperty;
            }
        }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets data from webscraper and binds to ObservableCollections
        /// </summary>
        public async void LoadData()
        {
            this.IsLoadingData = true;

            //Scrape news from bold.dk
            _1887.Backend.WebScraper.WebScraper wsMatchItems = new Backend.WebScraper.WebScraper();
            Task<List<MatchItem>> resultCallMatchItems = wsMatchItems.ScrapeBoldSuperligaMatchSchedule("www.bold.dk/fodbold/danmark/superligaen/ob/program", settingUseProperNames.Value);
            List<MatchItem> resultMatchItems = await resultCallMatchItems;

            //Scrape news from holdnyt.dk
            _1887.Backend.WebScraper.WebScraper ws = new Backend.WebScraper.WebScraper();
            Task<List<NewsArticleItem>> resultCall = ws.ScrapeHoldNytNewsArticles("www.holdnyt.dk/nyheder/fodbold/danmark-superligaen/ob");
            List<NewsArticleItem> result = await resultCall;

            //Scrape news from holdnyt.dk
            _1887.Backend.WebScraper.WebScraper wsLeagueTable = new Backend.WebScraper.WebScraper();
            Task<List<LeagueTableItem>> resultCallLeagueTableItems = ws.ScrapeBoldLeagueTable("www.bold.dk/fodbold/danmark/superligaen", settingUseProperNames.Value);
            List<LeagueTableItem> resultTableItems = await resultCallLeagueTableItems;

            //Add to View Lists
            AddNewsItemsToObservableCollection(result, settingNewsToShowList.Value);
            AddMatchItemsToObservableCollection(resultMatchItems);
            AddLeagueTableItemsToObservableCollection(resultTableItems);


            this.IsDataLoaded = true;

            if (resultCall.IsCompleted && resultCallMatchItems.IsCompleted && resultCallLeagueTableItems.IsCompleted)
            { 
                this.IsLoadingData = false;
            }
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

        //Add news items to list
        private void AddNewsItemsToObservableCollection(List<NewsArticleItem> result, int shortListItems)
        {
            int counter = 1;
            if (result != null && result.Count > 0)
            {
                this.NewsItems.Clear();
                this.NewsItemsLongList.Clear();

                foreach (NewsArticleItem item in result)
                {
                    if(counter <= shortListItems)
                    { 
                        this.NewsItems.Add(item);
                    }
                    
                    counter++;

                    this.NewsItemsLongList.Add(item);
                }
            }
        }

        private void AddMatchItemsToObservableCollection(List<MatchItem> result)
        {
            this.MatchItemsLongList.Clear();
            this.MatchItems.Clear();
            foreach (MatchItem item in result)
            {
                this.MatchItemsLongList.Add(item);
            }

            //Get Most Recent and Next Match
            DateTime dt = DateTime.Now;
            MatchItem recentMatch = result.Where(x => x.dateTime < dt).OrderByDescending(x => x.dateTime).FirstOrDefault();
            MatchItem nextMatch = result.Where(x => x.dateTime > dt).OrderBy(x => x.dateTime).FirstOrDefault();
            MatchItems.Add(recentMatch);
            MatchItems.Add(nextMatch);
        }

        private void AddLeagueTableItemsToObservableCollection(List<LeagueTableItem> result)
        {
            if(result.Count > 0)
            {
                //Clear for refresh purposes
                this.LeagueTableItems.Clear();

                foreach(LeagueTableItem item in result)
                {
                    this.LeagueTableItems.Add(item);
                }
            }
        }


    }
}