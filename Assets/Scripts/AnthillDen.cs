using System.Collections.Generic;
using UnityEngine;

public class AnthillDen : MonoBehaviour
{
    [SerializeField] private Transform eggsPrefabsParent;
    [SerializeField] private Transform antsPrefabsParent;
    [SerializeField] private Transform eggsParent;
    [SerializeField] private Transform antsParent;

    private List<GameObject> eggsPrefabs = new();
    private List<GameObject> antsPrefabs = new();
    private List<GameObject> eggs = new();
    private List<GameObject> ants = new();

    private void Start()
    {
        foreach (Transform child in eggsPrefabsParent)
        {
            child.gameObject.SetActive(false);
            eggsPrefabs.Add(child.gameObject);
        }

        foreach (Transform child in antsPrefabsParent)
        {
            child.gameObject.SetActive(false);
            antsPrefabs.Add(child.gameObject);
        }
    }

    public void CreateEgg()
    {
        int randomEggIndex = Random.Range(0, eggsPrefabs.Count);
        GameObject egg = Instantiate(eggsPrefabs[randomEggIndex], eggsParent);
        egg.SetActive(true);
        eggs.Add(egg);
    }

    internal void DestroyEgg()
    {
        if (eggs.Count <= 0)
        {
            return;
        }

        int randomEggIndex = Random.Range(0, eggs.Count);
        GameObject eggToDestroy = eggs[randomEggIndex];
        eggs.RemoveAt(randomEggIndex);
        Destroy(eggToDestroy);
    }

    public void CreateAnt()
    {
        int randomAntIndex = Random.Range(0, antsPrefabs.Count);
        GameObject antGameObject = Instantiate(antsPrefabs[randomAntIndex], antsParent);
        Ant ant = antGameObject.GetComponent<Ant>();
        ant.SetupRandom();
        antGameObject.SetActive(true);
        ants.Add(antGameObject);
    }

    public bool HasEggs()
    {
        return eggs.Count > 0;
    }
}