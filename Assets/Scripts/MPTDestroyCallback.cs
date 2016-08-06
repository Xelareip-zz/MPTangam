using UnityEngine;
using System.Collections;

public class MPTDestroyCallback : MonoBehaviour
{
    public void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}
