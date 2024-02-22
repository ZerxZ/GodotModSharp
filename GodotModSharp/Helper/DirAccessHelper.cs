using System.Text.RegularExpressions;
using Godot;
using JetBrains.Annotations;

namespace GodotModSharp.Helper;

public static class DirAccessHelper
{

    public static DirAccessFilesEnumerator GetFiles(this DirAccess dirAccess, [RegexPattern] string expression) => new DirAccessFilesEnumerator(dirAccess, expression);

    public ref struct DirAccessFilesEnumerator
    {
        private          DirAccess    _dirAccess;
        private          string       _expression;
        public           List<string> Current          { get; private set; }
        private          List<string> CurrentDirectory { get; set; } = new List<string>(128);
        private          bool         _initialized = false;
        private          List<string> newDirectory;
        private readonly List<string> fileList;
        public DirAccessFilesEnumerator(DirAccess dirAccess, [RegexPattern] string expression)
        {
            _dirAccess = dirAccess;
            _expression = expression;
            Current = new List<string>(128);
            CurrentDirectory = new List<string>(128);
            newDirectory = new List<string>(128);
            fileList = new List<string>(128);
        }
        public DirAccessFilesEnumerator GetEnumerator() => this;
        public bool TryGetFiles(List<string> dir, DirAccess dirAccess, string currentDirectoryPath, Regex regex)
        {
            var files = dirAccess.GetFiles();
            if (files != null)
            {
                foreach (var file in files)
                {
                    if (IsMatch(file))
                    {
                        fileList.Add(GetFullPath(file));
                    }
                }
            }
            var currentDirectory = dirAccess.GetDirectories();
            if (currentDirectory == null)
            {
                return false;
            }
            foreach (var directory in currentDirectory)
            {
                if (IsNotEmpty(directory))
                {
                    dir.Add(GetFullPath(directory));
                }
            }
            return true;
            bool   IsMatch(string     filepath) => regex.IsMatch(filepath);
            bool   IsNotEmpty(string  path)     => !string.IsNullOrEmpty(path);
            string GetFullPath(string path)     => $"{currentDirectoryPath}/{path}";
        }
        public bool MoveNext()
        {
            if (_initialized && Current.Count == 0 && CurrentDirectory.Count == 0)
            {
                return false;
            }
            var    regex = new Regex(_expression, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            string currentDirectoryPath;
            if (!_initialized)
            {
                _initialized = true;
                currentDirectoryPath = _dirAccess.GetCurrentDir();
                if (!TryGetFiles(CurrentDirectory,_dirAccess,currentDirectoryPath,regex))
                {
                    return false;
                }
            }


            foreach (var directory in CurrentDirectory)
            {
                
                using var currentDirAccess = DirAccess.Open(directory);
              
                if (currentDirAccess == null)
                {
                    continue;
                }
                currentDirectoryPath = currentDirAccess.GetCurrentDir();
                if (!TryGetFiles(newDirectory, currentDirAccess, currentDirectoryPath, regex))
                {
                    continue;
                }
            }
            Current.Clear();
            CurrentDirectory.Clear();
            Current.AddRange(fileList);
            CurrentDirectory.AddRange(newDirectory);
            newDirectory.Clear();
            fileList.Clear();
            return true;
        }


    }

}