using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Library;

namespace Cinema
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Filter filter = new Filter(null);

        public MainWindow()
        {
            InitializeComponent();
            remove.Click += Remove_Click;
            Closing += MainWindow_Closing;
            videolist.Items.Filter = (object obj) => filter.Meets(obj as Movie);
            videolist.MouseLeftButtonUp += VideoList_MouseLeftButtonUp;
            Movies.Instance.OnUpdate += Movies_Update;
            Movies.Instance.Init();
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            var item = taglist.SelectedItem as string;
            if (item != null)
                taglist.Items.Remove(item);
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (lastMov != null)
                SaveMovie();
        }

        Movie lastMov;

        private void VideoList_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var selected = videolist.SelectedItem as Movie;

            if (lastMov != null)
                SaveMovie();

            if (selected != null)
            {
                var path = selected.FullName.Substring(Math.Max(0, selected.FullName.Length - 73));
                filepath.Content = path == selected.FullName ? path : $"...{path}";
                filename.Text = selected.Name;
                director.Text = selected.Director;
                language.Text = selected.Language;
                date.SelectedDate = selected.ReleaseDate;

                taglist.Items.Clear();
                foreach (var item in selected.Tags)
                    taglist.Items.Add(item);
            }

            videolist.Items.Refresh();
            lastMov = selected;
        }

        void SaveMovie()
        {
            lastMov.Name = filename.Text;
            lastMov.Director = director.Text;
            lastMov.Language = language.Text;
            lastMov.ReleaseDate = date.SelectedDate.Value;

            lastMov.Tags.Clear();
            foreach (var tag in taglist.Items)
                lastMov.Tags.Add(tag.ToString());

            lastMov.Save();
        }

        void Movies_Update(IUpdateable<Movie> sender, UpdateEventArgs<Movie> args)
        {
            var movies = sender as Movies;
            switch (args.How)
            {
                case Change.ADD:
                    videolist.Dispatcher.Invoke(() => videolist.Items.Add(args.What));
                    break;
                case Change.EDIT:
                    break;
                case Change.REMOVE:
                    videolist.Dispatcher.Invoke(() => videolist.Items.Remove(args.What));
                    break;
            }
        }

        private void AddTag_Click(object sender, RoutedEventArgs e)
        {
            taglist.Items.Add(tags.Text);
            tags.Text = "";
        }

        private void Filter_Click(object sender, RoutedEventArgs e)
        {
            var filters = this.filters.Text.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < filters.Length; i++)
                filters[i] = filters[i].Trim();

            filter = new Filter(filters);

            videolist.Dispatcher.Invoke(() => { videolist.Items.Filter = (object obj) => filter.Meets(obj as Movie); });
        }

        private void filters_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Filter_Click(sender, null);
        }

        private void tags_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                AddTag_Click(sender, null);
        }

        private void videolist_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start((videolist.SelectedItem as Movie).FullName);
        }
    }
}
