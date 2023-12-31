using System;
using System.IO;

namespace Ionic.Zip
{
	/// <summary>
	///   A Stream for reading and concurrently decrypting data from a zip file,
	///   or for writing and concurrently encrypting data to a zip file.
	/// </summary>
	internal class ZipCipherStream : Stream
	{
		private ZipCrypto _cipher;

		private Stream _s;

		private CryptoMode _mode;

		public override bool CanRead => _mode == CryptoMode.Decrypt;

		public override bool CanSeek => false;

		public override bool CanWrite => _mode == CryptoMode.Encrypt;

		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		public override long Position
		{
			get
			{
				throw new NotSupportedException();
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		/// <summary>  The constructor. </summary>
		/// <param name="s">The underlying stream</param>
		/// <param name="mode">To either encrypt or decrypt.</param>
		/// <param name="cipher">The pre-initialized ZipCrypto object.</param>
		public ZipCipherStream(Stream s, ZipCrypto cipher, CryptoMode mode)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			_cipher = cipher;
			_s = s;
			_mode = mode;
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			if (_mode == CryptoMode.Encrypt)
			{
				throw new NotSupportedException("This stream does not encrypt via Read()");
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			byte[] array = new byte[count];
			int num = _s.Read(array, 0, count);
			byte[] array2 = _cipher.DecryptMessage(array, num);
			for (int i = 0; i < num; i++)
			{
				buffer[offset + i] = array2[i];
			}
			return num;
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			if (_mode == CryptoMode.Decrypt)
			{
				throw new NotSupportedException("This stream does not Decrypt via Write()");
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (count == 0)
			{
				return;
			}
			byte[] array = null;
			if (offset != 0)
			{
				array = new byte[count];
				for (int i = 0; i < count; i++)
				{
					array[i] = buffer[offset + i];
				}
			}
			else
			{
				array = buffer;
			}
			byte[] array2 = _cipher.EncryptMessage(array, count);
			_s.Write(array2, 0, array2.Length);
		}

		public override void Flush()
		{
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}
	}
}
