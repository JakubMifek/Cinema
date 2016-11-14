using System;
using System.Collections.Generic;
using System.IO;
using Library;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Cinema
{

    class Movie : IDisposable
    {
        public string InfoPath { get; }
        public string Path { get; }
        public string FileName { get; }
        public string FullName { get; }
        public string Name { get; set; }
        public string Director { get; set; }
        public string Language { get; set; }
        public DateTime ReleaseDate { get; set; }
        public ConcurrentUpdateableList<string> Tags { get; } = new ConcurrentUpdateableList<string>();

        public Movie(FileInfo file)
        {
            Path = file.Directory.FullName;
            FileName = file.Name;
            FullName = file.FullName;
            Name = file.Name;
            InfoPath = $"{Path}\\.{Name}.info";
            Director = "Unknown";
            Language = "English";
            ReleaseDate = DateTime.Now;

            FindMoreInformation();
        }

        void FindMoreInformation()
        {
            if (File.Exists(InfoPath))
            {
                using (var reader = File.OpenText(InfoPath))
                {
                    var nameLine = reader.ReadLine();
                    var directorLine = reader.ReadLine();
                    var languageLine = reader.ReadLine();
                    var releaseDateLine = reader.ReadLine();

                    if (nameLine != null)
                        Name = nameLine;
                    if (directorLine != null)
                        Director = directorLine;
                    if (languageLine != null)
                        Language = languageLine;
                    if (releaseDateLine != null)
                        ReleaseDate = DateTime.Parse(releaseDateLine);

                    string line;
                    while ((line = reader.ReadLine()) != null)
                        Tags.Add(line);
                }
            }
        }

        public void Save()
        {
            using (var writer = File.CreateText(InfoPath))
            {
                writer.WriteLine(Name);
                writer.WriteLine(Director);
                writer.WriteLine(Language);
                writer.WriteLine(ReleaseDate.ToString());
                foreach (var tag in Tags)
                    writer.WriteLine(tag);
            }
        }

        public void Dispose()
        {
            Save();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
