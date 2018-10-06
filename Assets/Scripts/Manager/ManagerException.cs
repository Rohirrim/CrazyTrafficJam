using System;
using System.Runtime.Serialization;

namespace IronSideStudio.CrazyTrafficJam
{
	[Serializable]
	internal class ManagerException : Exception
	{
		public ManagerException()
		{
		}

		public ManagerException(string message) : base(message)
		{
		}

		public ManagerException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected ManagerException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}