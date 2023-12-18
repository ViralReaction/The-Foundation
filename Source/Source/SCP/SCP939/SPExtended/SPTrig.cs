using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace SCP
{
    public static class SPTrig
    {
        public static SPTuples.SPTuple2<float, float> RotatePointClockwise(
          float x,
          float y,
          float theta)
        {
            theta = -theta;
            return new SPTuples.SPTuple2<float, float>(x * (float)Math.Cos(theta.DegreesToRadians()) - y * (float)Math.Sin(theta.DegreesToRadians()), x * (float)Math.Sin(theta.DegreesToRadians()) + y * (float)Math.Cos(theta.DegreesToRadians()));
        }

        public static SPTuples.SPTuple2<float, float> RotatePointCounterClockwise(
          float x,
          float y,
          float theta)
        {
            return new SPTuples.SPTuple2<float, float>(x * (float)Math.Cos(theta.DegreesToRadians()) - y * (float)Math.Sin(theta.DegreesToRadians()), x * (float)Math.Sin(theta.DegreesToRadians()) + y * (float)Math.Cos(theta.DegreesToRadians()));
        }

        public static double DegreesToRadians(this double deg) => deg * Math.PI / 180.0;

        public static double DegreesToRadians(this float deg) => Convert.ToDouble(deg).DegreesToRadians();

        public static double RadiansToDegrees(this double radians) => radians * (180.0 / Math.PI);

        public static double AngleThroughOrigin(this IntVec3 c, Map map)
        {
            int num1 = c.x - map.Size.x / 2;
            double num2 = Math.Abs(Math.Atan((double)(c.z - map.Size.z / 2) / (double)num1).RadiansToDegrees());
            switch (SPExtra.Quadrant.QuadrantOfIntVec3(c, map).AsInt)
            {
                case 2:
                    return 360.0 - num2;
                case 3:
                    return 180.0 + num2;
                case 4:
                    return 180.0 - num2;
                default:
                    return num2;
            }
        }

        public static double AngleToPoint(this IntVec3 pos, IntVec3 point, Map map)
        {
            int num = pos.x - point.x;
            double point1 = Math.Abs(Math.Atan((double)(pos.z - point.z) / (double)num).RadiansToDegrees());
            switch (SPExtra.Quadrant.QuadrantRelativeToPoint(pos, point, map).AsInt)
            {
                case 2:
                    return 360.0 - point1;
                case 3:
                    return 180.0 + point1;
                case 4:
                    return 180.0 - point1;
                default:
                    return point1;
            }
        }

        public static int LeftRightOfLine(IntVec3 A, IntVec3 B, IntVec3 C) => Math.Sign((B.x - A.x) * (C.z - A.z) - (B.z - A.z) * (C.x - A.x));

        public static IntVec3 PointFromOrigin(double angle, Map map)
        {
            int x = map.Size.x;
            int z = map.Size.z;
            if (angle < 0.0 || angle > 360.0)
                return IntVec3.Invalid;
            Rot4 invalid = Rot4.Invalid;
            Rot4 rot4;
            if (angle <= 45.0 || angle > 315.0)
                rot4 = Rot4.East;
            else if (angle <= 135.0 && angle >= 45.0)
                rot4 = Rot4.North;
            else if (angle <= 225.0 && angle >= 135.0)
            {
                rot4 = Rot4.West;
            }
            else
            {
                if (angle > 315.0 || angle < 225.0)
                    return new IntVec3(z / 2, 0, 1);
                rot4 = Rot4.South;
            }
            double num = Math.Tan(angle.DegreesToRadians());
            switch (rot4.AsInt)
            {
                case 0:
                    return new IntVec3((int)((double)z / (2.0 * num) + (double)(z / 2)), 0, z - 1);
                case 1:
                    return new IntVec3(x - 1, 0, (int)((double)(x / 2) * num) + x / 2);
                case 2:
                    return new IntVec3((int)((double)z - ((double)z / (2.0 * num) + (double)(z / 2))), 0, 1);
                case 3:
                    return new IntVec3(1, 0, (int)((double)x - ((double)(x / 2) * num + (double)(x / 2))));
                default:
                    return IntVec3.Invalid;
            }
        }
    }
}