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

namespace BCGSA.Android
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        public async void SelectDevice(Spinner spinner)
        {
            List<string> devicesNames = new List<string>();

            await Task.Run(() => 
            {
                while (!spinner.Selected)
                {
                    foreach (var item in BluetoothSender.Scan())
                    {
                        devicesNames.Add(item.Name);
                    }

                    var adapter = new ArrayAdapter<string>(this,
                        global::Android.Resource.Layout.SimpleSpinnerItem, devicesNames);

                    adapter.SetDropDownViewResource(global::Android.Resource.Layout.SimpleSpinnerDropDownItem);
                    spinner.Adapter = adapter;

                    devicesNames.Clear();
                }
            });
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            var spinner = FindViewById<Spinner>(Resource.Id.select_device);
            SelectDevice(spinner);

            spinner.ItemSelected += (o, e) => {

                var name = (string)(o as Spinner).SelectedItem;

                var device = (from devs in BluetoothSender.Scan()
                              where devs.Name == name
                              select devs).FirstOrDefault();

                if (device == null)
                {
                    throw new Exception("Device Not Found");
                }

                BluetoothSender.Connect(device);

                DataSender.Sended += BluetoothSender.SendData;
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