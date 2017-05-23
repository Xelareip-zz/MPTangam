using UnityEngine;
using System;
using System.Collections.Generic;

public class MPTInteractiveTutoTouch : MPTInteractiveTutoBase
{
	void Update()
	{
		foreach (Touch touch in Input.touches)
		{
			if (touch.phase == TouchPhase.Began)
			{
				End();
			}
		}

		if (Input.GetMouseButtonDown(0))
		{
			End();
		}
	}

	public override bool BlockadeDragPiece(MPTShape shape)
	{
		return false;
	}

	public override bool BlockadeDropAtSpot(MPTShape shape, Vector2 spot)
	{
		return false;
	}
}
