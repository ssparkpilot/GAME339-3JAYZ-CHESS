using System.Linq;
using Game339.Shared.Models;
using Game339.Shared.Services;
using Game339.Shared.Services.Implementation;
using NUnit.Framework;

namespace Game339.Tests
{
    public class PawnMovementRuleTests
    {
        private GridBoard board;
        private IChessMovementRule pawnRule;

        [SetUp]
        public void Setup()
        {
            board = new GridBoard();
            pawnRule = new PawnMovementRule();
        }

        [Test]
        public void Pawn_Can_Move_Forward_One_Tile_When_Clear()
        {
            var from = new GridPosition(1, 3);

            var moves = pawnRule.GetLegalMoves(from, board).ToList();

            Assert.That(moves.Count, Is.EqualTo(1));
            Assert.That(moves[0], Is.EqualTo(new GridPosition(2, 3)));
        }

        [Test]
        public void Pawn_Cannot_Move_Off_Board()
        {
            var from = new GridPosition(7, 4);

            var moves = pawnRule.GetLegalMoves(from, board);

            Assert.That(moves, Is.Empty);
        }

        [Test]
        public void Pawn_Cannot_Move_If_Blocked()
        {
            var from = new GridPosition(2, 2);
            var blockedPos = new GridPosition(3, 2);

            // Place blocking unit
            var blocker = new TestUnit(blockedPos);
            board.GetTile(blockedPos).Place(blocker);

            var moves = pawnRule.GetLegalMoves(from, board);

            Assert.That(moves, Is.Empty);
        }
    }
}