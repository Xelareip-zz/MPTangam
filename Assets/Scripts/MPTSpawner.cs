using UnityEngine;
using System.Collections.Generic;

public class MPTSpawner : MonoBehaviour
{
    private static MPTSpawner instance;
    public static MPTSpawner Instance
    {
        get
        {
            return instance;
        }
    }

    public List<GameObject> spawnables;

    void Start()
    {
        instance = this;
    }

    public void SpawnNew()
    {
        int newSpawnId = Random.Range(0, spawnables.Count - 1);

        GameObject go = Instantiate<GameObject>(spawnables[newSpawnId]);
        go.transform.SetParent(transform);
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;
    }
}
