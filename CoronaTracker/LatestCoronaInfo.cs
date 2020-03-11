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
	public class LatestCoronaInfo
	{
		#region Properties

		public int Confirmed { get; set; }

		public int Deaths { get; set; }

		public int Recovered { get; set; }

		#endregion
	}
}
