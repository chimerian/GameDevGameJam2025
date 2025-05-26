using System.Collections.Generic;

public class Player
{
    protected Anthill anthill;
    protected Board board;
    protected Players players;
    protected TurnSystem turnSystem;

    protected readonly string name;
    protected readonly PlayerType type;
    protected readonly Dictionary<ResourceType, int> resources;
    protected readonly PointsVisual pointsVisual;

    private readonly int pointsToWin = 30;

    public Player(string name, PlayerType type, PointsVisual pointsVisual, Board board, Players players, Anthill anthill, TurnSystem turnSystem)
    {
        this.name = name;
        this.type = type;
        this.pointsVisual = pointsVisual;
        this.board = board;
        this.players = players;
        this.anthill = anthill;
        this.turnSystem = turnSystem;

        pointsVisual.SetPlayerName(name);

        resources = new Dictionary<ResourceType, int>
        {
            { ResourceType.type1, 0 },
            { ResourceType.type2, 0 },
            { ResourceType.type3, 0 },
            { ResourceType.type4, 0 },
            { ResourceType.type5, 0 },
            { ResourceType.eggs, 0 },
            { ResourceType.smallAnt, 0 },
            { ResourceType.ants, 1 },
            { ResourceType.tunnels, 7 },
            { ResourceType.dens, 7 }
        };
        this.players = players;
    }

    public string Name => name;

    public PlayerType Type => type;

    public virtual void PlayTurn()
    {
    }

    public void ChangePoints(ResourceType tileType, int amountToChange)
    {
        resources[tileType] += amountToChange;
        pointsVisual.SetPoints(tileType, resources[tileType]);
    }

    public void ShowBorder()
    {
        pointsVisual.ShowBorder();
    }

    public void HideBorder()
    {
        pointsVisual.HideBorder();
    }

    public int GetPointsCount(ResourceType tileType)
    {
        return resources[tileType];
    }

    public void ExecuteAction(ActionType actionType, List<int> costs)
    {
        for (int i = 0; i < costs.Count; i++)
        {
            if (costs[i] > 0)
            {
                ResourceType tileType = (ResourceType)i;
                ChangePoints(tileType, -costs[i]);
                pointsVisual.SetPoints(tileType, resources[tileType]);
            }
        }

        if (actionType == ActionType.EggLaying)
        {
            resources[ResourceType.eggs]++;
            anthill.CreateEgg();
        }
        else if (actionType == ActionType.HatchingEgg)
        {
            resources[ResourceType.smallAnt]++;
            anthill.CreateSmallAnt();
        }
        else if (actionType == ActionType.AntTransformation)
        {
            resources[ResourceType.ants]++;
            anthill.CreateAnt();

            if (resources[ResourceType.ants] == pointsToWin)
            {
                turnSystem.ShowEndGame(name);
                board.SetEndGame();
            }
        }

        for (int i = 0; i < 8; i++)
        {
            ResourceType tileType = (ResourceType)i;
            pointsVisual.SetPoints(tileType, resources[tileType]);
        }

        players.SetupButtons();
    }
}