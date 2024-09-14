using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ProjMath
{
    public static float EaseInCubic(float x)
    {
        return x * x * x;
    }

    public static float EaseOutBounce(float x)
    {
        float n1 = 7.5625f;
        float d1 = 2.75f;

        if (x < 1 / d1)
        {
            return n1 * x * x;
        }
        else if (x < 2 / d1)
        {
            return n1 * (x - 1.5f / d1) * x + 0.75f;
        }
        else if (x < 2.5 / d1)
        {
            return n1 * (x - 2.25f / d1) * x + 0.9375f;
        }
        else
        {
            return n1 * (x - 2.625f / d1) * x + 0.984375f;
        }
    }

    public static float EaseInOutBounce(float x)
    {
        return x < 0.5 
            ? (1 - EaseOutBounce(1 - 2 * x)) / 2 
            : (1 + EaseOutBounce(2 * x - 1)) / 2;
    }

    public static float EaseOutQuint(float x)
    {
        return 1 - Mathf.Pow(1 - x, 5);
    }

    public static float RotateTowardsPosition(Vector3 startPosition, Vector3 position)
    {
        Vector3 diference = position - startPosition;
        return Mathf.Atan2(diference.y, diference.x) * Mathf.Rad2Deg;
    }

    public static float SinTime(float m = 1f)
    {
        return Mathf.Sin(Time.timeSinceLevelLoad) * m * (Mathf.Sin(Time.timeSinceLevelLoad) * m > 0 ? 1f : -1f);
    }
}
