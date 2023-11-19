using Hearthstone_Deck_Tracker.Properties;
using System.Windows;
using System.Windows.Controls;

using MahApps.Metro.Controls;
using Hearthstone_Deck_Tracker;
using System.Collections.Generic;
using System;
using System.Linq;

namespace HDTAnomalyDisplay
{
    public partial class SettingsView : UserControl
    {
        private static Flyout _flyout;

        public static Flyout Flyout
        {
            get
            {
                if (_flyout == null)
                {
                    _flyout = CreateSettingsFlyout();
                }
                return _flyout;
            }
        }

        private static Flyout CreateSettingsFlyout()
        {
            var settings = new Flyout();
            settings.Position = Position.Left;
            Panel.SetZIndex(settings, 100);
            settings.Header = "Settings";
            settings.Content = new SettingsView();
            Core.MainWindow.Flyouts.Items.Add(settings);
            return settings;
        }

        public SettingsView()
        {
            InitializeComponent();
        }
        public IEnumerable<Orientation> OrientationTypes => Enum.GetValues(typeof(Orientation)).Cast<Orientation>();

        private void BtnUnlock_Click(object sender, RoutedEventArgs e)
        {
            if (AnomalyDisplay.MoveManager != null)
            {
                BtnUnlock.Content = AnomalyDisplay.MoveManager.ToggleUILockState() ? "Lock overlay" : "Unlock overlay";
            }
            else
            {
                // No ongoing game, create a dummy Norgannon card that can be moved around to save the position

            }

        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            Settings.Default.AnomalyCardLeft = 0;
            Settings.Default.AnomalyCardTop = 630;
            Settings.Default.AnomalyCardScale = 100;
            Settings.Default.Save();
        }
    }
}
