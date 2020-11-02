using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Sequel.Infrastructure.Component
{
    public sealed class DllWatcher : IDisposable
    {
        private static readonly object _lock = new object();

        private const string PATH = "./dll";

        private readonly ComponentManager _moduleManager;
        private readonly FileSystemWatcher _fileSystemWatcher;

        public DllWatcher(ComponentManager moduleManager, string localPath)
        {
            _moduleManager = moduleManager;

            string dllPath = Path.Combine(localPath, PATH);
            if (Directory.Exists(dllPath) == false)
                Directory.CreateDirectory(dllPath);

            Init(dllPath);

            _fileSystemWatcher = new FileSystemWatcher(dllPath)
            {
                NotifyFilter = NotifyFilters.FileName,
                Filter = "*.dll"
            };

            _fileSystemWatcher.Changed += OnChanged;
            _fileSystemWatcher.Deleted += OnChanged;
            _fileSystemWatcher.Created += OnChanged;
            _fileSystemWatcher.EnableRaisingEvents = true;
        }

        public void Dispose()
        {
            _fileSystemWatcher.Changed -= OnChanged;
            _fileSystemWatcher.Deleted -= OnChanged;
            _fileSystemWatcher.Deleted -= OnChanged;
            _fileSystemWatcher.Dispose();
        }

        private void Init(string path)
        {
            lock (_lock)
            {
                IEnumerable<string> dlls = Directory.EnumerateFiles(path, "*.dll");
                foreach (string dll in dlls)
                {
                    byte[] content = File.ReadAllBytes(dll);
                    Assembly newAssembly = Assembly.Load(content);

                    IEnumerable<Type> newModules = newAssembly.GetTypes().Where(p => typeof(IComponent).IsAssignableFrom(p) && p.IsClass && p.IsAbstract == false);

                    if (newModules.Any())
                        _moduleManager.AddAndLoadComponents(newModules.ToArray());
                    else
                        _moduleManager.LoadComponents();
                }
            }
        }

        private void OnChanged(object source, FileSystemEventArgs args)
        {
            Task.Factory.StartNew(() =>
            {
                if (args.ChangeType == WatcherChangeTypes.Created)
                {
                    lock (_lock)
                    {
                        WaitForFile(args.FullPath);
                        byte[] content = File.ReadAllBytes(args.FullPath);
                        Assembly newAssembly = Assembly.Load(content);
                        IEnumerable<Type> newModules = newAssembly.GetTypes().Where(p => typeof(IComponent).IsAssignableFrom(p) && p.IsClass && p.IsAbstract == false);
                        _moduleManager.AddAndLoadComponents(newModules.ToArray());
                    }
                }
            });
        }

        private void WaitForFile(string path)
        {
            bool fileLocked = true;
            while (fileLocked)
            {
                try
                {
                    using (File.Open(path, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        fileLocked = false;
                    }
                }
                catch (IOException)
                {
                    Thread.Sleep(100);
                }
            }
        }
    }
}
