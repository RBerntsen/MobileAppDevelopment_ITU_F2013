using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using _1887.Backend.Settings;
using _1887.Backend.Constants;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace _1887.App
{
    public partial class MainPage : PhoneApplicationPage
    {
        static readonly IsolatedStorageProperty<int> backgroundNo = new IsolatedStorageProperty<int>(LocalSettings.settingBackground, 0);
        private int backgroundSet { get; set; }
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            App.ViewModel.PropertyChanged += ViewModel_PropertyChanged;

            //Load Background
            SetBackground();
        }

        private void SetBackground()
        {
            if(backgroundNo.Exists && backgroundNo.Value == 3)
            {
                //Do nothing. Don't set background.
                return;
            }
            
            BitmapImage bi3 = new BitmapImage();

            if (backgroundNo.Exists && backgroundNo.Value > 0)
            {
                bi3.UriSource = new Uri(BackgroundSettings.BackgroundUri(backgroundNo.Value), UriKind.Relative);
                backgroundSet = backgroundNo.Value;
            }

            else
            {
                bi3.UriSource = new Uri(BackgroundSettings.BackgroundUri(0), UriKind.Relative);
                backgroundSet = backgroundNo.Value = 0;
            }

            ibBackground.ImageSource = bi3;
            ibBackground.Stretch = Stretch.UniformToFill;
        }

        void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(!App.ViewModel.IsLoadingData)
            {
                imgDataLoad.Visibility = Visibility.Collapsed;
                tbDataLoad.Visibility = Visibility.Collapsed;
                customIndeterminateProgressBar.Visibility = Visibility.Collapsed;

                if(App.ViewModel.LeagueTableItems.Count > 0)
                {
                    BindLeagueTableItemsToGrid();
                }
            }

            if(App.ViewModel.IsLoadingData)
            {
                imgDataLoad.Visibility = Visibility.Visible;
                tbDataLoad.Visibility = Visibility.Visible;
                customIndeterminateProgressBar.Visibility = Visibility.Visible;
            }
        }

        private void BindLeagueTableItemsToGrid()
        {
            if (gridLeagueTable.Children.Count > 0)
            {
                gridLeagueTable.ColumnDefinitions.Clear();
                gridLeagueTable.Children.Clear();
                gridLeagueTable.RowDefinitions.Clear();
            }

            // Define the Columns
            ColumnDefinition colDef1 = new ColumnDefinition();
            ColumnDefinition colDef2 = new ColumnDefinition();
            ColumnDefinition colDef3 = new ColumnDefinition();
            ColumnDefinition colDef4 = new ColumnDefinition();
            ColumnDefinition colDef5 = new ColumnDefinition();
            ColumnDefinition colDef6 = new ColumnDefinition();
            ColumnDefinition colDef7 = new ColumnDefinition();
            ColumnDefinition colDef8 = new ColumnDefinition();
            ColumnDefinition colDef9 = new ColumnDefinition();

            colDef1.Width = colDef2.Width = colDef3.Width = colDef4.Width = colDef5.Width = colDef6.Width = colDef7.Width = colDef8.Width = colDef9.Width = System.Windows.GridLength.Auto;

            gridLeagueTable.ColumnDefinitions.Add(colDef1);
            gridLeagueTable.ColumnDefinitions.Add(colDef2);
            gridLeagueTable.ColumnDefinitions.Add(colDef3);
            gridLeagueTable.ColumnDefinitions.Add(colDef4);
            gridLeagueTable.ColumnDefinitions.Add(colDef5);
            gridLeagueTable.ColumnDefinitions.Add(colDef6);
            gridLeagueTable.ColumnDefinitions.Add(colDef7);
            gridLeagueTable.ColumnDefinitions.Add(colDef8);
            gridLeagueTable.ColumnDefinitions.Add(colDef9);

            // Define the Rows
            RowDefinition headerRow = new RowDefinition();
            RowDefinition rowDef1 = new RowDefinition();
            RowDefinition rowDef2 = new RowDefinition();
            RowDefinition rowDef3 = new RowDefinition();
            RowDefinition rowDef4 = new RowDefinition();
            RowDefinition rowDef5 = new RowDefinition();
            RowDefinition rowDef6 = new RowDefinition();
            RowDefinition rowDef7 = new RowDefinition();
            RowDefinition rowDef8 = new RowDefinition();
            RowDefinition rowDef9 = new RowDefinition();
            RowDefinition rowDef10 = new RowDefinition();
            RowDefinition rowDef11 = new RowDefinition();
            RowDefinition rowDef12 = new RowDefinition();
            RowDefinition rowDef13 = new RowDefinition();

            gridLeagueTable.RowDefinitions.Add(headerRow);
            gridLeagueTable.RowDefinitions.Add(rowDef1);
            gridLeagueTable.RowDefinitions.Add(rowDef2);
            gridLeagueTable.RowDefinitions.Add(rowDef3);
            gridLeagueTable.RowDefinitions.Add(rowDef4);
            gridLeagueTable.RowDefinitions.Add(rowDef5);
            gridLeagueTable.RowDefinitions.Add(rowDef6);
            gridLeagueTable.RowDefinitions.Add(rowDef7);
            gridLeagueTable.RowDefinitions.Add(rowDef8);
            gridLeagueTable.RowDefinitions.Add(rowDef9);
            gridLeagueTable.RowDefinitions.Add(rowDef10);
            gridLeagueTable.RowDefinitions.Add(rowDef11);
            gridLeagueTable.RowDefinitions.Add(rowDef12);
            gridLeagueTable.RowDefinitions.Add(rowDef13);

            TextBlock rank = new TextBlock();
            TextBlock club = new TextBlock();
            TextBlock matchesPlayed = new TextBlock();
            TextBlock matchesWon = new TextBlock();
            TextBlock matchesDraw = new TextBlock();
            TextBlock matchesLost = new TextBlock();
            TextBlock goalsScored = new TextBlock();
            TextBlock goalsConceded = new TextBlock();
            TextBlock points = new TextBlock();

            rank.Text = "#";
            club.Text = "Club";
            matchesPlayed.Text = "P";
            matchesWon.Text = "W";
            matchesDraw.Text = "D";
            matchesLost.Text = "L";
            goalsScored.Text = "+";
            goalsConceded.Text = "-";
            points.Text = "Points";

            //Style it
            rank.FontWeight = club.FontWeight = matchesPlayed.FontWeight = matchesWon.FontWeight = matchesDraw.FontWeight = matchesLost.FontWeight = goalsScored.FontWeight = goalsConceded.FontWeight = points.FontWeight = FontWeights.Bold;
            club.Margin = matchesPlayed.Margin = matchesWon.Margin = matchesDraw.Margin = matchesLost.Margin = goalsScored.Margin = goalsConceded.Margin = points.Margin = new Thickness(10, 0, 0, 0);

            Grid.SetRow(rank, 0);
            Grid.SetRow(club, 0);
            Grid.SetRow(matchesPlayed, 0);
            Grid.SetRow(matchesWon, 0);
            Grid.SetRow(matchesDraw, 0);
            Grid.SetRow(matchesLost, 0);
            Grid.SetRow(goalsScored, 0);
            Grid.SetRow(goalsConceded, 0);
            Grid.SetRow(points, 0);

            Grid.SetColumn(rank, 0);
            Grid.SetColumn(club, 1);
            Grid.SetColumn(matchesPlayed, 2);
            Grid.SetColumn(matchesWon, 3);
            Grid.SetColumn(matchesDraw, 4);
            Grid.SetColumn(matchesLost, 5);
            Grid.SetColumn(goalsScored, 6);
            Grid.SetColumn(goalsConceded, 7);
            Grid.SetColumn(points, 8);

            gridLeagueTable.Children.Add(rank);
            gridLeagueTable.Children.Add(club);
            gridLeagueTable.Children.Add(matchesPlayed);
            gridLeagueTable.Children.Add(matchesWon);
            gridLeagueTable.Children.Add(matchesDraw);
            gridLeagueTable.Children.Add(matchesLost);
            gridLeagueTable.Children.Add(goalsScored);
            gridLeagueTable.Children.Add(goalsConceded);
            gridLeagueTable.Children.Add(points);

            int counter = 1;

            foreach(_1887.Backend.Model.LeagueTableItem item in App.ViewModel.LeagueTableItems)
            {
                rank = new TextBlock();
                club = new TextBlock();
                matchesPlayed = new TextBlock();
                matchesWon = new TextBlock();
                matchesDraw = new TextBlock();
                matchesLost = new TextBlock();
                goalsScored = new TextBlock();
                goalsConceded = new TextBlock();
                points = new TextBlock();

                rank.Text = item.ranking.ToString();
                club.Text = item.club;
                matchesPlayed.Text = item.matchesPlayed.ToString();
                matchesWon.Text = item.matchesWon.ToString();
                matchesDraw.Text = item.matchesDraw.ToString();
                matchesLost.Text = item.matchesLost.ToString();
                goalsScored.Text = item.goalsScored.ToString();
                goalsConceded.Text = item.goalsConceded.ToString();
                points.Text = item.pointsTotal.ToString();

                //Style it
                club.Margin = matchesPlayed.Margin = matchesWon.Margin = matchesDraw.Margin = matchesLost.Margin = goalsScored.Margin = goalsConceded.Margin = points.Margin = new Thickness(10, 0, 0, 0);

                if (item.club.ToLower().Equals("odense"))
                {
                    club.FontWeight = rank.FontWeight = matchesPlayed.FontWeight = matchesWon.FontWeight = matchesDraw.FontWeight = matchesLost.FontWeight = goalsScored.FontWeight = goalsConceded.FontWeight = points.FontWeight = FontWeights.Bold;
                    spMyClubLeagueStanding.DataContext = item;
                }

                Grid.SetRow(rank, counter);
                Grid.SetRow(club, counter);
                Grid.SetRow(matchesPlayed, counter);
                Grid.SetRow(matchesWon, counter);
                Grid.SetRow(matchesDraw, counter);
                Grid.SetRow(matchesLost, counter);
                Grid.SetRow(goalsScored, counter);
                Grid.SetRow(goalsConceded, counter);
                Grid.SetRow(points, counter);

                Grid.SetColumn(rank, 0);
                Grid.SetColumn(club, 1);
                Grid.SetColumn(matchesPlayed, 2);
                Grid.SetColumn(matchesWon, 3);
                Grid.SetColumn(matchesDraw, 4);
                Grid.SetColumn(matchesLost, 5);
                Grid.SetColumn(goalsScored, 6);
                Grid.SetColumn(goalsConceded, 7);
                Grid.SetColumn(points, 8);

                gridLeagueTable.Children.Add(rank);
                gridLeagueTable.Children.Add(club);
                gridLeagueTable.Children.Add(matchesPlayed);
                gridLeagueTable.Children.Add(matchesWon);
                gridLeagueTable.Children.Add(matchesDraw);
                gridLeagueTable.Children.Add(matchesLost);
                gridLeagueTable.Children.Add(goalsScored);
                gridLeagueTable.Children.Add(goalsConceded);
                gridLeagueTable.Children.Add(points);
                
                counter++;
            }
        }

        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
            
            if (backgroundNo.Exists && backgroundNo.Value != backgroundSet)
            {
                SetBackground();
            }
        }

        private void abibHome_Click(object sender, EventArgs e)
        {
            Panorama pana = (Panorama)LayoutRoot.Children.Where(x => x is Panorama).FirstOrDefault();
            pana.DefaultItem = pana.Items[0]; 
        }

        private void abibRefresh_Click(object sender, EventArgs e)
        {
            App.ViewModel.LoadData();
            Panorama pana = (Panorama)LayoutRoot.Children.Where(x => x is Panorama).FirstOrDefault();
            pana.DefaultItem = pana.Items[0];
        }

        private void abmiSettings_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Settings.xaml", UriKind.Relative));
        }

        private void abmiAbout_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/About.xaml", UriKind.Relative));
        }

        private void hlb_InternalBrowser_OnUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            
        }

        private void LongListSelector_Loaded(object sender, RoutedEventArgs e)
        {
            LongListSelector lls = (LongListSelector)sender;
            if(lls.ItemsSource.Count > 3)
            {
            }
        }
    }
}