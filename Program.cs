using happy_squirrel_mc;
using System;
using System.Diagnostics;
using System.IO;
using static System.Console;

namespace HappySquirrelMc
{
    internal static class Program
    {
        private static void Main()
        {
            Title = "행복해";
            OutputEncoding = System.Text.Encoding.UTF8;

            while (true)
            {
                ClearAndWriteTitle();
                WriteLine("┌──────────────────────────────────────────┐");
                WriteLine("\t1. 설치하기\t2. 되돌리기");
                WriteLine("└──────────────────────────────────────────┘");

                switch (ReadKey(intercept: true).Key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        Install();
                        break;

                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        Restore();
                        break;

                    case ConsoleKey.Q:
                    case ConsoleKey.Escape:
                        return;
                }
            }
        }

        private static void Install()
        {
            ClearAndWriteTitle();

            BackupAndApply(Strings.RESOURCE_PACK);
            BackupAndApply(Strings.MODS);
            BackupAndApply(Strings.CONFIG);
            BackupAndApply(Strings.SHADER_PACK);

            RunForgeInstaller();

            Write("끝!");
            ReadKey();
        }

        private static void Restore()
        {
            ClearAndWriteTitle();

            ApplyFromBackup(Strings.RESOURCE_PACK);
            ApplyFromBackup(Strings.MODS);
            ApplyFromBackup(Strings.CONFIG);
            ApplyFromBackup(Strings.SHADER_PACK);

            Write("끝!");
            ReadKey();
        }

        private static void BackupAndApply(string folderName)
        {
            string str = Strings.korName[folderName];

            if (FsExtra.SameDir(Path.Combine(Strings.BinDir, folderName), Path.Combine(Strings.MinecraftDir, folderName)))
            {
                WriteGrayLine($"\n동일한 {str}이(가) 이미 적용되어 있습니다.");
                return;
            }

            WriteSuccessLine($"\n{str} 백업 중");

            if (Backup(folderName))
            {
                WriteSuccessLine($"\n{str} 적용 중");
                ApplyToMinecraft(folderName);
            }
        }

        private static void ApplyFromBackup(string folderName)
        {
            WriteSuccessLine($"\n{Strings.korName[folderName]} 되돌리는 중");
            ApplyToMinecraft(folderName, srcIsBak: true);
        }

        private static bool Backup(string name)
        {
            string src = Path.Combine(Strings.MinecraftDir, name);
            string dest = Path.Combine(Strings.BakDir, name);

            if (!Directory.Exists(src))
            {
                WriteGrayLine($" -> 건너뜁니다. (\"{src}\"를 찾을 수 없음.)");
                return false;
            }

            FsExtra.EmptyDir(dest);
            FsExtra.CopyDir(src, dest, true);
            return true;
        }

        private static bool ApplyToMinecraft(string name, bool srcIsBak = false)
        {
            string srcDir = Path.Combine(srcIsBak ? Strings.BakDir : Strings.BinDir, name);
            string destDir = Path.Combine(Strings.MinecraftDir, name);

            if (!Directory.Exists(srcDir))
            {
                WriteGrayLine(" -> 건너뜁니다.");
                return false;
            }

            FsExtra.EmptyDir(destDir);
            FsExtra.CopyDir(srcDir, destDir, true);
            return true;
        }

        private static void RunForgeInstaller()
        {
            if (!File.Exists(Strings.InstallJar) || !File.Exists(Strings.JavaExe))
            {
                WriteGrayLine("\n포지 설치 파일을 찾을 수 없습니다.");
                return;
            }

            WriteSuccessLine("\n포지를 설치해주세요.");


            ProcessStartInfo ps = new ProcessStartInfo
            {
                FileName = Strings.JavaExe,
                Arguments = $"-jar \"{Strings.InstallJar}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var proc = Process.Start(ps))
            {
                try
                {
                    WriteLine(proc.StandardOutput.ReadToEnd());
                    WriteLine(proc.StandardError.ReadToEnd());
                    proc.WaitForExit();
                }
                catch (Exception e)
                {
                    WriteError($"설치 중 오류가 발생했어요. {e.Message}");
                }
            }
            ;
        }

        private static void ClearAndWriteTitle()
        {
            Clear();
            WriteTitle();
        }

        private static void WriteTitle()
        {
            const string TitleText = "\n\n\t 행복한 다람쥐 ( •̀ ω •́ )✧\n\n";
            ForegroundColor = ConsoleColor.Yellow;
            WriteLine(TitleText);
            ResetColor();
        }

        private static void WriteSuccessLine(string text)
        {
            ForegroundColor = ConsoleColor.Green;
            Write(text);
            ResetColor();
        }

        private static void WriteGrayLine(string text)
        {
            ForegroundColor = ConsoleColor.Gray;
            Write(text);
            ResetColor();
        }

        private static void WriteError(string text)
        {
            ForegroundColor = ConsoleColor.Red;
            WriteLine(text);
            ResetColor();
        }
    }
}
