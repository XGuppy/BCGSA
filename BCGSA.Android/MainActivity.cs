using System;
using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System.Collections.Generic;
using Android.Views;
using Android.Content;
using System.Threading.Tasks;
using System.Linq;
using Android.Support.V4.Content;
using Android.Support.V4.App;
using Android.Content.PM;
using Android.Bluetooth;
using System.Threading;
using Android.Hardware;
using System.Numerics;
using BCGSA.ConfigMaster;

namespace BCGSA.Android
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, ISensorEventListener
    {
        private AccelerometerEntity Data { get; set; } = new AccelerometerEntity(default(Vector3), default(Vector3));

        private BluetoothSender _bluetoothSender = new BluetoothSender();
        
        private void CheckPermissions()
        {
            const int locationPermissionsRequestCode = 1000;

            var locationPermissions = new[]
            {
                global::Android.Manifest.Permission.AccessCoarseLocation,
                global::Android.Manifest.Permission.AccessFineLocation
            };

            // check if the app has permission to access coarse location
            var coarseLocationPermissionGranted =
                ContextCompat.CheckSelfPermission(this, global::Android.Manifest.Permission.AccessCoarseLocation);

            // check if the app has permission to access fine location
            var fineLocationPermissionGranted =
                ContextCompat.CheckSelfPermission(this, global::Android.Manifest.Permission.AccessFineLocation);

            // if either is denied permission, request permission from the user
            if (coarseLocationPermissionGranted == Permission.Denied ||
                fineLocationPermissionGranted == Permission.Denied)
            {
                ActivityCompat.RequestPermissions(this, locationPermissions, locationPermissionsRequestCode);
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            CheckPermissions();

            var manager = ConfManager.GetManager;
            Enum.TryParse(manager.ConnectMod, out SensorDelay speed);
            var sensorManager = GetSystemService(SensorService) as SensorManager;
            sensorManager.RegisterListener(this, sensorManager.GetDefaultSensor(SensorType.Gyroscope), speed);
            sensorManager.RegisterListener(this, sensorManager.GetDefaultSensor(SensorType.LinearAcceleration), speed);


            RegisterReceiver(_bluetoothSender, new IntentFilter(BluetoothDevice.ActionFound));

            var spinner = FindViewById<Spinner>(Resource.Id.select_device);

            _bluetoothSender.InitAdapter(this, Resource.Layout.activity_main, spinner);

            _bluetoothSender.StartDiscovery();

            spinner.ItemSelected += async (o, e) => {

                try
                {
                    var name = (string)(o as Spinner).SelectedItem;
                    if (_bluetoothSender.ScanResult.ContainsKey(name))
                    {
                        await Task.Run(() =>
                       {
                           while (_bluetoothSender.BondState(_bluetoothSender.ScanResult[name]) != Bond.Bonded)
                           {
                               _bluetoothSender.CreateBond(_bluetoothSender.ScanResult[name]);
                           }
                           _bluetoothSender.Connect(_bluetoothSender.ScanResult[name]);
                           this.Sended += _bluetoothSender.SendData;
                       });
                    }
                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
                }
            };
        }

        /// <summary>
        /// Creates menu from custom file
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            menu.Clear();
            MenuInflater.Inflate(Resource.Menu.app_menu, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_settings:
                    var intent = new Intent(this, typeof(Settings));
                    StartActivity(intent);
                    return true;
                case Resource.Id.action_exit:
                    CloseApplication();
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        private void CloseApplication()
        {
            var activity = (Activity)this;
            activity.FinishAffinity();
        }

        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
        {

        }

        public void OnSensorChanged(SensorEvent e)
        {
            switch (e.Sensor.Type)
            {
                case SensorType.LinearAcceleration:
                    Data.Accelerometer = AccelerometerEntity.FromVector3(new Vector3(e.Values[0], e.Values[1], e.Values[2]));
                    break;
                case SensorType.Gyroscope:
                    Data.Gyroscope = AccelerometerEntity.FromVector3(new Vector3(e.Values[0], e.Values[1], e.Values[2]));
                    break;
            }
            Sended?.Invoke(Data);
        }

        public delegate void SendAccelerometerHandler(AccelerometerEntity e);
        public event SendAccelerometerHandler Sended; // Sender event
    }
}