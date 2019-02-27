using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using Plugin.BluetoothLE;
using System.Threading;
using PCLAppConfig;

namespace BCGSA
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            if (CrossBleAdapter.Current.CanControlAdapterState())
                CrossBleAdapter.Current.SetAdapterState(true);
            var scanner = CrossBleAdapter.Current.ScanInterval(TimeSpan.FromSeconds(15),TimeSpan.FromSeconds(5)).Subscribe(scanResult =>
            {
                lbl.Text = $"{scanResult.Device.Name}:{scanResult.Device.Uuid}:{scanResult.Rssi}";
            });
            //Thread.Sleep(5000);
            //scanner.Dispose();
        }

        public async Task ConfigDemo()
        {
            var answer = await DisplayAlert("Question?", $"Do you see this = \"{ConfigurationManager.AppSettings["config.text"]}\"", "Yes", "No");
        }

        private void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            var data = e.Reading;
            lbl.Text = $"X:{data.Acceleration.X} Y:{data.Acceleration.Y} Z:{data.Acceleration.Z}";
        }
    }
}
