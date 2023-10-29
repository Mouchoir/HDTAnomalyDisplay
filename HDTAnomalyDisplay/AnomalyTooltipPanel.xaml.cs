using Hearthstone_Deck_Tracker.Enums.Hearthstone;
using Hearthstone_Deck_Tracker;
using System;
using static Hearthstone_Deck_Tracker.Windows.MessageDialogs;
using System.Windows.Controls;

namespace HDTAnomalyDisplay
{
    public partial class AnomalyTooltipPanel : UserControl, IDisposable
    {

        public AnomalyTooltipPanel()
        {
/*            InitializeComponent();
            Settings.Load();

            resize = new Resize(this, ReconnectButton, ReconnectText, Settings.Instance.reconnect);

            Visibility = Visibility.Collapsed;
            oriBrush = ReconnectButton.Background;

            resize.AddToOverlayWindowPrivate();
            resize.UpdatePosition();

            updatePositionHandler = new RoutedEventHandler((sender, e) =>
            {
                resize.UpdatePosition();
            });
            Core.OverlayCanvas.AddHandler(SizeChangedEvent, updatePositionHandler);*/
        }

        public void Dispose()
        {
/*            timer.Dispose();
            Settings.Instance.reconnect = resize.settings;
            Settings.Save();
            resize.RemoveFromOverlayWindowPrivate();
            Core.OverlayCanvas.RemoveHandler(SizeChangedEvent, updatePositionHandler);
            connectionLogWatcher?.Stop();*/
        }

        public void OnUpdate()
        {
            // Show reconnect button:
            // 1. When we want to resize to move it in menu
            // 2. When we're in the match
/*            Visibility = Core.Game.IsInMenu ? resize.resizeGrip.Visibility : Visibility.Visible;*/

        }

        private bool IsInMainOrBgMenu()
        {
            return Core.Game.CurrentMode == Mode.HUB || Core.Game.CurrentMode == Mode.BACON;
        }
    }
}
