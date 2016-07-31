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
    private SpriteRenderer spriteRenderer;

	void Start ()
    {
        initialPosition = transform.position;
        initialScale = transform.localScale;
        startTime = Time.time;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if ((Time.time - startTime) > animationTime)
        {
            Destroy(gameObject);
            return;
        }
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1.0f - (Time.time - startTime) / animationTime);
        //transform.position = initialPosition + (targetPosition - initialPosition) * (Time.time - startTime) / animationTime;
        //transform.localScale = initialScale + (targetScale - initialScale) * (Time.time - startTime) / animationTime;
    }
}
