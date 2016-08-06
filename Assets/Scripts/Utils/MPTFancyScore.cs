using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MPTFancyScore : MonoBehaviour
{
    public float speed;
    public Vector3 direction;
    public float lifeTime;
    public float fadeOutTime;

    private float startTime;
    private TextMesh textMesh;

    void Start()
    {
        direction = Quaternion.AngleAxis(Random.Range(0.0f, 360.0f), Vector3.forward) * Vector3.up;
        startTime = Time.time;
        textMesh = GetComponent<TextMesh>();
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;

        if (startTime + lifeTime - fadeOutTime < Time.time)
        {
            if (textMesh)
            {
                textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, (startTime + lifeTime - Time.time) / fadeOutTime);
            }
        }
        if (startTime + lifeTime < Time.time)
        {
            Destroy(gameObject);
        }
    }
}
