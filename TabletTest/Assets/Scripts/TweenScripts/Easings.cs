    using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Easings
{
    #region Linear
    public static float Linear(float x)
    {
        return x;
    }
    #endregion
    #region Ease In's
    public static float EaseInQuad(float x)
    {
        return x * x;
    }

    public static float EaseInCubic(float x)
    {
        return x * x * x;
    }

    public static float EaseInQuart(float x)
    {
        return x * x * x * x;
    }

    public static float EaseInQuint(float x)
    {
        return x * x * x * x * x;
    }

    public static float EaseInCirc(float x)
    {
        return 1 - Mathf.Sqrt(1 - Mathf.Pow(x, 2));
    }

    public static float EaseInBack(float x)
    {
        return 1.70158f * x * x * x - 1.70158f + 1 - x * x;
    }

    public static float EaseInSine(float x)
    {
        return 1 - Mathf.Cos((x * Mathf.PI) / 2);
    }
    #endregion
    #region Ease Out's
    public static float EaseOutQuad(float x)
    {
        return 1 - (1 - x) * (1 - x);
    }

    public static float EaseOutCubic(float x)
    {
        return 1 - Mathf.Pow(1 - x, 3);
    }

    public static float EaseOutQuart(float x)
    {
        return 1 - Mathf.Pow(1 - x, 4);
    }

    public static float EaseOutQuint(float x)
    {
        return 1 - Mathf.Pow(1 - x, 5);
    }

    public static float EaseOutCirc(float x)
    {
        return Mathf.Sqrt(1 - Mathf.Pow(x - 1, 2));
    }

    public static float EaseOutBack(float x)
    {
        return 1 + 1.70158f + 1 * Mathf.Pow(x - 1, 3) + 1.70158f * Mathf.Pow(x - 1, 2);
    }

    public static float EaseOutSine(float x)
    {
        return Mathf.Sin((x * Mathf.PI) / 2);
    }
    #endregion
}
