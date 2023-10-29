using System.Linq;
using System.Threading.Tasks;
using static HearthDb.Enums.GameTag;
using Hearthstone_Deck_Tracker.Enums;
using Hearthstone_Deck_Tracker.Hearthstone;
using Hearthstone_Deck_Tracker.Hearthstone.Entities;
using Hearthstone_Deck_Tracker.Utility.Logging;
using Hearthstone_Deck_Tracker.API;
using System.Windows.Controls;
using System;

namespace HDTAnomalyDisplay
{
    public class AnomalyDisplay
    {
        private readonly NormalView view;

        public AnomalyDisplay()
        {
            view = new NormalView();
            InitializeView();
            UpdateViewWithDefaultCard();
        }

        private void InitializeView()
        {
            Core.OverlayCanvas.Children.Add(view);
            Canvas.SetTop(view, 200);
            Canvas.SetLeft(view, 800);
        }

        public void UpdateViewWithDefaultCard()
        {
            // Using a named constant for clarity
            const int defaultCardDbfId = 103078;
            UpdateView(defaultCardDbfId);
        }

        internal void UpdateView(int cardDbfId)
        {
            try
            {
                Card card = Database.GetCardFromDbfId(cardDbfId, false);
                view.Update(card);
            }
            catch (Exception ex)
            {
                Log.Error($"Failed to update view: {ex}");
            }
        }

        internal async void HandleGameStart()
        {
            if (Core.Game.CurrentGameMode != GameMode.Battlegrounds)
                return;

            await AwaitGameEntity();

            Entity gameEntity = Core.Game.GameEntity;
            if (gameEntity == null)
                return;

            int? anomalyDbfId = BattlegroundsUtils.GetBattlegroundsAnomalyDbfId(gameEntity);

            if (anomalyDbfId.HasValue)
                UpdateView(anomalyDbfId.Value);
        }

        private async Task AwaitGameEntity()
        {
            const int maxAttempts = 20;
            const int delayBetweenAttempts = 500;
            const int gameFadeInDelay = 3000;

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
    }
}
