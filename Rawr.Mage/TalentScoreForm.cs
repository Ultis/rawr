using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Collections;

namespace Rawr.Mage
{
    public partial class TalentScoreForm : Form
    {
        private class TalentScore
        {
            float[] talentScore;
            int index;
            string name;

            public string Name
            {
                get
                {
                    return name;
                }
            }

            public float Score
            {
                get
                {
                    return talentScore[index];
                }
                set
                {
                    talentScore[index] = value;
                }
            }

            public TalentScore(float[] talentScore, int index, string name)
            {
                this.talentScore = talentScore;
                this.index = index;
                this.name = name;
            }
        }

        public TalentScoreForm(float[] talentScore)
        {
            InitializeComponent();

            List<TalentScore> list = new List<TalentScore>();
            foreach (PropertyInfo pi in typeof(MageTalents).GetProperties())
            {
                TalentDataAttribute[] talentDatas = pi.GetCustomAttributes(typeof(TalentDataAttribute), true) as TalentDataAttribute[];
                if (talentDatas.Length > 0)
                {
                    TalentDataAttribute talentData = talentDatas[0];
                    list.Add(new TalentScore(talentScore, talentData.Index, talentData.Name));
                }
            }

            bindingSourceTalentScore.DataSource = list;
        }
    }
}
