using System;

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
				Test.Given(_collection)
					.When(asked_if_a_configuration_exists)
					.With(nothing_configured)
					.Should(return_false)
					.Verify();
			}

			[Test]
			public void Given_something_has_been_configured()
			{
				Test.Given(_collection)
					.When(asked_if_a_configuration_exists)
					.With(something_configured)
					.Should(return_true)
					.Verify();
			}

			private void asked_if_a_configuration_exists()
			{
				_result = _collection.HasConfiguration();
			}

			private static void nothing_configured()
			{
			}

			private void return_false()
			{
				_result.ShouldBeFalse();
			}

			private void return_true()
			{
				_result.ShouldBeTrue();
			}

			private void something_configured()
			{
				_collection.Store(new ResolutionInfo());
			}
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
		public class When_asked_to_get_a_named_configuration
		{
			private ConfigurationCollection _collection;
			private ResolutionInfo _configuration;
			private string _name;
			private ResolutionInfo _result;

			[SetUp]
			public void BeforeEachTest()
			{
				_collection = new ConfigurationCollection();
			}

			[Test]
			public void Given_a_matching_named_configuration_exists()
			{
				Test.Given(_collection)
					.When(asked_to_get_a_named_configuration)
					.With(a_specific_name)
					.With(an_existing_configuration_with_the_requested_name)
					.Should(get_the_requested_configuration)
					.Verify();
			}

			private void a_specific_name()
			{
				_name = "Foo";
			}

			private void an_existing_configuration_with_the_requested_name()
			{
				_configuration = new ResolutionInfo();
				new ResolutionContext(_configuration).Named(_name);
				_collection.Store(_configuration);
			}

			private void asked_to_get_a_named_configuration()
			{
				_result = _collection.Get(_name);
			}

			private void get_the_requested_configuration()
			{
				_result.ShouldBeSameInstanceAs(_configuration);
			}
		}

		[TestFixture]
		public class When_asked_to_get_the_configuration
		{
			private ConfigurationCollection _collection;
			private ResolutionInfo _namedConfiguration;
			private ResolutionInfo _result;
			private ResolutionInfo _unnamedConfiguration;

			[SetUp]
			public void BeforeEachTest()
			{
				_collection = new ConfigurationCollection();
			}

			[Test]
			public void Given_a_single_named_configuration_exists()
			{
				Test.Given(_collection)
					.When(asked_to_get_the_configuration)
					.With(an_existing_named_configuration)
					.Should(get_the_named_configuration)
					.Verify();
			}

			[Test]
			public void Given_a_single_unnamed_configuration_exists()
			{
				Test.Given(_collection)
					.When(asked_to_get_the_configuration)
					.With(an_existing_unnamed_configuration)
					.Should(get_the_unnamed_configuration)
					.Verify();
			}

			[Test]
			public void Given_multiple_configurations_exist_but_one_is_unnamed()
			{
				Test.Given(_collection)
					.When(asked_to_get_the_configuration)
					.With(an_existing_named_configuration)
					.With(an_existing_unnamed_configuration)
					.Should(get_the_unnamed_configuration)
					.Verify();
			}

			[Test]
			public void Given_multiple_named_configurations_exist()
			{
				Test.Given(_collection)
					.When(asked_to_get_the_configuration)
					.With(an_existing_named_configuration)
					.With(an_existing_named_configuration)
					.ShouldThrowException<InvalidOperationException>()
					.Verify();
			}

			[Test]
			public void Given_multiple_unnamed_configurations_were_stored()
			{
				Test.Given(_collection)
					.When(asked_to_get_the_configuration)
					.With(an_existing_unnamed_configuration)
					.With(an_existing_unnamed_configuration)
					.Should(get_the_last_stored_unnamed_configuration)
					.Verify();
			}

			private void an_existing_named_configuration()
			{
				_namedConfiguration = new ResolutionInfo
					{
						Name = Guid.NewGuid().ToString()
					};
				_collection.Store(_namedConfiguration);
			}

			private void an_existing_unnamed_configuration()
			{
				_unnamedConfiguration = new ResolutionInfo();
				_collection.Store(_unnamedConfiguration);
			}

			private void asked_to_get_the_configuration()
			{
				_result = _collection.Get();
			}

			private void get_the_last_stored_unnamed_configuration()
			{
				get_the_unnamed_configuration();
			}

			private void get_the_named_configuration()
			{
				_result.ShouldBeSameInstanceAs(_namedConfiguration);
			}

			private void get_the_unnamed_configuration()
			{
				_result.ShouldBeSameInstanceAs(_unnamedConfiguration);
			}
		}

		[TestFixture]
		public class When_asked_to_store_a_configuration
		{
			private ConfigurationCollection _collection;
			private ResolutionInfo _existingConfiguration;
			private DisposeTester _instance;
			private ResolutionInfo _newConfiguration;

			[SetUp]
			public void BeforeEachTest()
			{
				_collection = new ConfigurationCollection();
			}

			[Test]
			public void Given_a_configuration()
			{
				Test.Given(_collection)
					.When(asked_to_store_a_configuration)
					.With(a_new_configuration)
					.Should(store_the_configuration)
					.Verify();
			}

			[Test]
			public void Given_a_configuration_and_a_configuration_already_exists()
			{
				Test.Given(_collection)
					.When(asked_to_store_a_configuration)
					.With(an_existing_configuration)
					.With(a_new_configuration)
					.Should(replace_the_existing_configuration)
					.Verify();
			}

			[Test]
			public void Given_a_configuration_and_a_configuration_with_a_disposable_instance_already_exists()
			{
				Test.Given(_collection)
					.When(asked_to_store_a_configuration)
					.With(an_existing_configuration)
					.With(an_existing_disposable_instance)
					.With(a_new_configuration)
					.Should(dispose_of_the_existing_instance)
					.Verify();
			}

			[Test]
			public void Given_a_named_configuration_and_a_differently_named_configuration_already_exists()
			{
				Test.Given(_collection)
					.When(asked_to_store_a_configuration)
					.With(an_existing_named_configuration)
					.With(a_named_configuration)
					.Should(add_the_new_named_configuration)
					.Should(not_remove_the_other_named_configuration)
					.Verify();
			}

			private void a_named_configuration()
			{
				_newConfiguration = new ResolutionInfo
					{
						Name = "Bar"
					};
			}

			private void a_new_configuration()
			{
				_newConfiguration = new ResolutionInfo();
			}

			private void add_the_new_named_configuration()
			{
				_collection.Get(_newConfiguration.Name)
					.ShouldBeSameInstanceAs(_newConfiguration);
			}

			private void an_existing_configuration()
			{
				_existingConfiguration = new ResolutionInfo
					{
						IsSingleton = true
					};
				_collection.Store(_existingConfiguration);
			}

			private void an_existing_disposable_instance()
			{
				_instance = new DisposeTester();
				_existingConfiguration.Instance = _instance;
			}

			private void an_existing_named_configuration()
			{
				_existingConfiguration = new ResolutionInfo
					{
						Name = "Foo"
					};
				_collection.Store(_existingConfiguration);
			}

			private void asked_to_store_a_configuration()
			{
				_collection.Store(_newConfiguration);
			}

			private void dispose_of_the_existing_instance()
			{
				_instance.Disposed.ShouldBeTrue();
			}

			private void not_remove_the_other_named_configuration()
			{
				_collection.Get(_existingConfiguration.Name)
					.ShouldBeSameInstanceAs(_existingConfiguration);
			}

			private void replace_the_existing_configuration()
			{
				_collection.Get().ShouldBeSameInstanceAs(_newConfiguration);
			}

			private void store_the_configuration()
			{
				_collection.HasConfiguration().ShouldBeTrue();
			}
		}
	}
}