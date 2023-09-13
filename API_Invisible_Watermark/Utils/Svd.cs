using Accord.Math;
using Accord.Math.Decompositions;
using API_Invisible_Watermark.Models;

namespace API_Invisible_Watermark.Utils;

public static class Svd
{
    public static ResultSvd SingularValueDecomposition(this double[,] source)
    {
        var svd = new SingularValueDecomposition(source);


        return new ResultSvd
        {
            U = svd.LeftSingularVectors,
            S = svd.DiagonalMatrix,
            V = svd.RightSingularVectors
        };

    }

    public static double[,] Addition(this double[,] a, double[,] b)
    {
        var result = new double[a.GetLength(0), a.GetLength(1)];

        for (int i = 0; i < result.GetLength(0); i++)
        {
            for (int j = 0; j < result.GetLength(1); j++)
            {
                if(i < b.GetLength(0) && j < b.GetLength(1))
                    result[i, j] = a[i, j] + b[i, j] * 0.25;
                else if (i >= b.GetLength(0) || j >= b.GetLength(1))
                    result[i, j] = a[i, j];
            }
        }

        return result;
    }
}