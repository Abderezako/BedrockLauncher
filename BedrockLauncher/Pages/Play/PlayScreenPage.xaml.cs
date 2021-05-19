﻿using BedrockLauncher.Classes;
using BedrockLauncher.Methods;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace BedrockLauncher.Pages.Play
{
    public partial class PlayScreenPage : Page
    {

        private const string ImagePathPrefix = @"pack://application:,,,/BedrockLauncher;component/resources/images/ui/bg/play_screen/";

        public Dictionary<string, string> Images = new Dictionary<string, string>()
        {
            { "NetherUpdate", "1.16_nether_update.png" },
            { "BuzzyBeesUpdate", "1.15_buzzy_bees_update.jpg" },
            { "VillagePillageUpdate", "1.14_village_pillage_update.png" },
            { "UpdateAquatic", "1.13_update_aquatic.png" },
            { "TechnicallyUpdated", "1.13_technically_updated_java.jpg" },
            { "WorldOfColorUpdate", "1.12_world_of_color_update_java.png" },
            { "ExplorationUpdate", "1.11_exploration_update_java.jpg" },
            { "CombatUpdate", "1.09_combat_update_java.jpg" },
            { "CatsAndPandasUpdate", "1.08_cats_and_pandas.jpg" },
            { "PocketEditionRelease", "1.0_pocket_edition.png" },
            { "BedrockStandard", "bedrock_standard.jfif" },
            { "BedrockMaster", "bedrock_master.jfif" },
            { "EarlyLegacyConsole", "other_early_console_era.png" },
            { "MidLegacyConsole", "other_mid_legacy_console.jpeg" },
            { "LateLegacyConsole", "other_late_legacy_console.jpg" },
            { "IndieDays", "other_indie_days.jpg" },
            { "Dungeons", "other_dungeons.jpg" },
            { "Original", "original_image.png" }
        };

        public PlayScreenPage()
        {
            InitializeComponent();
            ConfigManager.ConfigStateChanged += ConfigManager_ConfigStateChanged;
            for (int i = 0; i < Images.Count; i++)
            {
                var entry = Images.ElementAt(i);
                Images[entry.Key] = ImagePathPrefix + entry.Value;
            }
        }

        private void ConfigManager_ConfigStateChanged(object sender, EventArgs e)
        {
            RefreshInstallations();
        }

        private void RefreshInstallations()
        {
            InstallationsList.Items.Cast<MCInstallation>().ToList().ForEach(x => x.Update());

            InstallationsList.ItemsSource = ConfigManager.CurrentInstallations;
            if (InstallationsList.SelectedItem == null) InstallationsList.SelectedItem = ConfigManager.CurrentInstallations.First();
        }

        private string GetLatestImage()
        {
            return Images.First().Value;
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModels.LauncherModel.Default.UpdateUI();

            string packUri = string.Empty;
            string currentTheme = Properties.LauncherSettings.Default.CurrentTheme;

            bool isBugRock = Program.IsBugRockOfTheWeek();
            if (isBugRock)
            {
                BedrockLogo.Visibility = Visibility.Collapsed;
                BugrockLogo.Visibility = Visibility.Visible;
                BugrockOfTheWeekLogo.Visibility = Visibility.Visible;
            }
            else
            {
                BedrockLogo.Visibility = Visibility.Visible;
                BugrockLogo.Visibility = Visibility.Collapsed;
                BugrockOfTheWeekLogo.Visibility = Visibility.Collapsed;
            }

            switch (currentTheme)
            {
                case "LatestUpdate":
                    packUri = GetLatestImage();
                    break;
                default:
                    if (Images.ContainsKey(currentTheme)) packUri = Images.Where(x => x.Key == currentTheme).FirstOrDefault().Value;
                    else packUri = Images.Where(x => x.Key == "Original").FirstOrDefault().Value;
                    break;
            }

            var bmp = new BitmapImage(new Uri(packUri, UriKind.Absolute));
            ImageBrush.ImageSource = bmp;
        }

        private void MainPlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (ConfigManager.GameManager.GameProcess != null) ConfigManager.GameManager.KillGame();
            else
            {
                var i = InstallationsList.SelectedItem as MCInstallation;
                ConfigManager.GameManager.Play(i);
            }
        }
        private void InstallationsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ConfigManager.OnConfigStateChanged(sender, Events.ConfigStateArgs.Empty);
        }
    }
}
