using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace _1887.Backend.Utilities
{
    public class MemoryUsage
    {
        private static Timer timer = null;

        public static void BeginRecording()
        {
            timer = new Timer(state =>
            {
                string output = "Unkown error";
                try{}
                catch (ArgumentOutOfRangeException ar)
                {
                    var c1 = ar.Message;
                }
                catch
                {
                    output = "Unkown error";
                }


                string report = "";
                report += Environment.NewLine +
                   "ApplicationCurrentMemoryUsage: " + (Microsoft.Phone.Info.DeviceStatus.ApplicationCurrentMemoryUsage / 1000000).ToString() + "MB\n" +
                   "ApplicationPeakMemoryUsage: " + (Microsoft.Phone.Info.DeviceStatus.ApplicationPeakMemoryUsage / 1000000).ToString() + "MB\n" +
                   "ApplicationMemoryUsageLimit: " + (Microsoft.Phone.Info.DeviceStatus.ApplicationMemoryUsageLimit / 1000000).ToString() + "MB\n\n" +
                   "ApplicationWorkingSetLimit: " + Convert.ToInt32((Convert.ToDouble(Microsoft.Phone.Info.DeviceExtendedProperties.GetValue("ApplicationWorkingSetLimit")) / 1000000)).ToString() + "MB\n" +
                   "DeviceTotalMemory: " + (Microsoft.Phone.Info.DeviceStatus.DeviceTotalMemory / 1000000).ToString() + "MB\n";

                Deployment.Current.Dispatcher.BeginInvoke(delegate
                {
                    System.Diagnostics.Debug.WriteLine(report);
                });

            },
                null,
                TimeSpan.FromSeconds(3),
                TimeSpan.FromSeconds(3));
        } 
    }
}
