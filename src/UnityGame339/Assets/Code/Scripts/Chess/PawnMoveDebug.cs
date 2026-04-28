using UnityEngine;
using System.Linq;
using Game339.Shared.Models;

public class PawnMoveDebug : MonoBehaviour
{
    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Space))
            return;

        EnemyUnit pawn = BoardManager.main.DebugPawn;
        if (pawn == null)
            return;

        var board = BoardManager.main.Board;

        // Ask the pawn's movement rule for legal moves
        var moves = pawn.MovementRule
            .GetLegalMoves(pawn.Position, board)
            .ToList();

        if (moves.Count == 0)
            return;

        // Move to the first legal tile (pawn → right)
        var target = moves[0];
        board.MoveUnit(pawn, target);

        // Update visual
        FindObjectOfType<EnemyView>().UpdatePosition();

        Debug.Log($"Pawn moved to {target.X},{target.Y}");
    }
}