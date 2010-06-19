using System;

namespace gar3t.LucidIoC
{
	public class Configuration
	{
		public MulticastDelegate Initializer { get; set; }
		public object Instance { get; set; }
		public bool IsSingleton { get; set; }
		public string Name { get; set; }
	}
}