using UnityEngine;
using System;
using System.Collections.Generic;

public class MPTInteractiveTutoBase : MonoBehaviour
{
	public MPTInteractiveTutoBase nextTuto;
	public string finishTutorial;

	public string spawnableShape;
	public string keepableShape;
	public int rotation;

	public List<GameObject> linkedUI = new List<GameObject>();

	public virtual void Begin()
	{
		//Debug.Log("Tuto started: " + name + " stack: " + Environment.StackTrace);
		gameObject.SetActive(true);
		MPTInteractiveTutoManager.Instance.currentTuto = this;

		foreach (GameObject uiObject in linkedUI)
		{
			uiObject.SetActive(true);
		}
	}

	public virtual void End()
	{
		gameObject.SetActive(false);
		foreach (GameObject uiObject in linkedUI)
		{
			uiObject.SetActive(false);
		}
		if (finishTutorial != "")
		{
			MPTPlayer.Instance.SetTutoDone(true);
		}
		if (nextTuto != null)
		{
			nextTuto.Begin();
		}
		else
		{
			MPTInteractiveTutoManager.Instance.currentTuto = null;
		}
		//Debug.Log("Tuto ended: " + name + " stack: " + Environment.StackTrace);
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
		return rotation;
	}

	public virtual string GetSpawnableShape()
	{
		return spawnableShape;
	}

	public virtual string GetKeepableShape()
	{
		return keepableShape;
	}

	public virtual bool GetCanRestart()
	{
		return false;
	}
}
