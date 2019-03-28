using System;
using System.Numerics;
using System.Threading.Tasks;
using BCGSA.ConfigMaster;
using Xamarin.Essentials;

namespace BCGSA.Android
{
    public class DataSender
    {
        private AccelerometerEntity Data { get; set; } = new AccelerometerEntity(default(Vector3), default(Vector3));
        private Vector4 _bufVector;
        private SensorSpeed _speed;
        
        public DataSender()
        {
            Gyroscope.ReadingChanged += GyroscopeOnReadingChanged;
            OrientationSensor.ReadingChanged += OrientationSensorOnReadingChanged;
            Accelerometer.ReadingChanged += AccelerometerOnReadingChanged;
        }

        private void AccelerometerOnReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            Data.Accelerometer = new Vector3S(e.Reading.Acceleration.X - _bufVector.X / _bufVector.W,
                e.Reading.Acceleration.Y - _bufVector.Y / _bufVector.W, e.Reading.Acceleration.Z - _bufVector.Z / _bufVector.W);
            Sended?.Invoke(Data);
        }

        private void OrientationSensorOnReadingChanged(object sender, OrientationSensorChangedEventArgs e)
        {
            _bufVector.X = e.Reading.Orientation.X;
            _bufVector.Y = e.Reading.Orientation.Y;
            _bufVector.Z = e.Reading.Orientation.Z;
            _bufVector.W = e.Reading.Orientation.W;
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
            Enum.TryParse(manager.ConnectMod, out _speed);
            Accelerometer.Start(_speed);
            Gyroscope.Start(_speed);
            OrientationSensor.Start(_speed);
        }
        
        public void StopReading()
        {
            Accelerometer.Stop();
            Gyroscope.Stop();
            OrientationSensor.Stop();
        }
        
        
        
        public delegate void SendAccelerometerHandler(AccelerometerEntity e);
        public event SendAccelerometerHandler Sended; // Sender event
    }
}