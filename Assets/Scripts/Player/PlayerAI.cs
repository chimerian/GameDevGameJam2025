using System.Collections.Generic;

public class PlayerAI : Player
{
    public PlayerAI(string name, PlayerType type, PointsVisual pointsVisual, Board board, Players players, Anthill anthill, TurnSystem turnSystem) : base(name, type, pointsVisual, board, players, anthill, turnSystem)
    {
    }

    public override void PlayTurn()
    {
        ExecuteAction();
        ExecuteAction();

        TileSolution tileSolution = board.GetRandomSolution();
        Tile tile1 = board.GetTile(tileSolution.Position1);
        board.SelectTile(tile1);
        Tile tile2 = board.GetTile(tileSolution.Position2);
        board.SelectTile(tile2);
    }

    private void ExecuteAction()
    {
        while (resources[ResourceType.type2] >= 2 && resources[ResourceType.type4] >= 4 && resources[ResourceType.smallAnt] >= 1)
        {
            ExecuteAction(ActionType.AntTransformation, new List<int> { 0, 2, 0, 4, 0, 0, 1 });
        }
        while (resources[ResourceType.type1] >= 1 && resources[ResourceType.type5] >= 3 && resources[ResourceType.eggs] >= 1 && resources[ResourceType.smallAnt] < 5)
        {
            ExecuteAction(ActionType.HatchingEgg, new List<int> { 1, 0, 0, 0, 3, 1, 0 });
        }
        while (resources[ResourceType.type1] >= 2 && resources[ResourceType.type2] >= 2 && resources[ResourceType.type3] >= 3 && resources[ResourceType.eggs] < 5)
        {
            ExecuteAction(ActionType.EggLaying, new List<int> { 2, 2, 3 });
        }
    }
}