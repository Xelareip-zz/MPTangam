using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Analytics;

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

    public GameObject[] spawnPoints = new GameObject[3];
    public GameObject[] spawnedShapes = new GameObject[3];

    public List<GameObject> spawnables;
    private Dictionary<string, GameObject> spawnablesDict;
    private Dictionary<string, Dictionary<int, float>> _weights;
    public TextAsset weightsTextAsset;
    public TextAsset sequencesTextAsset;
    public TextAsset sequencesWeightsTextAsset;
    private Dictionary<string, List<string>> _sequences;
    private Dictionary<string, float> _sequenceToWeight;
    public string currentSequenceName;
    public List<string> currentSequence;
    public int currentRotation;
    private int squaresDone;
    private float totalWeights;

    public MPTShape currentShape;

    public GameObject cantDropText;
	public SpriteRenderer ghostRenderer;
	public PolygonCollider2D ghostCollider;

	void Start()
    {
        Analytics.SetUserGender(Gender.Male);
        squaresDone = 0;
        instance = this;
        spawnablesDict = new Dictionary<string, GameObject>();
        _sequenceToWeight = new Dictionary<string, float>();
        foreach (GameObject cur in spawnables)
        {
            spawnablesDict.Add(cur.name, cur);
            cur.GetComponent<MPTShape>().weight = 0;
        }
        //LoadWeightsData();
        LoadSequencesData();
        UpdateWeights();
    }

    public void StartGame()
    {
        SpawnNew();
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

    private void LoadSequencesData()
    {
        string saveString = sequencesTextAsset.text;
        saveString = saveString.Replace("\r", "\n");
        saveString = saveString.Replace("\n\n", "\n");
        string[] saveLines = saveString.Split('\n');

        _sequences = new Dictionary<string, List<string>>();


        foreach (string line in saveLines)
        {
            string[] lineSplit = line.Split(':');
            string shapeName = lineSplit[0];
            _sequences.Add(shapeName, new List<string>());
            for (int i = 1; i < lineSplit.Length; ++i)
            {
                _sequences[shapeName].Add(lineSplit[i]);
            }
        }
        
        saveString = sequencesWeightsTextAsset.text;
        saveString = saveString.Replace("\r", "\n");
        saveString = saveString.Replace("\n\n", "\n");
        saveLines = saveString.Split('\n');
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

    private void LoadWeightsData()
    {
        string saveString = weightsTextAsset.text;
        saveString = saveString.Replace("\r", "\n");
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

    private void UpdateWeightsSequences()
    {
        totalWeights = 0;
        _sequenceToWeight = new Dictionary<string, float>();
        foreach (KeyValuePair<string, Dictionary<int, float>> kvp in _weights)
        {
            int lastSquareNumber = 0;
            float lastWeight = 0;
            foreach (KeyValuePair<int, float> kvpWeights in kvp.Value)
            {
                if (kvpWeights.Key <= squaresDone && kvpWeights.Key >= lastSquareNumber)
                {
                    lastSquareNumber = kvpWeights.Key;
                    lastWeight = kvpWeights.Value;
                }
            }
            totalWeights += lastWeight;
            _sequenceToWeight.Add(kvp.Key, lastWeight);
        }
    }

    private void UpdateWeights()
    {
        UpdateWeightsSequences();
        return;
        /*
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
        }*/
    }

    private void PickSequence()
	{
		currentRotation = MPTInteractiveTutoManager.Instance.GetRotation();
		if (currentRotation == -1)
		{
			currentRotation = Random.Range(0, 4);
		}

		string nextShape = MPTInteractiveTutoManager.Instance.GetSpawnableShape();
		if (nextShape != "")
		{
			currentSequence = new List<string>();
			currentSequence.Add(nextShape);
			return;
		}

		float totalWeightsCopy = Random.Range(0.0f, totalWeights);
        
        foreach (string sequence in _sequences.Keys)
        {
            totalWeightsCopy -= _sequenceToWeight[sequence];
            if (totalWeightsCopy < 0.0f)
            {
                currentSequenceName = sequence;
                currentSequence = new List<string>(_sequences[currentSequenceName]);
                break;
            }
        }
    }

    public void SpawnNewSequence()
    {
        if (currentSequence.Count == 0)
        {
            UpdateWeightsSequences();
            PickSequence();
        }

        int newSpawnRand = Random.Range(0, currentSequence.Count);
        string shapeName = currentSequence[newSpawnRand];
		for (int i = 0; i < spawnedShapes.Length; ++i)
		{
			if (spawnedShapes[i] == null)
			{
				currentSequence.RemoveAt(newSpawnRand);
				CreateShape(spawnablesDict[shapeName]);
				break;
			}
		}
    }

	public bool UpdateCantDropText()
	{
		bool spaceFound = false;
		for (int shapeId = 0; shapeId < spawnedShapes.Length; ++shapeId)
		{
			if (spawnedShapes[shapeId] == null || spawnedShapes[shapeId].GetComponent<MPTShape>().FindDropSpaces())
			{
				spaceFound = true;
			}
		}
		cantDropText.SetActive(!spaceFound);
		return spaceFound;
	}

    public void SpawnNew()
    {
        SpawnNewSequence();
		UpdateCantDropText();
        return;
        /*
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

        CreateShape(selectedGO);*/
    }

    private void CreateShapeInQueue(GameObject selectedGO)
    {
        for (int spawnId = 0; spawnId < spawnedShapes.Length; ++spawnId)
        {
            if (spawnedShapes[spawnId] == null)
            {
                for (int i = 0; i < currentRotation; ++i)
                {
                    selectedGO = selectedGO.GetComponent<MPTShape>().nextRotationPrefab;
                }
                
                GameObject go = Instantiate(selectedGO);
				go.name = go.name.Replace("(Clone)", "");
                go.transform.SetParent(spawnPoints[spawnId].transform);
                go.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                go.GetComponent<MPTShape>().Init();
                go.GetComponent<MPTDraggable>().additionalColliders.Add(spawnPoints[spawnId].GetComponent<Collider2D>());
                spawnedShapes[spawnId] = go;
                //nextShape.GetComponent<MPTDraggable>().enabled = false;
                return;
            }
        }
    }

    public void ShapeDropped(GameObject shape)
    {
        for (int spawnId = 0; spawnId < spawnedShapes.Length; ++spawnId)
        {
            if (spawnedShapes[spawnId] == shape)
            {
                spawnedShapes[spawnId] = null;
            }
        }
    }

    private void CreateShape(GameObject selectedGO)
    {
        CreateShapeInQueue(selectedGO);
        return;
        /*
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
        */
    }

    public void Clear()
    {
        currentSequence.Clear();
        for (int shapeId = 0; shapeId < spawnedShapes.Length; ++shapeId)
        {
            spawnedShapes[shapeId] = null;
        }
    }

    public void SetNextShape(MPTShape shapeToKeep)
    {
        shapeToKeep.draggable.enabled = true;
        for (int spawnId = 0; spawnId < spawnedShapes.Length; ++spawnId)
        {
            if (spawnedShapes[spawnId] == null)
            {
                shapeToKeep.gameObject.transform.SetParent(spawnPoints[spawnId].transform);
                MPTKeepShapeAnimation anim = shapeToKeep.gameObject.AddComponent<MPTKeepShapeAnimation>();
                anim.animationTime = 1.0f;
                anim.targetPosition = shapeToKeep.gameObject.transform.parent.position;
                anim.targetScale = shapeToKeep.initialScale / 2.0f;
                //shapeToKeep.gameObject.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                shapeToKeep.gameObject.GetComponent<MPTShape>().Init();
                shapeToKeep.draggable.additionalColliders.Clear();
                shapeToKeep.draggable.additionalColliders.Add(spawnPoints[spawnId].GetComponent<Collider2D>());
                shapeToKeep.draggable.initialPos = shapeToKeep.gameObject.transform.parent.position;
                spawnedShapes[spawnId] = shapeToKeep.gameObject;
                return;
            }
        }
    }
}
