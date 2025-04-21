using UnityEngine;
using System.Collections.Generic;

public class BoardShelf : MonoBehaviour
{
    public List<Transform> spawnPoints = new List<Transform>();

    void Awake()
    {
        foreach (Transform child in transform)
        {
            if (child.CompareTag("SpawnPoint"))
            {
                spawnPoints.Add(child);
            }
        }

        Debug.Log($"Найдено точек спавна: {spawnPoints.Count}");
    }
}
