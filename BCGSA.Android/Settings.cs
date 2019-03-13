using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BCGSA.ConfigMaster;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;

namespace BCGSA.Android
{
    [Activity(Label = "@string/action_settings", Theme = "@style/AppTheme")]
    class Settings: AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.settings);

            ConfManager confManager = ConfManager.GetManager;

            Switch switchInversX = FindViewById<Switch>(Resource.Id.XSwitch);

            switchInversX.CheckedChange += (o, e) =>
                confManager.InversX = (o as Switch).Checked;

            Switch switchInversY = FindViewById<Switch>(Resource.Id.YSwitch);

            switchInversY.CheckedChange += (o, e) =>
                confManager.InversY = (o as Switch).Checked;

            switchInversX.Checked = confManager.InversX;
            switchInversY.Checked = confManager.InversY;

            Spinner spinnerMode = FindViewById<Spinner>(Resource.Id.sensorSpinner);

            spinnerMode.ItemSelected += (o, e) =>
                confManager.ConnectMod = (string)(o as Spinner).SelectedItem;

            var adapter = new ArrayAdapter<string>(this,
                global::Android.Resource.Layout.SimpleSpinnerItem, ConfManager.GetModes);

            adapter.SetDropDownViewResource(global::Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinnerMode.Adapter = adapter;
            spinnerMode.SetSelection(adapter.GetPosition(confManager.ConnectMod));

        }
    }
}