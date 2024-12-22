using System;
using VRageMath;

namespace IngameScript
{
    public static class GpsParser
    {
        public static Vector3D Parse(string gps)
        {
            if (string.IsNullOrWhiteSpace(gps))
                throw new ArgumentException("GPS string cannot be null or empty.", nameof(gps));

            var parts = gps.Split(':');
            if (parts.Length < 5)
                throw new FormatException("Invalid GPS format.");

            double x, y, z;
            if (!double.TryParse(parts[2], out x) ||
                !double.TryParse(parts[3], out y) ||
                !double.TryParse(parts[4], out z))
                throw new FormatException("Failed to parse GPS coordinates as numbers.");

            return new Vector3D(x, y, z);
        }
    }
}