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

        public string ButtonText => "NO SETTINGS";
        // public string ButtonText => Strings.GetLocalized("");

        public string Author => "Mouchoir";

        public Version Version => new Version(0, 2);

        public MenuItem MenuItem => null;

        public AnomalyDisplay anomalyDisplay;

        public void OnButtonPress()
        {
        }

        public void OnLoad()
        {
            anomalyDisplay = new AnomalyDisplay();
            GameEvents.OnGameStart.Add(anomalyDisplay.HandleGameStart);
            GameEvents.OnGameEnd.Add(anomalyDisplay.ClearCard);

            // Processing GameStart logic in case plugin was loaded after starting a game
            anomalyDisplay.HandleGameStart();
        }

        public void OnUnload()
        {
            anomalyDisplay.ClearCard();
            anomalyDisplay = null;
        }

        public void OnUpdate()
        {
        }
    }
}
