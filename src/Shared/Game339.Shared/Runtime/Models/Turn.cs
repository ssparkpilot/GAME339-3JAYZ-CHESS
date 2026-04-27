namespace Game339.Shared.Models
{
    public enum TurnOwner
    {
        Player,
        Enemy
    }

    public enum TurnPhase
    {
        None,

        PlayerTurnStart,
        PlayerActing,
        PlayerTurnEnd,

        EnemyTurnStart,
        EnemyMoving,
        EnemyTurnEnd
    }
}