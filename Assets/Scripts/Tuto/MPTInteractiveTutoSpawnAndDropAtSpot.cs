using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MPTInteractiveTutoSpawnAndDropAtSpot : MPTInteractiveTutoBase
{
	public Vector2 droppableSpot;
	public string shapeType;
	public int rotation;

	public override void Begin()
	{
		base.Begin();
		MPTShapeManager.Instance.shapeRegistered += ShapeRegistered;
		MPTShapeManager.Instance.shapeUnregistered += ShapeUnregistered;
		foreach (MPTShape shape in MPTShapeManager.Instance.listOfShapes)
		{
			shape.shapeTryDrop += ShapeTriedDrop;
		}
    }

	public void ShapeRegistered(MPTShape shape)
	{
		shape.shapeTryDrop += ShapeTriedDrop;
	}

	public void ShapeUnregistered(MPTShape shape)
	{
		shape.shapeTryDrop -= ShapeTriedDrop;
	}

	public void ShapeTriedDrop(MPTShape draggable)
	{
		MPTShape shape = draggable.gameObject.GetComponent<MPTShape>();
		if (shape != null && shape.canDrop)
		{
			End();
			if (nextTuto != null)
			{
				nextTuto.Begin();
			}
			else
			{
				MPTInteractiveTutoManager.Instance.currentTuto = null;
			}
		}
	}

	public override bool BlockadeDragPiece(MPTShape shape)
	{
		return shape.name == shapeType && shape.hasBeenDropped == false;
	}

	public override bool BlockadeDropAtSpot(MPTShape shape, Vector2 spot)
	{
		return shape.name == shapeType && spot == droppableSpot;
	}

	public override int GetRotation()
	{
		return rotation;
	}

	public override string GetSpawnableShape()
	{
		return shapeType;
	}
}
