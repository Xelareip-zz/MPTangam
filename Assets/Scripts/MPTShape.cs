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
    public MPTDraggable draggable;
    SpriteRenderer spriteRenderer;
    public float weight;
    public int multiplier;

    private float width;
    private float height;
    private int[,] miniTriangles;
    public bool isOnGrid;
    public bool isFullyInGrid;
    public bool canDrop;
    public bool hasBeenDropped;
    public int miniTrianglesCount;

    private Vector3 initialScale;

    void Start()
    {
        draggable = GetComponent<MPTDraggable>();
        draggable.beenDragged += AfterDrag;
        draggable.beenDropped += AfterDrop;
        /*
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
        }*/

        MPTShapeManager.Instance.RegisterShape(this);
        //polygonCollider.points;
    }

    void Update()
    {
        UpdateColor();
        if (MPTGrid.Instance != null && MPTGrid.Instance.coll.IsTouching(polygonCollider))
        {
            isOnGrid = true;
        }
        else
        {
            isOnGrid = false;
        }
        CheckCanDrop();
    }

    public void Init()
    {
        initialScale = transform.localScale;
        transform.localScale = initialScale / 2.0f;
        hasBeenDropped = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void CheckCanDrop()
    {
        canDrop = true;
        foreach (MPTShape current in MPTShapeManager.Instance.listOfShapes)
        {
            if (current == this)
            {
                continue;
            }
            else
            {
                if (current.polygonCollider.IsTouching(polygonCollider))
                {
                    canDrop = false;
                    break;
                }
            }
        }
    }

    public void AfterDrag()
    {
        transform.localScale = initialScale;
        UpdateColor();
        if (isOnGrid)
        {
            transform.position = new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y + 1), transform.position.z);
        }

        isFullyInGrid = true;
        foreach (Vector2 point in polygonCollider.points)
        {
            Vector2 localPos = transform.rotation * new Vector2(
                (polygonCollider.offset.x + point.x) * transform.localScale.x,
                (polygonCollider.offset.y + point.y) * transform.localScale.y);
            Vector2 worldPointPos = new Vector2(
                transform.position.x,
                transform.position.y) + localPos;
            if (MPTGrid.Instance.coll.OverlapPoint(worldPointPos) == false)
            {
                isFullyInGrid = false;
                break;
            }
        }
    }

    public bool CheckGridHasSpace()
    {
        float xStart = MPTGrid.Instance.transform.position.x - MPTGrid.Instance.width / 2.0f;
        float yStart = MPTGrid.Instance.transform.position.y + MPTGrid.Instance.height / 2.0f;

        for (int gridX = 0; gridX <= MPTGrid.Instance.width; ++gridX)
        {
            for (int gridY = 0; gridY <= MPTGrid.Instance.height; ++gridY)
            {
                Vector2 centerPosition = new Vector2(xStart + gridX, yStart - gridY);
                Vector2[] points = new Vector2[polygonCollider.points.Length];

                for (int pointId = 0; pointId < points.Length; ++pointId)
                {
                    Vector2 localPos = transform.rotation * new Vector2(
                        (polygonCollider.offset.x + polygonCollider.points[pointId].x) * initialScale.x,
                        (polygonCollider.offset.y + polygonCollider.points[pointId].y) * initialScale.y);
                    Vector2 loopWorldPointPos = localPos + centerPosition;
                    points[pointId] = loopWorldPointPos;
                }
                bool notInGrid = false;
                foreach (Vector2 point in points)
                {
                    if (MPTGrid.Instance.coll.OverlapPoint(point) == false)
                    {
                        notInGrid = true;
                        break;
                    }
                }
                if (notInGrid)
                {
                    continue;
                }

                bool shapeWasHit = false;

                foreach (MPTShape current in MPTShapeManager.Instance.listOfShapes)
                {
                    if (current == this || current.hasBeenDropped == false)
                    {
                        continue;
                    }
                    else
                    {
                        Vector2[] currentPoints = new Vector2[current.polygonCollider.points.Length];
                        for (int pointId = 0; pointId < currentPoints.Length; ++pointId)
                        {
                            Vector2 localPosCurrent = current.transform.rotation * new Vector2(
                                (current.polygonCollider.offset.x + current.polygonCollider.points[pointId].x) * current.transform.localScale.x,
                                (current.polygonCollider.offset.y + current.polygonCollider.points[pointId].y) * current.transform.localScale.y);
                            Vector2 worldPointPosCurrent = new Vector2(
                                current.transform.position.x,
                                current.transform.position.y) + localPosCurrent;
                            currentPoints[pointId] = worldPointPosCurrent;
                        }

                        for (int i = 0; i < points.Length && shapeWasHit == false; ++i)
                        {
                            for (int j = 0; j < currentPoints.Length; ++j)
                            {
                                if (MPTUtils.SegmentIntersect(points[i], points[(i + 1) % points.Length], currentPoints[j], currentPoints[(j + 1) % currentPoints.Length]))
                                {
                                    shapeWasHit = true;
                                    break;
                                }
                            }
                        }
                        if (shapeWasHit)
                        {
                            break;
                        }
                    }
                }
                if (shapeWasHit == false)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void AfterDrop()
    {
        CheckCanDrop();
        if (isOnGrid && isFullyInGrid && canDrop)
        {
            Destroy(draggable);
            hasBeenDropped = true;
            MPTSpawner.Instance.ShapeDropped(this.gameObject);
            MPTSpawner.Instance.SpawnNew();
            MPTGrid.Instance.ShapeDropped(this);
        }
        else
        {
            transform.localPosition = Vector3.zero;
            transform.localScale = initialScale / 2.0f;
        }
    }

    public void Consume(int currentMultiplier, bool keepShape)
    {
        MPTGameManager.Instance.ShapeConsumed(this, currentMultiplier);
        if (keepShape == false)
        {
            MPTShapeManager.Instance.UnregisterShape(this);
            //Destroy(gameObject);
        }/*
        else
        {
            multiplier += currentMultiplier;
        }*/
    }

    public void SetMultiplier(int newMultiplier)
    {
        if (newMultiplier < MPTGameManager.Instance.multiplierColors.Count)
        {
            multiplier = newMultiplier;
            spriteRenderer.color = MPTGameManager.Instance.multiplierColors[multiplier];
        }
    }

    public void UpdateColor()
    {
        if (hasBeenDropped || canDrop)
        {
            spriteRenderer.color = MPTGameManager.Instance.multiplierColors[multiplier];
        }
        else
        {
            spriteRenderer.color = MPTGameManager.Instance.cantDropColor;
        }
    }
}
