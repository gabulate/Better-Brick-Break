using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper
{
    public static float FastDistance(Vector2 firstPosition, Vector2 secondPosition)
    {

        Vector2 heading;
        float distance = 0;
        Vector2 direction;
        float distanceSquared;

        for (int i = 0; i < 10000; i++)
        {
            heading.x = firstPosition.x - secondPosition.x;
            heading.y = firstPosition.y - secondPosition.y;

            distanceSquared = heading.x * heading.x + heading.y * heading.y;
            distance = Mathf.Sqrt(distanceSquared);

            direction.x = heading.x / distance;
            direction.y = heading.y / distance;
        }

        return distance;
    }
}
