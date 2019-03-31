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
using Android.Support.V7.App;
using BCGSA.Android.ConfigMaster;

namespace BCGSA.Android
{
    [Activity(Label = "@string/action_settings", Theme = "@style/AppTheme")]
    class Settings: AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.settings);

            var confManager = ConfManager.GetManager;

            var switchInversX = FindViewById<Switch>(Resource.Id.XSwitch);

            switchInversX.CheckedChange += (o, e) =>
                confManager.InversX = ((Switch) o).Checked;

            var switchInversY = FindViewById<Switch>(Resource.Id.YSwitch);

            switchInversY.CheckedChange += (o, e) =>
                confManager.InversY = ((Switch) o).Checked;
            
            var switchInversZ = FindViewById<Switch>(Resource.Id.ZSwitch);

            switchInversZ.CheckedChange += (o, e) =>
                confManager.InversZ = ((Switch) o).Checked;
            
            switchInversX.Checked = confManager.InversX;
            switchInversY.Checked = confManager.InversY;
            switchInversZ.Checked = confManager.InversZ;
            
            var spinnerMode = FindViewById<Spinner>(Resource.Id.sensorSpinner);

            spinnerMode.ItemSelected += (o, e) =>
                confManager.ConnectMod = (string)(o as Spinner)?.SelectedItem;

            var adapter = new ArrayAdapter<string>(this,
                global::Android.Resource.Layout.SimpleSpinnerItem, ConfManager.GetModes);

            adapter.SetDropDownViewResource(global::Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinnerMode.Adapter = adapter;
            spinnerMode.SetSelection(adapter.GetPosition(confManager.ConnectMod));

        }
    }
}