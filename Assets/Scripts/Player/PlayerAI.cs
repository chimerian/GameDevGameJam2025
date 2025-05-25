using System.Collections.Generic;

public class PlayerAI : Player
{
    public PlayerAI(string name, PlayerType type, PointsVisual pointsVisual, Board board, Players players, Anthill anthill, TurnSystem turnSystem) : base(name, type, pointsVisual, board, players, anthill, turnSystem)
    {
    }

    public override void PlayTurn()
    {
        while (turnSystem.TurnCount % 2 == 0 && resources[ResourceType.type1] >= 2 && resources[ResourceType.type2] >= 2 && resources[ResourceType.type3] >= 2)
        {
            ExecuteAction(ActionType.LayEgg, new List<int> { 2, 2, 2 });
        }
        while (turnSystem.TurnCount % 2 == 1 && resources[ResourceType.type4] >= 2 && resources[ResourceType.type5] >= 2 && resources[ResourceType.eggs] >= 1)
        {
            ExecuteAction(ActionType.HatchAnt, new List<int> { 0, 0, 0, 2, 2, 1 });
        }

        TileSolution tileSolution = board.GetRandomSolution();
        Tile tile1 = board.GetTile(tileSolution.Position1);
        board.SelectTile(tile1);
        Tile tile2 = board.GetTile(tileSolution.Position2);
        board.SelectTile(tile2);
    }
}