using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoronaTracker
{
	[JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
	public class CoronaInfo
	{
		#region Properties

		public DateTime LastUpdated { get; set; }

		public int Latest { get; set; }

		public ObservableCollection<CoronaLocationInfo> Locations { get; }

		public string Source { get; set; }

		#endregion

		#region Constructor

		public CoronaInfo() => Locations = new ObservableCollection<CoronaLocationInfo>();

		#endregion
	}
}
