using System;
using System.ComponentModel;

namespace Ionic
{
	/// <summary>
	/// Summary description for EnumUtil.
	/// </summary>
	internal sealed class EnumUtil
	{
		private EnumUtil()
		{
		}

		/// <summary>
		///   Returns the value of the DescriptionAttribute if the specified Enum
		///   value has one.  If not, returns the ToString() representation of the
		///   Enum value.
		/// </summary>
		/// <param name="value">The Enum to get the description for</param>
		/// <returns></returns>
		internal static string GetDescription(Enum value)
		{
			DescriptionAttribute[] array = (DescriptionAttribute[])value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
			if (array.Length != 0)
			{
				return array[0].Description;
			}
			return value.ToString();
		}

		/// <summary>
		///   Converts the string representation of the name or numeric value of one
		///   or more enumerated constants to an equivalent enumerated object.
		///   Note: use the DescriptionAttribute on enum values to enable this.
		/// </summary>
		/// <param name="enumType">The System.Type of the enumeration.</param>
		/// <param name="stringRepresentation">
		///   A string containing the name or value to convert.
		/// </param>
		/// <returns></returns>
		internal static object Parse(Type enumType, string stringRepresentation)
		{
			return Parse(enumType, stringRepresentation, false);
		}

		/// <summary>
		///   Converts the string representation of the name or numeric value of one
		///   or more enumerated constants to an equivalent enumerated object.  A
		///   parameter specified whether the operation is case-sensitive.  Note:
		///   use the DescriptionAttribute on enum values to enable this.
		/// </summary>
		/// <param name="enumType">The System.Type of the enumeration.</param>
		/// <param name="stringRepresentation">
		///   A string containing the name or value to convert.
		/// </param>
		/// <param name="ignoreCase">
		///   Whether the operation is case-sensitive or not.</param>
		/// <returns></returns>
		internal static object Parse(Type enumType, string stringRepresentation, bool ignoreCase)
		{
			if (ignoreCase)
			{
				stringRepresentation = stringRepresentation.ToLower();
			}
			foreach (Enum value in Enum.GetValues(enumType))
			{
				string text = GetDescription(value);
				if (ignoreCase)
				{
					text = text.ToLower();
				}
				if (text == stringRepresentation)
				{
					return value;
				}
			}
			return Enum.Parse(enumType, stringRepresentation, ignoreCase);
		}
	}
}
