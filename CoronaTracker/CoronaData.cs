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
	class CoronaData
	{
		#region Properties

		public CoronaInfo Confirmed { get; set; }

		public CoronaInfo Deaths { get; set; }

		public LatestCoronaInfo Latest { get; set; }

		public CoronaInfo Recovered { get; set; }

		#endregion
	}
}
