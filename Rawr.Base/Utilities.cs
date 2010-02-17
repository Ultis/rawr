using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace Rawr
{
    public static class Utilities
    {

        private const int BitsPerUInt32 = 32;


        /// <summary>
        /// Make a deep clone of the object
        /// </summary>
        /// <typeparam name="T">The type of object to be cloned</typeparam>
        /// <param name="obj">The object to be cloned</param>
        /// <returns>A clone of the object</returns>
        public static T Clone<T>(T obj)
        {
            T ret = default(T);
            if (obj != null)
            {
                //for deep cloning, this should be a binary serializer (which requires objects be marked as serializable; however,
                //we save our objects back out to disk with xml and don't store
                //any interal state that would be mised with xml serialization, so this should be fine.
                XmlSerializer cloner = new XmlSerializer(typeof(T));
                MemoryStream stream = new MemoryStream();
                cloner.Serialize(stream, obj);
                stream.Seek(0, SeekOrigin.Begin);
                ret = (T)cloner.Deserialize(stream);
            }
            return ret;
        }

        public static uint RotateLeft(uint value, int rotationBitCount)
        {
            int effectiveRotationBitCount = rotationBitCount % BitsPerUInt32;
            if (effectiveRotationBitCount < 0)
                effectiveRotationBitCount = BitsPerUInt32 - effectiveRotationBitCount;

            if (effectiveRotationBitCount == 0)
                return value;

            return (value << rotationBitCount) | (value >> (BitsPerUInt32 - rotationBitCount));
        }

        public static uint RotateRight(uint value, int rotationBitCount)
        {
            // We cannot just use -rotationBitCount to avoid overflow on int.MinValue
            return RotateLeft(value, -(rotationBitCount % BitsPerUInt32));
        }

        public static int RotateLeft(int value, int rotationBitCount)
        {
            return (int)RotateLeft((uint)value, rotationBitCount);
        }

        public static int RotateRight(int value, int rotationBitCount)
        {
            return (int)RotateRight((uint)value, rotationBitCount);
        }

        public static int CombineHashCodes(IEnumerable<int> hashCodes)
        {
            if (hashCodes == null)
                throw new ArgumentNullException("hashCodes");

            int combinedHashCode = 0;
            foreach (var hashCode in hashCodes)
                combinedHashCode = RotateLeft(combinedHashCode, 23) ^ hashCode;

            return combinedHashCode;
        }

        public static int CombineHashCodes(params int[] hashCodes)
        {
            return CombineHashCodes((IEnumerable<int>)hashCodes);
        }

        public static int GetCombinedHashCode(IEnumerable<object> objects)
        {
            if (objects == null)
                throw new ArgumentNullException("objects");

            List<int> hashCodes = new List<int>();
            foreach (object anObject in objects)
                hashCodes.Add(anObject == null ? 0 : anObject.GetHashCode());

            return CombineHashCodes(hashCodes);
        }

        public static int GetCombinedHashCode(params object[] objects)
        {
            return GetCombinedHashCode((IEnumerable<object>)objects);
        }

        public static int GetArrayHashCode<T>(T[] array)
        {
            if (array == null)
                return 0;

            object[] values = new object[array.Length];
            Array.Copy(array, values, array.Length);

            return GetCombinedHashCode(values);
        }

    }

}
