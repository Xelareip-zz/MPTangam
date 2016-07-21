using UnityEngine;
using System.Collections.Generic;

public class MPTTests : MonoBehaviour
{
    public List<Vector2> testList = new List<Vector2>();
    public List<bool> resultList = new List<bool>();

    void Start ()
    {

        testList.Add(new Vector2(0, 0));
        testList.Add(new Vector2(0, 1));
        testList.Add(new Vector2(1, 1));
        testList.Add(new Vector2(1, 0));
        testList.Add(new Vector2(-1, 2));
        testList.Add(new Vector2(1, -1));
        /*
        testList.Add(new Vector2());
        testList.Add(new Vector2());
        testList.Add(new Vector2());
        testList.Add(new Vector2());
        testList.Add(new Vector2());
        testList.Add(new Vector2());
        testList.Add(new Vector2());
        testList.Add(new Vector2());
        testList.Add(new Vector2());
        testList.Add(new Vector2());
        testList.Add(new Vector2());
        testList.Add(new Vector2());//*/

        for (int i = 0; i < testList.Count; ++i)
        {
            for (int j = i + 1; j < testList.Count; ++j)
            {
                for (int k = j + 1; k < testList.Count; ++k)
                {
                    for (int l = k + 1; l < testList.Count; ++l)
                    {
                        resultList.Add(MPTUtils.SegmentIntersect(testList[i], testList[j], testList[k], testList[l]));
                    }
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        int count = 0;
        for (int i = 0; i < testList.Count; ++i)
        {
            for (int j = i + 1; j < testList.Count; ++j)
            {
                for (int k = j + 1; k < testList.Count; ++k)
                {
                    for (int l = k + 1; l < testList.Count; ++l)
                    {
                        if (resultList[count])
                            Gizmos.color = Color.green;
                        else
                            Gizmos.color = Color.red;
                        Gizmos.DrawLine(Vector2.right * 3 * count + testList[k], Vector2.right * 3 * count + testList[l]);
                        Gizmos.DrawLine(Vector2.right * 3 * count + testList[i], Vector2.right * 3 * count + testList[j]);
                        ++count;
                    }
                }
            }
        }
    }
}
