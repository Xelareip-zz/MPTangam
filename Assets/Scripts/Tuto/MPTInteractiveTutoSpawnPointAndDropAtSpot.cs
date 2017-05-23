using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MPTInteractiveTutoSpawnPointAndDropAtSpot : MPTInteractiveTutoSpawnAndDropAtSpot
{
	public int dropPointId;
	public override void Begin()
	{
		base.Begin();
		targetShape = MPTSpawner.Instance.spawnedShapes[dropPointId];
    }

	void Update()
	{
		if (targetShape == null)
		{
			targetShape = MPTSpawner.Instance.spawnedShapes[dropPointId];
		}
	}

	public override void ShapeRegistered(MPTShape shape)
	{
		if (targetShape == null && shape.name == spawnableShape)
		{
			shape.shapeTryDrop += ShapeTriedDrop;
		}
	}

	public override void ShapeUnregistered(MPTShape shape)
	{
		shape.shapeTryDrop -= ShapeTriedDrop;
	}
}
