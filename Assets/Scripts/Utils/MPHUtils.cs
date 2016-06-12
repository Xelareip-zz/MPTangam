using UnityEngine;
using System.Collections;

public class MPHUtils
{
    public static Vector2 ScreenToWorld(Vector2 screenPoint)
    {
        float y = Camera.main.transform.position.y + Camera.main.orthographicSize * (-1.0f + 2.0f / Screen.height * Input.mousePosition.y);
        float x = Camera.main.transform.position.x + Camera.main.orthographicSize * (-1.0f + 2.0f / Screen.width * Input.mousePosition.x) * Screen.width / Screen.height;
        
        return new Vector2(x, y);
    }
}
