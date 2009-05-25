using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Rawr.UserControls.Options
{
	public partial class GeneralSettings : UserControl, IOptions
	{
        string _locale = "en";

        public GeneralSettings()
		{
			InitializeComponent();
			//cannot be in load, because its possible this tab won't show, and the values will not be initialized.
			//if this happens, then the users settings will be cleared.
            checkBoxUseMultithreading.Checked = Rawr.Properties.GeneralSettings.Default.UseMultithreading;
            chbBuffSource.Checked = Rawr.Properties.GeneralSettings.Default.DisplayBuffSource;
            chbGemNames.Checked = Rawr.Properties.GeneralSettings.Default.DisplayGemNames;
            comboBoxProcEffectCalculationMode.SelectedIndex = Rawr.Properties.GeneralSettings.Default.ProcEffectMode;
            checkBoxDisplayItemIds.Checked = Rawr.Properties.GeneralSettings.Default.DisplayItemIds;
            setLocale(Rawr.Properties.GeneralSettings.Default.Locale);
        }

        private void setLocale(string locale)
        {
            _locale = locale;
            switch (locale)
            {
                case "en": 
                    rbEnglish.Checked = true;
                    break;
                case "de":
                    rbGerman.Checked = true;
                    break;
                case "fr":
                    rbFrench.Checked = true;
                    break;
                case "es":
                    rbSpanish.Checked = true;
                    break;
                case "ru":
                    rbRussian.Checked = true;
                    break;
            }
        }

		#region IOptions Members

		public void Save()
		{
            string message = string.Empty;
            string title = string.Empty;
			Rawr.Properties.GeneralSettings.Default.UseMultithreading = checkBoxUseMultithreading.Checked;
            Rawr.Properties.GeneralSettings.Default.DisplayBuffSource = chbBuffSource.Checked;
            Rawr.Properties.GeneralSettings.Default.DisplayGemNames = chbGemNames.Checked;
            Rawr.Properties.GeneralSettings.Default.DisplayItemIds = checkBoxDisplayItemIds.Checked;
            Rawr.Properties.GeneralSettings.Default.Locale = _locale;
            Rawr.Properties.GeneralSettings.Default.ProcEffectMode = comboBoxProcEffectCalculationMode.SelectedIndex;
			Rawr.Properties.GeneralSettings.Default.Save();
            switch(_locale)
            {
                case "de":
                    title = "Profil Aktualisieren";
                    message = "Um die deutsche Lokalisierung nachzuladen müssen sie auf 'Update Item Cache from Wowhead' drücken.";
                    break;
                case "fr":
                    title = "Mise à jour du Profil";
                    message = "Vous devez utiliser 'Update Item Cache from Wowhead' pour recharger articles locale pour le Français.";
                    break;
                case "es":
                    title = "Actualiza tu Perfil";
                    message = "Debe utilizar 'Update Item Cache from Wowhead' para volver a cargar los elementos de localización para el Español.";
                    break;
                case "ru":
                    title = "Обновить профиль";
                    message = "Вы должны использовать 'Update Item Cache from Wowhead' чтобы перезагрузить пунктов для Российских локаль.";
                    break;
            }
            if (!_locale.Equals("en"))
                System.Windows.Forms.MessageBox.Show(message, title, System.Windows.Forms.MessageBoxButtons.OK);
            OnDisplayBuffChanged();
            SpecialEffect.UpdateCalculationMode();
            //ItemCache.OnItemsChanged();
            FormMain.Instance.Character.OnCalculationsInvalidated();
		}

		public void Cancel()
		{
			//NOOP;
		}

		public bool HasValidationErrors()
		{
			return CheckChildrenValidation(this);
		}

		private bool CheckChildrenValidation(Control control)
		{
			bool invalid = false;

			for (int i = 0; i < control.Controls.Count; i++)
			{
				if (!String.IsNullOrEmpty(errorProvider1.GetError(control.Controls[i])))
				{
					invalid = true;
					break;
				}
				else
				{
					invalid = CheckChildrenValidation(control.Controls[i]);
					if (invalid)
					{
						break;
					}
				}
			}

			return invalid;
		}

		public string DisplayName
		{
			get { return "General Settings"; }
		}


		public string TreePosition
		{
			get { return DisplayName; }
		}

		public Image MenuIcon
		{
			get { return null; }
		}

		#endregion

        private void rbEnglish_CheckedChanged(object sender, EventArgs e)
        {
            _locale = "en";
        }

        private void rbGerman_CheckedChanged(object sender, EventArgs e)
        {
            _locale = "de";
        }

        private void rbFrench_CheckedChanged(object sender, EventArgs e)
        {
            _locale = "fr";
        }

        private void rbSpanish_CheckedChanged(object sender, EventArgs e)
        {
            _locale = "es";
        }

        private void rbRussian_CheckedChanged(object sender, EventArgs e)
        {
            _locale = "ru";
        }

        public static event EventHandler DisplayBuffChanged;
        protected static void OnDisplayBuffChanged()
        {
            if (DisplayBuffChanged != null)
                DisplayBuffChanged(null, EventArgs.Empty);
        }
    }
}
