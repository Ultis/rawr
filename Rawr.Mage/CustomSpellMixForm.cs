using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace Rawr.Mage
{
    public partial class CustomSpellMixForm : Form
    {
        public CustomSpellMixForm(List<SpellWeight> spellMix)
        {
            InitializeComponent();

            Spell.DataSource = GetValuesAndDescription();
            Spell.DisplayMember = "Value";
            Spell.ValueMember = "Key";

            bindingSourceSpellMix.DataSource = spellMix;
        }

        /// <summary>
        /// Get the description attribute for one enum value
        /// </summary>
        /// <param name="value">Enum value</param>
        /// <returns>The description attribute of an enum, if any</returns>
        private static string GetDescription(Enum value)
        {
            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return (attributes.Length > 0) ? attributes[0].Description : null;
        }

        /// <summary>
        /// Gets a list of key/value pairs for an enum, using the description attribute as value
        /// </summary>
        /// <param name="enumType">typeof(your enum type)</param>
        /// <returns>A list of KeyValuePairs with enum values and descriptions</returns>
        private static List<KeyValuePair<CycleId, string>> GetValuesAndDescription()
        {
            List<KeyValuePair<CycleId, string>> kvPairList = new List<KeyValuePair<CycleId, string>>();

            foreach (CycleId enumValue in Enum.GetValues(typeof(CycleId)))
            {
                string description = GetDescription(enumValue);
                if (description != null)
                {
                    kvPairList.Add(new KeyValuePair<CycleId, string>(enumValue, description));
                }
            }

            kvPairList.Sort((x, y) =>
                {
                    bool xnone = (x.Key == CycleId.None);
                    bool ynone = (y.Key == CycleId.None);
                    int comp = xnone.CompareTo(ynone);
                    if (comp != 0) return -comp;
                    return x.Value.CompareTo(y.Value);
                });
            return kvPairList;
        }

        private void bindingSourceSpellMix_AddingNew(object sender, AddingNewEventArgs e)
        {
            e.NewObject = new SpellWeight() { Spell = ((List<KeyValuePair<CycleId, string>>)Spell.DataSource)[0].Key };
        }
    }
}
