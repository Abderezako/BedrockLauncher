﻿using System;
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
using BedrockLauncher.Methods;
using BedrockLauncher.Downloaders;
using MdXaml;

namespace BedrockLauncher.Pages.News
{
    /// <summary>
    /// Interaction logic for LauncherNewsPage.xaml
    /// </summary>
    public partial class LauncherNewsPage : Page
    {

        private LauncherUpdater updater;

        public LauncherNewsPage()
        {
            InitializeComponent();
        }

        public LauncherNewsPage(LauncherUpdater updater)
        {
            InitializeComponent();
            this.updater = updater;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshData();
        }

        private void RefreshData()
        {
            string latest_tag = updater.GetLatestTag();
            string latest_description = updater.GetLatestTagBody();

            if (latest_tag == string.Empty) latest_tag = "0.0.0";

            if (latest_description == string.Empty) latest_description = "N/A";

            latest_description = latest_description.Replace("\r\n", "\r\n\r\n");

            Markdown engine = new Markdown();
            FlowDocument document = engine.Transform(latest_description);

            buildVersion.Text = "v" + latest_tag;
            buildChanges.Document = document;
        }

        private void CheckForUpdatesButton_Click(object sender, RoutedEventArgs e)
        {
            updater.CheckForUpdates();
            RefreshData();
        }

        private void ForceUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            updater.UpdateButton_Click(sender, e);
        }
    }
}
