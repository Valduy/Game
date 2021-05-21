using System;
using ECS.Serialization.Readers;
using ECS.Serialization.Writers;
using UnityEngine;

namespace Assets.Scripts.Networking.Serializers
{
    public static class ConverterHelper
    {
        #region Vector2.

        public static void WriteVector2(Vector2 vector2, ISequentialWriter writer)
        {
            writer.WriteFloat(vector2.x);
            writer.WriteFloat(vector2.y);
        }

        public static void WriteVector2(Vector2 vector2, int digits, ISequentialWriter writer)
        {
            writer.WriteFloat((float)Math.Round(vector2.x, digits, MidpointRounding.AwayFromZero));
            writer.WriteFloat((float)Math.Round(vector2.y, digits, MidpointRounding.AwayFromZero));
        }

        public static Vector2 ReadVector2(ISequentialReader reader)
            => new Vector2(reader.ReadFloat(), reader.ReadFloat());

        #endregion

        #region Vector3.
        
        public static void WriteVector3(Vector3 vector3, ISequentialWriter writer)
        {
            writer.WriteFloat(vector3.x);
            writer.WriteFloat(vector3.y);
            writer.WriteFloat(vector3.z);
        }

        public static void WriteVector3(Vector3 vector3, int digits, ISequentialWriter writer)
        {
            writer.WriteFloat((float)Math.Round(vector3.x, digits, MidpointRounding.AwayFromZero));
            writer.WriteFloat((float)Math.Round(vector3.y, digits, MidpointRounding.AwayFromZero));
            writer.WriteFloat((float)Math.Round(vector3.z, digits, MidpointRounding.AwayFromZero));
        }

        public static Vector3 ReadVector3(ISequentialReader reader)
            => new Vector3(reader.ReadFloat(), reader.ReadFloat(), reader.ReadFloat());

        #endregion

        #region Quaternion.

        public static void WriteQuaternion(Quaternion quaternion, ISequentialWriter writer)
        {
            writer.WriteFloat(quaternion.x);
            writer.WriteFloat(quaternion.y);
            writer.WriteFloat(quaternion.z);
            writer.WriteFloat(quaternion.w);
        }

        public static Quaternion ReadQuaternion(ISequentialReader reader)
            => new Quaternion(reader.ReadFloat(), reader.ReadFloat(), reader.ReadFloat(), reader.ReadFloat());

        #endregion
    }
}
