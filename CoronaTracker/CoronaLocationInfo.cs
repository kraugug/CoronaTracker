using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoronaTracker
{
	[JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
	public class CoronaLocationInfo
	{
		#region Properties

		public Coordinates Coordinates { get; set; }

		public string Country { get; set; }

		public string CountryCode { get; set; }

		public Dictionary<DateTime, int> History { get; }

		public int Latest { get; set; }

		public string Province { get; set; }

		#endregion

		#region Constructor

		public CoronaLocationInfo() => History = new Dictionary<DateTime, int>();

		#endregion
	}
}
