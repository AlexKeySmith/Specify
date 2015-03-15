﻿using System;
using NSubstitute;
using NUnit.Framework;
using Shouldly;
using Specify.Containers;
using Specify.Tests.Stubs;

namespace Specify.Tests.Containers
{
    [TestFixture]
    public class SutFactoryTests
    {
        [Test]
        public void should_use_container_to_create_sut()
        {
            var sut = CreateSut<ConcreteObjectWithNoConstructor>();
            var result = sut.SystemUnderTest;
            sut.Container.Received().Get<ConcreteObjectWithNoConstructor>();
        }

        [Test]
        public void sut_should_be_singleton()
        {
            var sut = CreateSut<ConcreteObjectWithNoConstructor>();
            var result = sut.SystemUnderTest;

            sut.SystemUnderTest.ShouldBeSameAs(result);
            sut.Container.Received(1).Get<ConcreteObjectWithNoConstructor>();
        }

        [Test]
        public void should_be_able_to_set_sut_independently()
        {
            var instance = new ConcreteObjectWithNoConstructor();
            var sut = CreateSut<ConcreteObjectWithNoConstructor>();
            var original = sut.SystemUnderTest;

            sut.SystemUnderTest = instance;

            sut.SystemUnderTest.ShouldBeSameAs(instance);
            sut.SystemUnderTest.ShouldNotBeSameAs(original);
        }

        [Test]
        public void RegisterType_should_call_container_to_register_type_if_SUT_not_set()
        {
            var sut = CreateSut<ConcreteObjectWithNoConstructor>();
            sut.RegisterType<ConcreteObjectWithNoConstructor>();
            sut.Container.Received().RegisterType<ConcreteObjectWithNoConstructor>();
        }

        [Test]
        public void RegisterType_should_throw_if_SUT_is_set()
        {
            var sut = CreateSut<ConcreteObjectWithMultipleConstructors>();
            var result = sut.SystemUnderTest;
            Should.Throw<InvalidOperationException>(() => sut.RegisterType<ConcreteObjectWithNoConstructor>())
                .Message.ShouldBe("Cannot register type after SUT is created.");
        }

        [Test]
        public void RegisterInstance_should_call_container_to_register_instance_if_SUT_not_set()
        {
            var instance = new ConcreteObjectWithNoConstructor();
            var sut = CreateSut<ConcreteObjectWithMultipleConstructors>();

            sut.RegisterInstance(instance);
            
            sut.Container.Received().RegisterInstance(instance);
        }

        [Test]
        public void RegisterInstance_should_throw_if_SUT_is_set()
        {
            var sut = CreateSut<ConcreteObjectWithMultipleConstructors>();
            var result = sut.SystemUnderTest;
            Should.Throw<InvalidOperationException>(() => sut.RegisterInstance(new ConcreteObjectWithNoConstructor()))
                .Message.ShouldBe("Cannot register instance after SUT is created.");
        }

        [Test]
        public void Resolve_generic_should_call_container_resolve_generic()
        {
            var sut = CreateSut<ConcreteObjectWithNoConstructor>();
            sut.Get<ConcreteObjectWithNoConstructor>();
            sut.Container.Received().Get<ConcreteObjectWithNoConstructor>();
        }

        [Test]
        public void Resolve_should_call_container_resolve()
        {
            var sut = CreateSut<ConcreteObjectWithNoConstructor>();
            sut.Get(typeof(ConcreteObjectWithNoConstructor));
            sut.Container.Received().Get(typeof(ConcreteObjectWithNoConstructor));
        }

        [Test]
        public void IsRegistered_generic_should_call_container_IsRegistered_generic()
        {
            var sut = CreateSut<ConcreteObjectWithNoConstructor>();
            sut.IsRegistered<ConcreteObjectWithNoConstructor>();
            sut.Container.Received().IsRegistered<ConcreteObjectWithNoConstructor>();
        }

        [Test]
        public void IsRegistered_should_call_container_IsRegistered()
        {
            var sut = CreateSut<ConcreteObjectWithNoConstructor>();
            sut.IsRegistered(typeof(ConcreteObjectWithNoConstructor));
            sut.Container.Received().IsRegistered(typeof(ConcreteObjectWithNoConstructor));
        }

        [Test]
        public void should_dispose_container_when_disposed()
        {
            var sut = CreateSut<ConcreteObjectWithNoConstructor>();
            sut.Dispose();
            sut.Container.Received().Dispose();
        }

        private SutFactory<T> CreateSut<T>() where T : class
        {
            return new SutFactory<T>(Substitute.For<IContainer>());
        }
    }
}
