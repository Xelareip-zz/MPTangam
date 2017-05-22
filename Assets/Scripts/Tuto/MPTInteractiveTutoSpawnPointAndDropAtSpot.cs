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
}
