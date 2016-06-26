using UnityEngine;
using System;

[RequireComponent(typeof(Collider2D))]
public class MPTDraggable : MonoBehaviour
{
    public bool currentlyDragged;
    private Vector3 initialPos;
    private Collider2D coll;
    public float dragDelay = 0.5f;
    public float dragStartTime;


    public event Action beenDragged;
    public event Action beenDropped;

    void Start()
    {
        coll = GetComponent<Collider2D>();
        currentlyDragged = false;
        initialPos = transform.position;
    }

    void Update()
    {
        Vector2 initPosition = new Vector2(float.MinValue, float.MinValue);
        Vector2 position = initPosition;
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            position = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            position = Input.mousePosition;
        }
#endif
        if (Input.touchCount == 1)
        {
            position = Input.touches[0].position;
        }

        if (position == initPosition)
        {
            dragStartTime = Time.time + dragDelay;
            if (currentlyDragged)
            {
                if (beenDragged != null)
                {
                    beenDropped();
                }
            }
            currentlyDragged = false;
            return;
        }

        position = MPHUtils.ScreenToWorld(position);
        
        
        if (dragStartTime <= Time.time)
        {
            currentlyDragged = true;
        }

        if (currentlyDragged)
        {
            transform.position = new Vector3(position.x, position.y, 0.0f);
            if (beenDragged != null)
            {
                beenDragged();
            }
        }

    }
}
