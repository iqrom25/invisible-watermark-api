namespace API_Invisible_Watermark.Utils;

public class Dwt
{
    // static double w0 = Math.Pow(2,-0.5);
    // static double w1 = -Math.Pow(2,-0.5);
    // static double s0 = Math.Pow(2,-0.5);
    // static double s1 = Math.Pow(2,-0.5);
    
    private const double w0 = 0.5;
    private const double w1 = -0.5;
    private const double s0 = 0.5;
    private const double s1 = 0.5;
    
   
    public static void FWT(double[] data)
    {
        var temp = new double[data.Length];

        var h = data.Length/2;
        for (int i = 0; i < h; i++)
        {
            var k = i * 2;
            temp[i] = data[k] * s0 + data[k + 1] * s1;
            temp[i + h] = data[k] * w0 + data[k + 1] * w1;
        }

        for (int i = 0; i < data.Length; i++)
            data[i] = temp[i];
    }
    
    /// <summary>
    ///   Discrete Haar Wavelet 2D Transform
    /// </summary>
    /// 
    public static void FWT(double[,] data, int iterations)
    {
        var rows = data.GetLength(0);
        var cols = data.GetLength(1);

        var row = new double[cols];
        var col = new double[rows];

        for (int k = 0; k < iterations; k++)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < row.Length; j++)
                    row[j] = data[i, j];

                FWT(row);

                for (int j = 0; j < row.Length; j++)
                    data[i, j] = row[j];
            }

            for (int j = 0; j < cols; j++)
            {
                for (int i = 0; i < col.Length; i++)
                    col[i] = data[i, j];

                FWT(col);

                for (int i = 0; i < col.Length; i++)
                    data[i, j] = col[i];
            }
        }
    }
    
    /// <summary>
    ///   Inverse Haar Wavelet Transform
    /// </summary>
    /// 
    public static void IWT(double[] data)
    {
        var temp = new double[data.Length];

        int h = data.Length / 2;
        for (int i = 0; i < h; i++)
        {
            int k = i * 2;
            temp[k] = (data[i] * s0 + data[i + h] * w0) / w0;
            temp[k + 1] = (data[i] * s1 + data[i + h] * w1) / s0;
        }

        for (int i = 0; i < data.Length; i++)
            data[i] = temp[i];
    }

    /// <summary>
    ///   Inverse Haar Wavelet 2D Transform
    /// </summary>
    /// 
    public static void IWT(double[,] data, int iterations)
    {
        var rows = data.GetLength(0);
        var cols = data.GetLength(1);

       var col = new double[rows];
       var row = new double[cols];

        for (int l = 0; l < iterations; l++)
        {
            for (int j = 0; j < cols; j++)
            {
                for (int i = 0; i < col.Length; i++)
                    col[i] = data[i, j];

                IWT(col);

                for (int i = 0; i < col.Length; i++)
                    data[i, j] = col[i];
            }

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < row.Length; j++)
                    row[j] = data[i, j];

                IWT(row);

                for (int j = 0; j < row.Length; j++)
                    data[i, j] = row[j];
            }
        }
    }

    public static double[,] GetLl2(double[,] source)
    {
        var m = source.GetLength(0) / 4;
        var n = source.GetLength(1) / 4;
        var LL2 = new double[m, n];

        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < n; j++)
            {
                LL2[i, j] = source[i, j];
            }
        }

        return LL2;
    }

    public static void ChangeLl2(double[,] source,double[,] subtitute)
    {
        var m = source.GetLength(0) / 4;
        var n = source.GetLength(1) / 4;
        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < n; j++)
            {
                source[i, j] = subtitute[i, j];
            }
        }
    }
    

}