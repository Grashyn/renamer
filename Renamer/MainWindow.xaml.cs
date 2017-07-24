using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace Renamer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string Substring = "{0}[0-9]+{1}";
        private const string NumberSubstring = "[0-9]+";
        private const string NewPattern = "{0} - s{1}e{2}{3}";

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void OnStart(object sender, RoutedEventArgs e)
        {
            this.ChangeStatus(false);
            var files = Directory.GetFiles(this.DirectoryPath.Text);
            var directoryPath = this.DirectoryPath.Text;
            var leftSeason = this.LeftSeason.Text;
            var leftEpisode = this.LeftEpisode.Text;
            var rightSeason = this.RightSeason.Text;
            var rightEpisode = this.RightEpisode.Text;
            var name = this.Name.Text;
            await Task.Run(() =>
            {
                foreach (var filePath in files)
                {
                    var extension = Path.GetExtension(filePath);
                    var seasonPattern = String.Format(Substring, leftSeason, rightSeason);
                    var seasonMatche = Regex.Match(filePath, seasonPattern);
                    if (seasonMatche.Success)
                    {
                        var seasonNumber = Regex.Match(seasonMatche.Value, NumberSubstring);
                        var episodePattern = String.Format(Substring, leftEpisode, rightEpisode);
                        var episodeMatche = Regex.Match(filePath, episodePattern);
                        var episodeNumber = Regex.Match(episodeMatche.Value, NumberSubstring);
                        var sNumber = seasonNumber.Value;
                        var eNumber = episodeNumber.Value;
                        var newName = String.Format(NewPattern, name, sNumber, eNumber, extension);
                        var newPath = Path.Combine(directoryPath, newName);
                        File.Move(filePath, newPath);
                    }
                }
            });

            this.ChangeStatus(true);
        }

        private void ChangeStatus(bool isEnabled)
        {
            this.DirectoryPath.IsEnabled = isEnabled;
            this.Name.IsEnabled = isEnabled;
            this.LeftEpisode.IsEnabled = isEnabled;
            this.RightEpisode.IsEnabled = isEnabled;
            this.LeftSeason.IsEnabled = isEnabled;
            this.RightSeason.IsEnabled = isEnabled;
            this.Start.IsEnabled = isEnabled;
            this.Loader.Visibility = isEnabled ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}
