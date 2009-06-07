using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Rawr.Silverlight
{
    public partial class App : Application
    {

        public static Application CurrentApplication { get; set; }

        public App()
        {
            CurrentApplication = this;
            this.Startup += this.Application_Startup;
            this.Exit += this.Application_Exit;
            this.UnhandledException += this.Application_UnhandledException;

            InitializeComponent();
        }

        private bool enchantFinished;
        private bool buffFinished;
        private bool itemcacheFinished;
        private bool talentsFinished;
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Grid g = new Grid();
            g.Children.Add(new SplashScreen());
            RootVisual = g;

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
				Calculations.LoadModel(typeof(Rawr.Healadin.CalculationsHealadin));

                new FileUtils("BuffCache.xml", new EventHandler(BuffCache_Ready));
                new FileUtils("EnchantCache.xml", new EventHandler(EnchantCache_Ready));
                new FileUtils("ItemCache.xml", new EventHandler(ItemCache_Ready));
                new FileUtils("Talents.xml", new EventHandler(Talents_Ready));
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

        private void BuffCache_Ready(object sender, EventArgs e)
        {
            FileUtils f = sender as FileUtils;
            buffFinished = true;
            Buff.Load(f.Reader);
            CheckLoadFinished();
        }

        private void EnchantCache_Ready(object sender, EventArgs e)
        {
            FileUtils f = sender as FileUtils;
            enchantFinished = true;
            Enchant.Load(f.Reader);
            CheckLoadFinished();
        }

        private void ItemCache_Ready(object sender, EventArgs e)
        {
            FileUtils f = sender as FileUtils;
            itemcacheFinished = true;
            ItemCache.Load(f.Reader);
            CheckLoadFinished();
        }

        private void Talents_Ready(object sender, EventArgs e)
        {
            FileUtils f = sender as FileUtils;
            talentsFinished = true;
            SavedTalentSpec.Load(f.Reader);
            CheckLoadFinished();
        }

        public void CheckLoadFinished()
        {
            if (!itemcacheFinished || !enchantFinished || !buffFinished || !talentsFinished) return;

            ((Grid)RootVisual).Children.RemoveAt(0);
            ((Grid)RootVisual).Children.Add(new MainPage());
        }

        private void Application_Exit(object sender, EventArgs e)
        {

        }
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            // If the app is running outside of the debugger then report the exception using
            // the browser's exception mechanism. On IE this will display it a yellow alert 
            // icon in the status bar and Firefox will display a script error.
            if (!System.Diagnostics.Debugger.IsAttached)
            {

                // NOTE: This will allow the application to continue running after an exception has been thrown
                // but not handled. 
                // For production applications this error handling should be replaced with something that will 
                // report the error to the website and stop the application.
                e.Handled = true;
                Deployment.Current.Dispatcher.BeginInvoke(delegate { ReportErrorToDOM(e); });
            }
        }
        private void ReportErrorToDOM(ApplicationUnhandledExceptionEventArgs e)
        {
            try
            {
                string errorMsg = e.ExceptionObject.Message + e.ExceptionObject.StackTrace;
                errorMsg = errorMsg.Replace('"', '\'').Replace("\r\n", @"\n");

                System.Windows.Browser.HtmlPage.Window.Eval("throw new Error(\"Unhandled Error in Silverlight Application " + errorMsg + "\");");
            }
            catch (Exception)
            {
            }
        }
    }
}
