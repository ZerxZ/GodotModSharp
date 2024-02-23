using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Godot;
using JetBrains.Annotations;


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
        private readonly Regex     _regex     = new Regex(expression, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private          DirAccess _dirAccess = dirAccess;

        public           ReadOnlySpan<string>     Current          { get; private set; } = ReadOnlySpan<string>.Empty;
        private          Queue<string>            CurrentDirectory { get; set; }         = new Queue<string>();
        public           DirAccess                CurrentDirAccess;
        private          bool                     _initialized = false;
        private readonly List<string>             _fileList    = new List<string>();
        public           string                   CurrentDirectoryPath;
        public           string                   RootPath = dirAccess.GetCurrentDir();
        public           DirAccessFilesEnumerator GetEnumerator() => this;
        public bool MoveNext()
        {
            if (!_initialized)
            {
                _initialized = true;
                CurrentDirAccess = _dirAccess;
                CurrentDirectoryPath = dirAccess.GetCurrentDir();
            }
            else
            {
                if (CurrentDirectory.Count == 0)
                {
                    return false;
                }
                var nowDirectory = CurrentDirectory.Dequeue();
                var relative     = Path.GetRelativePath(CurrentDirectoryPath, nowDirectory);
                CurrentDirectoryPath = nowDirectory;
                // GD.Print(relative);
                _dirAccess.ChangeDir(relative);
            }

            switch (CurrentDirAccess.ListDirBegin())
            {

                case Error.Ok:
                    var path = CurrentDirAccess.GetNext();
                    while (true)
                    {
                        if (string.IsNullOrEmpty(path))
                        {
                            break;
                        }
                        var fullpaths = $"{CurrentDirectoryPath}/{path}";
                        if (CurrentDirAccess.DirExists(fullpaths))
                        {
                            if (searchOption == SearchOption.AllDirectories)
                            {
                                CurrentDirectory.Enqueue(fullpaths);
                            }

                        }
                        else
                        {
                            if (_regex.IsMatch(path))
                            {
                                _fileList.Add(fullpaths);
                            }

                        }
                        path = CurrentDirAccess.GetNext();
                    }

                    Current = _fileList.ToArray().AsSpan();
                    _fileList.Clear();
                    return true;
                case Error.Failed:
                    Current = Array.Empty<string>();
                    return false;
            }
            Current = Array.Empty<string>();
            return false;
        }
    }
}