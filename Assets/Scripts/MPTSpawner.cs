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

    public MPTShape currentShape;
    public MPTShape nextShape;

    void Start()
    {
        instance = this;
        UpdateWeights();
        SpawnNew();
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

        currentShape = nextShape;
        if (currentShape != null)
        {
            currentShape.transform.localPosition = Vector3.zero;
            currentShape.GetComponent<MPTDraggable>().enabled = true;
            currentShape.transform.localPosition = Vector3.zero;
        }

        GameObject go = Instantiate<GameObject>(selectedGO);
        go.transform.SetParent(transform);
        go.transform.localPosition = new Vector3(2.0f, 0.0f, 0.0f);
        nextShape = go.GetComponent<MPTShape>();
        nextShape.GetComponent<MPTDraggable>().enabled = false;
        
    }
}
