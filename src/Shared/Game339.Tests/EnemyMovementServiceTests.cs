using Game339.Shared.Models;
using Game339.Shared.Services;
using Game339.Shared.Services.Implementation;
using NUnit.Framework;

namespace Game339.Tests
{
    public class EnemyMovementServiceTests
    {
        private GridBoard board;
        private EnemyMovementService service;

        [SetUp]
        public void Setup()
        {
            board = new GridBoard();
            service = new EnemyMovementService();
        }

        [Test]
        public void Enemy_Moves_According_To_Its_Movement_Rule()
        {
            var enemy = new EnemyUnit(
                new GridPosition(1, 1),
                new PawnMovementRule()
            );

            board.GetTile(enemy.Position).Place(enemy);

            service.ExecuteEnemyMoves(new[] { enemy }, board);

            Assert.That(enemy.Position, Is.EqualTo(new GridPosition(2, 1)));
        }

        [Test]
        public void Enemy_Does_Not_Move_If_Blocked()
        {
            var enemy = new EnemyUnit(
                new GridPosition(1, 1),
                new PawnMovementRule()
            );

            var blocker = new TestUnit(new GridPosition(2, 1));

            board.GetTile(enemy.Position).Place(enemy);
            board.GetTile(blocker.Position).Place(blocker);

            service.ExecuteEnemyMoves(new[] { enemy }, board);

            Assert.That(enemy.Position, Is.EqualTo(new GridPosition(1, 1)));
        }
    }
}
``