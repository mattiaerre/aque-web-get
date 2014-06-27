using System;

namespace Aque.WebGet.Core.Events
{
	public class ScanCompletedEventArgs : EventArgs
	{
		public ScanCompletedStatus Status { get; private set; }
		public Exception Exception { get; private set; }

		public ScanCompletedEventArgs(ScanCompletedStatus status, Exception exception = null)
		{
			Status = status;
			Exception = exception;
		}
	}
}