﻿using System;
using NSubstitute;
using Specify.Mocks;

namespace Specify.Examples.Mocks
{
    public class NSubstituteMockFactory : IMockFactory
    {
        public object CreateMock(Type typedService)
        {
            return Substitute.For(new[] { typedService }, null);
        }

        public bool IsAvailable()
        {
            return true;
        }
    }
}