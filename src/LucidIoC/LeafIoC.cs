using System;
using System.Collections.Generic;
using System.Configuration;

namespace gar3t.LucidIoC
{
	public static class LeafIoC
	{
		private static readonly Dictionary<Type, ConfigurationCollection> Configuration
			= new Dictionary<Type, ConfigurationCollection>();

		public static ResolutionContext Configure<TInterface, TImplementation>() where TImplementation : TInterface, new()
		{
			var type = typeof(TInterface);
			var configuration = new ConfigurationCollection();
			var resolutionInfo = new ResolutionInfo
				{
					Initializer = (Func<TInterface>)(() => new TImplementation())
				};
			configuration.Store(resolutionInfo);
			Configuration[type] = configuration;

			return new ResolutionContext(resolutionInfo);
		}

		private static void DisposeDisposableInstances()
		{
			foreach (var configurationCollection in Configuration.Values)
			{
				configurationCollection.DisposeDisposableInstances();
			}
		}

		public static TInterface GetInstance<TInterface>()
		{
			var type = typeof(TInterface);
			ConfigurationCollection configuration;
			if (!Configuration.TryGetValue(type, out configuration))
			{
				throw new ConfigurationErrorsException(string.Format("No instance of {0} has been configured.", type.FullName));
			}

			var info = configuration.Get();
			if (info.Instance != null)
			{
				return (TInterface)info.Instance;
			}

			var instance = ((Func<TInterface>)(info.Initializer)).Invoke();
			if (info.IsSingleton)
			{
				info.Instance = instance;
			}
			return instance;
		}

		public static bool IsConfigured<TInterface>()
		{
			return Configuration.ContainsKey(typeof(TInterface));
		}

		public static void Reset()
		{
			DisposeDisposableInstances();
			Configuration.Clear();
		}
	}
}