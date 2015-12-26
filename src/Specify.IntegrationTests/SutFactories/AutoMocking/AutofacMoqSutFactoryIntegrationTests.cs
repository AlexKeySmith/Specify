using System;
using Specify.Autofac;
using Specify.IntegrationTests.SutFactories.Ioc;
using Specify.Mocks;

namespace Specify.IntegrationTests.SutFactories.AutoMocking
{
    public class AutofacMoqSutFactoryIntegrationTests : SutFactoryIntegrationTestsBase
    {
        protected override SutFactory<T> CreateSut<T>()
        {
            Func<IMockFactory> mockFactory = () => new MoqMockFactory();
            var container = new AutofacContainerFactory().Create(mockFactory).Build();
            return new SutFactory<T>(new AutofacContainer(container));
        }
    }
}