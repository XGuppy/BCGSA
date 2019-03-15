using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Bluetooth;
using Java.Util;
using System.Runtime.Serialization.Formatters.Binary;

namespace BCGSA.Android
{
    static class BluetoothSender
    {
        private static BluetoothAdapter _adapter;
        private static BluetoothSocket _socket;
        private static BinaryFormatter _formatter = new BinaryFormatter();

        public static bool IsConnected
        {
            get => _socket.IsConnected;
        }

        static BluetoothSender()
        {
            _adapter = BluetoothAdapter.DefaultAdapter;
            if (_adapter == null)
                throw new Exception("No Bluetooth adapter found");

            if (!_adapter.IsEnabled)
            {
                if (!_adapter.Enable())
                    throw new Exception("Bluetooth failed to turn on");
            }
        }

        public static List<BluetoothDevice> Scan() => _adapter.BondedDevices.ToList();

        public static async void Connect(BluetoothDevice device)
        {
            _socket = device.CreateRfcommSocketToServiceRecord(UUID.FromString(Guid.NewGuid().ToString()));
            await _socket.ConnectAsync();
        }


        public static void SendData(AccelerometerEntity accelerometerEntity)
        {
            using (var binaryDataStream = _socket.OutputStream)
            {
                _formatter.Serialize(binaryDataStream, accelerometerEntity);
            }
        }
    }
}