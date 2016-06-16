using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MPTGameManager : MonoBehaviour
{
    private static MPTGameManager instance;
    public static MPTGameManager Instance
    {
        get
        {
            return instance;
        }
    }

    public Text scoreText;
    public GameObject debugUI;
    public Transform debugPosition;
    public Transform gamePosition;
    public GameObject debugWeightModel;
    public int score;

    void Start()
    {
        instance = this;
        score = 0;
    }

    public void ShapeConsumed(MPTShape shape)
    {
        ++score;
        scoreText.text = "Score : " + score;
    }

    public void Restart()
    {
        MPTGrid.Instance.ResetGrid();
        foreach(MPTShape shape in MPTShapeManager.Instance.listOfShapes)
        {
            Destroy(shape.gameObject);
        }
        MPTShapeManager.Instance.listOfShapes.Clear();
        MPTSpawner.Instance.SpawnNew();
    }

    public void ToggleDebugMode()
    {
        if (debugUI.activeInHierarchy)
        {
            Camera.main.transform.position = gamePosition.position;
            debugUI.SetActive(false);
            for (int i = 0; i < debugUI.transform.childCount; ++i)
            {
                MPTWeightButton weightButton = debugUI.transform.GetChild(i).GetComponent<MPTWeightButton>();
                MPTShape shape = MPTSpawner.Instance.spawnables[i].GetComponent<MPTShape>();
                shape.weight = weightButton.GetWeight();
                Destroy(debugUI.transform.GetChild(i).gameObject);
            }
        }
        else
        {
            Camera.main.transform.position = debugPosition.position;
            debugUI.SetActive(true);
            for (int i = 0; i < MPTSpawner.Instance.spawnables.Count; ++i)
            {
                GameObject go = MPTSpawner.Instance.spawnables[i];
                MPTShape shape = go.GetComponent<MPTShape>();
                GameObject debugWeight = GameObject.Instantiate(debugWeightModel);
                MPTWeightButton weightButton = debugWeight.GetComponent<MPTWeightButton>();
                weightButton.SetWeight(shape.weight);
                weightButton.SetName(go.name);
                debugWeight.transform.SetParent(debugUI.transform, false);
                RectTransform tr = (RectTransform)debugWeight.transform;
                float height = tr.offsetMax.y - tr.offsetMin.y;
                (debugWeight.transform as RectTransform).offsetMin = new Vector2(0.0f, -height * i);
                (debugWeight.transform as RectTransform).offsetMax = new Vector2(0.0f, -height * (i + 1));
            }
        }
    }
}
