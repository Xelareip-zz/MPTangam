using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

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
    public Text bestScoreText;
    public Text scoreFinalText;
    public Text bestScoreFinalText;
    public Text trashPriceText;
    public GameObject looseScreen;
    public GameObject debugUI;
    public GameObject startUI;
    public Transform debugPosition;
    public Transform gamePosition;
    public GameObject debugWeightModel;
    public int score;
    public int currentTopScore;
    public int trashPrice;

    public List<Color> multiplierColors;

    void Start()
    {
        instance = this;
        score = 0;
        trashPrice = 1;
        UpdateScoreText();
    }

    public void StartGame()
    {
        Destroy(startUI);
        MPTSpawner.Instance.SpawnNew();
        MPTSpawner.Instance.SpawnNew();
        MPTSpawner.Instance.SpawnNew();
    }
    
    void Loose()
    {
        looseScreen.SetActive(true);
    }

    public void ShapeConsumed(MPTShape shape, int multiplier)
    {
        score += multiplier;
        currentTopScore = Mathf.Max(score, currentTopScore);
        UpdateScoreText();
    }

    public void UpdateScoreText()
    {
        MPTPlayer.Instance.UpdateBestScore(score);
        UpdateBestScoreText();
        scoreText.text = "Score : " + score;
        scoreFinalText.text = "" + currentTopScore;
}

    public void UpdateBestScoreText()
    {
        bestScoreText.text = "Best : " + MPTPlayer.Instance.GetBestScore();
        bestScoreFinalText.text = "" + MPTPlayer.Instance.GetBestScore();
}

    public void UpdateTrashText()
    {
        trashPriceText.text = "Trash : " + trashPrice;
    }

    public void IncreaseTrashPrice()
    {
        trashPrice *= 1; //Test crado pour laisser toujours le cost a 1
        UpdateTrashText();
    }

    public void DecreaseTrashPrice()
    {
        if (trashPrice > 1)
        {
            --trashPrice;
            UpdateTrashText();
        }
    }

    public void TrashShape()
    {
        score -= trashPrice;
        if (score < 0)
        {
            Loose();
        }
        else
        {
            IncreaseTrashPrice();
            UpdateScoreText();
            MPTShapeManager.Instance.UnregisterShape(MPTSpawner.Instance.currentShape);
            Destroy(MPTSpawner.Instance.currentShape.gameObject);
            MPTSpawner.Instance.SpawnNew();
        }
    }

    public void Restart()
    {
        score = 0;
        trashPrice = 1;
        UpdateScoreText();
        MPTSpawner.Instance.ResetSquaresDone();
        MPTGrid.Instance.ResetGrid();
        foreach(MPTShape shape in MPTShapeManager.Instance.listOfShapes)
        {
            Destroy(shape.gameObject);
        }
        MPTShapeManager.Instance.listOfShapes.Clear();
        MPTSpawner.Instance.SpawnNew();
        MPTSpawner.Instance.SpawnNew();
        looseScreen.SetActive(false);
    }

    public void ToggleDebugMode()
    {
        if (debugUI.activeInHierarchy)
        {
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
