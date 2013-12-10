using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;
using _1887.Backend.Constants;
using _1887.Backend.Settings;
using System.ComponentModel;

namespace _1887.App
{
    public partial class Settings : PhoneApplicationPage, INotifyPropertyChanged
    {
        List<int> lpNoOfNewsToShowList;
        //List<int> lpBackgroundList;
        Dictionary<int, string> dicBackgroundList;
        static readonly IsolatedStorageProperty<int> NoOfNewsToShow = new IsolatedStorageProperty<int>(LocalSettings.settingNewsToShowList, 3);
        static readonly IsolatedStorageProperty<bool> UseProperNames = new IsolatedStorageProperty<bool>(LocalSettings.settingUseProperNames, false);
        static readonly IsolatedStorageProperty<int> backgroundNo = new IsolatedStorageProperty<int>(LocalSettings.settingBackground, 0);

        public Settings()
        {
            InitializeComponent();
            lpNoOfNewsToShowList = new List<int>() {1, 3, 5};
            dicBackgroundList = new Dictionary<int, string>();
            dicBackgroundList.Add(0, "Normal");
            dicBackgroundList.Add(1, "Grå");
            dicBackgroundList.Add(2, "Blå");
            dicBackgroundList.Add(3, "Nej tak");

            this.Loaded += Settings_Loaded;
        }

        private void Settings_Loaded(object sender, RoutedEventArgs e)
        {
            //Settings for Number of news to show
            this.lpNoOfNewsToShow.ItemsSource = this.lpNoOfNewsToShowList;

            if (NoOfNewsToShow.Value > 0)
            {
                this.lpNoOfNewsToShow.SelectedItem = NoOfNewsToShow.Value;
            }

            //Settings for Use Proper Names
            if(UseProperNames.Exists)
            {
                this.cbProperNames.IsChecked = UseProperNames.Value;
            }

            //Setting for Background // Fill data
            foreach(KeyValuePair<int, string> entry in dicBackgroundList)
            {
                this.lpBackgroundSelector.Items.Add(new ListPickerItem() {Name = entry.Key.ToString(), Content = entry.Value});
                if(backgroundNo.Value > 0)
                {
                    foreach (ListPickerItem item in lpBackgroundSelector.Items)
                    {
                        if(item.Name.Equals(backgroundNo.Value.ToString()))
                        {
                            this.lpBackgroundSelector.SelectedItem = item;
                        }
                    }
                }
            }

            //if (backgroundNo.Value > 0)
            //{
            //    this.lpBackgroundSelector.SelectedItem = backgroundNo.Value;
            //}

            //Sign up for actions
            this.lpNoOfNewsToShow.SelectionChanged += lpNoOfNewsToShow_SelectionChanged;
            this.cbProperNames.Click += cbProperNames_Click;
            this.cbOBForTheWin.Click += cbOBForTheWin_Click;
            this.lpBackgroundSelector.SelectionChanged += lpBackgroundSelector_SelectionChanged;
        }

        void lpBackgroundSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.lpBackgroundSelector.SelectedItem != null)
            {
                    ListPickerItem item = (ListPickerItem)this.lpBackgroundSelector.SelectedItem;
                    backgroundNo.Value = int.Parse(item.Name);
                    NotifyPropertyChanged("BackgroundChanged");
            }
        }

        private void lpNoOfNewsToShow_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.lpNoOfNewsToShow.SelectedItem.ToString()))
            { 
                NoOfNewsToShow.Value = int.Parse(this.lpNoOfNewsToShow.SelectedItem.ToString());
            }
        }

        void cbProperNames_Click(object sender, RoutedEventArgs e)
        {
            bool valueToSet = false;
            if(cbProperNames.IsChecked != null && cbProperNames.IsChecked == true)
            {
                valueToSet = true;
            }

            UseProperNames.Value = valueToSet; 
        }

        void cbOBForTheWin_Click(object sender, RoutedEventArgs e)
        {
            this.cbOBForTheWin.IsChecked = true;
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