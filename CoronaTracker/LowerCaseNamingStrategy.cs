using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoronaTracker
{
	public class LowerCaseNamingStrategy : NamingStrategy
	{
		protected override string ResolvePropertyName(string name)
		{
			return name.ToLower();
		}
	}
}
