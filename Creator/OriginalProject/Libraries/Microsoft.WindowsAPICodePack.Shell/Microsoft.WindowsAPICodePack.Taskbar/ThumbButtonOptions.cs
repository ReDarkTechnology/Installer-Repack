using System;

namespace Microsoft.WindowsAPICodePack.Taskbar
{
	[Flags]
	internal enum ThumbButtonOptions
	{
		Enabled = 0,
		Disabled = 1,
		DismissOnClick = 2,
		NoBackground = 4,
		Hidden = 8,
		NonInteractive = 0x10
	}
}
