using System.Text.RegularExpressions;
using Godot;
using JetBrains.Annotations;

namespace GodotModSharp.Helper;

public static class DirAccessHelper
{

    public static DirAccessFilesEnumerator GetDirAccessFilesEnumerator(this DirAccess dirAccess, [RegexPattern] string expression, SearchOption searchOption = SearchOption.AllDirectories) => new DirAccessFilesEnumerator(dirAccess, expression, searchOption);
    public static ReadOnlySpan<string> GetFiles(this DirAccess dirAccess, [RegexPattern] string expression, SearchOption searchOption = SearchOption.AllDirectories)
    {
        var dirAccessFilesEnumerator = dirAccess.GetDirAccessFilesEnumerator(expression, searchOption);
        var fileList                 = new List<string>(64);
        foreach (var files in dirAccessFilesEnumerator)
        {
            fileList.AddRange(files.ToArray());
        }
        return fileList.ToArray().AsSpan();
    }

    public ref struct DirAccessFilesEnumerator(DirAccess dirAccess, [RegexPattern] string expression, SearchOption searchOption = SearchOption.AllDirectories)
    {
        private readonly Regex _regex = new Regex(expression, RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public           ReadOnlySpan<string>     Current          { get; private set; } = ReadOnlySpan<string>.Empty;
        private          List<string>             CurrentDirectory { get; set; }         = new List<string>(128);
        private          bool                     _initialized = false;
        private readonly List<string>             _fileList    = new List<string>(128);
        public           string                   CurrentDirectoryPath;
        public           DirAccessFilesEnumerator GetEnumerator() => this;
        public bool MoveNext()
        {
            string[] files;
            string   currentDirectoryPath;
            string[] directories;
            if (!_initialized)
            {
                _initialized = true;
                CurrentDirectoryPath = dirAccess.GetCurrentDir();
                currentDirectoryPath = CurrentDirectoryPath;
                files = dirAccess.GetFiles() ?? Array.Empty<string>();
                foreach (var path in files)
                {
                    if (!_regex.IsMatch(path)) continue;
                    _fileList.Add(GetFullPath(path));
                }
                if (_fileList.Count > 0)
                {
                    Current = _fileList.ToArray().AsSpan();
                    _fileList.Clear();
                }
                else
                {
                    Current = ReadOnlySpan<string>.Empty;
                }
                if (searchOption != SearchOption.AllDirectories) return false;
                directories = dirAccess.GetDirectories() ?? Array.Empty<string>();

                CurrentDirectory.AddRange(directories.Select(GetFullPath));
                return true;
            }

            if (CurrentDirectory.Count == 0)
            {
                Current = ReadOnlySpan<string>.Empty;
                return false;
            }
            var nowDirectory = CurrentDirectory[0];
            currentDirectoryPath = nowDirectory;
            CurrentDirectory.RemoveAt(0);
            var relative = Path.GetRelativePath(CurrentDirectoryPath, nowDirectory);
            CurrentDirectoryPath = nowDirectory;
            dirAccess.ChangeDir(relative);
            directories = dirAccess.GetDirectories() ?? Array.Empty<string>();
            CurrentDirectory.AddRange(directories.Select(GetFullPath));
            files = dirAccess.GetFiles() ?? Array.Empty<string>();
            foreach (var path in files)
            {
                if (!_regex.IsMatch(path)) continue;
                _fileList.Add(GetFullPath(path));
            }
            if (_fileList.Count > 0)
            {
                Current = _fileList.ToArray().AsSpan();
                _fileList.Clear();
                return true;
            }

            Current = ReadOnlySpan<string>.Empty;
            return true;
            string GetFullPath(string path) => $"{currentDirectoryPath}/{path}";
        }
    }
}