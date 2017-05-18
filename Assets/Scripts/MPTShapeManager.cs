using UnityEngine;
using System;
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

	public event Action<MPTShape> shapeRegistered;
	public event Action<MPTShape> shapeUnregistered;

	void Start()
    {
        instance = this;
        listOfShapes = new List<MPTShape>();
    }

    public void RegisterShape(MPTShape newShape)
    {
		if (shapeRegistered != null)
		{
			shapeRegistered(newShape);
		}
        listOfShapes.Add(newShape);
    }

    public void UnregisterShape(MPTShape newShape)
	{
		if (shapeUnregistered != null)
		{
			shapeUnregistered(newShape);
		}
		listOfShapes.Remove(newShape);
    }
}
