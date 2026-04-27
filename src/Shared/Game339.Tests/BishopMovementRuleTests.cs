using System.Linq;
using Game339.Shared.Models;
using Game339.Shared.Services;
using Game339.Shared.Services.Implementation;
using NUnit.Framework;

namespace Game339.Tests
{
    public class BishopMovementRuleTests
    {
        private GridBoard board;
        private IChessMovementRule bishopRule;

        [SetUp]
        public void Setup()
        {
            board = new GridBoard();
            bishopRule = new BishopMovementRule();
        }

        [Test]
        public void Bishop_From_Center_Moves_In_Four_Diagonals()
        {
            var from = new GridPosition(3, 3);

            var moves = bishopRule.GetLegalMoves(from, board).ToList();

            Assert.That(moves, Does.Contain(new GridPosition(4, 4)));
            Assert.That(moves, Does.Contain(new GridPosition(5, 5)));
            Assert.That(moves, Does.Contain(new GridPosition(2, 2)));
            Assert.That(moves, Does.Contain(new GridPosition(1, 1)));

            Assert.That(moves, Does.Contain(new GridPosition(4, 2)));
            Assert.That(moves, Does.Contain(new GridPosition(5, 1)));
            Assert.That(moves, Does.Contain(new GridPosition(2, 4)));
            Assert.That(moves, Does.Contain(new GridPosition(1, 5)));
        }

        [Test]
        public void Bishop_Stops_Before_Blocking_Unit()
        {
            var from = new GridPosition(3, 3);
            var blocker = new GridPosition(5, 5);

            board.GetTile(blocker).Place(new TestUnit(blocker));

            var moves = bishopRule.GetLegalMoves(from, board).ToList();

            Assert.That(moves, Does.Contain(new GridPosition(4, 4)));
            Assert.That(moves, Does.Not.Contain(blocker));
            Assert.That(moves, Does.Not.Contain(new GridPosition(6, 6)));
        }

        [Test]
        public void Bishop_Cannot_Move_Through_Blockers()
        {
            var from = new GridPosition(2, 2);
            var blocker = new GridPosition(4, 4);

            board.GetTile(blocker).Place(new TestUnit(blocker));

            var moves = bishopRule.GetLegalMoves(from, board).ToList();

            Assert.That(moves, Does.Not.Contain(blocker));
            Assert.That(moves, Does.Not.Contain(new GridPosition(5, 5)));
        }

        [Test]
        public void Bishop_At_Corner_Has_No_Moves_Outside_Diagonal()
        {
            var from = new GridPosition(0, 0);

            var moves = bishopRule.GetLegalMoves(from, board).ToList();

            Assert.That(moves.All(p =>
                p.X >= 0 && p.X < 8 &&
                p.Y >= 0 && p.Y < 8));
        }
    }
}