using Reflex.Attributes;
using System.Collections.Generic;
using UnityEngine;

public class Anthill : MonoBehaviour
{
    [Inject] private Players players;

    [SerializeField] private List<AnthillDen> anthillDens = new();

    public void CreateEgg()
    {
        AnthillDen den = GetRandomDen();
        den.CreateEgg();
    }

    public void CreateAnt()
    {
        AnthillDen den = GetDenWithEgg();
        den.CreateAnt();
        den.DestroyEgg();
    }

    private AnthillDen GetRandomDen()
    {
        int denCount = players.CurrentPlayer.GetPointsCount(ResourceType.dens);
        int randomDen = Random.Range(0, denCount);
        return anthillDens[randomDen];
    }

    private AnthillDen GetDenWithEgg()
    {
        int attempts = 0;
        int maxAttempts = 1000;
        while (attempts < maxAttempts)
        {
            int randomIndex = Random.Range(0, anthillDens.Count);
            if (anthillDens[randomIndex].HasEggs())
                return anthillDens[randomIndex];
            attempts++;
        }
        return null;
    }
}