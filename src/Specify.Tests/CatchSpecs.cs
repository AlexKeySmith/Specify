﻿using System;
using NSubstitute;
using Specify.Tests.Stubs;

namespace Specify.Tests
{
    using NUnit.Framework;

    using Shouldly;

    public class CatchSpecs
    {
        public class when_calling_Catch_Exception_with_an_action
        {
            [Test]
            public void throwing_action_should_return_same_exception()
            {
                Catch.Exception(() => { throw new InvalidOperationException(); })
                    .ShouldBeOfType<InvalidOperationException>();
            }

            [Test]
            public void non_throwing_action_should_return_null()
            {
                Catch.Exception(() =>
                {
                    var x = 1;
                    x++;
                })
                    .ShouldBe(null);
            }
        }

        public class when_calling_Catch_Exception_with_a_func
        {
            private int _returnValue;
            private IDependency1 _behaviour;

            [SetUp]
            public void SetUp()
            {
                _returnValue = 3;
                _behaviour = Substitute.For<IDependency1>();
            }

            [Test]
            public void throwing_func_should_fail_with_operation_and_return_same_exception()
            {
                _behaviour.Value.Returns(x => { throw new InvalidOperationException(); });

                var result = Catch.Exception(() => _returnValue = _behaviour.Value);

                result.ShouldBeOfType<InvalidOperationException>();
                _returnValue.ShouldBe(3);
            }

            [Test]
            public void non_throwing_func_should_succeed_with_operation_and_return_null()
            {
                _behaviour.Value.Returns(23);

                var result = Catch.Exception(() => _returnValue = _behaviour.Value);

                result.ShouldBe(null);
                _returnValue.ShouldBe(23);
            }
        }

        public class when_calling_Catch_Only_with_an_action
        {
            [Test]
            public void throwing_matching_exception_should_return_same_exception()
            {
                Catch.Only<InvalidOperationException>(() => { throw new InvalidOperationException(); })
                    .ShouldBeOfType<InvalidOperationException>();
            }

            [Test]
            public void throwing_non_matching_exception_should_throw_non_matching_exception()
            {
                Action action = () => Catch.Only<InvalidOperationException>(() => { throw new ArgumentException(); });
                Should.Throw<ArgumentException>(action);
            }

            [Test]
            public void non_throwing_action_should_return_null()
            {
                Catch.Only<InvalidOperationException>(() =>
                {
                    var x = 1;
                    x++;
                })
                    .ShouldBe(null);
            }
        }

    }
}
