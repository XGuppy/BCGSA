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
namespace BCGSA.Android
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
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

            RegisterReceiver(_bluetoothSender, new IntentFilter(BluetoothDevice.ActionFound));

            var spinner = FindViewById<Spinner>(Resource.Id.select_device);

            //Сюда писать инициализатор адаптера

            _bluetoothSender.StartDiscovery();

            spinner.ItemSelected += (o, e) => {

                var name = (string)(o as Spinner).SelectedItem;

                var device = (from devs in _bluetoothSender.Scan()
                              where devs.Name == name
                              select devs).FirstOrDefault();

                if (device == null)
                {
                    throw new Exception("Device Not Found");
                }

                _bluetoothSender.Connect(device);

                DataSender.Sended += _bluetoothSender.SendData;
                

                
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
    }
}