using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
#if !SILVERLIGHT
using System.Windows.Forms;
#endif

namespace Rawr.Base
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
        public bool Done { get { lock (lockObject) { return done || (cancel != null && cancel()); } } }
        public Item this[int id]
        {
            get
            {
                lock (lockObject)
                {
                    if (id > 0)
                    {
                        Item item = null; 
                        newItems.TryGetValue(id, out item); 
                        return item;
                    }

                    return null;
                }
            }
        }

        private Dictionary<int, Item> newItems; // was a SortedDictionary

        private bool useArmory;
        private bool usePTR;
        private bool multiThreaded;
        private WowheadService.UpgradeCancelCheck cancel;

        private AutoResetEvent eventDone;

        private Object lockObject; // because lock(this) is discouraged by documentation

        private Queue<ItemToUpdate> itemQueue;
        private int itemsPerSecond;

        public ItemUpdater(bool multiThreaded, bool useArmory, bool usePTR, int itemsPerSecond, WowheadService.UpgradeCancelCheck check)
        {
            this.itemsDone = 0;
            this.itemsToDo = 0;
            this.finishedInput = false;
            this.done = false;
            this.newItems = new Dictionary<int, Item>(); // was a SortedDictionary
            this.multiThreaded = multiThreaded;
            this.cancel = check;
            this.useArmory = useArmory;
            this.usePTR = usePTR;
            this.eventDone = new AutoResetEvent(false);
            this.lockObject = new Object();
            this.itemsPerSecond = itemsPerSecond;
            this.itemQueue = new Queue<ItemToUpdate>();

            if (multiThreaded)
            {
                // Thread t = new Thread(new ThreadStart(Throttle));
                ThreadPool.QueueUserWorkItem(Throttle);
                // t.Start();
            }
        }

        private void Throttle(object e)
        {
            DateTime start = DateTime.Now;

            while (!Done)
            {
                //long before = stopwatch.ElapsedMilliseconds;

                for (int i = 0; i < itemsPerSecond || itemsPerSecond == 0; i++)
                {
                    if (Done)
                        return;

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

                double elapsed = DateTime.Now.Subtract(start).TotalMilliseconds;                

                if (elapsed < 1000) Thread.Sleep((int)(1000 - elapsed));
            }
        }

        private void LoadItem(object state)
        {
            // check done status
            if( Done )
                return;

            // continue updating
            ItemToUpdate info = state as ItemToUpdate;
            Item i = null;

            try
            {
                if (useArmory)
                {
                    i = Item.LoadFromId(info.item.Id, true, false, false, false);
                }
                else
                {
                    i = Item.LoadFromId(info.item.Id, true, false, true, usePTR, Rawr.Properties.GeneralSettings.Default.Locale);
                }
            }
            catch (Exception ex)
            {
                StatusMessaging.ReportError("Load item", ex, string.Format("Unable to update '{0}' due to an error: {1}\r\n\r\n{2}", info.item.Name, ex.Message, ex.StackTrace));
            }

            bool completelyDone = false;

            lock (lockObject)
            {
                if (i != null /* && (i.Stats.ToString().Contains(",") || (i.Stats.Armor + i.Stats.BonusArmor == 0 && i.Stats.ToString().Length > 0))*/)
                {
                    newItems.Add(info.index, i);
                }
                itemsDone++;
                completelyDone = done = finishedInput && itemsDone == itemsToDo;
            }

            if (completelyDone) eventDone.Set();
        }

        public void AddItem(int index, Item item)
        {
            if (!multiThreaded)
            {
                Item i = null;
                try
                {
                    if (useArmory)
                    {
                        i = Item.LoadFromId(item.Id, true, false, false, false);
                    }
                    else
                    {
                        i = Item.LoadFromId(item.Id, true, false, true, usePTR, Rawr.Properties.GeneralSettings.Default.Locale);
                    }
                }
                catch (Exception ex)
                {
                    StatusMessaging.ReportError("Load item", ex, string.Format("Unable to update '{0}' due to an error: {1}\r\n\r\n{2}", item.Name, ex.Message, ex.StackTrace));
                }
                if (i != null)
                {
                    newItems.Add(index, i);
                }
                return;
            }

            ItemToUpdate info = new ItemToUpdate()
            {
                index = index,
                item = item
            };
            lock (lockObject)
            {
                itemsToDo++;
                itemQueue.Enqueue(info);
            }
        }

        public void FinishAdding()
        {
            if (!multiThreaded)
            {
                done = true;
            }
            lock (lockObject)
            {
                finishedInput = true;
            }
        }

        public void WaitUntilDone()
        {
            if (multiThreaded) eventDone.WaitOne();
        }
    }
}