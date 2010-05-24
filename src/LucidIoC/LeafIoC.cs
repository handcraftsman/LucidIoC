using System;
using System.Collections.Generic;
using System.Configuration;

namespace gar3t.LucidIoC
{
	public static class LeafIoC
	{
		private static readonly Dictionary<Type, object> Configuration = new Dictionary<Type, object>();

		public static void Configure<TInterface, TImplementation>() where TImplementation : TInterface, new()
		{
			var type = typeof(TInterface);
//			if (!Configuration.ContainsKey(type))
//			{
//				Configuration.Add(type, (Func<TInterface>)(() => new TImplementation()));
//			}
//			else
//			{
				Configuration[type] = (Func<TInterface>)(() => new TImplementation());
//			}
		}

		public static TInterface GetInstance<TInterface>()
		{
			var type = typeof(TInterface);
			object value;
			if (!Configuration.TryGetValue(type, out value))
			{
				throw new ConfigurationErrorsException(string.Format("No instance of {0} has been configured.", type.FullName));
			}
			return ((Func<TInterface>)value)();
		}

		public static bool IsConfigured<TInterface>()
		{
			return Configuration.ContainsKey(typeof(TInterface));
		}

		public static void Reset()
		{
			Configuration.Clear();
		}
	}
}