using System;
using System.Globalization;

namespace CalculadoraAnestesica.Shared
{
	public static class UserInfo
	{
		public static char DecimalSeparator =>
            CultureInfo.CurrentCulture
			.NumberFormat
			.NumberDecimalSeparator[0];
    }
}

