#if ASPNET50
using Microsoft.Framework.Logging;
using System;
using System.Diagnostics.Tracing;

namespace OAuthBearerClient.Logging
{
	public class SampleLoggerProvider : ILoggerProvider
	{
		private EventListener _eventListener;
		public SampleLoggerProvider(EventListener listener)
		{
			_eventListener = listener;
		}

		public ILogger Create(string name)
		{
			return new SampleLogger(name, _eventListener);
		}
	}
}
#endif
