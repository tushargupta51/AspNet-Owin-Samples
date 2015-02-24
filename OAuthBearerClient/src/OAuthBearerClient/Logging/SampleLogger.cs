// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if ASPNET50
using Microsoft.Framework.Logging;
using System;
using System.Diagnostics.Tracing;

namespace OAuthBearerClient.Logging
{
	public class SampleLogger : ILogger
	{
		private readonly SampleEventSource _eventTraceSource;
		private readonly SampleListener _eventListener;
		private string _name;

		public SampleLogger(string name, EventListener eventListener)
		{
			_eventTraceSource = SampleEventSource.EventSource;
			_name = name;
			_eventListener = (SampleListener)eventListener;
			_eventListener.EnableEvents(SampleEventSource.EventSource, EventLevel.Verbose);
		}

		public void Write(LogLevel logLevel, int eventId, object state, Exception exception, Func<object, Exception, string> formatter)
		{
			if (!IsEnabled(logLevel))
			{
				return;
			}
			var message = string.Empty;
			if (formatter != null)
			{
				message = formatter(state, exception);
			}
			else
			{
				if (state != null)
				{
					message += state;
				}
				if (exception != null)
				{
					message += Environment.NewLine + exception;
				}
			}
			if (!string.IsNullOrEmpty(message))
			{
				switch (eventId)
				{
					case 0:
					case 1:
						_eventTraceSource.WriteVerbose(message);
						break;
					case 2:
						_eventTraceSource.WriteInformation(message);
						break;
					case 3:
						_eventTraceSource.WriteWarning(message);
						break;
					case 4:
						_eventTraceSource.WriteError(message);
						break;
					case 5:
						_eventTraceSource.WriteCritical(message);
						break;
				}
			}
		}

		public IDisposable BeginScope(object state)
		{
			return null;
		}
		public bool IsEnabled(LogLevel logLevel)
		{
			return logLevel >= LogLevel.Verbose;
		}
	}
}

#endif

