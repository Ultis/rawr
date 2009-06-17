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
        public OptimizerSettings Optimzer { get; set; }

        public static void Save(TextWriter writer)
        {
            Settings current = new Settings()
            {
                Cache = CacheSettings.Default,
                General = GeneralSettings.Default,
                Network = NetworkSettings.Default,
                Optimzer = OptimizerSettings.Default
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

                if (loaded.Cache != null) CacheSettings.Default = loaded.Cache;
                if (loaded.General != null) GeneralSettings.Default = loaded.General;
                if (loaded.Network != null) NetworkSettings.Default = loaded.Network;
                if (loaded.Optimzer != null) OptimizerSettings.Default = loaded.Optimzer;
            }
            catch { }
        }

    }
}
