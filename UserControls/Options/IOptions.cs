using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.UserControls.Options
{
	internal interface IOptions
	{
		void Save();
		void Cancel();
		bool HasValidationErrors();
		string DisplayName { get; }
		string TreePosition { get; }
	}
}
