using System;

#if ASPNET50
using System.Diagnostics.Tracing;

namespace OAuthBearerClient.Logging
{
	public class SampleListener : EventListener
	{
		public string TraceBuffer { get; set; }

		protected override void OnEventWritten(EventWrittenEventArgs eventData)
		{
			if (eventData.Payload != null)
			{
				TraceBuffer += (eventData.Payload[0] + "\n");
			}
		}
	}
}
#endif