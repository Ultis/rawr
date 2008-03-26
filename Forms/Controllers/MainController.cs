using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Rawr.Forms.Utilities;

namespace Rawr.Forms.Controllers
{
	public class MainController
	{
		private Control _Parent;

		public MainController(Control parent)
		{
			_Parent = parent;
		}

		public void LoadModel(string displayName)
		{
			try
			{
				Calculations.LoadModel(Calculations.Models[displayName]);
			}
			finally
			{
				this.ConfigModel = displayName;
			}
		}

		public string ConfigModel
		{
			get
			{
				return Calculations.ValidModel(Properties.Recent.Default.RecentModel);
			}
			set { Properties.Recent.Default.RecentModel = value; }
		}

		public string[] ConfigRecentCharacters
		{
			get
			{
				string recentCharacters = Properties.Recent.Default.RecentFiles;
				if (string.IsNullOrEmpty(recentCharacters))
				{
					return new string[0];
				}
				else
				{
					return recentCharacters.Split(';');
				}
			}
			set { Properties.Recent.Default.RecentFiles = string.Join(";", value); }
		}

		public void AddRecentCharacter(string character)
		{
			List<string> recentCharacters = new List<string>(ConfigRecentCharacters);
			recentCharacters.Remove(character);
			recentCharacters.Add(character);
			while (recentCharacters.Count > 8)
				recentCharacters.RemoveRange(0, recentCharacters.Count - 8);
			ConfigRecentCharacters = recentCharacters.ToArray();
		}

		public void UpdateAllCachedItems()
		{
			WebRequestWrapper.ResetFatalErrorIndicator();
			StatusMessaging.UpdateStatus("UpdateAllCachedItems", "Beginning Update");
			StatusMessaging.UpdateStatus("CacheAllIcons", "Not Started");
			for (int i = 0; i < ItemCache.AllItems.Length; i++)
			{
				Item item = ItemCache.AllItems[i];
				StatusMessaging.UpdateStatus("UpdateAllCachedItems", "Updating " + i + " of " + ItemCache.AllItems.Length + " items");
				if (item.Id < 90000)
				{
					Item newItem = Item.LoadFromId(item.GemmedId, true, "Refreshing",false);
					if (newItem == null)
					{
						//treat this as a status error.
						MessageBox.Show("Unable to find item " + item.Id + ". Reverting to previous data.");
						ItemCache.AddItem(item, true, false);
					}
				}

			}
			StatusMessaging.UpdateStatusFinished("UpdateAllCachedItems");
			ItemIcons.CacheAllIcons(ItemCache.AllItems);
			ItemCache.OnItemsChanged();
		}

		public void GetArmoryUpgrades(Character currentCharacter)
		{            
			StatusMessaging.UpdateStatus("GetArmoryUpgrades", "Getting Armory Updates");
			Armory.LoadUpgradesFromArmory(currentCharacter);
			ItemCache.OnItemsChanged();
            StatusMessaging.UpdateStatusFinished("GetArmoryUpgrades");
		}
	}
}
