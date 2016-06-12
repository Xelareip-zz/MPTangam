using UnityEngine;
using System.Collections.Generic;

public class MINI_TRIANGLE_POS
{
    public static int N = 1;
    public static int E = 2;
    public static int S = 4;
    public static int W = 8;
};

public class TRIANGLE_POS
{
    public static int NW = 9;
    public static int NE = 3;
    public static int SE = 6;
    public static int SW = 12;
};

public class MPTShape : MonoBehaviour
{
    public Sprite mainSprite;
    public PolygonCollider2D polygonCollider;
    private float width;
    private float height;
    private int[,] miniTriangles;

    void Start()
    {
        mainSprite = GetComponent<SpriteRenderer>().sprite;
        width = mainSprite.bounds.extents.x;
        height = mainSprite.bounds.extents.y;
        miniTriangles = new int[(int)height, (int)width];

        Texture2D spriteTexture = mainSprite.texture;

        float spriteWidthPx = spriteTexture.width;
        float spriteHeightPx = spriteTexture.height;

        for (int currentX = 0; currentX < width; ++currentX)
        {
            for (int currentY = 0; currentY < height; ++currentY)
            {
                miniTriangles[currentX, currentY] = 0;
                int pxX = Mathf.RoundToInt(spriteWidthPx * (currentX + 0.5f) / width);
                int pxY = Mathf.RoundToInt(spriteHeightPx * (currentY + 0.25f) / height);
                Color pxColor = spriteTexture.GetPixel(pxX, pxY);
                if (pxColor.a != 0.0f)
                {
                    miniTriangles[currentX, currentY] |= MINI_TRIANGLE_POS.N;
                }

                pxX = Mathf.RoundToInt(spriteWidthPx * (currentX + 0.5f) / width);
                pxY = Mathf.RoundToInt(spriteHeightPx * (currentY + 0.75f) / height);
                pxColor = spriteTexture.GetPixel(pxX, pxY);
                if (pxColor.a != 0.0f)
                {
                    miniTriangles[currentX, currentY] |= MINI_TRIANGLE_POS.S;
                }

                pxX = Mathf.RoundToInt(spriteWidthPx * (currentX + 0.75f) / width);
                pxY = Mathf.RoundToInt(spriteHeightPx * (currentY + 0.5f) / height);
                pxColor = spriteTexture.GetPixel(pxX, pxY);
                if (pxColor.a != 0.0f)
                {
                    miniTriangles[currentX, currentY] |= MINI_TRIANGLE_POS.E;
                }

                pxX = Mathf.RoundToInt(spriteWidthPx * (currentX + 0.25f) / width);
                pxY = Mathf.RoundToInt(spriteHeightPx * (currentY + 0.5f) / height);
                pxColor = spriteTexture.GetPixel(pxX, pxY);
                if (pxColor.a != 0.0f)
                {
                    miniTriangles[currentX, currentY] |= MINI_TRIANGLE_POS.W;
                }
            }
        }



        List<Vector2> colliderPoints = new List<Vector2>();
        Vector2 lastPoint = new Vector2(-1, -1);
        for (int i = 0; i < height && lastPoint.x < 0; ++i)
        {
            for (int j = 0; j < width; ++j)
            {
                int triangle = miniTriangles[i, j];
                if (triangle != 0)
                {
                    lastPoint = new Vector2(i, j);
                    if (triangle == TRIANGLE_POS.SE)
                    {
                        colliderPoints.Add(new Vector2(j + 1, i));
                    }
                    else
                    {
                        colliderPoints.Add(new Vector2(j, i));
                    }
                    break;
                }
            }
        }
        //polygonCollider.points;
    }
}
