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
    private Dictionary<string, GameObject> spawnablesDict;
    private Dictionary<string, Dictionary<int, float>> _weights;
    public TextAsset weightsTextAsset;
    private int squaresDone;
    private float totalWeights;

    public MPTShape currentShape;
    public MPTShape nextShape;

    void Start()
    {
        squaresDone = 0;
        instance = this;
        spawnablesDict = new Dictionary<string, GameObject>();
        foreach (GameObject cur in spawnables)
        {
            spawnablesDict.Add(cur.name, cur);
            cur.GetComponent<MPTShape>().weight = 0;
        }
        LoadWeightsData();
        UpdateWeights();
        SpawnNew();
        SpawnNew();
    }

    public void SquareDone()
    {
        ++squaresDone;
        UpdateWeights();
        MPTGameManager.Instance.DecreaseTrashPrice();
    }

    public void ResetSquaresDone()
    {
        squaresDone = 0;
        UpdateWeights();
    }

    private void LoadWeightsData()
    {
        string saveString = weightsTextAsset.text;
        saveString = saveString.Replace("\r", "");
        string[] saveLines = saveString.Split('\n');
        _weights = new Dictionary<string, Dictionary<int, float>>();

        foreach (string line in saveLines)
        {
            string[] lineSplit = line.Split(':');
            string shapeName = lineSplit[0];
            _weights.Add(shapeName, new Dictionary<int, float>());
            for (int i = 1; i < lineSplit.Length; i += 2)
            {
                _weights[shapeName].Add(int.Parse(lineSplit[i]), float.Parse(lineSplit[i + 1]));
            }
        }
    }

    private void UpdateWeights()
    {
        foreach (KeyValuePair<string, Dictionary<int, float>> kvp in _weights)
        {
            if (kvp.Value.ContainsKey(squaresDone))
            {
                spawnablesDict[kvp.Key].GetComponent<MPTShape>().weight = kvp.Value[squaresDone];
            }
        }
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
