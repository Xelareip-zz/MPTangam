using UnityEngine;
using System.Collections;

public class MPTSquareDoneAnimation : MonoBehaviour
{
    public float animationTime;

    public Vector3 targetPosition;
    private Vector3 initialPosition;
    public Vector3 targetScale;
    private Vector3 initialScale;

    private float startTime;

	void Start ()
    {
        initialPosition = transform.position;
        initialScale = transform.localScale;
        startTime = Time.time;
    }

    void Update()
    {
        if ((Time.time - startTime) > animationTime)
        {
            Destroy(gameObject);
            return;
        }
        transform.position = initialPosition + (targetPosition - initialPosition) * (Time.time - startTime) / animationTime;
        transform.localScale = initialScale + (targetScale - initialScale) * (Time.time - startTime) / animationTime;
    }
}
