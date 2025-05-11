using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace happy_squirrel_mc
{
    internal class FsExtra
    {
        public static IEnumerable<string> GetRelativePaths(string root, IEnumerable<string> fullPaths)
        {
            int rootLen = root.EndsWith(Path.DirectorySeparatorChar.ToString())
                            ? root.Length
                            : root.Length + 1; // '\' 제외
            foreach (var f in fullPaths)
                yield return f.Substring(rootLen);
        }

        /// <summary>
        /// 두 폴더가 같은 파일 구조와 각 파일 크기를 가지는지 비교합니다.
        /// </summary>
        /// <param name="path1">비교할 첫 번째 폴더 경로</param>
        /// <param name="path2">비교할 두 번째 폴더 경로</param>
        /// <returns>같으면 true, 다르면 false</returns>
        /// <summary>
        /// 두 폴더가 (1) 동일한 하위 디렉터리 구조와 (2) 각 파일의 크기까지 같으면 true.
        /// 폴더가 하나라도 없으면 false.
        /// </summary>
        public static bool SameDir(string path1, string path2)
        {
            if (!Directory.Exists(path1) || !Directory.Exists(path2))
                return false;

            var files1 = GetRelativePaths(
                            path1,
                            Directory.EnumerateFiles(path1, "*", SearchOption.AllDirectories))
                        .Select(rel => (rel, size: new FileInfo(Path.Combine(path1, rel)).Length))
                        .OrderBy(x => x.rel, StringComparer.OrdinalIgnoreCase)
                        .ToList();

            var files2 = GetRelativePaths(
                            path2,
                            Directory.EnumerateFiles(path2, "*", SearchOption.AllDirectories))
                        .Select(rel => (rel, size: new FileInfo(Path.Combine(path2, rel)).Length))
                        .OrderBy(x => x.rel, StringComparer.OrdinalIgnoreCase)
                        .ToList();

            if (files1.Count != files2.Count)
                return false;

            for (int i = 0; i < files1.Count; i++)
            {
                if (!files1[i].rel.Equals(files2[i].rel, StringComparison.OrdinalIgnoreCase) ||
                    files1[i].size != files2[i].size)
                    return false;
            }

            var dirs1 = GetRelativePaths(
                            path1,
                            Directory.EnumerateDirectories(path1, "*", SearchOption.AllDirectories))
                        .OrderBy(x => x, StringComparer.OrdinalIgnoreCase)
                        .ToList();

            var dirs2 = GetRelativePaths(
                            path2,
                            Directory.EnumerateDirectories(path2, "*", SearchOption.AllDirectories))
                        .OrderBy(x => x, StringComparer.OrdinalIgnoreCase)
                        .ToList();

            if (dirs1.Count != dirs2.Count)
                return false;

            for (int i = 0; i < dirs1.Count; i++)
            {
                if (!dirs1[i].Equals(dirs2[i], StringComparison.OrdinalIgnoreCase))
                    return false;
            }

            return true;
        }


        public static void EmptyDir(string folderPath)
        {
            if (Directory.Exists(folderPath))
            {
                foreach (string file in Directory.GetFiles(folderPath))
                {
                    File.Delete(file);
                }

                // 하위 디렉터리 삭제
                foreach (string dir in Directory.GetDirectories(folderPath))
                {
                    Directory.Delete(dir, true);
                }
            }
        }

        public static void CopyDir(string sourceDir, string destinationDir, bool recursive)
        {
            var dir = new DirectoryInfo(sourceDir);

            if (!dir.Exists)
                throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

            DirectoryInfo[] dirs = dir.GetDirectories();

            Directory.CreateDirectory(destinationDir);

            foreach (FileInfo file in dir.GetFiles())
            {
                string targetFilePath = Path.Combine(destinationDir, file.Name);
                file.CopyTo(targetFilePath);
            }

            if (recursive)
            {
                foreach (DirectoryInfo subDir in dirs)
                {
                    string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                    FsExtra.CopyDir(subDir.FullName, newDestinationDir, true);
                }
            }
        }
    }
}
