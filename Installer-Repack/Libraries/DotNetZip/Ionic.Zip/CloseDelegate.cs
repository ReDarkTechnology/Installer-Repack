using System.IO;

namespace Ionic.Zip
{
	/// <summary>
	///   Delegate in which the application closes the stream, just-in-time, for the named entry.
	/// </summary>
	///
	/// <param name="entryName">
	/// The name of the ZipEntry that the application should close the stream for.
	/// </param>
	///
	/// <param name="stream">The stream to be closed.</param>
	///
	/// <remarks>
	///   When you add an entry via <see cref="M:Ionic.Zip.ZipFile.AddEntry(System.String,Ionic.Zip.OpenDelegate,Ionic.Zip.CloseDelegate)" />, the application code provides the logic that
	///   opens and closes the stream for the given ZipEntry.
	/// </remarks>
	///
	/// <seealso cref="M:Ionic.Zip.ZipFile.AddEntry(System.String,Ionic.Zip.OpenDelegate,Ionic.Zip.CloseDelegate)" />
	public delegate void CloseDelegate(string entryName, Stream stream);
}
