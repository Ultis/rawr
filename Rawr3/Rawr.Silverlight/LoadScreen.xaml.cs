using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Reflection;
using System.Collections.Generic;

namespace Rawr.Silverlight
{
	public partial class LoadScreen : UserControl
	{
        private event EventHandler LoadFinished;

        private Dictionary<string, MethodInfo> Loaders;
        private static Dictionary<string, MethodInfo> Savers;

        public LoadScreen()
        {
            // Required to initialize variables
            InitializeComponent();
        }

        public void StartLoading(EventHandler callback)
        {
            LoadFinished += callback;
            LoadFiles();
        }

        public static void SaveFiles()
        {
            Savers = new Dictionary<string, MethodInfo>();
            Savers["Talents.xml"] = typeof(SavedTalentSpec).GetMethod("Save");
            Savers["EnchantCache.xml"] = typeof(Enchant).GetMethod("Save");
            Savers["ItemCache.xml"] = typeof(ItemCache).GetMethod("Save");
            Savers["BuffCache.xml"] = typeof(Buff).GetMethod("Save");
            Savers["ItemSource.xml"] = typeof(LocationFactory).GetMethod("Save");
            Savers["ItemFilter.xml"] = typeof(ItemFilter).GetMethod("Save");

            foreach (KeyValuePair<string, MethodInfo> kvp in Savers)
            {
                FileUtils f = new FileUtils(kvp.Key);
                kvp.Value.Invoke(null, new object[] { f.Writer });
            }
        }

        private void LoadFiles()
        {
            Loaders = new Dictionary<string, MethodInfo>();
            Loaders["Talents.xml"] = typeof(SavedTalentSpec).GetMethod("Load");
            Loaders["EnchantCache.xml"] = typeof(Enchant).GetMethod("Load");
            Loaders["ItemCache.xml"] = typeof(ItemCache).GetMethod("Load");
            Loaders["BuffCache.xml"] = typeof(Buff).GetMethod("Load");
            Loaders["ItemSource.xml"] = typeof(LocationFactory).GetMethod("Load");
            Loaders["ItemFilter.xml"] = typeof(ItemFilter).GetMethod("Load");


            if (!FileUtils.HasQuota(20480))
            {
                IncreaseQuota iq = new IncreaseQuota(20480);
                iq.Closed += new EventHandler(iq_Closed);
                iq.Show();
            }
            else iq_Closed(this, EventArgs.Empty);
        }

        private void iq_Closed(object sender, EventArgs e)
        {
            IncreaseQuota iq = sender as IncreaseQuota;
            if (iq == null || iq.DialogResult.GetValueOrDefault(false))
            {
                Calculations.RegisterModel(typeof(Rawr.Retribution.CalculationsRetribution));
                Calculations.RegisterModel(typeof(Rawr.Healadin.CalculationsHealadin));
                Calculations.RegisterModel(typeof(Rawr.Mage.CalculationsMage));
                Calculations.RegisterModel(typeof(Rawr.Bear.CalculationsBear));
                Calculations.RegisterModel(typeof(Rawr.Cat.CalculationsCat));
                Calculations.RegisterModel(typeof(Rawr.Rogue.CalculationsRogue));
                Calculations.RegisterModel(typeof(Rawr.DPSWarr.CalculationsDPSWarr));
                Calculations.LoadModel(typeof(Rawr.Healadin.CalculationsHealadin));

                string[] keys = new string[Loaders.Count];
                Loaders.Keys.CopyTo(keys, 0);

                foreach (string s in keys)
                {
                    FileUtils f = new FileUtils(s);
                    f.DownloadIfNotExists(new EventHandler(fileLoaded));
                }
            }
            else
            {
                new ErrorWindow()
                {
                    Message = "Rawr will not work if you do not allow it to increase its available"
                        + "storage size. Please referesh this page and accept to continue."
                }.Show();
            }
        }

        private void fileLoaded(object sender, EventArgs e)
        {
            FileUtils f = sender as FileUtils;
            Loaders[f.Filename].Invoke(null, new object[] { f.Reader } );
            Loaders.Remove(f.Filename);
            CheckLoadFinished();
        }

        public void CheckLoadFinished()
        {
            if (Loaders.Count == 0)
            {
                if (LoadFinished != null) LoadFinished.Invoke(this, EventArgs.Empty);
            }
        }
	}
}