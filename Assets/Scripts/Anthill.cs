using Reflex.Attributes;
using System.Collections.Generic;
using UnityEngine;

public class Anthill : MonoBehaviour
{
    [Inject] private Players players;

    [SerializeField] private List<AnthillDen> anthillDens = new();

    public void CreateAnt()
    {
        AnthillDen den = GetDen();
        den.CreateAnt();
    }

    public void CreateEgg()
    {
        AnthillDen den = GetDen();
        den.CreateEgg();
    }

    private AnthillDen GetDen()
    {
        int denCount = players.CurrentPlayer.GetPointsCount(ResourceType.dens);
        Debug.Log($"Dens count: {denCount}");
        int randomDen = Random.Range(0, denCount);
        Debug.Log($"Random den: {randomDen}");
        return anthillDens[randomDen];
    }
}