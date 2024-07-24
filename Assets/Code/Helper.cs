using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper
{
    public static float FastDistance(Vector2 firstPosition, Vector2 secondPosition)
    {
        Vector2 heading = firstPosition - secondPosition;
        float distanceSquared = heading.x * heading.x + heading.y * heading.y;
        return Mathf.Sqrt(distanceSquared);
    }
}
