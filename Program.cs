using System;

class SquareMatrix
{
  private double[,] matrix;
  public int Size { get; set; }

  public SquareMatrix(int size)
  {
    if (size <= 0)
      throw new ArgumentException("Размер матрицы должен быть положительным");

    Size = size;
    matrix = new double[size, size];

    Random random = new Random();
    for (int i = 0; i < size; i++)
      for (int j = 0; j < size; j++)
        matrix[i, j] = random.Next(-10, 11);
  }

  public SquareMatrix(double[,] values)
  {
    int size = values.GetLength(0);
    if (size != values.GetLength(1))
      throw new ArgumentException("Матрица должна быть квадратной");

    Size = size;
    matrix = (double[,])values.Clone();
  }

  public override string ToString()
  {
    string result = "";
    for (int i = 0; i < Size; i++)
    {
      for (int j = 0; j < Size; j++)
        result += $"{matrix[i, j],4:F2}";
    }
    return result;
  }

  public static SquareMatrix operator +(SquareMatrix a, SquareMatrix b)
  {
    if (a.Size != b.Size)
      throw new ArgumentException("Матрицы должны быть одинакового размера");

    int size = a.Size;
    double[,] result = new double[size, size];

    for (int i = 0; i < size; i++)
      for (int j = 0; j < size; j++)
        result[i, j] = a.matrix[i, j] + b.matrix[i, j];

    return new SquareMatrix(result);
  }

  public static SquareMatrix operator *(SquareMatrix a, SquareMatrix b)
  {
    if (a.Size != b.Size)
      throw new ArgumentException("Матрицы должны быть одинакового размера");

    int size = a.Size;
    double[,] result = new double[size, size];

    for (int i = 0; i < size; i++)
      for (int j = 0; j < size; j++)
      {
        result[i, j] = 0;
        for (int k = 0; k < size; k++)
          result[i, j] += a.matrix[i, k] * b.matrix[k, j];
      }

    return new SquareMatrix(result);
  }

  public static bool operator >(SquareMatrix a, SquareMatrix b)
  {
    if (a == null || b == null)
      throw new ArgumentNullException("Матрицы не могут быть null");

    return a.Determinant() > b.Determinant();
  }

  public static bool operator <(SquareMatrix a, SquareMatrix b)
  {
    if (a == null || b == null)
      throw new ArgumentNullException("Матрицы не могут быть null");

    return a.Determinant() < b.Determinant();
  }

  public static bool operator ==(SquareMatrix a, SquareMatrix b)
  {
    if (a == null && b == null)
      return true;
    if (a == null || b == null)
      return false;

    return a.Determinant() == b.Determinant();
  }

  public static bool operator !=(SquareMatrix a, SquareMatrix b)
  {
    return !(a == b);
  }

  public int Determinant()
  {
    if (Size == 1) return (int)matrix[0, 0];
    if (Size == 2)
      return (int)(matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0]);

    int det = 0;
    for (int j = 0; j < Size; j++)
    {
      det += (j % 2 == 0 ? 1 : -1) * (int)(matrix[0, j] * Minor(0, j).Determinant());
    }
    return det;
  }

  private SquareMatrix Minor(int row, int col)
  {
    double[,] minor = new double[Size - 1, Size - 1];
    for (int i = 0, mi = 0; i < Size; i++)
    {
      if (i == row) continue;
      for (int j = 0, mj = 0; j < Size; j++)
      {
        if (j == col) continue;
        minor[mi, mj] = matrix[i, j];
        mj++;
      }
      mi++;
    }
    return new SquareMatrix(minor);
  }

  public SquareMatrix Inverse()
  {
    int det = Determinant();
    if (det == 0)
      throw new InvalidOperationException("Обратная матрица не существует (det = 0)");

    int size = Size;
    double[,] inverse = new double[size, size];

    for (int i = 0; i < size; i++)
    {
      for (int j = 0; j < size; j++)
      {
        int minorDet = Minor(i, j).Determinant();
        inverse[j, i] = ((i + j) % 2 == 0 ? 1 : -1) * minorDet / (double)det;
      }
    }

    return new SquareMatrix(inverse);
  }

  public SquareMatrix Clone()
  {
    int size = this.Size;
    double[,] copyMatrix = new double[size, size];

    for (int i = 0; i < size; i++)
      for (int j = 0; j < size; j++)
        copyMatrix[i, j] = this.matrix[i, j];

    return new SquareMatrix(copyMatrix);
  }
}

class Program
{
  static void Main()
  {
    try
    {
      Console.WriteLine("Введите размер матриц:");
      int size = int.Parse(Console.ReadLine());

      // Две случайные матрицы
      SquareMatrix matrix1 = new SquareMatrix(size);
      SquareMatrix matrix2 = new SquareMatrix(size);

      Console.WriteLine("Матрица 1:");
      Console.WriteLine(matrix1);
      Console.WriteLine("Матрица 2:");
      Console.WriteLine(matrix2);

      // Операция сложения
      var sumMatrix = matrix1 + matrix2;
      Console.WriteLine("Сложение матриц:");
      Console.WriteLine(sumMatrix);

      // Операция умножения
      var productMatrix = matrix1 * matrix2;
      Console.WriteLine("Умножение матриц:");
      Console.WriteLine(productMatrix);

      // Сравнение матриц
      if (matrix1 == matrix2)
        Console.WriteLine("Матрицы равны");
      else
        Console.WriteLine("Матрицы не равны");

      // Вычисление детерминанта
      Console.WriteLine("Детерминант матрицы 1: " + matrix1.Determinant());

      // Обратная матрица
      try
      {
        var inverseMatrix = matrix1.Inverse();
        Console.WriteLine("Обратная матрица 1:");
        Console.WriteLine(inverseMatrix);
      }
      catch (Exception ex)
      {
        Console.WriteLine("Ошибка при вычислении обратной матрицы: " + ex.Message);
      }
    }
    catch (Exception ex)
    {
      Console.WriteLine("Ошибка: " + ex.Message);
    }
  }
}