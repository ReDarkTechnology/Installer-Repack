using System;
using System.IO;

namespace Ionic.Zip
{
	/// <summary>
	///   A decorator stream. It wraps another stream, and performs bookkeeping
	///   to keep track of the stream Position.
	/// </summary>
	/// <remarks>
	///   <para>
	///     In some cases, it is not possible to get the Position of a stream, let's
	///     say, on a write-only output stream like ASP.NET's
	///     <c>Response.OutputStream</c>, or on a different write-only stream
	///     provided as the destination for the zip by the application.  In this
	///     case, programmers can use this counting stream to count the bytes read
	///     or written.
	///   </para>
	///   <para>
	///     Consider the scenario of an application that saves a self-extracting
	///     archive (SFX), that uses a custom SFX stub.
	///   </para>
	///   <para>
	///     Saving to a filesystem file, the application would open the
	///     filesystem file (getting a <c>FileStream</c>), save the custom sfx stub
	///     into it, and then call <c>ZipFile.Save()</c>, specifying the same
	///     FileStream. <c>ZipFile.Save()</c> does the right thing for the zipentry
	///     offsets, by inquiring the Position of the <c>FileStream</c> before writing
	///     any data, and then adding that initial offset into any ZipEntry
	///     offsets in the zip directory. Everything works fine.
	///   </para>
	///   <para>
	///     Now suppose the application is an ASPNET application and it saves
	///     directly to <c>Response.OutputStream</c>. It's not possible for DotNetZip to
	///     inquire the <c>Position</c>, so the offsets for the SFX will be wrong.
	///   </para>
	///   <para>
	///     The workaround is for the application to use this class to wrap
	///     <c>HttpResponse.OutputStream</c>, then write the SFX stub and the ZipFile
	///     into that wrapper stream. Because <c>ZipFile.Save()</c> can inquire the
	///     <c>Position</c>, it will then do the right thing with the offsets.
	///   </para>
	/// </remarks>
	public class CountingStream : Stream
	{
		private Stream _s;

		private long _bytesWritten;

		private long _bytesRead;

		private long _initialOffset;

		/// <summary>
		///   Gets the wrapped stream.
		/// </summary>
		public Stream WrappedStream => _s;

		/// <summary>
		///   The count of bytes written out to the stream.
		/// </summary>
		public long BytesWritten => _bytesWritten;

		/// <summary>
		///   the count of bytes that have been read from the stream.
		/// </summary>
		public long BytesRead => _bytesRead;

		/// <summary>
		///   Whether the stream can be read.
		/// </summary>
		public override bool CanRead => _s.CanRead;

		/// <summary>
		///   Whether it is possible to call Seek() on the stream.
		/// </summary>
		public override bool CanSeek => _s.CanSeek;

		/// <summary>
		///   Whether it is possible to call Write() on the stream.
		/// </summary>
		public override bool CanWrite => _s.CanWrite;

		/// <summary>
		///   The length of the underlying stream.
		/// </summary>
		public override long Length => _s.Length;

		/// <summary>
		///   Returns the sum of number of bytes written, plus the initial
		///   offset before writing.
		/// </summary>
		public long ComputedPosition => _initialOffset + _bytesWritten;

		/// <summary>
		///   The Position of the stream.
		/// </summary>
		public override long Position
		{
			get
			{
				return _s.Position;
			}
			set
			{
				_s.Seek(value, SeekOrigin.Begin);
			}
		}

		/// <summary>
		/// The constructor.
		/// </summary>
		/// <param name="stream">The underlying stream</param>
		public CountingStream(Stream stream)
		{
			_s = stream;
			try
			{
				_initialOffset = _s.Position;
			}
			catch
			{
				_initialOffset = 0L;
			}
		}

		/// <summary>
		///    Adjust the byte count on the stream.
		/// </summary>
		///
		/// <param name="delta">
		///   the number of bytes to subtract from the count.
		/// </param>
		///
		/// <remarks>
		///   <para>
		///     Subtract delta from the count of bytes written to the stream.
		///     This is necessary when seeking back, and writing additional data,
		///     as happens in some cases when saving Zip files.
		///   </para>
		/// </remarks>
		public void Adjust(long delta)
		{
			_bytesWritten -= delta;
			if (_bytesWritten < 0)
			{
				throw new InvalidOperationException();
			}
			if (_s is CountingStream)
			{
				((CountingStream)_s).Adjust(delta);
			}
		}

		/// <summary>
		///   The read method.
		/// </summary>
		/// <param name="buffer">The buffer to hold the data read from the stream.</param>
		/// <param name="offset">the offset within the buffer to copy the first byte read.</param>
		/// <param name="count">the number of bytes to read.</param>
		/// <returns>the number of bytes read, after decryption and decompression.</returns>
		public override int Read(byte[] buffer, int offset, int count)
		{
			int num = _s.Read(buffer, offset, count);
			_bytesRead += num;
			return num;
		}

		/// <summary>
		///   Write data into the stream.
		/// </summary>
		/// <param name="buffer">The buffer holding data to write to the stream.</param>
		/// <param name="offset">the offset within that data array to find the first byte to write.</param>
		/// <param name="count">the number of bytes to write.</param>
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (count != 0)
			{
				_s.Write(buffer, offset, count);
				_bytesWritten += count;
			}
		}

		/// <summary>
		///   Flushes the underlying stream.
		/// </summary>
		public override void Flush()
		{
			_s.Flush();
		}

		/// <summary>
		///   Seek in the stream.
		/// </summary>
		/// <param name="offset">the offset point to seek to</param>
		/// <param name="origin">the reference point from which to seek</param>
		/// <returns>The new position</returns>
		public override long Seek(long offset, SeekOrigin origin)
		{
			return _s.Seek(offset, origin);
		}

		/// <summary>
		///   Set the length of the underlying stream.  Be careful with this!
		/// </summary>
		///
		/// <param name="value">the length to set on the underlying stream.</param>
		public override void SetLength(long value)
		{
			_s.SetLength(value);
		}
	}
}
