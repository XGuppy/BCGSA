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
    class BluetoothSender: BroadcastReceiver
    {
        private BluetoothAdapter _adapter;
        private BluetoothSocket _socket;
        private List<BluetoothDevice> _btDevices = new List<BluetoothDevice>();
        private static BinaryFormatter _formatter = new BinaryFormatter();

        public bool IsConnected
        {
            get => _socket.IsConnected;
        }

        public BluetoothSender()
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

        public List<BluetoothDevice> Scan() => _btDevices;

        public async void Connect(BluetoothDevice device)
        {
            _socket = device.CreateRfcommSocketToServiceRecord(UUID.FromString(Guid.NewGuid().ToString()));
            await _socket.ConnectAsync();
        }

        public void CreateBond(BluetoothDevice device) =>
            device.CreateBond();

        public bool IsBonded(BluetoothDevice device) =>
            device.BondState == Bond.Bonded ? true : false;

        public void SendData(AccelerometerEntity accelerometerEntity)
        {
            if (!IsConnected)
            {
                throw new Exception("Device is not connected");
            }
            else
            {
                using (var binaryDataStream = _socket.OutputStream)
                {
                    _formatter.Serialize(binaryDataStream, accelerometerEntity);
                }
            }
        }

        public void StartDiscovery()
        {
            _adapter.StartDiscovery();
        }

        public override void OnReceive(Context context, Intent intent)
        {
            var action = intent.Action;

            if (action != BluetoothDevice.ActionFound)
            {
                return;
            }

            var device = (BluetoothDevice)intent.GetParcelableExtra(BluetoothDevice.ExtraDevice);
            
            if (device.BondState != Bond.Bonded)
            {
                _btDevices.Add(device);
            }
        }
    }
}