using UnityEngine;
using System.Collections.Generic;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private List<GameObject> objects;
    [SerializeField] private GameObject playerTurn;

    private int currentIndex = 0;

    private void Start()
    {
        for (int i = 1; i < objects.Count; i++)
        {
            objects[i].SetActive(false);
        }
        objects[0].SetActive(true);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (currentIndex < objects.Count)
            {
                objects[currentIndex].SetActive(false);
                currentIndex++;

                if (currentIndex < objects.Count)
                {
                    objects[currentIndex].SetActive(true);
                }
                else
                {
                    playerTurn.SetActive(true);
                    gameObject.SetActive(false);
                }
            }
        }
    }
}