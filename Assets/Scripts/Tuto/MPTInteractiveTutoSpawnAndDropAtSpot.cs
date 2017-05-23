using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MPTInteractiveTutoSpawnAndDropAtSpot : MPTInteractiveTutoBase
{
	public Vector2 droppableSpot;
	public GameObject targetShape;
	public List<MPTShape> shapesSubscribed = new List<MPTShape>();

	public override void Begin()
	{
		base.Begin();
		MPTShapeManager.Instance.shapeRegistered += ShapeRegistered;
		MPTShapeManager.Instance.shapeUnregistered += ShapeUnregistered;
		foreach (MPTShape shape in MPTShapeManager.Instance.listOfShapes)
		{
			shapesSubscribed.Add(shape);
			shape.shapeTryDrop += ShapeTriedDrop;
		}
    }

	public override void End()
	{
		foreach (MPTShape shape in shapesSubscribed)
		{
			if (shape != null)
			{
				shape.shapeTryDrop -= ShapeTriedDrop;
			}
		}
		MPTShapeManager.Instance.shapeRegistered -= ShapeRegistered;
		MPTShapeManager.Instance.shapeUnregistered -= ShapeUnregistered;

		base.End();
	}

	public virtual void ShapeRegistered(MPTShape shape)
	{
		if (targetShape == null && shape.name == spawnableShape)
		{
			targetShape = shape.gameObject;
			shapesSubscribed.Add(shape);
			shape.shapeTryDrop += ShapeTriedDrop;
		}
	}

	public virtual void ShapeUnregistered(MPTShape shape)
	{
		shape.shapeTryDrop -= ShapeTriedDrop;
	}

	public void ShapeTriedDrop(MPTShape draggable)
	{
		MPTShape shape = draggable.gameObject.GetComponent<MPTShape>();
		if (shape != null && shape.canDrop)
		{
			End();
		}
	}

	public override bool BlockadeDragPiece(MPTShape shape)
	{
		if (targetShape != null)
		{
			return shape.gameObject == targetShape;
		}
		return shape.name == spawnableShape && shape.hasBeenDropped == false;
	}

	public override bool BlockadeDropAtSpot(MPTShape shape, Vector2 spot)
	{
		if (targetShape != null)
		{
			return shape.gameObject == targetShape && spot == droppableSpot;
		}
		return shape.name == spawnableShape && spot == droppableSpot;
	}

	public override int GetRotation()
	{
		return rotation;
	}
}
