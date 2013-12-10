using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1887.Backend.Constants
{
    static public class BackgroundSettings
    {
        static public string BackgroundUri(int backgroundId)
        {
            const string assetsFolder = "Assets/";
            string background = assetsFolder;

            switch (backgroundId)
            {
                case 0:
                    background += "obbg1.jpg";
                    break;
                case 1:
                    background += "obbgbw.jpg";
                    break;
                case 2:
                    background += "obbg1blue.jpg";
                    break;
                case 3:
                    background = String.Empty;
                    break;
                default:
                    background += "obbg1.jpg";
                    break;
            }

            return background;
        }


    }
}
