using Game339.Shared.Models;

namespace Game339.Shared.Services
{
    public interface ITurnManager
    {
        TurnOwner CurrentOwner { get; }
        TurnPhase CurrentPhase { get; }

        void StartGame();

        int GetTurnNumber();

        void AdvancePhase();
        bool CanPlayerAct();
        bool CanEnemyAct();
    }
}