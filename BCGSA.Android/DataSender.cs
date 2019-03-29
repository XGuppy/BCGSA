using System;
using System.Numerics;
using Android.App;
using BCGSA.ConfigMaster;
using Android.Hardware;

namespace BCGSA.Android
{
    public class DataSender: Activity, ISensorEventListener
    {
        private AccelerometerEntity Data { get; set; } = new AccelerometerEntity(default(Vector3), default(Vector3));
        private SensorManager _manager;
        private Sensor _accelerometer;
        private Sensor _gyroscope;
        public DataSender(SensorManager manager)
        {
            _manager = manager;
            _accelerometer = _manager.GetDefaultSensor(SensorType.LinearAcceleration);
            _gyroscope = _manager.GetDefaultSensor(SensorType.Gyroscope);
            
        }
        public void StartReading()
        {
            var configManager = ConfManager.GetManager;
            Enum.TryParse(configManager.ConnectMod, out SensorDelay _speed);
            _manager.RegisterListener(this, _accelerometer, _speed);
            _manager.RegisterListener(this, _gyroscope, _speed);
        }

        public void StopReading()
        {
            _manager.UnregisterListener(this, _accelerometer);
            _manager.UnregisterListener(this, _gyroscope);
        }

        public void OnSensorChanged(SensorEvent e)
        {
            switch (e.Sensor.Type)
            {
                case SensorType.Gyroscope:
                    Data.Gyroscope = new Vector3S(e.Values[0], e.Values[1], e.Values[2]);
                    break;
                case SensorType.LinearAcceleration:
                    Data.Accelerometer = new Vector3S(e.Values[0], e.Values[1], e.Values[2]);
                    break;
            }
            Sended?.Invoke(Data);
        }
        
        public delegate void SendAccelerometerHandler(AccelerometerEntity e);
        public event SendAccelerometerHandler Sended; // Sender event
        
        
        public void OnAccuracyChanged(Sensor sensor, SensorStatus accuracy)
        {
            //
        }
    }
}