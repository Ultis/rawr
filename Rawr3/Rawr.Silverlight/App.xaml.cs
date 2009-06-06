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
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Grid g = new Grid();
            g.Children.Add(new SplashScreen());
            RootVisual = g;

            if (!FileUtils.HasQuota(5120))
            {
                IncreaseQuota iq = new IncreaseQuota(5120);
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
                Calculations.LoadModel(typeof(Rawr.Healadin.CalculationsHealadin));

                new FileUtils("BuffCache.xml", new EventHandler(BuffCache_Ready));
                new FileUtils("EnchantCache.xml", new EventHandler(EnchantCache_Ready));
                new FileUtils("ItemCache.xml", new EventHandler(ItemCache_Ready));
            }
            else
            {

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

        public void CheckLoadFinished()
        {
            if (!itemcacheFinished || !enchantFinished || !buffFinished) return;
            Character testChar =
                new Character("Ermad", "Bonechewer", Character.CharacterRegion.US,
                Character.CharacterRace.Human,
                new ItemInstance(46180, 41401, 42148, 0, 3820),
                new ItemInstance(44662, 40012, 0, 0, 0),
                new ItemInstance(40573, 40012, 0, 0, 3810),
                new ItemInstance(45541, 0, 0, 0, 3831),
                new ItemInstance(40569, 40012, 40012, 0, 3832),
                new ItemInstance(6833, 0, 0, 0, 0),
                new ItemInstance(5976, 0, 0, 0, 0),
                new ItemInstance(45252, 40012, 0, 0, 2332),
                new ItemInstance(40570, 42148, 40012, 0, 3246),
                new ItemInstance(45554, 40012, 40012, 40012, 0),
                new ItemInstance(40572, 42148, 40012, 0, 3721),
                new ItemInstance(45434, 40012, 0, 0, 3826),
                new ItemInstance(45235, 0, 0, 0, 0),
                new ItemInstance(45946, 40012, 0, 0, 0),
                new ItemInstance(45490, 0, 0, 0, 0),
                new ItemInstance(44255, 0, 0, 0, 0),
                new ItemInstance(40396, 0, 0, 0, 3834),
                new ItemInstance(39716, 0, 0, 0, 1128),
                new ItemInstance(40705, 0, 0, 0, 0),
                null,
                null);
            testChar.Class = Character.CharacterClass.Paladin;
            testChar.WaistBlacksmithingSocketEnabled = true;
            testChar.CalculationOptions = new Rawr.Healadin.CalculationOptionsHealadin();
            testChar.PaladinTalents = new PaladinTalents("503500520200130531005152210000000000000000000000000005032050203000000000000000.0000001000000100100000010000001010");
            testChar.CurrentModel = "Healadin";           

            //((Grid)RootVisual).Children.RemoveAt(0);
            ((Grid)RootVisual).Children.Add(new MainPage(testChar));
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
