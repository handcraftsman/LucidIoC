using System;
using System.Collections.Generic;
using System.Linq;

namespace gar3t.LucidIoC
{
	public class ConfigurationCollection
	{
		private readonly IList<ResolutionInfo> _configurations = new List<ResolutionInfo>();

		public void DisposeDisposableInstances()
		{
			foreach (var configuration in _configurations)
			{
				var instance = configuration.Instance as IDisposable;
				configuration.Instance = null;
				if (instance == null)
				{
					continue;
				}
				instance.Dispose();
			}
		}

		public ResolutionInfo Get()
		{
			return _configurations.FirstOrDefault();
		}

		public bool HasConfiguration()
		{
			return _configurations.Count != 0;
		}

		public void Store(ResolutionInfo configuration)
		{
			_configurations.Clear();
			_configurations.Add(configuration);
		}
	}
}