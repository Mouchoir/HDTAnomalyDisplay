using System.Linq;
using System.Threading.Tasks;
using static HearthDb.Enums.GameTag;
using Hearthstone_Deck_Tracker.Enums;
using Hearthstone_Deck_Tracker.Hearthstone;
using Hearthstone_Deck_Tracker.Hearthstone.Entities;
using Hearthstone_Deck_Tracker.Utility.Logging;
using Hearthstone_Deck_Tracker.API;
using System.Windows.Controls;
using Hearthstone_Deck_Tracker.Controls;

namespace HDTAnomalyDisplay
{
    public class AnomalyDisplay
    {
        public CardImage cardImage;

        public AnomalyDisplay()
        {
        }

        public async Task AwaitGameEntity()
        {
            const int maxAttempts = 40;
            const int delayBetweenAttempts = 250;
            const int gameFadeInDelay = 1000;

            // Loop until enough heroes are loaded
            for (var i = 0; i < maxAttempts; i++)
            {
                await Task.Delay(delayBetweenAttempts);

                var loadedHeroes = Core.Game.Player.PlayerEntities
                    .Where(x => x.IsHero && (x.HasTag(BACON_HERO_CAN_BE_DRAFTED) || x.HasTag(BACON_SKIN)));

                if (loadedHeroes.Count() >= 2)
                {
                    await Task.Delay(gameFadeInDelay);
                    break;
                }
            }
        }

        public void InitializeView(int cardDbfId)
        {
            if (cardImage == null)
            {
                cardImage = new CardImage();

                Core.OverlayCanvas.Children.Add(cardImage);
                Canvas.SetBottom(cardImage, 50);
                Canvas.SetLeft(cardImage, 0);
                cardImage.Visibility = System.Windows.Visibility.Visible;
            }

            cardImage.SetCardIdFromCard(Database.GetCardFromDbfId(cardDbfId, false));
        }

        public async void HandleGameStart()
        {
            if (Core.Game.CurrentGameMode != GameMode.Battlegrounds)
                return;

            await AwaitGameEntity();

            Entity gameEntity = Core.Game.GameEntity;
            if (gameEntity == null)
                return;

            int? anomalyDbfId = BattlegroundsUtils.GetBattlegroundsAnomalyDbfId(gameEntity);

            if (anomalyDbfId.HasValue)
            {
                Log.Info("Anomaly DbfId found: " + anomalyDbfId.Value);
                InitializeView(anomalyDbfId.Value);
            }
            else
            {
                Log.Warn("No anomaly DbfId found whereas game is already started !");
            }
        }

        public void ClearCard()
        {
            cardImage.SetCardIdFromCard(null);
            Core.OverlayCanvas.Children.Remove(cardImage);
            cardImage = null;
        }
    }
}
