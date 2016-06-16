using UnityEngine;
using System.Collections.Generic;

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
    public MPTShape[,] shapesTab;

    void Start()
    {
        instance = this;
        coll = GetComponent<Collider2D>();
        shapesTab = new MPTShape[height * 2, width];
    }

    public void ResetGrid()
    {
        for (int i = 0; i < width; ++i)
        {
            for (int j = 0; j < height * 2; ++j)
            {
                shapesTab[j, i] = null;
            }
        }
    }

    public void ShapeDropped(MPTShape shape)
    {
        float xStart = transform.position.x - width / 2.0f + 0.5f;
        float yStart = transform.position.y + height / 2.0f - 0.25f;

        int miniTrianglesCount = 0;

        for (int shapeX = 0; shapeX < width; ++shapeX)
        {
            for (int shapeY = 0; shapeY < height * 2; ++shapeY)
            {
                Vector2 point = new Vector2(xStart + shapeX, yStart - shapeY * 0.5f);
                if (shape.polygonCollider.OverlapPoint(point))
                {
                    shapesTab[shapeY, shapeX] = shape;
                    ++miniTrianglesCount;
                }
            }
        }

        shape.miniTrianglesCount = miniTrianglesCount;

        List<List<MPTShape>> squares = new List<List<MPTShape>>();

        for (int xNW = 0; xNW < width; ++xNW)
        {
            for (int yNW = 0; yNW < height; ++yNW)
            {
                for (int squareSize = 1; squareSize < width - xNW + 1 && squareSize < height - yNW + 1; ++squareSize)
                {
                    bool brokenSquare = false;
                    Dictionary<MPTShape, int> shapeToCount = new Dictionary<MPTShape, int>();
                    for (int squareX = 0; squareX < squareSize; ++squareX)
                    {
                        for (int squareY = 0; squareY < squareSize; ++squareY)
                        {
                            Vector2 miniSquarePos = new Vector2(xNW + squareX, yNW + squareY);
                            if (shapesTab[(int)miniSquarePos.y * 2, (int)miniSquarePos.x] == null || shapesTab[(int)miniSquarePos.y * 2 + 1, (int)miniSquarePos.x] == null)
                            {
                                brokenSquare = true;
                                break;
                            }
                            else
                            {
                                MPTShape shape1 = shapesTab[(int)miniSquarePos.y * 2, (int)miniSquarePos.x];
                                MPTShape shape2 = shapesTab[(int)miniSquarePos.y * 2 + 1, (int)miniSquarePos.x];

                                if (shapeToCount.ContainsKey(shape1))
                                {
                                    shapeToCount[shape1] = shapeToCount[shape1] + 1;
                                }
                                else
                                {
                                    shapeToCount.Add(shape1, 1);
                                }

                                if (shapeToCount.ContainsKey(shape2))
                                {
                                    shapeToCount[shape2] = shapeToCount[shape2] + 1;
                                }
                                else
                                {
                                    shapeToCount.Add(shape2, 1);
                                }
                            }
                        }
                        if (brokenSquare)
                        {
                            break;
                        }
                    }
                    if (brokenSquare)
                    {
                        break;
                    }
                    squares.Add(new List<MPTShape>());
                    foreach (KeyValuePair<MPTShape, int> kvp in shapeToCount)
                    {
                        if (kvp.Key.miniTrianglesCount != kvp.Value)
                        {
                            brokenSquare = true;
                            break;
                        }
                        else
                        {
                            squares[squares.Count - 1].Add(kvp.Key);
                        }
                    }
                    if (brokenSquare)
                    {
                        squares.RemoveAt(squares.Count - 1);
                    }
                }
            }
        }

        List<MPTShape> finalSquare = new List<MPTShape>();

        foreach (List<MPTShape> square in squares)
        {
            if (square.Count > finalSquare.Count)
            {
                finalSquare = square;
            }
        }

        foreach (MPTShape currentShape in finalSquare)
        {
            currentShape.Consume();
        }
    }
}
