using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

using System.Runtime.Serialization.Formatters.Binary;

namespace BCGSA
{
    [Serializable()]
    public class AccelerometerEntity
    {
        /// <summary>
        /// Stores data for accelerometer and gyroscope
        /// </summary>
        public Vector3S Accelerometer { get; set; } // Accelerometer sensor data
        public Vector3S Gyroscope { get; set; } // Gyroscope sensor data

        public static Vector3S FromVector3(Vector3 vector)
        {
            return new Vector3S(vector.X, vector.Y, vector.Z);
        }


        public AccelerometerEntity(Vector3 accelerometer, Vector3 gyroscope)
        {
            Accelerometer = FromVector3(accelerometer);
            Gyroscope = FromVector3(gyroscope);
        }
    }
}
