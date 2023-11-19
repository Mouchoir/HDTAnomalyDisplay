using Hearthstone_Deck_Tracker;
using Core = Hearthstone_Deck_Tracker.API.Core;
using System;
using System.Windows;
using Hearthstone_Deck_Tracker.Controls;

namespace HDTAnomalyDisplay
{
    public class MoveCardManager
    {
        private User32.MouseInput _mouseInput;
        private readonly CardImage _card;
        public bool IsUnlocked { get; private set; }


        private double xMouseDeltaFromCard;
        private double yMouseDeltaFromCard;

        private bool _selected;

        public MoveCardManager(CardImage cardImageToMove, bool isUnlocked)
        {
            _card = cardImageToMove;
            IsUnlocked = isUnlocked;
            if (IsUnlocked)
            {
                ToggleUILockState();
            }
        }

        public bool ToggleUILockState()
        {
            if (Hearthstone_Deck_Tracker.Core.Game.IsRunning && _mouseInput == null)
            {
                _mouseInput = new User32.MouseInput();
                _mouseInput.LmbDown += MouseInputOnLmbDown;
                _mouseInput.LmbUp += MouseInputOnLmbUp;
                _mouseInput.MouseMoved += MouseInputOnMouseMoved;
                IsUnlocked = true;
            }
            else
            {
                Dispose();
                IsUnlocked = false;
            }

            return IsUnlocked;
        }

        public bool isUILocked()
        {
            return _mouseInput == null;
        }

        public void Dispose()
        {
            _mouseInput?.Dispose();
            _mouseInput = null;
            _selected = false;
        }

        private void MouseInputOnLmbDown(object sender, EventArgs eventArgs)
        {
            var pos = User32.GetMousePos();
            var _mousePos = new Point(pos.X, pos.Y);
            if (PointInsideControl(_mousePos, _card))
            {
                _selected = true;
            }
        }

        private void MouseInputOnLmbUp(object sender, EventArgs eventArgs)
        {
            _selected = false;
        }

        private void MouseInputOnMouseMoved(object sender, EventArgs eventArgs)
        {
            if (!_selected)
            {
                return;
            }

            var pos = User32.GetMousePos();
            double mouseVerticalPositionAdjust = (yMouseDeltaFromCard * Settings.Default.AnomalyCardScale / 100);
            double mouseHorizontalPositionAdjust = (xMouseDeltaFromCard * Settings.Default.AnomalyCardScale / 100);
            var p = Core.OverlayCanvas.PointFromScreen(new Point(pos.X - mouseHorizontalPositionAdjust, pos.Y - mouseVerticalPositionAdjust));

            Settings.Default.AnomalyCardTop = p.Y;
            Settings.Default.AnomalyCardLeft = p.X;
        }

        private bool PointInsideControl(Point p, FrameworkElement control)
        {
            var pos = control.PointFromScreen(p);
            xMouseDeltaFromCard = pos.X;
            yMouseDeltaFromCard = pos.Y;

            return pos.X > 0 && pos.X < control.ActualWidth && pos.Y > 0 && pos.Y < control.ActualHeight;
        }
    }
}
