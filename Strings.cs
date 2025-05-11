using System.IO;
using System;
using System.Collections.Generic;

namespace happy_squirrel_mc
{
    internal class Strings
    {
        public static readonly string RootDir = AppDomain.CurrentDomain.BaseDirectory;
        public static readonly string MinecraftDir =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".minecraft");

        public static readonly string BinDir = Path.Combine(RootDir, "bin");
        public static readonly string BakDir = Path.Combine(RootDir, "bak");

        public static readonly string JavaExe = Path.Combine(BinDir, @"jre\bin\java.exe");
        public static readonly string InstallJar = Path.Combine(BinDir, "install.jar");

        public static readonly string RESOURCE_PACK = "resourcepacks";
        public static readonly string MODS = "mods";
        public static readonly string SHADER_PACK = "shaderpacks";
        public static readonly string CONFIG = "config";

        public static readonly Dictionary<string, string> korName = new Dictionary<string, string>
        {
            { Strings.RESOURCE_PACK, "리소스팩" },
            { Strings.MODS, "모드" },
            { Strings.SHADER_PACK, "쉐이더" },
            { Strings.CONFIG, "모드 설정" }
        };
    }
}
