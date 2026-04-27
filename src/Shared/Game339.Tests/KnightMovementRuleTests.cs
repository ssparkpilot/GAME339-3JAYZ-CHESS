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
        public void Knight_From_Center_Has_Eight_Moves()
        {
            var from = new GridPosition(3, 3);

            var moves = knightRule.GetLegalMoves(from, board).ToList();

            Assert.That(moves.Count, Is.EqualTo(8));
        }

        [Test]
        public void Knight_At_Corner_Has_Two_Moves()
        {
            var from = new GridPosition(0, 0);

            var moves = knightRule.GetLegalMoves(from, board).ToList();

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

            // Place blocking units arbitrarily (should not matter)
            board.GetTile(new GridPosition(4, 3)).Place(new TestUnit(new GridPosition(4, 3)));
            board.GetTile(new GridPosition(3, 4)).Place(new TestUnit(new GridPosition(3, 4)));

            var moves = knightRule.GetLegalMoves(from, board).ToList();

            Assert.That(moves.Count, Is.EqualTo(8));
        }
    }
}
