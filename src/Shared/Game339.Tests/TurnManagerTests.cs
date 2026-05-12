using Game339.Shared.Models;
using Game339.Shared.Services;
using Game339.Shared.Services.Implementation;
using NUnit.Framework;

namespace Game339.Tests
{
    public class TurnManagerTests
    {
        private ITurnManager turnManager;

        [SetUp]
        public void Setup()
        {
            turnManager = new TurnManager(EmptyGameLog.Instance);
            turnManager.StartGame();
        }

        [Test]
        public void Game_Starts_On_Player_Turn()
        {
            Assert.That(turnManager.CurrentOwner, Is.EqualTo(TurnOwner.Player));
            Assert.That(turnManager.CurrentPhase, Is.EqualTo(TurnPhase.PlayerTurnStart));
        }

        [Test]
        public void Player_Can_Act_Only_During_PlayerActing()
        {
            turnManager.AdvancePhase(); // PlayerActing
            Assert.That(turnManager.CanPlayerAct(), Is.True);

            turnManager.AdvancePhase(); // PlayerTurnEnd
            Assert.That(turnManager.CanPlayerAct(), Is.False);
        }

        [Test]
        public void Enemy_Can_Act_Only_During_EnemyMoving()
        {
            // Advance to EnemyMoving
            turnManager.AdvancePhase(); // PlayerActing
            turnManager.AdvancePhase(); // PlayerTurnEnd
            turnManager.AdvancePhase(); // EnemyTurnStart
            turnManager.AdvancePhase(); // EnemyMoving

            Assert.That(turnManager.CanEnemyAct(), Is.True);
        }

        [Test]
        public void Turns_Cycle_Back_To_Player()
        {
            // Full cycle
            for (int i = 0; i < 6; i++)
                turnManager.AdvancePhase();

            Assert.That(turnManager.CurrentOwner, Is.EqualTo(TurnOwner.Player));
            Assert.That(turnManager.CurrentPhase, Is.EqualTo(TurnPhase.PlayerTurnStart));
        }
    }
}