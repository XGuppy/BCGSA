﻿using System;
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

namespace BCGSA.Android
{
    [Activity(Label = "@string/action_settings", Theme = "@style/AppTheme")]
    class Settings: Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.settings);

            ConfManager confManager = ConfManager.GetManager;

            Switch switchInversX = FindViewById<Switch>(Resource.Id.XSwitch);
            switchInversX.CheckedChange += (o, e) =>
            {
                confManager.InversX = (o as Switch).Checked;
                confManager.SaveConfiguration();
            };

            Switch switchInversY = FindViewById<Switch>(Resource.Id.YSwitch);
            switchInversY.CheckedChange += (o, e) =>
            {
                confManager.InversY = (o as Switch).Checked;
                confManager.SaveConfiguration();
            };

            switchInversX.Checked = confManager.InversX;
            switchInversY.Checked = confManager.InversY;

            Spinner spinnerMode = FindViewById<Spinner>(Resource.Id.sensorSpinner);
            
            
        }
    }
}