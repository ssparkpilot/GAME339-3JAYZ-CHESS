using Game339.Shared.Models;
using Game339.Shared.Services;

namespace Game339.Shared.Services.Implementation
{
    public class TurnManager : ITurnManager
    {
        public TurnOwner CurrentOwner { get; private set; }
        public TurnPhase CurrentPhase { get; private set; }

        public void StartGame()
        {
            CurrentOwner = TurnOwner.Player;
            CurrentPhase = TurnPhase.PlayerTurnStart;
        }

        public void AdvancePhase()
        {
            switch (CurrentPhase)
            {
                case TurnPhase.PlayerTurnStart:
                    CurrentPhase = TurnPhase.PlayerActing;
                    break;

                case TurnPhase.PlayerActing:
                    CurrentPhase = TurnPhase.PlayerTurnEnd;
                    break;

                case TurnPhase.PlayerTurnEnd:
                    CurrentOwner = TurnOwner.Enemy;
                    CurrentPhase = TurnPhase.EnemyTurnStart;
                    break;

                case TurnPhase.EnemyTurnStart:
                    CurrentPhase = TurnPhase.EnemyMoving;
                    break;

                case TurnPhase.EnemyMoving:
                    CurrentPhase = TurnPhase.EnemyTurnEnd;
                    break;

                case TurnPhase.EnemyTurnEnd:
                    CurrentOwner = TurnOwner.Player;
                    CurrentPhase = TurnPhase.PlayerTurnStart;
                    break;
            }
        }

        public bool CanPlayerAct()
        {
            return CurrentOwner == TurnOwner.Player &&
                   CurrentPhase == TurnPhase.PlayerActing;
        }

        public bool CanEnemyAct()
        {
            return CurrentOwner == TurnOwner.Enemy &&
                   CurrentPhase == TurnPhase.EnemyMoving;
        }
    }
}