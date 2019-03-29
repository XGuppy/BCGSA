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
using System.IO;
using System.Threading.Tasks;

namespace BCGSA.Android
{
    class BluetoothSender: BroadcastReceiver
    {
        private readonly BluetoothAdapter _adapter;
        private BluetoothSocket _socket;
        private static ArrayAdapter<string> _uiDev;
        private static BluetoothDevice _btDevice;
        private static readonly BinaryFormatter Formatter = new BinaryFormatter();

        private Context _ctx;


        public Dictionary<string, BluetoothDevice> ScanResult { get; } = new Dictionary<string, BluetoothDevice>();

        private bool IsConnected => _socket.IsConnected;

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
            
            _adapter.CancelDiscovery();
            _btDevice = device;
            
        }

        public void CreateBond(BluetoothDevice device) =>
            device.CreateBond();

        public Bond BondState(BluetoothDevice device) =>
            device.BondState;

        public async void SendData(AccelerometerEntity accelerometerEntity)
        {
            await Task.Run(() =>
            {
                try
                {
                    if (!_socket.IsConnected)
                    {
                        _socket = _btDevice.CreateRfcommSocketToServiceRecord(UUID.FromString("4d89187e-476a-11e9-b210-d663bd873d93"));
                        _socket.Connect();
                        if(!_socket.IsConnected)
                            throw new Exception("Device lost connection");
                    }
                    else
                    {
                        using (var sw = _socket.OutputStream)
                        {
                            Formatter.Serialize(sw, accelerometerEntity);
                        }    
                    }
                }
                catch (Exception e)
                {
                    (_ctx as Activity)?.RunOnUiThread(() => {
                        Toast.MakeText(_ctx, e.Message, ToastLength.Long).Show();
                    });
                }
            });
        }

        public void StartDiscovery()
        {
            _adapter.StartDiscovery();
        }

        public void InitAdapter(Context ctx, int resourceId, Spinner spin)
        {
            _ctx = ctx;
            _uiDev = new ArrayAdapter<string>(ctx, global::Android.Resource.Layout.SimpleSpinnerItem);
            _uiDev.Add("Select Device");
            _uiDev.SetDropDownViewResource(global::Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spin.Adapter = _uiDev;
            
        }

        public  override void OnReceive(Context context, Intent intent)
        {
            var action = intent.Action;

            if (action != BluetoothDevice.ActionFound)
            {
                return;
            }

            var device = (BluetoothDevice) intent.GetParcelableExtra(BluetoothDevice.ExtraDevice);

            if (device.Name != null)
            {
                if (!ScanResult.ContainsKey(device.Name))
                {
                    _uiDev.Add(device.Name);
                    ScanResult.Add(device.Name, device);
                }
            }
        }
    }
}