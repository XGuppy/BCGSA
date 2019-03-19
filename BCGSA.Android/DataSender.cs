using System;
using BCGSA.ConfigMaster;
using Android.Hardware;
using System.Numerics;
using Android.Runtime;
using System.Threading.Tasks;

namespace BCGSA
{
    public class DataSender: Java.Lang.Object, ISensorEventListener
    {
        /// <summary>
        /// Send an accelerometer data
        /// </summary>
        private AccelerometerEntity Data { get; set; } = new AccelerometerEntity(default(Vector3), default(Vector3));
        public DataSender(SensorManager sensorManager)
        {
            var manager = ConfManager.GetManager;
            Enum.TryParse(manager.ConnectMod, out SensorDelay speed);
            sensorManager.RegisterListener(this, sensorManager.GetDefaultSensor(SensorType.Gyroscope), speed);
            sensorManager.RegisterListener(this, sensorManager.GetDefaultSensor(SensorType.LinearAcceleration), speed);
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
            Sended?.Invoke(Sended, Data);
        }

        public delegate void SendAccelerometerHandler(SendAccelerometerHandler dataSendEvent, AccelerometerEntity e);
        public event SendAccelerometerHandler Sended; // Sender event
    }
}
