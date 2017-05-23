using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MPTInteractiveTutoManager : MonoBehaviour
{
	private static MPTInteractiveTutoManager instance;
	public static MPTInteractiveTutoManager Instance
	{
		get
		{
			return instance;
		}
	}

	public MPTInteractiveTutoBase currentTuto;

	public List<MPTInteractiveTutoBase> tutoList;
	public Dictionary<string, MPTInteractiveTutoBase> tutoDict;

	void Awake()
	{
		instance = this;
		tutoDict = new Dictionary<string, MPTInteractiveTutoBase>();
		foreach (MPTInteractiveTutoBase tuto in tutoList)
		{
			tutoDict.Add(tuto.name, tuto);
		}
	}

	public bool StartTuto(string tutoName)
	{
		if (tutoDict.ContainsKey(tutoName))
		{
			tutoDict[tutoName].Begin();
			return true;
        }
		return false;
	}

	public bool BlockadeDragPiece(MPTShape shape)
	{
		if (currentTuto)
			return currentTuto.BlockadeDragPiece(shape);
		return true;
	}

	public bool BlockadeDropAtSpot(MPTShape shape, Vector2 spot)
	{
		if (currentTuto)
			return currentTuto.BlockadeDropAtSpot(shape, spot);
		return true;
	}

	public virtual int GetRotation()
	{
		if (currentTuto)
			return currentTuto.GetRotation();
		return -1;
	}

	public virtual string GetSpawnableShape()
	{
		if (currentTuto)
			return currentTuto.GetSpawnableShape();
		return "";
	}

	public virtual string GetKeepableShape()
	{
		if (currentTuto)
			return currentTuto.GetKeepableShape();
		return "";
	}

	public virtual bool GetCanRestart()
	{
		if (currentTuto)
			return currentTuto.GetCanRestart();
		return true;
	}
}
