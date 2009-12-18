using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;

namespace Rawr
{
    public class ItemUpdater
    {
        internal class ItemToUpdate
        {
            public Item item;
            public int index;
        }

        private int itemsDone;
        private int itemsToDo;
        private bool finishedInput;
        private bool done;

        public int ItemsDone { get { lock (lockObject) { return itemsDone; } } }
        public int ItemsToDo { get { lock (lockObject) { return itemsToDo; } } }
        public bool Done { get { lock (lockObject) { return done; } } }
        public Item this[int id]
        {
            get
            {
                lock (lockObject)
                {
                    if (id > 0)
                    {
                        Item item; 
                        newItems.TryGetValue(id, out item); 
                        return item;
                    }

                    return null;
                }
            }
        }

        private SortedDictionary<int, Item> newItems;

        private bool useArmory;
        private bool usePTR;
        private bool multiThreaded;

        private AutoResetEvent eventDone;

        private Object lockObject; // because lock(this) is discouraged by documentation

        private Queue<ItemToUpdate> itemQueue;
        private int itemsPerSecond;

        public ItemUpdater(bool multiThreaded, bool useArmory, bool usePTR, int itemsPerSecond)
        {
            this.itemsDone = 0;
            this.itemsToDo = 0;
            this.finishedInput = false;
            this.done = false;
            this.newItems = new SortedDictionary<int, Item>();
            this.multiThreaded = multiThreaded;
            this.useArmory = useArmory;
            this.usePTR = usePTR;
            this.eventDone = new AutoResetEvent(false);
            this.lockObject = new Object();
            this.itemsPerSecond = itemsPerSecond;
            this.itemQueue = new Queue<ItemToUpdate>();

            Thread t = new Thread(new ThreadStart(Throttle));
            t.Start();
        }

        private void Throttle()
        {
            Stopwatch stopwatch = new Stopwatch(); ;
            stopwatch.Start();

            while (!Done)
            {
                long before = stopwatch.ElapsedMilliseconds;

                for (int i = 0; i < itemsPerSecond || itemsPerSecond == 0; i++)
                {
                    ItemToUpdate info = null;
                    lock (lockObject)
                    {
                        if (itemQueue.Count == 0) break;
                        info = itemQueue.Dequeue();
                    }
                    if (multiThreaded)
                    {
                        ThreadPool.QueueUserWorkItem(LoadItem, info);
                    }
                    else
                    {
                        LoadItem(info);
                        Interlocked.Increment(ref itemsDone);
                    }
                }

                long elapsed = stopwatch.ElapsedMilliseconds - before;                

                if (elapsed < 1000) Thread.Sleep((int)(1000 - elapsed));
            }
        }

        private void LoadItem(object state)
        {
            ItemToUpdate info = state as ItemToUpdate;
            Item i = null;

            try
            {
                if (useArmory)
                {
                    i = Item.LoadFromId(info.item.Id, true, false, false);
                }
                else
                {
                    i = Item.LoadFromId(info.item.Id, true, false, true, Rawr.Properties.GeneralSettings.Default.Locale, usePTR ? "ptr" : "www");
                }
            }
            catch (Exception ex)
            {
                StatusMessaging.ReportError("Load item", ex, string.Format("Unable to update '{0}' due to an error: {1}\r\n\r\n{2}", info.item.Name, ex.Message, ex.StackTrace));
            }

            bool completelyDone = false;

            lock (lockObject)
            {
                if (i != null && (i.Stats.ToString().Contains(",") || (i.Stats.Armor + i.Stats.BonusArmor == 0 && i.Stats.ToString().Length > 0)))
                {
                    newItems.Add(info.index, i);
                }
                itemsDone++;
                completelyDone = done = finishedInput && itemsDone == itemsToDo;
            }

            if (completelyDone) eventDone.Set();
        }

        public void AddItem(int index, Item i)
        {
            ItemToUpdate info = new ItemToUpdate()
            {
                index = index,
                item = i
            };
            lock (lockObject)
            {
                itemsToDo++;
                itemQueue.Enqueue(info);
            }
        }

        public void FinishAdding()
        {
            lock (lockObject)
            {
                finishedInput = true;
            }
        }

        public void WaitUntilDone()
        {
            eventDone.WaitOne();
        }
    }
}
