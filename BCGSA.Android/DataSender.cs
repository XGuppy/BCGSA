using System;
using System.Numerics;
using Android.App;
using Android.Hardware;
using BCGSA.Android.ConfigMaster;

namespace BCGSA.Android
{
    public class DataSender: Activity, ISensorEventListener
    {
        private AccelerometerEntity Data { get; set; } = new AccelerometerEntity(default(Vector3), default(Vector3));
        private readonly SensorManager _manager;
        private readonly Sensor _accelerometer;
        private readonly Sensor _gyroscope;
        private (bool, bool, bool) _invers;
        private readonly object _locker = new object();
        public DataSender(SensorManager manager)
        {
            _manager = manager;
            _accelerometer = _manager.GetDefaultSensor(SensorType.LinearAcceleration);
            _gyroscope = _manager.GetDefaultSensor(SensorType.Gyroscope);
            
        }
        public void StartReading()
        {
            var configManager = ConfManager.GetManager;
            _invers.Item1 = configManager.InversX;
            _invers.Item2 = configManager.InversY;
            _invers.Item3 = configManager.InversZ;
            Enum.TryParse(configManager.ConnectMod, out SensorDelay speed);
            _manager.RegisterListener(this, _accelerometer, speed);
            _manager.RegisterListener(this, _gyroscope, speed);
        }

        public void StopReading()
        {
            _manager.UnregisterListener(this, _accelerometer);
            _manager.UnregisterListener(this, _gyroscope);
        }

        public void OnSensorChanged(SensorEvent e)
        {
            lock (_locker)
            {
                switch (e.Sensor.Type)
                {
                    case SensorType.Gyroscope:
                        Data.Gyroscope = new Vector3S(e.Values[0], e.Values[1], e.Values[2]);
                        break;
                    case SensorType.LinearAcceleration:
                        Data.Accelerometer = new Vector3S(_invers.Item1 ? e.Values[0] : -e.Values[0],
                            _invers.Item2 ? e.Values[1] : -e.Values[1], _invers.Item3 ? e.Values[2] : -e.Values[2]);
                        break;
                }

                Sended?.Invoke(Data);
            }
        }
        
        public delegate void SendAccelerometerHandler(AccelerometerEntity e);
        public event SendAccelerometerHandler Sended; // Sender event
        
        
        public void OnAccuracyChanged(Sensor sensor, SensorStatus accuracy)
        {
            //
        }
    }
}