using UnityEngine;
using System;
using System.Collections.Generic;

public class MPTInteractiveTutoBase : MonoBehaviour
{
	public MPTInteractiveTutoBase nextTuto;
	public string finishTutorial;

	public List<GameObject> linkedUI = new List<GameObject>();

	public virtual void Begin()
	{
		MPTInteractiveTutoManager.Instance.currentTuto = this;

		foreach (GameObject uiObject in linkedUI)
		{
			uiObject.SetActive(true);
		}
	}

	public virtual void End()
	{
		foreach (GameObject uiObject in linkedUI)
		{
			uiObject.SetActive(false);
		}
		if (finishTutorial != "")
		{
			MPTPlayer.Instance.SetTutoDone(true);
		}
	}

	public virtual bool BlockadeDragPiece(MPTShape shape)
	{
		return true;
	}

	public virtual bool BlockadeDropAtSpot(MPTShape shape, Vector2 spot)
	{
		return true;
	}

	public virtual int GetRotation()
	{
		return -1;
	}

	public virtual string GetSpawnableShape()
	{
		return "";
	}
}
