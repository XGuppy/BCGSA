using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BCGSA.ConfigMaster;
using Xamarin.Essentials;
using System.Numerics;

namespace BCGSA
{
    public static class DataSender
    {
        /// <summary>
        /// Send an accelerometer data
        /// </summary>
        public static AccelerometerEntity Data { get; private set; }

        public static void Initialize()
        {
            var manager = ConfManager.GetManager;
            Enum.TryParse(manager.ConnectMod, out SensorSpeed speed);
            Accelerometer.Start(speed);
            Gyroscope.Start(speed);

            Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
            Gyroscope.ReadingChanged += Gyroscope_ReadingChanged;
        }

        private static void Gyroscope_ReadingChanged(object sender, GyroscopeChangedEventArgs e)
        {
            Data.Gyroscope = e.Reading.AngularVelocity;
            Sended?.Invoke(Data);
        }

        private static void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            Data.Accelerometer = e.Reading.Acceleration;
            Sended?.Invoke(Data);
        }

        public delegate void SendAccelerometerHandler(AccelerometerEntity e);
        public static event SendAccelerometerHandler Sended; // Sender event
    }
}
