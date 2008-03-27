using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using Rawr.Forms.Utilities;

namespace Rawr.Forms
{
	public partial class Status : Form
	{
		private bool expanded = true;
		private List<StatusEventArgs> _StatusUpdates;
		private const string overallProgress = "{0} of {1} Tasks have completed successfully";
		private int _hScrollPosition = 0;
		private int _vScrollPosition = 0;

		public Status()
		{
			InitializeComponent();
			_StatusUpdates = new List<StatusEventArgs>();
			StatusMessaging.StatusUpdate += new StatusMessaging.StatusUpdateDelegate(StatusMessaging_StatusUpdate);
			statusEventArgsBindingSource.DataSource = _StatusUpdates;
			dataGridView1.Scroll += new ScrollEventHandler(dataGridView1_Scroll);
		}

		void dataGridView1_Scroll(object sender, ScrollEventArgs e)
		{
			if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
			{
				_hScrollPosition = e.NewValue;
			}
			else
			{
				_vScrollPosition = dataGridView1.FirstDisplayedScrollingRowIndex;
			}
		}

		private void StatusMessaging_StatusUpdate(StatusEventArgs args)
		{
			if (!this.IsDisposed)
			{
				bool found = false;
				if (args != null && !String.IsNullOrEmpty(args.Key))
				{
					for (int i = 0; i < _StatusUpdates.Count; i++)
					{
						if (_StatusUpdates[i].Key == args.Key)
						{
							//overwrite old status from that key
							_StatusUpdates[i] = args;
							found = true;
							break;
						}
					}
					if (!found)
					{
						_StatusUpdates.Add(args);
					}
				}
			
				InvokeHelper.BeginInvoke(this, "RefreshTaskList", null);
			}
		}

		private void RefreshTaskList()
		{
			int doneCount = 0;
			for (int i = 0; i < _StatusUpdates.Count; i++)
			{
				if (_StatusUpdates[i].Done)
				{
					doneCount++;
				}				
			}
			this.dataGridView1.SuspendLayout();
			//BUG:  http://forums.microsoft.com/MSDN/ShowPost.aspx?PostID=2850853&SiteID=1 --
			statusEventArgsBindingSource.DataSource = _StatusUpdates;
			statusEventArgsBindingSource.ResetBindings(false);
			dataGridView1.HorizontalScrollingOffset = _hScrollPosition;
			dataGridView1.UpdateRowHeightInfo(0, true);
			dataGridView1.Refresh();
			this.dataGridView1.ResumeLayout();
			//This is stupidly readonly.
			//dataGridView1.VerticalScrollingOffset = _vScrollPosition;
			
			label1.Text = string.Format(overallProgress,doneCount, _StatusUpdates.Count);
			progressBar1.Value = Convert.ToInt32(decimal.Round(((decimal)doneCount / (decimal)_StatusUpdates.Count) * 100));
		}

        private int _expandedHeight = 391;

		private void ShowHideDetails_Click(object sender, EventArgs e)
		{
			expanded = !expanded;
			if (expanded)
			{
				this.Size = new Size(this.Size.Width,_expandedHeight);
				ShowHideDetails.Text = "<< Details";
			}
			else
			{
                _expandedHeight = this.Size.Height;
				this.Size = new Size(this.Size.Width,130);
				ShowHideDetails.Text = "Details >>";
			}
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			StatusMessaging.StatusUpdate -= new StatusMessaging.StatusUpdateDelegate(StatusMessaging_StatusUpdate);
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}
