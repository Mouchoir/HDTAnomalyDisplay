using Hearthstone_Deck_Tracker.Properties;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MahApps.Metro.Controls;

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
            settings.Header = Strings.GetLocalized("SettingsTitle");
            settings.Content = new SettingsView();
            Core.MainWindow.Flyouts.Items.Add(settings);
            return settings;
        }

        // Assuming the overlay is a UserControl named 'AnomalyOverlay'
        private UserControl AnomalyOverlay;

        public SettingsView()
        {
            InitializeComponent();
            AnomalyOverlay = new AnomalyOverlay(); // Replace with your actual overlay control
            LoadSettings();
        }

        private void BtnUnlock_Click(object sender, RoutedEventArgs e)
        {
            IsOverlayLocked = !IsOverlayLocked;
            UpdateLockStateUI();
            ToggleOverlayDraggable(IsOverlayLocked);
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            ResetOverlayPosition();
        }

        private void LoadSettings()
        {
            var position = new Point(Settings.Default.AnomalyCardLeft, Settings.Default.AnomalyCardBottom);
            var scale = Settings.Default.AnomalyCardScale;
            ApplyOverlaySettings(position, scale);
        }

        private void ResetOverlayPosition()
        {
            var defaultPosition = GetDefaultOverlayPosition();
            ApplyOverlayPosition(defaultPosition);
            SaveOverlayPosition(defaultPosition);
        }

        private void SaveOverlayPosition(Point position)
        {
            Settings.Default.AnomalyCardLeft = position.X;
            Settings.Default.AnomalyCardBottom = position.Y;
            Settings.Default.Save();
        }

        private Point GetDefaultOverlayPosition()
        {
            return new Point(0, 50);
        }

        private void UpdateLockStateUI()
        {
            BtnUnlock.Content = IsOverlayLocked ? "Unlock Overlay" : "Lock Overlay";
        }

        private void ApplyOverlaySettings(Point position, double scale)
        {
            Canvas.SetLeft(AnomalyOverlay, position.X);
            Canvas.SetBottom(AnomalyOverlay, position.Y);
            AnomalyOverlay.LayoutTransform = new ScaleTransform(scale, scale);
        }

        private void ApplyOverlayPosition(Point position)
        {
            Canvas.SetLeft(AnomalyOverlay, position.X);
            Canvas.SetBottom(AnomalyOverlay, position.Y);
        }

        private void ToggleOverlayDraggable(bool locked)
        {
            if (locked)
            {
                AnomalyOverlay.MouseMove -= AnomalyOverlay_MouseMove;
            }
            else
            {
                AnomalyOverlay.MouseMove += AnomalyOverlay_MouseMove;
            }
        }

        private void AnomalyOverlay_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && !IsOverlayLocked)
            {
                var mousePos = e.GetPosition(null);
                var offset = new Point(mousePos.X - AnomalyOverlay.ActualWidth / 2, mousePos.Y - AnomalyOverlay.ActualHeight / 2);
                ApplyOverlayPosition(offset);
            }
        }
    }
}
