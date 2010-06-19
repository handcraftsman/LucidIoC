namespace gar3t.LucidIoC
{
	public class ConfigurationContext
	{
		private readonly Configuration _configuration;

		public ConfigurationContext(Configuration configuration)
		{
			_configuration = configuration;
		}

		public ConfigurationContext AsSingleton()
		{
			_configuration.IsSingleton = true;
			return this;
		}

		public ConfigurationContext Named(string name)
		{
			_configuration.Name = name;
			return this;
		}
	}
}