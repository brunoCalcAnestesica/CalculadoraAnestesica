using System;
using System.IO;
using Xamarin.Essentials;

namespace CalculadoraAnestesica.Shared
{
	public static class PlatformPaths
	{
		public static string Dbpath
		{
			get
			{
                string pathAppData = Environment
					.GetFolderPath(Environment
					.SpecialFolder
					.LocalApplicationData
				);

                if (DeviceInfo.Platform == DevicePlatform.iOS)
                {
                    pathAppData = Path.Combine(Environment.GetFolderPath(
                        Environment.SpecialFolder.MyDocuments),
                        "..",
                        "Library"
                    );
                }

                return Path.Combine(pathAppData, "appdb.db");
            }
		}
	}
}

