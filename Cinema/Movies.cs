using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Library;

namespace Cinema
{
    class Movies : SortedConcurrentUpdateableDictionary<int, DateTime, Movie>
    {
        static Movies instance;
        public static Movies Instance { get { return instance == null ? instance = new Movies() : instance; } }

        private Movies()
        {
        }

        public void Init()
        {
            Task.Run(() => LoadMoviesFromDirectory(Directory.GetCurrentDirectory()));
        }

        async void LoadMoviesFromDirectory(string path)
        {
            await Task.Yield();
            Console.WriteLine($"Working with directory: '{path}'");
            var dirs = Directory.GetDirectories(path);
            FileInfo[] files = new DirectoryInfo(path).GetFiles();
            foreach (var dir in dirs)
                Task.Run(() => LoadMoviesFromDirectory(dir));

            foreach (var file in files)
            {
                if (file.Extension == ".avi" || file.Extension == ".mkv" || file.Extension == ".mp4")
                {
                    Console.WriteLine($"File: {file.Name}");
                    Add(file.CreationTime, new Movie(file));
                }
            }
        }
    }
}
