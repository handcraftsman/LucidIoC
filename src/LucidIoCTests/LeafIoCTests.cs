using System;
using System.Configuration;

using FluentAssert;

using NUnit.Framework;

namespace gar3t.LucidIoC.Tests
{
	public class LeafIoCTests
	{
		[TestFixture]
		public class When_asked_if_a_type_is_configured
		{
			private Func<bool> _isConfigured;
			private bool _result;

			[Test]
			public void Given_a_type_that_has_been_configured()
			{
				Test.Static()
					.When(asked_if_a_type_is_configured)
					.With(a_type_that_has_been_configured)
					.Should(return_true)
					.Verify();
			}

			[Test]
			public void Given_a_type_that_has_not_been_configured()
			{
				Test.Static()
					.When(asked_if_a_type_is_configured)
					.With(a_type_that_has_not_been_configured)
					.Should(return_false)
					.Verify();
			}

			private void a_type_that_has_been_configured()
			{
				LeafIoC.Configure<IComparable, Int32>();
				_isConfigured = () => LeafIoC.IsConfigured<IComparable>();
			}

			private void a_type_that_has_not_been_configured()
			{
				_isConfigured = () => LeafIoC.IsConfigured<IDisposable>();
			}

			private void asked_if_a_type_is_configured()
			{
				_result = _isConfigured();
			}

			private void return_false()
			{
				_result.ShouldBeFalse();
			}

			private void return_true()
			{
				_result.ShouldBeTrue();
			}
		}

		[TestFixture]
		public class When_asked_to_configure_a_type
		{
			private Type _expectedType;

			[Test]
			public void Given_a_type_that_has_already_been_configured()
			{
				Test.Static()
					.When(asked_to_configure_a_type)
					.With(a_type_that_has_already_been_configured)
					.Should(store_the_requested_configuration)
					.Should(still_contain_the_other_configuration)
					.Verify();
			}

			[Test]
			public void Given_a_type_that_has_not_been_configured()
			{
				Test.Static()
					.When(asked_to_configure_a_type)
					.With(a_type_that_has_not_been_configured)
					.Should(store_the_requested_configuration)
					.Verify();
			}

			private void a_type_that_has_already_been_configured()
			{
				LeafIoC.Configure<IComparable, decimal>().Named("Decimal");
			}

			private void a_type_that_has_not_been_configured()
			{
				LeafIoC.Reset();
			}

			private void asked_to_configure_a_type()
			{
				LeafIoC.Configure<IComparable, Int32>();
				_expectedType = typeof(Int32);
			}

			private void still_contain_the_other_configuration()
			{
				var result = LeafIoC.GetInstance<IComparable>("Decimal");
				result.GetType().ShouldBeEqualTo(typeof(decimal));
			}

			private void store_the_requested_configuration()
			{
				var result = LeafIoC.GetInstance<IComparable>();
				result.GetType().ShouldBeEqualTo(_expectedType);
			}
		}

		[TestFixture]
		public class When_asked_to_get_a_named_object_instance
		{
			private Func<object> _getInstance;
			private object _result;

			[Test]
			public void Given_a_name_that_has_been_configured()
			{
				Test.Static()
					.When(asked_to_get_a_named_instance)
					.With(a_name_that_has_been_configured)
					.With(another_name_has_been_configured)
					.Should(get_a_non_null_instance)
					.Should(get_the_instance_requested)
					.Should(get_a_different_instance_every_time)
					.Verify();
			}

			private void a_name_that_has_been_configured()
			{
				LeafIoC.Configure<IComparable, Int32>().Named("Int");
				_getInstance = () => LeafIoC.GetInstance<IComparable>("Int");
			}

			private static void another_name_has_been_configured()
			{
				LeafIoC.Configure<IComparable, Decimal>().Named("Decimal");
			}

			private void asked_to_get_a_named_instance()
			{
				_result = _getInstance();
			}

			private void get_a_different_instance_every_time()
			{
				_result.ShouldNotBeNull();
				var other = _getInstance();
				_result.ShouldNotBeSameInstanceAs(other);
			}

