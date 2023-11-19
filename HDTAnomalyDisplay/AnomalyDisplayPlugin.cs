using Hearthstone_Deck_Tracker.API;
using Hearthstone_Deck_Tracker.Plugins;
using System;
using System.Windows.Controls;

namespace HDTAnomalyDisplay
{
    public class AnomalyDisplayPlugin : IPlugin
    {
        public string Name => "HDTAnomalyDisplay";

        public string Description => "Displays the current Battlegrounds anomaly on your overlay";

        public string ButtonText => "SETTINGS";

        public string Author => "Mouchoir & Tignus";

        public Version Version => new Version(1, 2, 0);

        public MenuItem MenuItem => CreateMenu();

        private MenuItem CreateMenu()
        {
            MenuItem settingsMenuItem = new MenuItem { Header = "Anomaly Display Settings" };

            settingsMenuItem.Click += (sender, args) =>
            {
                SettingsView.Flyout.IsOpen = true;
            };

            return settingsMenuItem;
        }
        public AnomalyDisplay anomalyDisplay;

        public void OnButtonPress() => SettingsView.Flyout.IsOpen = true;

        public void OnLoad()
        {
            anomalyDisplay = new AnomalyDisplay();
            GameEvents.OnGameStart.Add(anomalyDisplay.HandleGameStart);
            GameEvents.OnGameEnd.Add(anomalyDisplay.ClearCard);

            // Processing GameStart logic in case plugin was loaded/unloaded after starting a game without restarting HDT
            anomalyDisplay.HandleGameStart();
        }

        public void OnUnload()
        {
            Settings.Default.Save();
            anomalyDisplay.ClearCard();
            anomalyDisplay = null;
        }

        public void OnUpdate()
        {
        }
    }
}
