using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MPTDelayedCounter : MonoBehaviour
{
    public Text textDisplay;

    public float currentScore;
    public int targetScore;

    public float currentScale;
    public float maxScale;
    
    public float speed;

    void Update()
    {
        if (Mathf.Abs(targetScore - currentScore) > 0.25f)
        {
            currentScore = Mathf.Lerp(currentScore, targetScore, speed * Time.deltaTime);
            currentScale = Mathf.Lerp(currentScale, maxScale, speed * Time.deltaTime);
        }
        else
        {
            currentScale = Mathf.Lerp(currentScale, 1.0f, speed * Time.deltaTime);
        }
        textDisplay.text = string.Format("{0}", Mathf.RoundToInt(currentScore));
        transform.localScale = Vector3.one * currentScale;
    }
}
