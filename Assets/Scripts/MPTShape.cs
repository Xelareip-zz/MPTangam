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
    public GameObject nextRotationPrefab;

    public Sprite mainSprite;
    public PolygonCollider2D polygonCollider;
    public MPTDraggable draggable;
    SpriteRenderer spriteRenderer;
    public float weight;
    public int multiplier;
    public int points;

    private float width;
    private float height;
    private int[,] miniTriangles;
    public bool canDrop;
    public bool hasBeenDropped;
    public int miniTrianglesCount;

    public Vector3 initialScale;

	public List<Vector2> droppableSpaces = new List<Vector2>();

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
        /*if (MPTGrid.Instance != null && MPTGrid.Instance.coll.IsTouching(polygonCollider))
        {
            isOnGrid = true;
        }
        else
        {
            isOnGrid = false;
        }*/
		if (draggable.currentlyDragged)
		{
			CheckCanDrop(MPTSpawner.Instance.ghostCollider);
		}
    }

    public void Init()
    {
        points = 1;
        initialScale = transform.localScale;
        transform.localScale = initialScale / 2.0f;
        hasBeenDropped = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void CheckCanDrop(PolygonCollider2D polygonCollider)
	{
		Vector2 closestPoint = new Vector2(float.MaxValue, float.MaxValue);
		float minDistance = float.MaxValue;
		foreach (Vector2 droppableSpace in droppableSpaces)
		{
			float currentDistance = Vector2.Distance(droppableSpace, transform.position);
			if (currentDistance < Vector2.Distance(closestPoint, transform.position))
			{
				minDistance = currentDistance;
				closestPoint = droppableSpace;
			}
		}
		canDrop = minDistance < 1.5f;
		/*
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
		Debug.Log("Can drop " + canDrop);*/
    }

    public void AfterDrag()
	{
		MPTSpawner.Instance.ghostCollider.points = polygonCollider.points;
		MPTSpawner.Instance.ghostCollider.offset = polygonCollider.offset;
		MPTSpawner.Instance.ghostRenderer.transform.rotation = transform.rotation;
		MPTSpawner.Instance.ghostRenderer.transform.localScale = transform.localScale;
		MPTSpawner.Instance.ghostRenderer.sprite = spriteRenderer.sprite;
		Color ghostColor = GetColor();
		ghostColor.a /= 2;
		MPTSpawner.Instance.ghostRenderer.color = ghostColor;
        transform.position = new Vector3(transform.position.x, transform.position.y, draggable.initialPos.z - 1);
        transform.localScale = initialScale;
        UpdateColor();
		/*
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
        }*/
		
		Vector2 closestPoint = new Vector2(float.MaxValue, float.MaxValue);
		float minDistance = float.MaxValue;
		foreach (Vector2 droppableSpace in droppableSpaces)
		{
			float currentDistance = Vector2.Distance(droppableSpace, transform.position);
            if (currentDistance < Vector2.Distance(closestPoint, transform.position))
			{
				minDistance = currentDistance;
				closestPoint = droppableSpace;
			}
		}
		if (minDistance < 1.5f)
		{
			MPTSpawner.Instance.ghostRenderer.transform.position = new Vector3(closestPoint.x, closestPoint.y, transform.position.z);
		}
		else
		{
			MPTSpawner.Instance.ghostRenderer.transform.position = transform.position;
		}
    }

	public bool FindDropSpaces()
	{
		droppableSpaces.Clear();
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
						// current contains this
						for (int i = 0; i < points.Length; ++i)
						{
							if (current.polygonCollider.OverlapPoint(points[i]))
							{
								shapeWasHit = true;
							}
						}
						if (shapeWasHit)
						{
							break;
						}

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

						if (MPTUtils.PolygonsIntersect(points, currentPoints))
						{
							shapeWasHit = true;
							break;
						}
					}
				}
				if (shapeWasHit == false)
				{
					droppableSpaces.Add(centerPosition);

				}
			}
		}

		return droppableSpaces.Count != 0;
	}

    public void AfterDrop()
	{
		transform.position = MPTSpawner.Instance.ghostRenderer.transform.position + new Vector3(0, 0, 1);
		CheckCanDrop(MPTSpawner.Instance.ghostCollider);
		if (/*isOnGrid && isFullyInGrid && */canDrop)
        {
            //Destroy(draggable);
            //draggable.enabled = false;
            draggable.Reinit();
            draggable.additionalColliders.Clear();
            hasBeenDropped = true;
            MPTSpawner.Instance.ShapeDropped(this.gameObject);
            //MPTSpawner.Instance.SpawnNew();
            MPTGrid.Instance.ShapeDropped(this);
        }
        else
        {
            transform.position = draggable.initialPos;
            if (draggable.initialPos == draggable.transform.parent.position)
            {
                transform.localScale = initialScale / 2.0f;
            }
            else
            {
                transform.localScale = initialScale;
            }
        }
		MPTSpawner.Instance.ghostRenderer.transform.position = new Vector2(-1000, -1000);
    }

    public void Consume(int currentMultiplier, bool keepShape)
    {
        MPTGameManager.Instance.ShapeConsumed(this, currentMultiplier);
        GameObject scorePrefab = Instantiate(MPTGameManager.Instance.fancyScorePrefab);
        scorePrefab.GetComponent<TextMesh>().text = "+" + points;
        scorePrefab.GetComponent<TextMesh>().color = MPTGameManager.Instance.multiplierColors[points - 1];
        scorePrefab.transform.position = transform.position;
        
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

	public Color GetColor()
	{
		if (points <= MPTGameManager.Instance.multiplierColors.Count)
		{
			return MPTGameManager.Instance.multiplierColors[points - 1];
		}
		else
		{
			return MPTGameManager.Instance.multiplierColors[MPTGameManager.Instance.multiplierColors.Count - 1];
		}
	}

    public void UpdateColor()
    {
        if ((draggable.currentlyDragged && canDrop) || draggable.currentlyDragged == false)
        {
			spriteRenderer.color = GetColor();
        }
        else
        {
            spriteRenderer.color = MPTGameManager.Instance.cantDropColor;
        }
    }
}
