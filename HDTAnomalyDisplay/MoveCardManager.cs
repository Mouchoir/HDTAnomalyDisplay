using Hearthstone_Deck_Tracker;
using Core = Hearthstone_Deck_Tracker.API.Core;
using System;
using System.Windows.Controls;
using System.Windows;
using Hearthstone_Deck_Tracker.Controls;
using Hearthstone_Deck_Tracker.Utility;

namespace HDTAnomalyDisplay
{
    public class MoveCardManager
    {
        private User32.MouseInput _mouseInput;
        private CardImage _card;

        private bool _selected;

        public MoveCardManager(CardImage cardImageToMove)
        {
            _card = cardImageToMove;
        }

        public bool ToggleUILockState()
        {
            if (Hearthstone_Deck_Tracker.Core.Game.IsRunning && _mouseInput == null)
            {
                _mouseInput = new User32.MouseInput();
                _mouseInput.LmbDown += MouseInputOnLmbDown;
                _mouseInput.LmbUp += MouseInputOnLmbUp;
                _mouseInput.MouseMoved += MouseInputOnMouseMoved;
                return true;
            }
            Dispose();
            return false;
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
            var p = Core.OverlayCanvas.PointFromScreen(new Point(pos.X-100, pos.Y-200));

            // TODO check max height and width, does not work yet
            if (p.Y < 0)
            {
                p.Y = 0;
            }
            else if (p.Y > Core.OverlayCanvas.Height)
            {

                p.Y = Core.OverlayCanvas.Height;
            }

            if (p.X < 0)
            {
                p.X = 0;
            }
            else if (p.X > Core.OverlayCanvas.Width)
            {
                p.X = Core.OverlayCanvas.Width;
            }

            Settings.Default.AnomalyCardTop = p.Y;
            Settings.Default.AnomalyCardLeft = p.X;
        }

        private bool PointInsideControl(Point p, FrameworkElement control)
        {
            var pos = control.PointFromScreen(p);
            return pos.X > 0 && pos.X < control.ActualWidth && pos.Y > 0 && pos.Y < control.ActualHeight;
        }
    }
}
