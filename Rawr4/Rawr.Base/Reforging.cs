using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace Rawr
{
    public class Reforging
    {
        public AdditiveStat ReforgeFrom { get; set; }
        public AdditiveStat ReforgeTo { get; set; }
        public float ReforgeAmount { get; set; }

        public Reforging Clone()
        {
            return new Reforging()
            {
                ReforgeFrom = this.ReforgeFrom,
                ReforgeTo = this.ReforgeTo,
                ReforgeAmount = this.ReforgeAmount
            };
        }

        public Reforging() { }

        public Reforging(Item baseItem, AdditiveStat reforgeFrom, AdditiveStat reforgeTo)
        {
            ApplyReforging(baseItem, reforgeFrom, reforgeTo);
        }

        public void ApplyReforging(Item baseItem, AdditiveStat reforgeFrom, AdditiveStat reforgeTo)
        {
            ReforgeFrom = reforgeFrom;
            ReforgeTo = reforgeTo;
            ApplyReforging(baseItem);
        }

        public void ApplyReforging(Item baseItem)
        {
            // 0 is actually Agility, but we're using it as no reforging indicator
            if (baseItem != null && ReforgeFrom != 0 && ReforgeTo != 0)
            {
                float currentFrom = baseItem.Stats._rawAdditiveData[(int)ReforgeFrom];
                float currentTo = baseItem.Stats._rawAdditiveData[(int)ReforgeTo];
                if (currentFrom > 0 && currentTo == 0)
                {
                    ReforgeAmount = (float)Math.Floor(currentFrom * 0.4);
                    return;
                }
            }
            ReforgeAmount = 0;
        }

        public static List<Reforging> GetReforgingOptions(Item baseItem, AdditiveStat[] reforgeStatsFrom, AdditiveStat[] reforgeStatsTo)
        {
            List<Reforging> options = new List<Reforging>();
            options.Add(null);
            foreach (var from in reforgeStatsFrom)
            {
                float currentFrom = baseItem.Stats._rawAdditiveData[(int)from];
                if (currentFrom > 0)
                {
                    foreach (var to in reforgeStatsTo)
                    {
                        float currentTo = baseItem.Stats._rawAdditiveData[(int)to];
                        if (currentTo == 0)
                        {
                            options.Add(new Reforging(baseItem, from, to));
                        }
                    }
                }
            }
            return options;
        }
    }
}
