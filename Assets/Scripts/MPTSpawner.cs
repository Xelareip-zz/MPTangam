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
    private float totalWeights;

    void Start()
    {
        instance = this;
        UpdateWeights();
        SpawnNew();
    }

    private void UpdateWeights()
    {
        totalWeights = 0;
        foreach (GameObject go in spawnables)
        {
            MPTShape shape = go.GetComponent<MPTShape>();
            totalWeights += shape.weight;
        }
    }

    public void SpawnNew()
    {
        UpdateWeights();
        float newSpawnRand = Random.Range(0.0f, totalWeights);
        
        GameObject selectedGO = null;

        foreach (GameObject currentGo in spawnables)
        {
            MPTShape shape = currentGo.GetComponent<MPTShape>();
            newSpawnRand -= shape.weight;
            if (newSpawnRand <= 0.0f)
            {
                selectedGO = currentGo;
                break;
            }
        }

        GameObject go = Instantiate<GameObject>(selectedGO);
        go.transform.SetParent(transform);
        go.transform.localPosition = Vector3.zero;
    }
}
