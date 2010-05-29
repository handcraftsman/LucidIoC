using System;
using System.Collections.Generic;
using System.Configuration;

namespace gar3t.LucidIoC
{
	public static class LeafIoC
	{
		private static readonly Dictionary<Type, ResolutionInfo> Configuration
			= new Dictionary<Type, ResolutionInfo>();

		public static ResolutionContext Configure<TInterface, TImplementation>() where TImplementation : TInterface, new()
		{
			var type = typeof(TInterface);
			var resolutionInfo = new ResolutionInfo
				{
					Initializer = (Func<TInterface>)(() => new TImplementation())
				};
			Configuration[type] = resolutionInfo;

			return new ResolutionContext(resolutionInfo);
		}

		private static void DisposeDisposableInstances()
		{
			foreach (var resolutionInfo in Configuration.Values)
			{
				var instance = resolutionInfo.Instance as IDisposable;
				if (instance == null)
				{
					continue;
				}
				resolutionInfo.Instance = null;
				instance.Dispose();
			}
		}

		public static TInterface GetInstance<TInterface>()
		{
			var type = typeof(TInterface);
			ResolutionInfo info;
			if (!Configuration.TryGetValue(type, out info))
			{
				throw new ConfigurationErrorsException(string.Format("No instance of {0} has been configured.", type.FullName));
			}

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