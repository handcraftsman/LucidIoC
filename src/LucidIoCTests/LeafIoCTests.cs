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
			private Action _configure;
			private Type _expectedType;

			[Test]
			public void Given_a_type_that_has_already_been_configured()
			{
				Test.Static()
					.When(asked_to_configure_a_type)
					.With(a_type_that_has_already_been_configured)
					.Should(replace_the_existing_configuration)
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
				LeafIoC.Configure<IComparable, Int32>();
				_configure = () => LeafIoC.Configure<IComparable, decimal>();
				_expectedType = typeof(decimal);
			}

			private void a_type_that_has_not_been_configured()
			{
				_configure = () => LeafIoC.Configure<IComparable, Int32>();
				_expectedType = typeof(Int32);
			}

			private void asked_to_configure_a_type()
			{
				_configure();
			}

			private void replace_the_existing_configuration()
			{
				var result = LeafIoC.GetInstance<IComparable>();
				result.GetType().ShouldBeEqualTo(_expectedType);
			}

			private void store_the_requested_configuration()
			{
				var result = LeafIoC.GetInstance<IComparable>();
				result.GetType().ShouldBeEqualTo(_expectedType);
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

			private void a_type_that_has_not_been_configured()
			{
				_getInstance = () => LeafIoC.GetInstance<IDisposable>();
			}

			private void asked_to_get_an_instance()
			{
				_result = _getInstance();
			}

			private void get_a_non_null_instance()
			{
				_result.ShouldNotBeNull();
			}

			private void get_a_different_instance_every_time()
			{
				_result.ShouldNotBeNull();
				var other = _getInstance();
				ReferenceEquals(_result,other).ShouldBeFalse();
			}
		}

		[TestFixture]
		public class When_asked_to_reset
		{
			[Test]
			public void Given_any_configured_types()
			{
				Test.Static()
					.When(asked_to_reset)
					.With(any_types_already_configured)
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

			private static void remove_all_exising_configurations()
			{
				LeafIoC.IsConfigured<IComparable>().ShouldBeFalse();
			}
		}
	}
}