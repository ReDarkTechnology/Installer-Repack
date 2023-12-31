using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Ionic.Zip
{
	/// <summary>
	/// Issued when an <c>ZipEntry.ExtractWithPassword()</c> method is invoked
	/// with an incorrect password.
	/// </summary>
	[Serializable]
	[Guid("ebc25cf6-9120-4283-b972-0e5520d0000B")]
	public class BadPasswordException : ZipException
	{
		/// <summary>
		/// Default ctor.
		/// </summary>
		public BadPasswordException()
		{
		}

		/// <summary>
		/// Come on, you know how exceptions work. Why are you looking at this documentation?
		/// </summary>
		/// <param name="message">The message in the exception.</param>
		public BadPasswordException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Come on, you know how exceptions work. Why are you looking at this documentation?
		/// </summary>
		/// <param name="message">The message in the exception.</param>
		/// <param name="innerException">The innerException for this exception.</param>
		public BadPasswordException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		/// <summary>
		/// Come on, you know how exceptions work. Why are you looking at this documentation?
		/// </summary>
		/// <param name="info">The serialization info for the exception.</param>
		/// <param name="context">The streaming context from which to deserialize.</param>
		protected BadPasswordException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
