using UnityEngine;

namespace ChickenOut
{
    public static class IntExtentions
    {
        public static void Portal(this ref int nm, int min, int max)
        {
            if (nm > max)
                nm = min;
            else if (nm < min)
                nm = max;
        }

        public static void Next(this ref int nm, int nmMax, int amount = 1)
        {
            nm += amount;
            nm = (nm < nmMax) ? nm : 0;
        }
        public static void Before(this ref int nm, int nmMax, int amount = 1)
        {
            nm -= amount;
            nm = (nm < 0) ? nmMax : nm;
        }
    }

    public static class FloatExtentions
    {
        public static Vector2 ToVector2(this float[] floats) => new Vector2(floats[0], floats[1]);
        public static Vector3 ToVector3(this float[] floats) => new Vector3(floats[0], floats[1], floats[2]);
        public static Quaternion ToQuaternion(this float nm) => Quaternion.Euler(0, 0, nm);

        public static Vector2 GetDirectionFromAngle(this float angle) => new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        public static int GetYScale(this float angle) => (Mathf.Abs(angle) > 90) ? -1 : 1;
    }

    public static class Vector2Extentions
    {
        public static float[] ToFloats(this Vector2 vector) => new float[] { vector.x, vector.y };
        public static Vector2 With(this Vector2 vector, float? x, float? y) => new Vector2(x ?? vector.x, y ?? vector.y);

        public static float GetRadAngle(this Vector2 vector) => Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
        public static float GetDegAngle(this Vector2 vector) => Mathf.Atan2(vector.y, vector.x);

        public static Vector2 GetDirectionWithAngle(this Vector2 vector, float angle) =>
            ((angle *= Mathf.Deg2Rad) + vector.GetDegAngle()).GetDirectionFromAngle();
    }

    public static class Vector3Extentions
    {
        public static float[] ToFloats(this Vector3 vector) => new float[] { vector.x, vector.y, vector.z };
        public static Vector3 With(this Vector3 vector, float? x, float? y, float? z) =>
            new Vector3(x ?? vector.x, y ?? vector.y, z ?? vector.z);
    }

    public static class QuaternionExtentions
    {
        public static float ToFloat(this Quaternion quaternion) => quaternion.eulerAngles.z;
    }
}