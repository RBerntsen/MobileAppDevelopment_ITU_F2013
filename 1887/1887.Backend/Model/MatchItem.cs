using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1887.Backend.Model
{
    public class MatchItem : INotifyPropertyChanged
    {

        //Get a localized string(?)
        static string versus = "vs.";
        
        //Home Team (OB, OB U17, OB U19, OB Reserver etc.)
        string homeTeam;
        
        //Playing against
        string against;

        //Is it Superliga?
        public bool isATeam { get; set; }
        
        //Date and time
        public DateTime dateTime { get; set; }
        
        //Is it home (or away?)
        public bool isHomeMatch { get; set; }
        
        //Random ID for the match?
        public string id { get; set; }

        public string score { get; set; }

        public string Against
        {
            get { return against; }
            set { against = value; }
        }

        public string AgainstText
        {
            get { return VersusText(true); }
        }


        public string HomeTeam
        {
            get { return homeTeam; }
            set { homeTeam = value; }
        }

        //Public constructor
        public MatchItem(string homeTeam, string against, DateTime dateTime, bool isHomeMatch, string matchId, string score)
        {
            this.homeTeam = homeTeam;
            this.against = against;
            this.dateTime = dateTime;
            this.isHomeMatch = isHomeMatch;
            this.id = matchId;
            this.score = score;
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

        protected string VersusText(bool withDate = false)
        {
            string matchText = this.homeTeam + " " + versus + " " + this.against;

            string scoreText = string.IsNullOrEmpty(score) ? string.Empty : ", " + score;

            if (withDate)
            {
                matchText += " (" + string.Format("{0:d}", this.dateTime.ToLocalTime()) + scoreText + ")";
            }

            return matchText;

        }
    }
}
