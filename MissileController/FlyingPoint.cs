using System;
using VRageMath;

namespace IngameScript
{
    class FlyingPoint
    {
        public readonly Vector3D Point;
        
        /// <summary>
        /// May be null
        /// </summary>
        public readonly Action WhenReached;
        
        private double _sqrtlen = Double.NaN;
        
        public double LengthSquared => _sqrtlen == Double.NaN ? _sqrtlen = Point.LengthSquared() : _sqrtlen;
        
        public FlyingPoint(Vector3D point, Action whenReached = null)
        {
            Point = point;
            WhenReached = whenReached;
        }
    }
}