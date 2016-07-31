using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MPTFancyScore : MonoBehaviour
{
    public float speed;
    public Vector3 direction;
    public float lifeTime;

    private float startTime;

    void Start()
    {
        direction = Quaternion.AngleAxis(Random.Range(0.0f, 360.0f), Vector3.forward) * Vector3.up;
        startTime = Time.time;
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
        if (startTime + lifeTime < Time.time)
        {
            Destroy(gameObject);
        }
    }
}
