using System;
using System.Collections.Generic;
using System.Linq;

namespace gar3t.LucidIoC
{
	public class ConfigurationCollection
	{
		private readonly IList<Configuration> _configurations = new List<Configuration>();

		public void DisposeDisposableInstances()
		{
			foreach (var configuration in _configurations)
			{
				DisposeInstance(configuration);
			}
		}

		private static void DisposeInstance(Configuration configuration)
		{
			var instance = configuration.Instance as IDisposable;
			configuration.Instance = null;
			if (instance == null)
			{
				return;
			}
			instance.Dispose();
		}

		public Configuration Get()
		{
			return _configurations.SingleOrDefault(x => x.Name == null) ?? _configurations.Single();
		}

		public Configuration Get(string name)
		{
			return _configurations.Single(x => x.Name == name);
		}

		public bool HasConfiguration()
		{
			return _configurations.Count != 0;
		}

		public void Store(Configuration configuration)
		{
			var match = _configurations.FirstOrDefault(x => x.Name == configuration.Name);
			if (match == null)
			{
				_configurations.Add(configuration);
			}
			else
			{
				int index = _configurations.IndexOf(match);
				_configurations[index] = configuration;
				DisposeInstance(match);
			}
		}
	}
}