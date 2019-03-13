using System;
using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System.Collections.Generic;
using Android.Views;

namespace BCGSA.Android
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        /// <summary>
        /// Device Data 
        /// string - name of device
        /// int -- id of device
        /// </summary>
        private List<KeyValuePair<string, int>> devicesData = new List<KeyValuePair<string, int>>();

        /// <summary>
        /// Initialize list of devices
        /// </summary>
        private void InitListOfDevices()
        {
            // temporary data
            devicesData.Add(new KeyValuePair<string, int>("Hello", 1999));
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            Spinner spinner = FindViewById<Spinner>(Resource.Id.select_device);

            InitListOfDevices();
            var devicesNames = new List<string>();
            foreach (var item in devicesData)
            {
                devicesNames.Add(item.Key);
            }

            var adapter = new ArrayAdapter<string>(this,
                global::Android.Resource.Layout.SimpleSpinnerItem, devicesNames);

            adapter.SetDropDownViewResource(global::Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;
        }

        /// <summary>
        /// Creates menu from custom file
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.app_menu, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_settings:
                    //do something
                    return true;
                case Resource.Id.action_exit:
                    CloseApplication();
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        public void CloseApplication()
        {
            var activity = (Activity)this;
            activity.FinishAffinity();
        }
    }
}