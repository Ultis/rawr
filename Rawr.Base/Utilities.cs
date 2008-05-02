using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace Rawr
{
    public static class Utilities
    {
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
    }
}
