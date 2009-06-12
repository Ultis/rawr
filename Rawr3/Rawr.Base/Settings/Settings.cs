using System;
using Rawr.Properties;
using System.IO;
using System.Xml.Serialization;

namespace Rawr
{
    public class Settings
    {
        public CacheSettings Cache { get; set; }
        public GeneralSettings General { get; set; }
        public NetworkSettings Network { get; set; }

        public static void Save(TextWriter writer)
        {
            Settings current = new Settings()
            {
                Cache = CacheSettings.Default,
                General = GeneralSettings.Default,
                Network = NetworkSettings.Default
            };
            XmlSerializer serializer = new XmlSerializer(typeof(Settings));
            serializer.Serialize(writer, current);
            writer.Close();
        }

        public static void Load(TextReader reader)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                Settings loaded = (Settings)serializer.Deserialize(reader);
                reader.Close();

                CacheSettings.Default = loaded.Cache;
                GeneralSettings.Default = loaded.General;
                NetworkSettings.Default = loaded.Network;
            }
            catch { }
        }

    }
}
