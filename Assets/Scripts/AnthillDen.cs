using System.Collections.Generic;
using UnityEngine;

public class AnthillDen : MonoBehaviour
{
    [SerializeField] private Transform eggsPrefabsParent;
    [SerializeField] private Transform antsPrefabsParent;
    [SerializeField] private GameObject smallAntPrefabs;
    [SerializeField] private Transform eggsParent;
    [SerializeField] private Transform smallAntsParent;
    [SerializeField] private Transform antsParent;

    private List<GameObject> eggsPrefabs = new();
    private List<GameObject> antsPrefabs = new();
    private List<GameObject> eggs = new();
    private List<GameObject> ants = new();
    private List<GameObject> smallAnts = new();

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

    public void CreateSmallAnt()
    {
        int randomEggIndex = Random.Range(0, eggs.Count);
        GameObject eggToDestroy = eggs[randomEggIndex];
        Vector3 eggPosition = eggToDestroy.transform.position;
        eggs.RemoveAt(randomEggIndex);
        Destroy(eggToDestroy);

        GameObject smallAntGameObject = Instantiate(smallAntPrefabs, antsParent);
        smallAntGameObject.transform.position = eggPosition;
        smallAnts.Add(smallAntGameObject);

        return;
    }

    public void CreateAnt()
    {
        int randomSmallAntIndex = Random.Range(0, smallAnts.Count);
        GameObject smallAntToDestroy = smallAnts[randomSmallAntIndex];
        Vector3 smallAntPosition = smallAntToDestroy.transform.position;
        smallAnts.RemoveAt(randomSmallAntIndex);
        Destroy(smallAntToDestroy);

        int randomAntIndex = Random.Range(0, antsPrefabs.Count);
        GameObject antGameObject = Instantiate(antsPrefabs[randomAntIndex], antsParent);
        Ant ant = antGameObject.GetComponent<Ant>();
        antGameObject.transform.position = smallAntPosition;
        ant.SetupRandom();
        antGameObject.SetActive(true);
        ants.Add(antGameObject);
    }

    public bool HasEggs()
    {
        return eggs.Count > 0;
    }

    public bool HasSmallAnt()
    {
        return smallAnts.Count > 0;
    }
}