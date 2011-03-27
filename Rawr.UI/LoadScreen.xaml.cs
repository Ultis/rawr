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
using System.IO;
using System.IO.IsolatedStorage;
using System.Text;

namespace Rawr.UI
{
    public partial class LoadScreen : UserControl
    {
        private event EventHandler LoadFinished;

        private static Dictionary<string, Type> Classes;

        static LoadScreen()
        {
            Classes = new Dictionary<string, Type>();
            string dir = "";
#if !SILVERLIGHT
            dir = "ClientBin\\";
#endif
            Classes[dir + "BuffCache.xml"] = typeof(Buff);
            Classes[dir + "BuffSets.xml"] = typeof(SavedBuffSet);
            Classes[dir + "EnchantCache.xml"] = typeof(Enchant);
            Classes[dir + "ItemCache.xml"] = typeof(ItemCache);
            Classes[dir + "ItemFilter.xml"] = typeof(ItemFilter);
            Classes[dir + "PetTalents.xml"] = typeof(Hunter.SavedPetTalentSpec);
            Classes[dir + "Settings.xml"] = typeof(Settings);
            Classes[dir + "Talents.xml"] = typeof(SavedTalentSpec);
            Classes[dir + "TinkeringCache.xml"] = typeof(Tinkering);
        }

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
            foreach (KeyValuePair<string, Type> kvp in Classes)
            {
                MethodInfo info = kvp.Value.GetMethod("Save");
                if (info != null)
                {
                    StringWriter sw = new StringWriter();
                    info.Invoke(null, new object[] { sw });

                    try
                    {
                        Stream stream = FileUtils.GetFileStream(kvp.Key, true);
                        StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);
                        writer.Write(sw.ToString());
                        writer.Flush();
                        writer.Close();
                    } catch (IsolatedStorageException) {
                        // they removed permissions after it was started, just ignore it
                        // next time they will start it they'll be asked for permissions again
                    } catch (Exception ex) {
                        new Base.ErrorBox() {
                            Title = "Error Serializing the Caches",
                            Function = "LoadScreen.SaveFiles()",
                            TheException = ex,
                        }.Show();
                    }
                }
            }
        }

        private void LoadFiles()
        {
            try
            {
                if (!FileUtils.HasQuota(131072))
                {
                    IncreaseQuota iq = new IncreaseQuota(131072);
                    iq.Closed += new EventHandler(iq_Closed);
                    iq.Show();
                }
                else iq_Closed(this, EventArgs.Empty);
            } catch (IsolatedStorageException) {
                new Base.ErrorBox("Issue Checking Storage Quota",
                    "Rawr does not have permission to create a Storage Cache which is necessary to run the program.",
                    "Please check your Silverlight Settings on the Permissions Tab and remove the Deny for Rawr. This will make the webpage prompt you to allow Rawr again on Refresh.").Show();
            }
        }

        private void iq_Closed(object sender, EventArgs e)
        {
            IncreaseQuota iq = sender as IncreaseQuota;
            if (iq == null || iq.DialogResult.GetValueOrDefault(false))
            {
                Calculations.RegisterModel(typeof(Rawr.Bear.CalculationsBear));
                Calculations.RegisterModel(typeof(Rawr.Cat.CalculationsCat));
                Calculations.RegisterModel(typeof(Rawr.DPSDK.CalculationsDPSDK));
                Calculations.RegisterModel(typeof(Rawr.DPSWarr.CalculationsDPSWarr));
                Calculations.RegisterModel(typeof(Rawr.Elemental.CalculationsElemental));
                Calculations.RegisterModel(typeof(Rawr.Enhance.CalculationsEnhance));
                Calculations.RegisterModel(typeof(Rawr.Healadin.CalculationsHealadin));
                Calculations.RegisterModel(typeof(Rawr.HealPriest.CalculationsHealPriest));
                Calculations.RegisterModel(typeof(Rawr.Hunter.CalculationsHunter));
                Calculations.RegisterModel(typeof(Rawr.Mage.CalculationsMage));
                Calculations.RegisterModel(typeof(Rawr.Moonkin.CalculationsMoonkin));
                Calculations.RegisterModel(typeof(Rawr.ProtPaladin.CalculationsProtPaladin));
                Calculations.RegisterModel(typeof(Rawr.ProtWarr.CalculationsProtWarr));
                Calculations.RegisterModel(typeof(Rawr.RestoSham.CalculationsRestoSham));
                Calculations.RegisterModel(typeof(Rawr.Retribution.CalculationsRetribution));
                Calculations.RegisterModel(typeof(Rawr.Rogue.CalculationsRogue));
                Calculations.RegisterModel(typeof(Rawr.ShadowPriest.CalculationsShadowPriest));
                Calculations.RegisterModel(typeof(Rawr.TankDK.CalculationsTankDK));
                Calculations.RegisterModel(typeof(Rawr.Tree.CalculationsTree));
                Calculations.RegisterModel(typeof(Rawr.Warlock.CalculationsWarlock));

                string[] files = new List<string>(Classes.Keys).ToArray();

                FileUtils f = new FileUtils(files, progressUpdated);
                f.DownloadIfNotExists(new EventHandler(filesLoaded));
            } else {
                new Base.ErrorBox("Not Enough Storage Space", 
                      "Rawr will not work if you do not allow it to increase its available "
                    + "storage size. Please refresh this page and accept to continue.").Show();
            }
        }

        private void progressUpdated(object sender, EventArgs e)
        {
            FileUtils f = sender as FileUtils;
            TextBlockLoadProgress.Text = f.Status;
            ProgressBarLoadProgress.Value = f.Progress;
        }

        public static int RetryCount = 0;
        public static List<Exception> exes = new List<Exception>();

        private void filesLoaded(object sender, EventArgs e)
        {
            try {
                FileUtils f = sender as FileUtils;
                foreach (string file in f.Filenames)
                {
                    MethodInfo info = Classes[file].GetMethod("Load");
                    if (info != null)
                    {
                        info.Invoke(null, new object[] { new StreamReader(FileUtils.GetFileStream(file, false), Encoding.UTF8) });
                    }
                }
                if (LoadFinished != null) LoadFinished.Invoke(this, EventArgs.Empty);
            } catch (Exception ex) {
                if (RetryCount > 3) {
                    new Base.ErrorBox() {
                        Title = "Error loading a cache",
                        Function = "filesLoaded()",
                        SuggestedFix = "The Caches have failed to load three times in a row. This most likely means you either need to forcibly update to a new version of Rawr in the Browser or you need to clear your Silverlight cache (in Silverlight Settings)",
                        TheException = ex,
                    }.Show();
                    return;
                }
                exes.Add(ex);
                new Base.ErrorBox() {
                    Title = "Error loading a cache",
                    Function = "filesLoaded()",
                    TheException = ex,
                }.Show();
                RetryCount++;
                string dir = "";
#if !SILVERLIGHT
                dir = "ClientBin\\";
#endif
                new FileUtils(new string[] {
                    dir+"BuffCache.xml", 
                    dir+"BuffSets.xml", 
                    dir+"EnchantCache.xml",
                    dir+"TinkeringCache.xml",
                    dir+"ItemCache.xml",
                    dir+"ItemFilter.xml",
                    dir+"PetTalents.xml",
                    dir+"Settings.xml",
                    dir+"Talents.xml",
                    dir+"TinkeringCache.xml",
                    }).Delete();
                // We delete the bad ones and try to load files again, which should put us in the proper loop
                LoadFiles();
            }
        }
    }
}
