using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1887.Backend.Model
{
    public class LeagueTableItem : INotifyPropertyChanged
    {
        public string club { get; set; }
        public int ranking { get; set; }
        public int matchesPlayed { get; set; }
        public int matchesWon { get; set; }
        public int matchesDraw { get; set; }
        public int matchesLost { get; set; }
        public int goalsScored { get; set; }
        public int goalsConceded { get; set; }
        public int pointsTotal { get; set; }
        public bool isTeamToLookFor { get { return club.Contains("ODENSE"); } }

        public string presentationViewSimple { get { return PresentationViewSimple(); } }

        public LeagueTableItem(string club, int ranking, int matchesPlayed, int matchesWon, int matchesDraw, int matchesLost, int goalsScored, int goalsConceded, int pointsTotal)
        {
            this.club = club;
            this.ranking = ranking;
            this.matchesPlayed = matchesPlayed;
            this.matchesWon = matchesWon;
            this.matchesDraw = matchesDraw;
            this.matchesLost = matchesLost;
            this.goalsScored = goalsScored;
            this.goalsConceded = goalsConceded;
            this.pointsTotal = pointsTotal;
        }

        //PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public override string ToString()
        {
            return club.ToString();
        }

        public string PresentationViewSimple()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this.ranking);
            sb.Append(". ");
            sb.Append(this.club);
            sb.Append(" - ");
            sb.Append(this.pointsTotal);
            sb.Append(" points");
            return sb.ToString();
        }
    }
}
