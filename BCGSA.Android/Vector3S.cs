using System;

namespace BCGSA
{
    [Serializable]
    public struct Vector3S
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Vector3S(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}