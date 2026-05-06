using System.Linq;
using Game339.Shared.Models;
using Game339.Shared.Services;
using Game339.Shared.Services.Implementation;
using NUnit.Framework;

namespace Game339.Tests
{
    public class KnightMovementRuleTests
    {
        private GridBoard board;
        private IChessMovementRule knightRule;

        [SetUp]
        public void Setup()
        {
            board = new GridBoard();
            knightRule = new KnightMovementRule();
        }

        [Test]
        public void Knight_From_Center_Has_Four_Rightward_Moves()
        {
            var from = new GridPosition(3, 3);

            var moves = knightRule.GetLegalMoves(from, board).ToList();

            Assert.That(moves.Count, Is.EqualTo(4));
            Assert.That(moves, Does.Contain(new GridPosition(5, 4)));
            Assert.That(moves, Does.Contain(new GridPosition(5, 2)));
            Assert.That(moves, Does.Contain(new GridPosition(4, 5)));
            Assert.That(moves, Does.Contain(new GridPosition(4, 1)));
        }

        [Test]
        public void Knight_At_Left_Edge_Has_Limited_Rightward_Moves()
        {
            var from = new GridPosition(0, 0);

            var moves = knightRule.GetLegalMoves(from, board).ToList();

            // Only moves that stay in bounds
            Assert.That(moves.Count, Is.EqualTo(2));
            Assert.That(moves, Does.Contain(new GridPosition(2, 1)));
            Assert.That(moves, Does.Contain(new GridPosition(1, 2)));
        }

        [Test]
        public void Knight_Respects_Board_Bounds()
        {
            var from = new GridPosition(7, 7);

            var moves = knightRule.GetLegalMoves(from, board).ToList();

            Assert.That(moves.All(p =>
                p.X >= 0 && p.X < 8 &&
                p.Y >= 0 && p.Y < 8));
        }

        [Test]
        public void Knight_Only_Moves_Right()
        {
            var from = new GridPosition(3, 3);

            var moves = knightRule.GetLegalMoves(from, board).ToList();

            Assert.That(moves.All(p => p.X > from.X));
        }

        [Test]
        public void Knight_Cannot_Land_On_Occupied_Tile()
        {
            var from = new GridPosition(3, 3);
            var blocked = new GridPosition(5, 4);

            board.GetTile(blocked).Place(new TestUnit(blocked));

            var moves = knightRule.GetLegalMoves(from, board).ToList();

            Assert.That(moves, Does.Not.Contain(blocked));
        }

        [Test]
        public void Knight_Can_Jump_Over_Other_Units()
        {
            var from = new GridPosition(3, 3);

            // These should not block movement
            board.GetTile(new GridPosition(4, 3)).Place(new TestUnit(new GridPosition(4, 3)));
            board.GetTile(new GridPosition(3, 4)).Place(new TestUnit(new GridPosition(3, 4)));

            var moves = knightRule.GetLegalMoves(from, board).ToList();

            // Still should allow up to 4 rightward moves
            Assert.That(moves.Count, Is.EqualTo(4));
        }
    }
}