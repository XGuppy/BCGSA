using System;
using System.Numerics;
using BCGSA.ConfigMaster;
using Xamarin.Essentials;

namespace BCGSA.Android
{
    public class DataSender
    {
        private AccelerometerEntity Data { get; set; } = new AccelerometerEntity(default(Vector3), default(Vector3));
        private Vector3 _bufVector;
        private SensorSpeed _speed;
        
        public DataSender()
        {
            Gyroscope.ReadingChanged += GyroscopeOnReadingChanged;
            OrientationSensor.ReadingChanged += OrientationSensorOnReadingChanged;
            Accelerometer.ReadingChanged += AccelerometerOnReadingChanged;
        }

        private void AccelerometerOnReadingChanged(object sender, AccelerometerChangedEventArgs e) =>
            Data.Accelerometer = new Vector3S(e.Reading.Acceleration.X - _bufVector.X * 9.81f, 
                e.Reading.Acceleration.Y - _bufVector.Y * 9.81f, e.Reading.Acceleration.Z - _bufVector.Z * 9.81f);

        private void OrientationSensorOnReadingChanged(object sender, OrientationSensorChangedEventArgs e)
        {
            _bufVector.X = e.Reading.Orientation.X;
            _bufVector.Y = e.Reading.Orientation.Y;
            _bufVector.Z = e.Reading.Orientation.Z;
        }

        private void GyroscopeOnReadingChanged(object sender, GyroscopeChangedEventArgs e)
        {
            var dataSensor = e.Reading.AngularVelocity;
            Data.Gyroscope = AccelerometerEntity.FromVector3(dataSensor);
            Sended?.Invoke(Data);
        }
        
        
        public void StartReading()
        {
            var manager = ConfManager.GetManager;
            Enum.TryParse(manager.ConnectMod, out SensorSpeed _speed);
            Gyroscope.Start(_speed);
            OrientationSensor.Start(_speed);
            Accelerometer.Start(_speed);
        }
        
        public void StopReading()
        {
            Gyroscope.Stop();
            OrientationSensor.Stop();
            Accelerometer.Stop();
        }
        
        
        
        public delegate void SendAccelerometerHandler(AccelerometerEntity e);
        public event SendAccelerometerHandler Sended; // Sender event
    }
}