using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathSolverUtility
{
    /// <summary>
    /// Solves linear equation of 0 = ax + b
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static float? SolveLinearEquation(float a, float b)
    {
        if (0 == a) return null;
        return -b / a;
    }

    /// <summary>
    /// Solves quadratic equation of 0 = ax^2 + bx + c
    /// </summary>
    /// <param name="a">coefficient of 2nd order term</param>
    /// <param name="b">coefficient of 1st order term</param>
    /// <param name="c">constant</param>
    /// <returns>nullable solution</returns>
    public static (float?, float?) SolveQuadraticEquation(float a, float b, float c)
    {
        if (0 == a) return (SolveLinearEquation(b, c), null);

        float discriminant = b * b - 4 * a * c;
        if (discriminant < 0) return (null, null);

        float sqrtDiscriminant = Mathf.Sqrt(discriminant);
        float inverseDenominator = 1 / (2 * a);
        float solution0 = (-b + sqrtDiscriminant) * inverseDenominator;
        float solution1 = (-b - sqrtDiscriminant) * inverseDenominator;
        return (solution0, solution1);
    }
}
