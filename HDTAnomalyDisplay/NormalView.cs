using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.Controls;
using System.Windows;
using System.Windows.Controls;
using GameCard = Hearthstone_Deck_Tracker.Hearthstone.Card;

namespace HDTAnomalyDisplay
{
    public class NormalView : StackPanel
    {
        public HearthstoneTextBlock Label { get; }
        public AnimatedCard View { get; }

        public NormalView() : this(new GameCard())
        {
        }

        public NormalView(GameCard card)
        {
            Orientation = Orientation.Vertical;
            Label = InitializeLabel();
            View = new AnimatedCard(card);
            Children.Add(Label);
            Children.Add(View);
        }

        private HearthstoneTextBlock InitializeLabel()
        {
            return new HearthstoneTextBlock
            {
                FontSize = 16,
                TextAlignment = TextAlignment.Center,
                Text = "JAIXISTEMAIVIREMOI", //TODO remove later
                Margin = new Thickness(0, 20, 0, 0),
                Visibility = Visibility.Visible
            };
        }

        public bool Update(GameCard card)
        {
            View.DataContext = card;
            View.Update(true);
            return true;
        }
    }
}
