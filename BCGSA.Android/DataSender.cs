using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BCGSA.ConfigMaster;
using Xamarin.Essentials;
using Android.Hardware;
using System.Numerics;
using Android.Runtime;

namespace BCGSA
{
    public class DataSender: Java.Lang.Object, ISensorEventListener
    {
        /// <summary>
        /// Send an accelerometer data
        /// </summary>
        private AccelerometerEntity _data { get; set; } = new AccelerometerEntity(default(Vector3), default(Vector3));
        public DataSender(SensorManager sensorManager)
        {
            var manager = ConfManager.GetManager;
            Enum.TryParse(manager.ConnectMod, out SensorDelay speed);
            var linearSensor = sensorManager.GetDefaultSensor(SensorType.LinearAcceleration);
            var gyroSensor = sensorManager.GetDefaultSensor(SensorType.Gyroscope);
            sensorManager.RegisterListener(this, gyroSensor, speed);
            sensorManager.RegisterListener(this, linearSensor, speed);
        }

        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
        {
            
        }

        public void OnSensorChanged(SensorEvent e)
        {
            if (e.Sensor.Type == SensorType.LinearAcceleration)
            {
                _data.Accelerometer = AccelerometerEntity.FromVector3(new Vector3(e.Values[0], e.Values[1], e.Values[2]));
            }
            else if (e.Sensor.Type == SensorType.Gyroscope)
            {
                _data.Gyroscope = AccelerometerEntity.FromVector3(new Vector3(e.Values[0], e.Values[1], e.Values[2]));
            }
            Sended?.Invoke(_data);
        }

        public delegate void SendAccelerometerHandler(AccelerometerEntity e);
        public event SendAccelerometerHandler Sended; // Sender event
    }
}
