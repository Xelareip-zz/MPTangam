using UnityEngine;
using System.Collections;

public class MPTUtils
{
    public static Vector2 ScreenToWorld(Vector2 screenPoint)
    {
        float y = Camera.main.transform.position.y + Camera.main.orthographicSize * (-1.0f + 2.0f / Screen.height * Input.mousePosition.y);
        float x = Camera.main.transform.position.x + Camera.main.orthographicSize * (-1.0f + 2.0f / Screen.width * Input.mousePosition.x) * Screen.width / Screen.height;
        
        return new Vector2(x, y);
    }

    public static bool SegmentIntersect(Vector2 seg0Start, Vector2 seg0End, Vector2 seg1Start, Vector2 seg1End)
    {

        // Get A,B,C of first line - points : ps1 to pe1
        float A1 = seg0End.y - seg0Start.y;
        float B1 = seg0Start.x - seg0End.x;
        float C1 = A1 * seg0Start.x + B1 * seg0Start.y;

        // Get A,B,C of second line - points : ps2 to pe2
        float A2 = seg1End.y - seg1Start.y;
        float B2 = seg1Start.x - seg1End.x;
        float C2 = A2 * seg1Start.x + B2 * seg1Start.y;

        // Get delta and check if the lines are parallel
        float delta = A1 * B2 - A2 * B1;
        bool coincidence = (B2 * C1 - B1 * C2) == 0 && (A2 * C1 - A1 * C2) == 0;
        float intersectionTest0;
        float intersectionTest1;
        float intersectionTest2;
        float intersectionTest3;
        if (delta == 0 && coincidence)
        {
            intersectionTest0 = (seg1Start.x - seg0Start.x) * (seg1Start.x - seg0End.x);
            intersectionTest1 = (seg1End.x - seg0Start.x) * (seg1End.x - seg0End.x);
            intersectionTest2 = (seg1Start.y - seg0Start.y) * (seg1Start.y - seg0End.y);
            intersectionTest3 = (seg1End.y - seg0Start.y) * (seg1End.y - seg0End.y);
            return intersectionTest0 <= 0 || intersectionTest1 <= 0 || intersectionTest2 <= 0 || intersectionTest3 <= 0;
        }
        else if (delta == 0)
        {
            return false;
        }

        // now return the Vector2 intersection point
        Vector2 intersection = new Vector2(
            (B2 * C1 - B1 * C2) / delta,
            (A1 * C2 - A2 * C1) / delta
        );


        intersectionTest0 = (intersection.x - seg0Start.x) * (intersection.x - seg0End.x);
        intersectionTest1 = (intersection.x - seg1Start.x) * (intersection.x - seg1End.x);
        intersectionTest2 = (intersection.y - seg0Start.y) * (intersection.y - seg0End.y);
        intersectionTest3 = (intersection.y - seg1Start.y) * (intersection.y - seg1End.y);
        return intersectionTest0 <= 0 && intersectionTest1 <= 0 && intersectionTest2 <= 0 && intersectionTest3 <= 0;
    }

    public static bool PolygonContains(Vector2[] polygon, Vector2 point)
    {
        float firstRes = Vector3.Cross(polygon[1] - polygon[0], point - polygon[0]).z;

        for (int i = 1; i < polygon.Length; ++i)
        {
            float crossRes = Vector3.Cross(polygon[(i + 1) % polygon.Length] - polygon[i], point - polygon[i]).z;
            if (firstRes * crossRes < 0)
            {
                return false;
            }
            else if (crossRes == 0.0f)
            {
                if ((point.x - polygon[(i + 1) % polygon.Length].x) * (point.x - polygon[i].x) <= 0)
                {
                    return true;
                }
            }
        }
        return true;
    }

    public static bool PolygonsIntersect(Vector2[] polygon0, Vector2[] polygon1)
    {
        for (int i = 0; i < polygon0.Length; ++i)
        {
            if (PolygonContains(polygon1, polygon0[i]))
            {
                return true;
            }
        }

        for (int i = 0; i < polygon1.Length; ++i)
        {
            if (PolygonContains(polygon0, polygon1[i]))
            {
                return true;
            }
        }

        for (int i = 0; i < polygon0.Length; ++i)
        {
            for (int j = 0; j < polygon1.Length; ++j)
            {
                if (MPTUtils.SegmentIntersect(polygon0[i], polygon0[(i + 1) % polygon0.Length], polygon1[j], polygon1[(j + 1) % polygon1.Length]))
                {
                    return true;
                }
            }
        }

        return false;
    }
}
