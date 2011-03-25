using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Rawr.UI
{
    public partial class DG_BossAttacks : ChildWindow
    {
        static DG_BossAttacks()
        {
        }

        public DG_BossAttacks()
        {
            InitializeComponent();
        }

        public DG_BossAttacks(List<Attack> list)
        {
            InitializeComponent();

#if !SILVERLIGHT
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            this.WindowState = System.Windows.WindowState.Normal;
#endif

            TheList = list;
            SetListBox();
        }

        #region Variables
        protected List<Attack> _TheList = null;
        public List<Attack> TheList
        {
            get { return _TheList ?? (_TheList = new List<Attack>()); }
            set { _TheList = value; }
        }
        #endregion

        private void SetListBox()
        {
            LB_TheList.Items.Clear();
            foreach (Attack s in TheList)
            {
                //string str = s.ToString();
                LB_TheList.Items.Add(s);
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void BT_Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Attack retVal = new Attack()
                {
                    // Basics
                    Name = "Dynamic",
                    DamageType = (ItemDamageType)CB_DmgType.SelectedIndex,
                    DamagePerHit = (float)NUD_DmgPerHit.Value,
                    DamageIsPerc = (bool)CK_DamageIsPerc.IsChecked,
                    MaxNumTargets = (float)NUD_MaxNumTargs.Value,
                    AttackSpeed = (float)NUD_AtkSpeed.Value,
                    AttackType = (ATTACK_TYPES)CB_AtkType.SelectedIndex,
                    UseParryHaste = (bool)CK_UseParryHaste.IsChecked,
                    Interruptable = (bool)CK_Interruptable.IsChecked,
                    IsTheDefaultMelee = (bool)CK_IsDefaultMelee.IsChecked,
                    IsDualWielding = (bool)CK_IsDualWielding.IsChecked,
                    // Player Avoidances
                    Missable = (bool)CK_Missable.IsChecked,
                    Dodgable = (bool)CK_Dodgable.IsChecked,
                    Parryable = (bool)CK_Parryable.IsChecked,
                    Blockable = (bool)CK_Blockable.IsChecked,
                };
                // Targeting Includes
                retVal.AffectsRole[PLAYER_ROLES.MainTank] = (bool)CK_AffectsMTank.IsChecked;
                retVal.AffectsRole[PLAYER_ROLES.OffTank] = (bool)CK_AffectsOTank.IsChecked;
                retVal.AffectsRole[PLAYER_ROLES.TertiaryTank] = (bool)CK_AffectsTTank.IsChecked;
                retVal.AffectsRole[PLAYER_ROLES.MeleeDPS] = (bool)CK_AffectsMeleeDPS.IsChecked;
                retVal.AffectsRole[PLAYER_ROLES.RangedDPS] = (bool)CK_AffectsRangedDPS.IsChecked;
                retVal.AffectsRole[PLAYER_ROLES.MainTankHealer] = (bool)CK_AffectsMainTankHealer.IsChecked;
                retVal.AffectsRole[PLAYER_ROLES.OffAndTertTankHealer] = (bool)CK_AffectsOffTankHealer.IsChecked;
                retVal.AffectsRole[PLAYER_ROLES.RaidHealer] = (bool)CK_AffectsRaidHealer.IsChecked;
                //
                if (isEditing) {
                    // Affect your changes to the currently selected one
                    isEditing = false;
                    int index = LB_TheList.SelectedIndex;
                    TheList.RemoveAt(LB_TheList.SelectedIndex);
                    TheList.Insert(index, retVal);
                } else { TheList.Add(retVal); }
                SetListBox();
            } catch (Exception ex) {
                new Base.ErrorBox()
                {
                    Title = "Error Adding a Boss Attack",
                    Function = "BT_Add_Clicked()",
                    TheException = ex,
                }.Show();
            }
        }

        private void BT_Delete_Click(object sender, RoutedEventArgs e)
        {
            int index = LB_TheList.SelectedIndex;
            if (index == -1) { return; }
            TheList.RemoveAt(index);
            SetListBox();
        }

        bool isEditing = false;
        private void LB_TheList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LB_TheList.SelectedIndex != -1) {
                Attack selected = LB_TheList.SelectedItem as Attack;
                // Basics
                TB_Name.Text = selected.Name;
                CB_DmgType.SelectedIndex = (int)selected.DamageType;
                NUD_DmgPerHit.Value = selected.DamagePerHit;
                CK_DamageIsPerc.IsChecked = selected.DamageIsPerc;
                NUD_MaxNumTargs.Value = selected.MaxNumTargets;
                NUD_AtkSpeed.Value = selected.AttackSpeed;
                CB_AtkType.SelectedIndex = (int)selected.AttackType;
                CK_UseParryHaste.IsChecked = selected.UseParryHaste;
                CK_Interruptable.IsChecked = selected.Interruptable;
                CK_IsDefaultMelee.IsChecked = selected.IsTheDefaultMelee;
                CK_IsDualWielding.IsChecked = selected.IsDualWielding;
                // Player Avoidances
                CK_Missable.IsChecked = selected.Missable;
                CK_Dodgable.IsChecked = selected.Dodgable;
                CK_Parryable.IsChecked = selected.Parryable;
                CK_Blockable.IsChecked = selected.Blockable;
                // Targeting Includes
                CK_AffectsMTank.IsChecked = selected.AffectsRole[PLAYER_ROLES.MainTank];
                CK_AffectsOTank.IsChecked = selected.AffectsRole[PLAYER_ROLES.OffTank];
                CK_AffectsTTank.IsChecked = selected.AffectsRole[PLAYER_ROLES.TertiaryTank];
                CK_AffectsMeleeDPS.IsChecked = selected.AffectsRole[PLAYER_ROLES.MeleeDPS];
                CK_AffectsRangedDPS.IsChecked = selected.AffectsRole[PLAYER_ROLES.RangedDPS];
                CK_AffectsMainTankHealer.IsChecked = selected.AffectsRole[PLAYER_ROLES.MainTankHealer];
                CK_AffectsOffTankHealer.IsChecked = selected.AffectsRole[PLAYER_ROLES.OffAndTertTankHealer];
                CK_AffectsRaidHealer.IsChecked = selected.AffectsRole[PLAYER_ROLES.RaidHealer];
                //
                isEditing = true;
            } else {
                // Reset the UI to a blank melee attack
                // Basics
                TB_Name.Text = "Melee";
                CB_DmgType.SelectedIndex = (int)ItemDamageType.Physical;
                NUD_DmgPerHit.Value = 0;
                CK_DamageIsPerc.IsChecked = false;
                NUD_MaxNumTargs.Value = 1;
                NUD_AtkSpeed.Value = 2.0;
                CB_AtkType.SelectedIndex = (int)ATTACK_TYPES.AT_MELEE;
                CK_UseParryHaste.IsChecked = false;
                CK_Interruptable.IsChecked = false;
                CK_IsDefaultMelee.IsChecked = true;
                CK_IsDualWielding.IsChecked = false;
                // Player Avoidances
                CK_Missable.IsChecked = true;
                CK_Dodgable.IsChecked = true;
                CK_Parryable.IsChecked = true;
                CK_Blockable.IsChecked = true;
                // Targeting Includes
                CK_AffectsMTank.IsChecked = false;
                CK_AffectsOTank.IsChecked = true;
                CK_AffectsTTank.IsChecked = true;
                CK_AffectsMeleeDPS.IsChecked = true;
                CK_AffectsRangedDPS.IsChecked = true;
                CK_AffectsMainTankHealer.IsChecked = true;
                CK_AffectsOffTankHealer.IsChecked = true;
                CK_AffectsRaidHealer.IsChecked = true;
                //
                isEditing = false;
            }
        }
    }
}

