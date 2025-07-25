﻿using System.Runtime.InteropServices;

namespace DllShared;

/// <summary>
/// Loading any dll.
/// </summary>
public partial class LoadDll
{
    [LibraryImport("kernel32.dll", StringMarshalling = StringMarshalling.Utf8)]
    internal static partial IntPtr LoadLibrary(string dllToLoad);

    [LibraryImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool FreeLibrary(IntPtr hModule);

    static readonly Dictionary<string, IntPtr> FileToModule = [];

    /// <summary>
    /// Path to load dlls from.
    /// </summary>
    public static string PluginPath = "dlls";

    /// <summary>
    /// Load dlls from <see cref="PluginPath"/>.
    /// </summary>
    public static void LoadPlugins()
    {
        if (!Directory.Exists(Path.Combine(AOTHelper.CurrentPath, PluginPath)))
            return;
        var files = Directory.GetFiles(Path.Combine(AOTHelper.CurrentPath, PluginPath), "*.dll");
        foreach (var file in files)
        {
            FileToModule.Add(file, LoadLibrary(file));
        }
    }

    /// <summary>
    /// Unload all dlls.
    /// </summary>
    public static void FreePlugins()
    {
        foreach (var file in FileToModule)
        {
            FreeLibrary(file.Value);
        }
        FileToModule.Clear();
    }
}
