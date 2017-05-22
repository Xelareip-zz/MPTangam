using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MPTInteractiveTutoDragPreviousAndDropAtSpot : MPTInteractiveTutoSpawnAndDropAtSpot
{
	public MPTInteractiveTutoSpawnAndDropAtSpot previousTuto;
	public override void Begin()
	{
		base.Begin();
		targetShape = previousTuto.targetShape;
    }
}
