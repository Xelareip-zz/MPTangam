using UnityEngine;
using System.Collections;

public class MPTSpawnText : MonoBehaviour
{
    public static readonly float SPAWN_TEXT_FREQUENCY = 0.015f;

    public float startScale;
    public float endScale;
    public float duration;
    public float stayDuration;
    
	void Start ()
    {
        StartCoroutine(ApplyScale());
	}
	
    IEnumerator ApplyScale()
    {
        float spentTime = 0.0f;
        while (spentTime < duration)
        {
            transform.localScale = Vector3.one * Mathf.Lerp(startScale, endScale, spentTime / duration);
            spentTime += SPAWN_TEXT_FREQUENCY;
            yield return new WaitForSeconds(SPAWN_TEXT_FREQUENCY);
        }
        spentTime = 0.0f;
        while (spentTime < stayDuration)
        {
            spentTime += SPAWN_TEXT_FREQUENCY;
            yield return new WaitForSeconds(SPAWN_TEXT_FREQUENCY);
        }
        Destroy(gameObject);
    }
}
