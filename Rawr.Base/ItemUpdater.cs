using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;

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

        public ItemUpdater(bool multiThreaded, bool useArmory, bool usePTR)
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
                if (i != null)
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
            lock (lockObject)
            {
                itemsToDo++;
            }
            ItemToUpdate info = new ItemToUpdate()
            {
                index = index,
                item = i
            };
            if (multiThreaded)
            {
                ThreadPool.QueueUserWorkItem(LoadItem, info);
            }
            else
            {
                LoadItem(info);
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
