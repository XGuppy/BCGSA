using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using Plugin.BluetoothLE;
using System.Threading;

namespace BCGSA
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent(); 
            //if (CrossBleAdapter.Current.CanControlAdapterState())
            //    CrossBleAdapter.Current.SetAdapterState(true);
            //var scanner = CrossBleAdapter.Current.ScanInterval(TimeSpan.FromSeconds(15),TimeSpan.FromSeconds(5)).Subscribe(scanResult =>
            //{
            //    lbl.Text = $"{scanResult.Device.Name}:{scanResult.Device.Uuid}:{scanResult.Rssi}";
            //});
            //Thread.Sleep(5000);
            //scanner.Dispose();
        }

        private void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            var data = e.Reading;
        }

        private void ListDevicesChanged(object sender, EventArgs e)
        {
            // use list of devices here
        }

        private void SettingsItemClicked(object sender, EventArgs e)
        {
            // go to settings
            Navigation.PushAsync(new Settings());
        }

        private void ExitItemClicked(object sender, EventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
        }
    }
}
