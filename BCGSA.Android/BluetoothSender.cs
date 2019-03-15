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
        private static ArrayAdapter<string> _uiDev;
        private static BinaryFormatter _formatter = new BinaryFormatter();


        public Dictionary<string, BluetoothDevice> ScanResult { get; } = new Dictionary<string, BluetoothDevice>();

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

        public void Connect(BluetoothDevice device)
        {
            _socket = device.CreateRfcommSocketToServiceRecord(UUID.FromString("4d89187e-476a-11e9-b210-d663bd873d93"));
            _adapter.CancelDiscovery();
            _socket.Connect();
        }

        public void CreateBond(BluetoothDevice device) =>
            device.CreateBond();

        public Bond BondState(BluetoothDevice device) =>
            device.BondState;

        public void SendData(AccelerometerEntity accelerometerEntity)
        {
            using (var binaryDataStream = _socket.OutputStream)
            {
                _formatter.Serialize(binaryDataStream, accelerometerEntity);
            }
        }

        public void StartDiscovery()
        {
            _adapter.StartDiscovery();
        }

        public void InitAdapter(Context ctx, int resourceID, Spinner spin)
        {

            _uiDev = new ArrayAdapter<string>(ctx, global::Android.Resource.Layout.SimpleSpinnerItem);
            _uiDev.Add("Select Device");
            _uiDev.SetDropDownViewResource(global::Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spin.Adapter = _uiDev;
            
        }


        public override void OnReceive(Context context, Intent intent)
        {
            var action = intent.Action;

            if (action != BluetoothDevice.ActionFound)
            {
                return;
            }

            var device = (BluetoothDevice)intent.GetParcelableExtra(BluetoothDevice.ExtraDevice);
            
                if (!ScanResult.ContainsKey(device.Name))
                {
                    _uiDev.Add(device.Name);
                    var i = _uiDev.Count;
                    ScanResult.Add(device.Name, device);
                }
        }
    }
}