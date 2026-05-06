using System.Linq;
using Game339.Shared.Models;
using Game339.Shared.Services;
using Game339.Shared.Services.Implementation;
using NUnit.Framework;

namespace Game339.Tests
{
    public class RookMovementRuleTests
    {
        private GridBoard board;
        private IChessMovementRule rookRule;

        [SetUp]
        public void Setup()
        {
            board = new GridBoard();
            rookRule = new RookMovementRule();
        }

        [Test]
        public void Rook_From_Center_Moves_In_Allowed_Directions()
        {
            var from = new GridPosition(3, 3);

            var moves = rookRule.GetLegalMoves(from, board).ToList();

            Assert.That(moves, Does.Contain(new GridPosition(3, 7))); // Up
            Assert.That(moves, Does.Contain(new GridPosition(3, 0))); // Down
            Assert.That(moves, Does.Contain(new GridPosition(7, 3))); // Right
        }

        [Test]
        public void Rook_Does_Not_Move_Left()
        {
            var from = new GridPosition(3, 3);

            var moves = rookRule.GetLegalMoves(from, board).ToList();

            Assert.That(moves.All(p => p.X >= from.X));
        }

        [Test]
        public void Rook_Stops_Before_Blocking_Unit()
        {
            var from = new GridPosition(3, 3);
            var blocker = new GridPosition(5, 3);

            board.GetTile(blocker).Place(new TestUnit(blocker));

            var moves = rookRule.GetLegalMoves(from, board).ToList();

            Assert.That(moves, Does.Contain(new GridPosition(4, 3))); // allowed
            Assert.That(moves, Does.Not.Contain(blocker));            // blocked
            Assert.That(moves, Does.Not.Contain(new GridPosition(6, 3)));
        }

        [Test]
        public void Rook_Cannot_Move_Through_Blockers()
        {
            var from = new GridPosition(2, 2);
            var blockerUp = new GridPosition(2, 4);
            var blockerDown = new GridPosition(2, 1);

            board.GetTile(blockerUp).Place(new TestUnit(blockerUp));
            board.GetTile(blockerDown).Place(new TestUnit(blockerDown));

            var moves = rookRule.GetLegalMoves(from, board).ToList();

            Assert.That(moves, Does.Not.Contain(blockerUp));
            Assert.That(moves, Does.Not.Contain(blockerDown));
            Assert.That(moves, Does.Not.Contain(new GridPosition(2, 5)));
            Assert.That(moves, Does.Not.Contain(new GridPosition(2, 0)));
        }

        [Test]
        public void Rook_At_Edge_Only_Moves_In_Valid_Directions()
        {
            var from = new GridPosition(0, 0);

            var moves = rookRule.GetLegalMoves(from, board).ToList();

            Assert.That(moves.All(p =>
                p.X >= 0 && p.X < 8 &&
                p.Y >= 0 && p.Y < 8));
        }
    }
}