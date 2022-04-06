using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AIUtils
{
    public static Vector2 GetPlayerPredictiveTarget(Vector2 targetPosition, Vector2 targetVelocity, Vector2 shootPosition, float projectileSpeed)
    {
        var dist = (targetPosition - shootPosition).magnitude;
        var time = dist / projectileSpeed;

        return targetPosition + (targetVelocity* time);
    }

    public static Vector2 GetPerpendicular(Vector2 vector)
    {
        Vector2 result;
        result.x = vector.y;
        result.y = -vector.x;

        return result;
    }

}