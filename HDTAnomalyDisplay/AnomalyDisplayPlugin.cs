using Hearthstone_Deck_Tracker.API;
using ControlCard = Hearthstone_Deck_Tracker.Controls.Card;
using Hearthstone_Deck_Tracker.Hearthstone;
using Hearthstone_Deck_Tracker.Plugins;
using Hearthstone_Deck_Tracker.Utility.Logging;
using System;
using System.Windows.Controls;
using static Hearthstone_Deck_Tracker.Windows.MessageDialogs;

namespace HDTAnomalyDisplay
{
    public class AnomalyDisplayPlugin : IPlugin
    {
        public string Name => "HDTAnomalyDisplay";

        public string Description => "Displays the current anomaly on your overlay";

        public string ButtonText => "NO SETTINGS";
        // public string ButtonText => Strings.GetLocalized("");

        public string Author => "Mouchoir";

        public Version Version => new Version(0, 1);

        public MenuItem MenuItem => null;

        private AnomalyDisplay anomalyDisplay;

        public void OnButtonPress()
        {
        }

        public void OnLoad()
        {
            anomalyDisplay = new AnomalyDisplay();
            GameEvents.OnGameStart.Add(anomalyDisplay.HandleGameStart);
            /*GameEvents.OnTurnStart.Add())*/

            GameEvents.OnGameEnd.Add(anomalyDisplay.UpdateViewWithDefaultCard);

        }

        public void OnUnload()
        {
        }

        public void OnUpdate()
        {
        }

    }
}
