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
	public class Coordinates
	{
		#region Properties

		[JsonIgnore]
		public string GoogleMapsLink { get { return string.Format("https://www.google.com/maps/place/{0},{1}", Lattitude, Longnitude); } }

		[JsonProperty(PropertyName = "lat")]
		public double? Lattitude { get; set; }

		[JsonProperty(PropertyName = "long")]
		public double? Longnitude { get; set; }

		#endregion

		#region Methods

		public override string ToString()
		{
			return string.Format("{0}, {1}", Lattitude, Longnitude);
		}

		#endregion
	}
}
