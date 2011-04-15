using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace Rawr.UI
{
    public partial class TalentPicker : UserControl
    {
        private Character character;
        public Character Character
        {
            get { return character; }
            set
            {
                if (character != null) { character.ClassChanged -= new EventHandler(character_ClassChanged); }
                character = value;
                if (character != null)
                {
                    character.ClassChanged += new EventHandler(character_ClassChanged);
                    character_ClassChanged(this, EventArgs.Empty);
                }
            }
        }

        public void RefreshSpec() { character_ClassChanged(this, EventArgs.Empty); }
        private void character_ClassChanged(object sender, EventArgs e)
        {
            Tree1.Talents = Character.CurrentTalents;
            TreeTab1.Header = Tree1.TreeName;
            Tree2.Talents = Character.CurrentTalents;
            TreeTab2.Header = Tree2.TreeName;
            Tree3.Talents = Character.CurrentTalents;
            TreeTab3.Header = Tree3.TreeName;
            Glyph.Character = Character;
            
            UpdateSavedSpecs();
        }

        public TalentPicker()
        {
            // Required to initialize variables
            InitializeComponent();
            Tree1.Tree = 0;
            Tree2.Tree = 1;
            Tree3.Tree = 2;

#if SILVERLIGHT
            Scroll1.SetIsMouseWheelScrollingEnabled(true);
            Scroll2.SetIsMouseWheelScrollingEnabled(true);
            Scroll3.SetIsMouseWheelScrollingEnabled(true);
#endif

            Tree1.TalentsChanged += new EventHandler(TalentsChanged);
            Tree2.TalentsChanged += new EventHandler(TalentsChanged);
            Tree3.TalentsChanged += new EventHandler(TalentsChanged);
            Glyph.TalentsChanged += new EventHandler(TalentsChanged);
        }

        public void TalentsChanged(object sender, EventArgs e)
        {
            UpdateSavedSpecs();
            Character.OnTalentChange();
            Character.OnCalculationsInvalidated();
        }

        public bool HasCustomSpec { get; private set; }

        private bool updating;
        private void UpdateSavedSpecs()
        {
            SavedTalentSpecList savedSpecs = SavedTalentSpec.SpecsFor(Character.Class);
            SavedTalentSpec current = null;
            updating = true;
            foreach (SavedTalentSpec sts in savedSpecs)
            {
                if (sts.Equals(Character.CurrentTalents))
                {
                    current = sts;
                    break;
                }
            }

            if (current != null)
            {
                HasCustomSpec = false;
                SavedCombo.ItemsSource = savedSpecs;
                SavedCombo.SelectedItem = current;
                SaveDeleteButton.Content = "Delete";
            }
            else
            {
                HasCustomSpec = true;
                current = new SavedTalentSpec("Custom", Character.CurrentTalents, Tree1.Points(), Tree2.Points(), Tree3.Points());
                SavedTalentSpecList currentList = new SavedTalentSpecList();
                currentList.AddRange(savedSpecs);
                currentList.Add(current);
                SavedCombo.ItemsSource = null;
                SavedCombo.ItemsSource = currentList;
                SavedCombo.SelectedItem = current;
                SaveDeleteButton.Content = "Save";
            }
            updating = false;
        }

        private void SaveDelete_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SavedTalentSpec currentSpec = SavedCombo.SelectedItem as SavedTalentSpec;
            if (HasCustomSpec)
            {
                SaveTalentSpecDialog dialog = new SaveTalentSpecDialog(currentSpec.TalentSpec(),
                    currentSpec.Tree1, currentSpec.Tree2, currentSpec.Tree3);
                dialog.Closed += new EventHandler(dialog_Closed);
                dialog.Show();
            }
            else
            {
                SavedTalentSpec.AllSpecs.Remove(currentSpec);
                UpdateSavedSpecs();
            }
        }

        private void dialog_Closed(object sender, EventArgs e)
        {
            if (((ChildWindow)sender).DialogResult.GetValueOrDefault(false))
            {
                UpdateSavedSpecs();
            }
        }

        private void SavedCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!updating)
            {
                SavedTalentSpec newSpec = SavedCombo.SelectedItem as SavedTalentSpec;
                Character.CurrentTalents = newSpec.TalentSpec();
                Character.OnTalentChange();
                character_ClassChanged(this, EventArgs.Empty);
                Character.OnCalculationsInvalidated();
            }
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
             ImportTalentSpecDialog itsdg = new ImportTalentSpecDialog();
             itsdg.Closed += new EventHandler(itsdg_Closed);
             itsdg.Show();
        }

        void itsdg_Closed(object sender, EventArgs e)
        {
            if ((sender as ImportTalentSpecDialog).DialogResult.GetValueOrDefault(false))
            {
                // Now we need to set the current talents to this new imported spec
                string newspec = (sender as ImportTalentSpecDialog).TB_Link.Text;
                if (newspec.Contains("wowhead")) {
                    // Example Warrior Spec: http://www.wowhead.com/talent#LubcfRMRurkcrZ0b:RMcrsR0kV
                    // Talents: LubcfRMRurkcrZ0b
                    // Glyphs:  RMcrsR0kV
                    // WowHead works now
                    string s = newspec.Split('#')[1];
                    Character.CurrentTalents = parse_talents_wowhead(character.Class, s);
                    TalentsChanged(null, null);
                    character_ClassChanged(null, null);
                    return;
                } else if (newspec.Contains("mmo-champ") || newspec.Contains("wowtal")) {
                    // Example Warrior Spec: http://wowtal.com/#k=sb38JzPD.ala.warrior.q9b2y
                    // Talents: sb38JzPD.ala
                    // Glyphs:  q9b2y
                    // mmo-champ won't work yet
                    return;
                } else if (newspec.Contains("battle")) { // battle.net
                    // We can't even do normal imports! BAH!
                    return;
                } else {
                    MessageBox.Show("The link you entered is not a valid talent spec, we need the full link of the talent spec for this to work.", "Invalid Talent Spec", MessageBoxButton.OK);
                }
            }
        }

        //*
        const int MAX_TALENT_POINTS = 41;
        const int MAX_TALENT_COL = 4;
        int MAX_TALENT_ROW { get { return ((MAX_TALENT_POINTS + 4) / 5); } }
        int MAX_TALENT_SLOTS { get{ return (3 * MAX_TALENT_ROW * MAX_TALENT_COL); } }

        //List<int>[] talent_trees = new List<int>[MAX_TALENT_TREES]; // 

        public class decode_t {
            public decode_t(char k, char f, char s) {
                key = k;
                first = f;
                second = s;
            }
            public char key;
            public char first;
            public char second;
        }

        public static decode_t[] decoding = new decode_t[] {
            new decode_t('0', '0', '0'), new decode_t('z', '0', '1'), new decode_t('M', '0', '2'), new decode_t('c', '0', '3' ), new decode_t('m', '0', '4'), new decode_t('V', '0', '5'),
            new decode_t('o', '1', '0'), new decode_t('k', '1', '1'), new decode_t('R', '1', '2'), new decode_t('s', '1', '3' ), new decode_t('a', '1', '4'), new decode_t('q', '1', '5'),
            new decode_t('b', '2', '0'), new decode_t('d', '2', '1'), new decode_t('r', '2', '2'), new decode_t('f', '2', '3' ), new decode_t('w', '2', '4'), new decode_t('i', '2', '5'),
            new decode_t('h', '3', '0'), new decode_t('u', '3', '1'), new decode_t('G', '3', '2'), new decode_t('I', '3', '3' ), new decode_t('N', '3', '4'), new decode_t('A', '3', '5'),
            new decode_t('L', '4', '0'), new decode_t('p', '4', '1'), new decode_t('T', '4', '2'), new decode_t('j', '4', '3' ), new decode_t('n', '4', '4'), new decode_t('y', '4', '5'),
            new decode_t('x', '5', '0'), new decode_t('t', '5', '1'), new decode_t('g', '5', '2'), new decode_t('e', '5', '3' ), new decode_t('v', '5', '4'), new decode_t('E', '5', '5'),
            new decode_t('\0','\0','\0')
        };

        TalentsBase parse_talents_wowhead(CharacterClass characterclass, string talent_string)
        {
            // wowhead format: [tree_1]Z[tree_2]Z[tree_3] where the trees are character encodings
            // each character expands to a pair of numbers [0-5][0-5]
            // unused deeper talents are simply left blank instead of filling up the string with zero-zero encodings

            bool hasGlyphs = talent_string.Contains(":");

            int[] talent_trees = new int[] { 0, 0, 0 };
            int[] glyph_trees = new int[] { 3, 3, 3 };

            switch (characterclass)
            {
                case CharacterClass.Warrior:     { WarriorTalents talents = new WarriorTalents(); talent_trees = talents.TreeLengths; break; }
                case CharacterClass.Paladin:     { PaladinTalents talents = new PaladinTalents(); talent_trees = talents.TreeLengths; break; }
                case CharacterClass.Hunter:      { HunterTalents talents = new HunterTalents(); talent_trees = talents.TreeLengths; break; }
                case CharacterClass.Rogue:       { RogueTalents talents = new RogueTalents(); talent_trees = talents.TreeLengths; break; }
                case CharacterClass.Priest:      { PriestTalents talents = new PriestTalents(); talent_trees = talents.TreeLengths; break; }
                case CharacterClass.DeathKnight: { DeathKnightTalents talents = new DeathKnightTalents(); talent_trees = talents.TreeLengths; break; }
                case CharacterClass.Shaman:      { ShamanTalents talents = new ShamanTalents(); talent_trees = talents.TreeLengths; break; }
                case CharacterClass.Mage:        { MageTalents talents = new MageTalents(); talent_trees = talents.TreeLengths; break; }
                case CharacterClass.Warlock:     { WarlockTalents talents = new WarlockTalents(); talent_trees = talents.TreeLengths; break; }
                case CharacterClass.Druid:       { DruidTalents talents = new DruidTalents(); talent_trees = talents.TreeLengths; break; }
            }

            int[] encoding = new int[talent_trees[0] + talent_trees[1] + talent_trees[2]];
            int[][] glyphEncoding = new int[][] {
                new int[3],
                new int[3],
                new int[3],
            };
            int[] tree_count = new int[] { 0, 0, 0 };
            int[] glyph_count = new int[] { 0, 0, 0 };

            int tree = 0;
            int count = 0;

            #region Talents parsing
            for (int i=1; i < talent_string.Length; i++) {
                if (tree >= 3) {
                    //sim -> errorf( "Player %s has malformed wowhead talent string. Too many talent trees specified.\n", name() );
                    return null;
                }

                char c = talent_string[i];

                if (c == ':') break; // glyph encoding follows
 
                if (c == 'Z') {
                    count = 0;
                    for (int j = 0; j <= tree; j++) {
                        count += talent_trees[tree];
                    }
                    tree++;
                    continue;
                }

                decode_t decode = null;
                for (int j=0; decoding[j].key != '\0' && decode==null; j++) {
                    if (decoding[j].key == c) { decode = decoding[j]; }
                }

                if (decode == null) {
                    //sim -> errorf( "Player %s has malformed wowhead talent string. Translation for '%c' unknown.\n", name(), c );
                    return null;
                }

                encoding[count++] += decode.first - '0';
                tree_count[tree] += 1;

                if (tree_count[tree] < talent_trees[tree]) {
                    encoding[count++] += decode.second - '0';
                    tree_count[tree] += 1;
                }

                if (tree_count[tree] >= talent_trees[tree]) {
                    tree++;
                }
            }
            #endregion

            #region Glyphs Parsing
            #region Notes
            /* This is totally crappy....
             * Glyphs do not follow the same parsing rules. If you apply what was there for talents directly
             * to glyphs you get 1202032213120011050000000000000000. Which should only have 1's and 0's
             * 
             * 
             * Warriors: As I'm checking glyphs, here's what I get:
             * == PRIMES ==
             *   Link                           decode  id       Name
            * * http://www.wowhead.com/talent#L:0 00 43415 58388 Devastate
            * * http://www.wowhead.com/talent#L:z 01 43416 58367 Bloodthirst
            * * http://www.wowhead.com/talent#L:M 02 43421 58368 Mortal Strike
            * * http://www.wowhead.com/talent#L:c 03 43422 58386 Overpower
            * * http://www.wowhead.com/talent#L:m 04 43423 58385 Slam
            * * http://www.wowhead.com/talent#L:V 05 43424 58364 Revenge
            * * http://www.wowhead.com/talent#L:o 10 43425 58375 Shield Slam
            * * http://www.wowhead.com/talent#L:k 11 43432 58370 Raging Blow
            * * http://www.wowhead.com/talent#L:R 12 45790 63324 Bladestorm
             * == MAJORS ==
             * * http://www.wowhead.com/talent#L:0 00 43397 Long Charge
             * * http://www.wowhead.com/talent#L:z 01 43399 Thunder Clap
             * * http://www.wowhead.com/talent#L:M 02 43413 Rapid Charge
             * * http://www.wowhead.com/talent#L:c 03 43414 Cleaving
             * * http://www.wowhead.com/talent#L:m 04 43417 Piercing Howl
             * * http://www.wowhead.com/talent#L:V 05 43418 Heroic Throw
             * * http://www.wowhead.com/talent#L:o 10 43419 Intervene
             * * http://www.wowhead.com/talent#L:k 11 43427 Sunder Armor
             * * http://www.wowhead.com/talent#L:R 12 43428 Sweeping Strikes
             * * http://www.wowhead.com/talent#L:s 13 43430 Resonating Power
             * * http://www.wowhead.com/talent#L:a 14 43431 Victory Rush
             * * http://www.wowhead.com/talent#L:q 15 45792 Shockwave
             * * http://www.wowhead.com/talent#L:b 20 45795 Spell Reflection
             * * http://www.wowhead.com/talent#L:d 21 45797 Shield Wall
             * * http://www.wowhead.com/talent#L:r 22 63481 Colossus Smash
             * * http://www.wowhead.com/talent#L:f 23 67482 Intercept
             * * http://www.wowhead.com/talent#L:w 24 67483 Death Wish
             * == MINORS ==
             * * http://www.wowhead.com/talent#L:0 00 43395 Battle
             * * http://www.wowhead.com/talent#L:z 01 43396 Berserker Rage
             * * http://www.wowhead.com/talent#L:M 02 43398 Demoralizing Shout
             * * http://www.wowhead.com/talent#L:c 03 43400 Enduring Victory
             * * http://www.wowhead.com/talent#L:m 04 43412 Bloody Healing
             * * http://www.wowhead.com/talent#L:V 05 45793 Furious Sundering
             * * http://www.wowhead.com/talent#L:o 10 45794 Intimidating Shout
             * * http://www.wowhead.com/talent#L:k 11 49084 Command
             * 
             * So http://www.wowhead.com/talent#LubcfRMRurkcrZ0b:RMcrsR0kV would mean:
             *   Prime: Bladestorm, Mortal Strike, Overpower
             *   Major: Colossus Smash, Resonating Power, Sweeping Strikes
             *   Minor: Battle, Command, Furious Sundering
             * Which is correct, that's what we come out to
             */
            #endregion

            tree = 0;
            count = 0;

            if (hasGlyphs) {
                for (int i=talent_string.IndexOf(":")+1; i < talent_string.Length; i++) {
                    if (tree >= 3) {
                        //sim -> errorf( "Player %s has malformed wowhead talent string. Too many talent trees specified.\n", name() );
                        return null;
                    }

                    char c = talent_string[i];

                    if (c == 'Z') {
                        count = 0;
                        /*for (int j = 0; j <= tree; j++) {
                            count += glyph_trees[tree];
                        }*/
                        tree++;
                        continue;
                    }

                    decode_t decode = null;
                    for (int j=0; decoding[j].key != '\0' && decode==null; j++) {
                        if (decoding[j].key == c) { decode = decoding[j]; }
                    }

                    if (decode == null) {
                        //sim -> errorf( "Player %s has malformed wowhead talent string. Translation for '%c' unknown.\n", name(), c );
                        return null;
                    }

                    glyphEncoding[tree][count++] += (decode.first - '0') * 10 + decode.second - '0';
                    glyph_count[tree] += 1;

                    if (glyph_count[tree] >= (glyph_trees[tree])) { tree++; count = 0; }
                }
            }
            #endregion

            string newtalentstring = "";
            foreach (int i in encoding) { newtalentstring += i.ToString(); }
            if (hasGlyphs) {
                //newtalentstring += ".";
                //for (int t = 0; t < 3; t++) {
                //    foreach (int i in glyphEncoding[t]) { newtalentstring += i.ToString(); }
                //}
            }

            switch (characterclass)
            {
                case CharacterClass.Warrior: { return new WarriorTalents(newtalentstring, hasGlyphs ? glyphEncoding : null); }
                case CharacterClass.Paladin: { return new PaladinTalents(newtalentstring, hasGlyphs ? glyphEncoding : null); }
                case CharacterClass.Hunter: { return new HunterTalents(newtalentstring, hasGlyphs ? glyphEncoding : null); }
                case CharacterClass.Rogue: { return new RogueTalents(newtalentstring, hasGlyphs ? glyphEncoding : null); }
                case CharacterClass.Priest: { return new PriestTalents(newtalentstring, hasGlyphs ? glyphEncoding : null); }
                case CharacterClass.DeathKnight: { return new DeathKnightTalents(newtalentstring, hasGlyphs ? glyphEncoding : null); }
                case CharacterClass.Shaman: { return new ShamanTalents(newtalentstring, hasGlyphs ? glyphEncoding : null); }
                case CharacterClass.Mage: { return new MageTalents(newtalentstring, hasGlyphs ? glyphEncoding : null); }
                case CharacterClass.Warlock: { return new WarlockTalents(newtalentstring, hasGlyphs ? glyphEncoding : null); }
                case CharacterClass.Druid: { return new DruidTalents(newtalentstring, hasGlyphs ? glyphEncoding : null); }
            }
            return null;
        }
    }
}
