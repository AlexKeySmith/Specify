﻿using System;
using System.IO;
using System.Reflection;
using NSubstitute;
using NUnit.Framework;
using Shouldly;
using Specify.Mocks;

namespace Specify.Tests.Mocks
{
    [TestFixture]
    public abstract class MockFactoryTestsFor<T> where T : IMockFactory, new()
    {
        protected abstract string AssemblyName { get; }
        protected abstract string TypeName { get; }

        protected abstract IMockFactory CreateSut(IFileSystem fileSystem);

        [Test]
        public void should_throw_FileNotFoundException_if_Mock_assembly_not_referenced()
        {
            var fileSystem = Substitute.For<IFileSystem>();
            fileSystem.Load(Arg.Any<string>()).Returns(x => { throw new FileNotFoundException(); });

            Should.Throw<FileNotFoundException>(() => CreateSut(fileSystem));
        }

        [Test]
        public void should_throw_InvalidOperationException_if_FakeItEasy_version_not_compatible()
        {
            var assembly = Assembly.Load(AssemblyName);
            var fileSystem = Substitute.For<IFileSystem>();
            fileSystem.Load(Arg.Any<string>()).Returns(assembly);
            fileSystem.GetTypeFrom(Arg.Any<Assembly>(), Arg.Any<string>()).Returns((Type)null);

            Should.Throw<InvalidOperationException>(() => CreateSut(fileSystem))
                .Message.ShouldStartWith($"Unable to find Type {TypeName} in assembly ");
        }
    }
}
