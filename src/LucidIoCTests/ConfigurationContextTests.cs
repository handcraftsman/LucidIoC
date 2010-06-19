using FluentAssert;

using NUnit.Framework;

namespace gar3t.LucidIoC.Tests
{
	public class ConfigurationContextTests
	{
		[TestFixture]
		public class When_asked_to_update_the_configuration
		{
			private Configuration _configuration;
			private ConfigurationContext _context;
			private string _name;
			private ConfigurationContext _result;

			[Test]
			public void Given_a_request_to_make_it_singleton()
			{
				Test.Static()
					.When(asked_to_make_it_a_singleton)
					.With(non_singleton_configuration)
					.With(context_containing_the_configuration)
					.Should(configure_the_configuration_to_be_singleton)
					.Should(return_the_context)
					.Verify();
			}

			[Test]
			public void Given_a_request_to_name_it()
			{
				Test.Static()
					.When(asked_to_name_it)
					.With(context_containing_the_configuration)
					.Should(configure_the_name_of_the_configuration)
					.Should(return_the_context)
					.Verify();
			}

			private void asked_to_make_it_a_singleton()
			{
				_result = _context.AsSingleton();
			}

			private void asked_to_name_it()
			{
				_name = "Foo";
				_result = _context.Named(_name);
			}

			private void configure_the_configuration_to_be_singleton()
			{
				_configuration.IsSingleton.ShouldBeTrue();
			}

			private void configure_the_name_of_the_configuration()
			{
				_configuration.Name.ShouldBeEqualTo(_name);
			}

			private void context_containing_the_configuration()
			{
				_context = new ConfigurationContext(_configuration);
			}

			private void non_singleton_configuration()
			{
				_configuration = new Configuration
					{
						IsSingleton = false
					};
			}

			private void return_the_context()
			{
				_result.ShouldBeSameInstanceAs(_context);
			}
		}
	}
}