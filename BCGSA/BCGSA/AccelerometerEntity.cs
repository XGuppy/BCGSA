using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

using System.Runtime.Serialization.Formatters.Binary;

namespace BCGSA
{
    [Serializable]
    public class AccelerometerEntity
    {
        /// <summary>
        /// Stores data for accelerometer and gyroscope
        /// </summary>
        public Vector3 Accelerometer { get; set; } // Accelerometer sensor data
        public Vector3 Gyroscope { get; set; } // Gyroscope sensor data

        public AccelerometerEntity(Vector3 accelerometer, Vector3 gyroscope)
        {
            Accelerometer = accelerometer;
            Gyroscope = gyroscope;
        }
    }
}
