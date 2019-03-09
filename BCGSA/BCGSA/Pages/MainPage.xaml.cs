﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using Plugin.BluetoothLE;
using System.Threading;
using BCGSA.ConfigMaster;
namespace BCGSA
{
    public partial class MainPage : ContentPage
    {
        private const int UpdateDeviceTime = 10;
        public List<IDevice> Devices;

        private IDevice Current;

        public MainPage()
        {
            InitializeComponent();
            var config = ConfManager.GetManager;

            if (CrossBleAdapter.Current.CanControlAdapterState())
                CrossBleAdapter.Current.SetAdapterState(true);

            var scanner = CrossBleAdapter.Current.ScanInterval(TimeSpan.FromSeconds(15),TimeSpan.FromSeconds(5)).Subscribe(scanResult =>
            {
                if (!Devices.Contains(scanResult.Device))
                    Devices.Add(scanResult.Device);

                foreach (var device in Devices)
                    if (device.Status == ConnectionStatus.Disconnected)
                        Devices.Remove(device);
            });
            
            scanner.Dispose();
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
