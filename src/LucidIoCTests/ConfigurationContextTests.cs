using FluentAssert;

using NUnit.Framework;

namespace gar3t.LucidIoC.Tests
{
	public class ResolutionContextTests
	{
		[TestFixture]
		public class When_asked_to_update_the_resolution_info
		{
			private ResolutionContext _context;
			private string _name;
			private ResolutionInfo _resolutionInfo;
			private ResolutionContext _result;

			[Test]
			public void Given_a_request_to_make_it_singleton()
			{
				Test.Static()
					.When(asked_to_make_it_a_singleton)
					.With(non_singleton_resolution_info)
					.With(context_containing_the_resolution_info)
					.Should(configure_the_resolution_info_to_be_singleton)
					.Should(return_the_context)
					.Verify();
			}

			[Test]
			public void Given_a_request_to_name_it()
			{
				Test.Static()
					.When(asked_to_name_it)
					.With(context_containing_the_resolution_info)
					.Should(configure_the_name_of_the_resolution_info)
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

			private void configure_the_name_of_the_resolution_info()
			{
				_resolutionInfo.Name.ShouldBeEqualTo(_name);
			}

			private void configure_the_resolution_info_to_be_singleton()
			{
				_resolutionInfo.IsSingleton.ShouldBeTrue();
			}

			private void context_containing_the_resolution_info()
			{
				_context = new ResolutionContext(_resolutionInfo);
			}

			private void non_singleton_resolution_info()
			{
				_resolutionInfo = new ResolutionInfo
					{
						IsSingleton = false
					};
			}

			private void return_the_context()
			{
				ReferenceEquals(_result, _context).ShouldBeTrue();
			}
		}
	}
}