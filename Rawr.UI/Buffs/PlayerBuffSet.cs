namespace Rawr.UI
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Media;

    public class PlayerBuffSet
    {
        public PlayerBuffSet() { }
        //
        public CharacterClass Class = CharacterClass.Warrior;
        public String Spec = "Unset";
        public Color Color = Colors.Brown;
        public Dictionary<String, String> BuffsToAdd = new Dictionary<String, String>();
        //
        public override string ToString()
        {
            string retVal = string.Format("{0} ({1})", Class, Spec);
            //
            foreach (String key in BuffsToAdd.Keys)
            {
                retVal += string.Format("\r\n - {0}", BuffsToAdd[key]);
            }
            //
            return retVal;
        }
    }
}
