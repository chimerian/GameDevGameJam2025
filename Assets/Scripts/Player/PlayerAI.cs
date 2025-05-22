public class PlayerAI : Player
{
    public PlayerAI(string name, PlayerType type, PointsVisual pointsVisual, Board board) : base(name, type, pointsVisual, board)
    {
    }

    public override void PlayTurn()
    {
        TileSolution tileSolution = board.GetRandomSolution();
        Tile tile1 = board.GetTile(tileSolution.Position1);
        board.SelectTile(tile1);
        Tile tile2 = board.GetTile(tileSolution.Position2);
        board.SelectTile(tile2);
    }
}