using System;

using Android.Content;
using Android.Bluetooth;

namespace BCGSA.Android
{
    /// <summary>
    /// Work with bluetooth devices
    /// </summary>
    public class BluetoothDeviceReceiver: BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            var action = intent.Action;

            if (action == BluetoothDevice.ActionFound)
            {
                var device = (BluetoothDevice)intent.GetParcelableExtra(BluetoothDevice.ExtraDevice);

                if (device.BondState != Bond.Bonded)
                {
                    // some work...
                }
            }
        }
    }
}