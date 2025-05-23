using System.Collections.Generic;

public class Player
{
    protected Anthill anthill;
    protected Board board;
    protected Players players;

    private readonly string name;
    private readonly PlayerType type;
    private readonly Dictionary<ResourceType, int> resources;
    private readonly PointsVisual pointsVisual;

    public Player(string name, PlayerType type, PointsVisual pointsVisual, Board board, Players players, Anthill anthill)
    {
        this.name = name;
        this.type = type;
        this.pointsVisual = pointsVisual;
        this.board = board;
        this.players = players;
        this.anthill = anthill;

        pointsVisual.SetPlayerName(name);

        resources = new Dictionary<ResourceType, int>
        {
            { ResourceType.type1, 0 },
            { ResourceType.type2, 0 },
            { ResourceType.type3, 0 },
            { ResourceType.type4, 0 },
            { ResourceType.type5, 0 },
            { ResourceType.eggs, 0 },
            { ResourceType.ants, 1 },
            { ResourceType.tunnels, 0 },
            { ResourceType.dens, 1 }
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

        if (actionType == ActionType.LayEgg)
        {
            resources[ResourceType.eggs]++;
            anthill.CreateEgg();
        }
        else if (actionType == ActionType.HatchAnt)
        {
            resources[ResourceType.eggs]--;
            resources[ResourceType.ants]++;
            anthill.CreateAnt();
        }
        else if (actionType == ActionType.BuildTunnel)
        {
            resources[ResourceType.tunnels]++;
        }
        else if (actionType == ActionType.BuildDen)
        {
            resources[ResourceType.dens]++;
        }

        players.SetupButtons();
    }
}