using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MPTQuickSave : MonoBehaviour
{
	private static MPTQuickSave instance;
	public static MPTQuickSave Instance
	{
		get
		{
			return instance;
		}
	}

	private string FILE_PATH;
	public bool isDirty = false;
	private bool isLoading = false;
	public bool IsLoading
	{
		get
		{
			return isLoading;
		}
	}

	private Coroutine coroutine;

	void Awake()
	{
		FILE_PATH = Application.persistentDataPath + "/QuickSave.dat";
	}

	void Start()
	{
		instance = this;
		MPTShapeManager.Instance.shapeRegistered += ShapeRegistered;
    }

	private void ShapeRegistered(MPTShape shape)
	{
		shape.shapeTryDrop += ShapeTryDrop;
	}

	private void ShapeTryDrop(MPTShape obj)
	{
		if (coroutine != null)
		{
			StopCoroutine(coroutine);
		}
		coroutine = StartCoroutine(SetDirtySoon());
	}

	private IEnumerator SetDirtySoon()
	{
		yield return new WaitForSeconds(1.0f);
		isDirty = true;
	}

	void Update()
	{
		if (MPTKeepShapeAnimation.GetAnimationsCount() != 0)
			return;
		if (MPTInteractiveTutoManager.Instance.currentTuto != null)
			return;
		if (isDirty)
		{
			QuickSave();
			isDirty = false;
		}
	}

	public void Erase()
	{
		File.Delete(FILE_PATH);
	}

	public void QuickSave()
	{
		string saveString = "" + MPTGameManager.Instance.score + "\n";

		foreach (MPTShape shape in MPTShapeManager.Instance.listOfShapes)
		{
			saveString += shape.name + ":" + shape.transform.position.x + ":" + shape.transform.position.y + ":" + shape.transform.position.z + ":" + shape.points + "\n";
		}

		Directory.GetParent(Application.persistentDataPath).Create();
		StreamWriter writer = new StreamWriter(FILE_PATH);
		writer.Write(saveString);
		writer.Close();
	}

	public bool HasSave()
	{
		return File.Exists(FILE_PATH);
	}

	public void QuickLoad()
	{
		isLoading = true;
		string[] lines = File.ReadAllLines(FILE_PATH);
		MPTGameManager.Instance.score = int.Parse(lines[0]);
		MPTGameManager.Instance.UpdateScoreText();

		for (int lineId = 1; lineId < lines.Length; ++lineId)
		{
			string[] split = lines[lineId].Split(':');
			if (split.Length < 4)
			{
				continue;
			}

			string shapeName = split[0];
			Vector3 pos = new Vector3(float.Parse(split[1]), float.Parse(split[2]), float.Parse(split[3]));
			int points = int.Parse(split[4]);

			GameObject newShapeGo = MPTSpawner.Instance.CreateShapeInQueue(MPTSpawner.Instance.spawnablesDict[shapeName]);
			

			MPTShape newShape = newShapeGo.GetComponent<MPTShape>();
			newShape.points = points;
			newShape.UpdateColor();

			if (pos.y != MPTSpawner.Instance.spawnPoints[0].transform.position.y)
			{
				newShapeGo.transform.position = pos;
				MPTDraggable draggable = newShapeGo.GetComponent<MPTDraggable>();
				draggable.initialPos = new Vector3(-1000.0f, -1000.0f, 0.0f);
				newShape.canDrop = true;
				newShape.transform.localScale = newShape.initialScale;
				newShape.AfterDrop();
			}
		}
		MPTSpawner.Instance.UpdateDropWarning();
		isLoading = false;
    }
}
