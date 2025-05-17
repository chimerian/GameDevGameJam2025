using Reflex.Core;
using UnityEngine;

public class GameSceneInstaller : MonoBehaviour, IInstaller
{
    [SerializeField] private Board board;

    public void InstallBindings(ContainerBuilder builder)
    {
        builder.AddSingleton(board, typeof(Board));
    }
}
