using System.Windows;
using System.Windows.Controls;

using MahApps.Metro.Controls;
using Hearthstone_Deck_Tracker;
using System.Collections.Generic;
using System;
using System.Linq;
using Hearthstone_Deck_Tracker.Utility.Logging;
using API = Hearthstone_Deck_Tracker.API;

namespace HDTAnomalyDisplay
{
    public partial class SettingsView : UserControl
    {
        private static Flyout _flyout;

        private AnomalyDisplay AnomalyDisplay;

        private readonly int NorgannonDbfId = 103078;

        public static bool IsUnlocked { get; private set; }


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

            if (AnomalyDisplay.MoveManager == null && AnomalyDisplay == null)
            {
                Log.Info("No ongoing game, create a dummy Norgannon card that can be moved around to save the position.");
                AnomalyDisplay = new AnomalyDisplay();
                AnomalyDisplay.InitializeView(NorgannonDbfId);
                API.GameEvents.OnGameStart.Add(ClearCardDisplay);
            }

            IsUnlocked = AnomalyDisplay.MoveManager.ToggleUILockState();
            if (!IsUnlocked && AnomalyDisplay != null)
            {
                AnomalyDisplay.ClearCard();
                AnomalyDisplay = null;
            }

            BtnUnlock.Content = IsUnlocked ? "Lock overlay" : "Unlock overlay";
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            Settings.Default.AnomalyCardLeft = 0;
            Settings.Default.AnomalyCardTop = 630;
            Settings.Default.AnomalyCardScale = 100;
            Settings.Default.Save();
        }

        public void ClearCardDisplay()
        {
            if (AnomalyDisplay != null)
            {
                AnomalyDisplay.ClearCard();
                AnomalyDisplay = null;
            }
        }
    }
}
