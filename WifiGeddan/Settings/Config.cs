/*
ExifGlass - EXIF metadata viewer
Copyright (C) 2023 DUONG DIEU PHAP
Project homepage: https://github.com/d2phap/ExifGlass

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/
using Avalonia;
using Avalonia.Controls;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace WifiGeddan.Core;

public class Config
{
    public const string DATETIME_FORMAT = "yyyy/MM/dd HH:mm:ss";
    public const string DATE_FORMAT = "yyyy/MM/dd";
    public const string MS_APPSTORE_ID = "9MX8S9HZ57W8";


    private static string ConfigFileName => "exifglass.config.json";
    public static string ConfigDir => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), AppName);
    private static string ConfigFilePath => Path.Combine(ConfigDir, ConfigFileName);


    /// <summary>
    /// Gets app name.
    /// </summary>
    public static string AppName => Process.GetCurrentProcess().MainModule?.FileVersionInfo.ProductName ?? "[ExifGlass]";

    /// <summary>
    /// Gets app version.
    /// </summary>
    public static Version AppVersion => new(Process.GetCurrentProcess().MainModule?.FileVersionInfo.FileVersion ?? "1.0.0.0");
}


public enum ThemeMode
{
    Default,
    Dark,
    Light,
}
