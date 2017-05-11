#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;

public class MPTEditorTrigger
{
    [MenuItem("Edges/Trigger")]
    public static void Execute()
    {
        foreach (GameObject cur in MPTSpawner.Instance.spawnedShapes)
        {
            cur.GetComponent<MPTShape>().FindDropSpaces();
        }
    }
}
#endif