#if ASPNET50
using System;
using System.Diagnostics.Tracing;

namespace OAuthBearerClient.Logging
{
	[EventSource(Name = "Microsoft.AspNet.Security")]
	public class SampleEventSource : EventSource
    {
		public static SampleEventSource EventSource = new SampleEventSource();

		[Event(1, Level = EventLevel.Verbose)]
		public void WriteVerbose(string message)
		{
			WriteEvent(1, message);
		}

		[Event(2, Level = EventLevel.Informational)]
		public void WriteInformation(string message)
		{
			WriteEvent(2, message);
		}

		[Event(3, Level = EventLevel.Warning)]
		public void WriteWarning(string message)
		{
			WriteEvent(3, message);
		}

		[Event(4, Level = EventLevel.Error)]
		public void WriteError(string message)
		{
			WriteEvent(4, message);
		}

		[Event(5, Level = EventLevel.Critical)]
		public void WriteCritical(string message)
		{
			WriteEvent(5, message);
		}
	}
}
#endif