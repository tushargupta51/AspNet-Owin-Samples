#if ASPNET50
using Microsoft.Framework.Logging;
using System;
using System.Diagnostics;

namespace OAuthBearerClient.Logging
{
    public static class SampleLoggerFactoryExtensions
    {
		public static ILoggerFactory AddEventSourceLogger(this ILoggerFactory factory)
		{
			factory.AddProvider(new SampleLoggerProvider(new SampleListener()));
			return factory;
		}

    }
}
#endif