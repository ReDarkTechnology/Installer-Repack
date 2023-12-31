namespace Ionic.Zip
{
	/// <summary>
	///   The method of compression to use for a particular ZipEntry.
	/// </summary>
	///
	/// <remarks>
	///   <see href="http://www.pkware.com/documents/casestudies/APPNOTE.TXT">PKWare's
	///   ZIP Specification</see> describes a number of distinct
	///   compression methods that can be used within a zip
	///   file. DotNetZip supports a subset of them.
	/// </remarks>
	public enum CompressionMethod
	{
		/// <summary>
		/// No compression at all. For COM environments, the value is 0 (zero).
		/// </summary>
		None = 0,
		/// <summary>
		///   DEFLATE compression, as described in <see href="http://www.ietf.org/rfc/rfc1951.txt">IETF RFC
		///   1951</see>.  This is the "normal" compression used in zip
		///   files. For COM environments, the value is 8.
		/// </summary>
		Deflate = 8,
		/// <summary>
		/// DEFLATE64 compression. (Decompress only).
		/// For COM environments, the value is 9.
		/// </summary>
		Deflate64 = 9,
		/// <summary>
		///   BZip2 compression, a compression algorithm developed by Julian Seward.
		///   For COM environments, the value is 12.
		/// </summary>
		BZip2 = 12
	}
}
