using System;
using Game339.Shared.Models;
using Game339.Shared.Services;
using System.Linq;
using Game339.Shared.Diagnostics;

namespace Game339.Shared.Services.Implementation
{
    public class TurnManager : ITurnManager
    {
        public TurnOwner CurrentOwner { get; private set; }
        public TurnPhase CurrentPhase { get; private set; }
        
        public readonly ObservableValue<int> CurrentTurnNumber = new ObservableValue<int>(0);
        
        public bool IsBusy { get; private set; }
        
        public Action<TurnPhase> OnPhaseChanged;
        public event Action<TurnOwner, TurnPhase> OnTurnStateChanged;

        private readonly IGameLog _log;

        public TurnManager(IGameLog log)
        {
            _log = log;
        }

        public void StartGame()
        {
            CurrentOwner = TurnOwner.Player;
            CurrentPhase = TurnPhase.PlayerTurnStart;
            CurrentTurnNumber.Value=0;

            OnTurnStateChanged?.Invoke(CurrentOwner, CurrentPhase);
        }

        public int GetTurnNumber()
        {
            return CurrentTurnNumber.Value;
        }

        public void AdvancePhase()
        {
            _log.Info($"{nameof(TurnManager)}.{nameof(AdvancePhase)} - Advancing phase");
            CurrentTurnNumber.Value++;

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

            OnPhaseChanged?.Invoke(CurrentPhase);
            OnTurnStateChanged?.Invoke(CurrentOwner, CurrentPhase);
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
        
        public void SetBusy(bool value)
        {
            IsBusy = value;
        }
    }
}