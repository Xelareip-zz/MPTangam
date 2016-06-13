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
    public int score;

    void Start()
    {
        instance = this;
        score = 0;
    }

    public void ShapeConsumed(MPTShape shape)
    {
        ++score;

        scoreText.text = "" + score;
    }
}
