using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr
{
    [Serializable]
    public class TalentItem : IComparable<TalentItem>
    {

        [System.Xml.Serialization.XmlElement("Tree")]
        public string _tree;

        [System.Xml.Serialization.XmlElement("Name")]
        public string _name;

        [System.Xml.Serialization.XmlElement("Description")]
        public SerializableDictionary<int, string> _description;

        [System.Xml.Serialization.XmlElement("Rank")]
        public int _rank;

        [System.Xml.Serialization.XmlElement("HorizontalPosition")]
        public int _horizontalPosition;

        [System.Xml.Serialization.XmlElement("VerticalPosition")]
        public int _verticalPosition;

        [System.Xml.Serialization.XmlElement("PointsInvested")]
        public int _pointsInvested;



        public TalentItem()
        {
        }

        public TalentItem(string talentLine)
        {
            talentLine = talentLine.Split('=')[1];
            talentLine = talentLine.Split(';')[0];
            talentLine = talentLine.Replace("[","").Replace("]","");
            string[] tal = talentLine.Split(',');
            this.Name = tal[1];
            this.Rank = Int32.Parse(tal[2]);
            this.HorizontalPosition = Int32.Parse(tal[3]);
            this.VerticalPosition = Int32.Parse(tal[4]);
        }


        [System.Xml.Serialization.XmlIgnore]
        public string Tree
        {
            get { return _tree;}
            set { _tree = value; }
        }

        [System.Xml.Serialization.XmlIgnore]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        [System.Xml.Serialization.XmlIgnore]
        public int Rank
        {
            get { return _rank; }
            set { _rank = value; }
        }

        [System.Xml.Serialization.XmlIgnore]
        public int HorizontalPosition
        {
            get { return _horizontalPosition; }
            set { _horizontalPosition = value; }
        }

        [System.Xml.Serialization.XmlIgnore]
        public int VerticalPosition
        {
            get { return _verticalPosition; }
            set { _verticalPosition = value; }
        }

        [System.Xml.Serialization.XmlIgnore]
        public int PointsInvested
        {
            get { return _pointsInvested; }
            set { _pointsInvested = value; }
        }

        [System.Xml.Serialization.XmlIgnore]
        public string Description
        {
            get { 
                    if (_description.ContainsKey(PointsInvested))
                        return _description[PointsInvested];
                    return "";
                }
        }


        #region IComparable<TalentItem> Members

        public int CompareTo(TalentItem other)
        {
            return (HorizontalPosition.ToString() + " x " + VerticalPosition.ToString()).CompareTo(other.HorizontalPosition.ToString() + " x " + other.VerticalPosition.ToString());
        }

        #endregion

        public override string ToString()
        {
            return "(" + PointsInvested + "/" + Rank + ") " + Name;
        }
    }
}
