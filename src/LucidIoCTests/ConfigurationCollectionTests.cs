using FluentAssert;

using NUnit.Framework;

namespace gar3t.LucidIoC.Tests
{
	public class ConfigurationCollectionTests
	{
		[TestFixture]
		public class When_asked_if_a_configuration_exists
		{
			private ConfigurationCollection _collection;
			private bool _result;

			[SetUp]
			public void BeforeEachTest()
			{
				_collection = new ConfigurationCollection();
			}

			[Test]
			public void Given_nothing_has_been_configured()
			{
				Test.Static()
					.When(asked_if_a_configuration_exists)
					.With(nothing_configured)
					.Should(return_false)
					.Verify();
			}

			private void an_unnamed_instance_configured()
			{
			}

			private void asked_if_a_configuration_exists()
			{
				_result = _collection.HasConfiguration();
			}

			private void nothing_configured()
			{
			}

			private void return_false()
			{
				_result.ShouldBeFalse();
			}

//	[Test]
//	public void Given_nothing_has_been_configured()
//	{
//		Test.Static()
//			.When(asked_if_an_instance_is_configured)
//			.With(an_unnamed_instance_configured)
//			.Should(return_true)
//			.Verify();
//	}
		}

		[TestFixture]
		public class When_asked_to_dispose_disposable_instances
		{
			private ConfigurationCollection _collection;
			private DisposeTester _disposeTester;

			[SetUp]
			public void BeforeEachTest()
			{
				_collection = new ConfigurationCollection();
			}

			[Test]
			public void Given_instances_of_disposable_types_exist()
			{
				Test.Given(_collection)
					.When(asked_to_dispose_disposable_instances)
					.With(existing_instances_of_disposable_types)
					.Should(dispose_of_any_disposable_instances)
					.Should(set_instances_to_null)
					.Verify();
			}

			[Test]
			public void Given_instances_of_non_disposable_types_exist()
			{
				Test.Given(_collection)
					.When(asked_to_dispose_disposable_instances)
					.With(existing_instances_of_non_disposable_types)
					.Should(set_instances_to_null)
					.Verify();
			}

			private void asked_to_dispose_disposable_instances()
			{
				_collection.DisposeDisposableInstances();
			}

			private void dispose_of_any_disposable_instances()
			{
				_disposeTester.Disposed.ShouldBeTrue();
			}

			private void existing_instances_of_disposable_types()
			{
				_disposeTester = new DisposeTester();
				_collection.Store(new ResolutionInfo
					{
						Instance = _disposeTester
					});
			}

			private void existing_instances_of_non_disposable_types()
			{
				_collection.Store(new ResolutionInfo
					{
						Instance = new NonDisposableTester()
					});
			}

			private void set_instances_to_null()
			{
				_collection.Get().Instance.ShouldBeNull();
			}
		}

		[TestFixture]
		public class When_asked_to_get_an_unnamed_configuration
		{
			private ConfigurationCollection _collection;
			private ResolutionInfo _configuration;
			private ResolutionInfo _result;

			[SetUp]
			public void BeforeEachTest()
			{
				_collection = new ConfigurationCollection();
			}

			[Test]
			public void Given_an_unnamed_configuration_exists()
			{
				Test.Static()
					.When(asked_to_get_a_configuration)
					.With(an_existing_unnamed_configuration)
					.Should(get_the_requested_configuration)
					.Verify();
			}

			private void an_existing_unnamed_configuration()
			{
				_configuration = new ResolutionInfo();
				_collection.Store(_configuration);
			}

			private void asked_to_get_a_configuration()
			{
				_result = _collection.Get();
			}

			private void get_the_requested_configuration()
			{
				ReferenceEquals(_result, _configuration).ShouldBeTrue();
			}
		}

		[TestFixture]
		public class When_asked_to_store_a_configuration
		{
			private ConfigurationCollection _collection;
			private ResolutionInfo _configuration;

			[SetUp]
			public void BeforeEachTest()
			{
				_collection = new ConfigurationCollection();
			}

			[Test]
			public void Given_an_unnamed_configuration()
			{
				Test.Given(_collection)
					.When(asked_to_store_a_configuration)
					.With(an_unnamed_configuration)
					.Should(store_the_configuration)
					.Verify();
			}

			[Test]
			public void Given_an_unnamed_configuration_and_an_unnamed_configuration_already_exists()
			{
				Test.Given(_collection)
					.When(asked_to_store_a_configuration)
					.With(an_existing_unnamed_configuration)
					.With(an_unnamed_configuration)
					.Should(replace_the_existing_configuration)
					.Verify();
			}

			private void an_existing_unnamed_configuration()
			{
				_collection.Store(new ResolutionInfo
					{
						IsSingleton = true
					});
			}

			private void an_unnamed_configuration()
			{
				_configuration = new ResolutionInfo();
			}

			private void asked_to_store_a_configuration()
			{
				_collection.Store(_configuration);
			}

			private void replace_the_existing_configuration()
			{
				_collection.Get().IsSingleton.ShouldBeFalse();
			}

			private void store_the_configuration()
			{
				_collection.HasConfiguration().ShouldBeTrue();
			}
		}
	}
}