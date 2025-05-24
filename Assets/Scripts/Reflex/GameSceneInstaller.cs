using Reflex.Core;
using UnityEngine;

public class GameSceneInstaller : MonoBehaviour, IInstaller
{
    [SerializeField] private Board board;
    [SerializeField] private Players players;
    [SerializeField] private TurnSystem turnSystem;
    [SerializeField] private Anthill anthill;

    public void InstallBindings(ContainerBuilder builder)
    {
        builder.AddSingleton(board, typeof(Board));
        builder.AddSingleton(players, typeof(Players));
        builder.AddSingleton(turnSystem, typeof(TurnSystem));
        builder.AddSingleton(anthill, typeof(Anthill));
    }
}