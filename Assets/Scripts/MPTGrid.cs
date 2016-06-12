using UnityEngine;
using System.Collections;

public class MPTGrid : MonoBehaviour
{
    private static MPTGrid instance;
    public static MPTGrid Instance
    {
        get
        {
            return instance;
        }
    }

    public Collider2D coll;
    public int width;
    public int height;

    void Start()
    {
        instance = this;
        coll = GetComponent<Collider2D>();
    }
}