			private void get_a_non_null_instance()
			{
				_result.ShouldNotBeNull();
			}

			private void get_the_instance_requested()
			{
				_result.GetType().ShouldBeEqualTo(typeof(int));
			}
		}

		[TestFixture]
		public class When_asked_to_get_an_object_instance
		{
			private Func<object> _getInstance;
			private object _result;

			[Test]
			public void Given_a_type_that_has_been_configured()
			{
				Test.Static()
					.When(asked_to_get_an_instance)
					.With(a_type_that_has_been_configured)
					.Should(get_a_non_null_instance)
					.Should(get_a_different_instance_every_time)
					.Verify();
			}

			[Test]
			public void Given_a_type_that_has_been_configured_as_a_singleton()
			{
				Test.Static()
					.When(asked_to_get_an_instance)
					.With(a_type_that_has_been_configured_as_a_singleton)
					.Should(get_a_non_null_instance)
					.Should(get_the_same_instance_every_time)
					.Verify();
			}

			[Test]
			public void Given_a_type_that_has_not_been_configured()
			{
				Test.Static()
					.When(asked_to_get_an_instance)
					.With(a_type_that_has_not_been_configured)
					.ShouldThrowException<ConfigurationErrorsException>()
					.Verify();
			}

			private void a_type_that_has_been_configured()
			{
				LeafIoC.Configure<IComparable, Int32>();
				_getInstance = () => LeafIoC.GetInstance<IComparable>();
			}

			private void a_type_that_has_been_configured_as_a_singleton()
			{
				LeafIoC.Configure<IComparable, Int32>()
					.AsSingleton();
				_getInstance = () => LeafIoC.GetInstance<IComparable>();
			}

			private void a_type_that_has_not_been_configured()
			{
				_getInstance = () => LeafIoC.GetInstance<IDisposable>();
			}

			private void asked_to_get_an_instance()
			{
				_result = _getInstance();
			}

			private void get_a_different_instance_every_time()
			{
				_result.ShouldNotBeNull();
				var other = _getInstance();
				ReferenceEquals(_result, other).ShouldBeFalse();
			}

			private void get_a_non_null_instance()
			{
				_result.ShouldNotBeNull();
			}

			private void get_the_same_instance_every_time()
			{
				var other = _getInstance();
				ReferenceEquals(_result, other).ShouldBeTrue();
			}
		}

		[TestFixture]
		public class When_asked_to_reset
		{
			private static object _instance;

			[Test]
			public void Given_any_configured_types()
			{
				Test.Static()
					.When(asked_to_reset)
					.With(any_types_already_configured)
					.Should(remove_all_exising_configurations)
					.Verify();
			}

			[Test]
			public void Given_instances_of_disposable_types_exist()
			{
				Test.Static()
					.When(asked_to_reset)
					.With(instances_of_disposable_types_exist)
					.Should(dispose_of_any_disposable_instances)
					.Should(remove_all_exising_configurations)
					.Verify();
			}

			[Test]
			public void Given_instances_of_non_disposable_types_exist()
			{
				Test.Static()
					.When(asked_to_reset)
					.With(instances_of_non_disposable_types_exist)
					.Should(remove_all_exising_configurations)
					.Verify();
			}

			private static void any_types_already_configured()
			{
				LeafIoC.Configure<IComparable, Int32>();
			}

			private static void asked_to_reset()
			{
				LeafIoC.Reset();
			}

			private static void dispose_of_any_disposable_instances()
			{
				((DisposeTester)_instance).Disposed.ShouldBeTrue();
			}

			private static void instances_of_disposable_types_exist()
			{
				LeafIoC.Configure<IDisposable, DisposeTester>().AsSingleton();
				_instance = LeafIoC.GetInstance<IDisposable>();
			}

			private static void instances_of_non_disposable_types_exist()
			{
				LeafIoC.Configure<IComparable, NonDisposableTester>().AsSingleton();
				_instance = LeafIoC.GetInstance<IComparable>();
			}

			private static void remove_all_exising_configurations()
			{
				LeafIoC.IsConfigured<IComparable>().ShouldBeFalse();
			}
		}
	}
}