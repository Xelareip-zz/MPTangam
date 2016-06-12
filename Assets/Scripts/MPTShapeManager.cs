using UnityEngine;
using System.Collections.Generic;

public class MPTShapeManager : MonoBehaviour
{
    private static MPTShapeManager instance;
    public static MPTShapeManager Instance
    {
        get
        {
            return instance;
        }
    }

    public List<MPTShape> listOfShapes;

    void Start()
    {
        instance = this;
        listOfShapes = new List<MPTShape>();
    }

    public void RegisterShape(MPTShape newShape)
    {
        listOfShapes.Add(newShape);
    }

    public void UnregisterShape(MPTShape newShape)
    {
        listOfShapes.Remove(newShape);
    }
}
