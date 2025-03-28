using System; 

class SquareMatrix
{
  private double[,] matrix;
  public int Size { get; set; }

  public SquareMatrix(int size)
  {
    if (size <= 0)
    {
      throw new ArgumentException("Размер матрицы должен быть положительным");
    }

    Size = size;
    matrix = new double[size, size];

    Random random = new Random();
    for (int line = 0; line < size; ++line)
    {
      for (int column = 0; column < size; ++column)
      {
        matrix[line, column] = random.Next(-10, 11);
      }
    }
  }

  public SquareMatrix(double[,] values)
  {
    int size = values.GetLength(0);
    if (size != values.GetLength(1))
    {
      throw new ArgumentException("Матрица должна быть квадратной");
    }

    Size = size;
    matrix = (double[,])values.Clone();
  }

  public override string ToString()
  {
    string result = "";
    for (int line = 0; line < Size; ++line)
    {
      for (int column = 0; column < Size; ++column)
      {
        result += $"{matrix[line, column],4:F2}";
      }
      result += "\n";
    }
    return result;
  }

  public static SquareMatrix operator +(SquareMatrix a, SquareMatrix b)
  {
    if (a.Size != b.Size)
    {
      throw new ArgumentException("Матрицы должны быть одинакового размера");
    }

    int size = a.Size;
    double[,] result = new double[size, size];

    for (int line = 0; line < size; ++line)
    {
      for (int column = 0; column < size; ++column)
      {
        result[line, column] = a.matrix[line, column] + b.matrix[line, column];
      }
    }

    return new SquareMatrix(result);
  }

  public static SquareMatrix operator *(SquareMatrix a, SquareMatrix b)
  {
    if (a.Size != b.Size)
    {
      throw new ArgumentException("Матрицы должны быть одинакового размера");
    }

    int size = a.Size;
    double[,] result = new double[size, size];

    for (int line = 0; line < size; ++line)
    {
      for (int column = 0; column < size; ++column)
      {
        result[line, column] = 0;
        for (int k = 0; k < size; ++k)
        {
          result[line, column] += a.matrix[line, k] * b.matrix[k, column];
        }
      }
    }

    return new SquareMatrix(result);
  }

  public static bool operator >(SquareMatrix a, SquareMatrix b)
  {
    if (a == null || b == null)
    {
      throw new ArgumentNullException("Матрицы не могут быть null");
    }
    return a.Determinant() > b.Determinant();
  }

  public static bool operator <(SquareMatrix a, SquareMatrix b)
  {
    if (a == null || b == null)
    {
      throw new ArgumentNullException("Матрицы не могут быть null");
    }
    return a.Determinant() < b.Determinant();
  }

  public static bool operator ==(SquareMatrix a, SquareMatrix b)
  {
    if (a == null && b == null)
    {
      return true;
    }
    if (a == null || b == null)
    {
      return false;
    }
    return a.Determinant() == b.Determinant();
  }

  public static bool operator !=(SquareMatrix a, SquareMatrix b)
  {
    return !(a == b);
  }

  public int Determinant()
  {
    if (Size == 1)
    {
      return (int)matrix[0, 0];
    }
    if (Size == 2)
    {
      return (int)(matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0]);
    }

    int det = 0;
    for (int column = 0; column < Size; ++column)
    {
      det += (column % 2 == 0 ? 1 : -1) * (int)(matrix[0, column] * Minor(0, column).Determinant());
    }
    return det;
  }

  private SquareMatrix Minor(int row, int column)
  {
    double[,] minor = new double[Size - 1, Size - 1];
    for (int line = 0, mi = 0; line < Size; ++line)
    {
      if (line == row)
      {
        continue;
      }
      for (int col = 0, mj = 0; col < Size; ++col)
      {
        if (col == column)
        {
          continue;
        }
        minor[mi, mj] = matrix[line, col];
        ++mj;
      }
      ++mi;
    }
    return new SquareMatrix(minor);
  }

  public SquareMatrix Inverse()
  {
    int det = Determinant();
    if (det == 0)
    {
      throw new InvalidOperationException("Обратная матрица не существует (det = 0)");
    }

    int size = Size;
    double[,] inverse = new double[size, size];

    for (int line = 0; line < size; ++line)
    {
      for (int column = 0; column < size; ++column)
      {
        int minorDet = Minor(line, column).Determinant();
        inverse[column, line] = ((line + column) % 2 == 0 ? 1 : -1) * minorDet / (double)det;
      }
    }

    return new SquareMatrix(inverse);
  }

  public SquareMatrix Clone()
  {
    int size = this.Size;
    double[,] copyMatrix = new double[size, size];

    for (int line = 0; line < size; ++line)
    {
      for (int column = 0; column < size; ++column)
      {
        copyMatrix[line, column] = this.matrix[line, column];
      }
    }

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

      SquareMatrix matrix1 = new SquareMatrix(size);
      SquareMatrix matrix2 = new SquareMatrix(size);

      Console.WriteLine("Матрица 1:");
      Console.WriteLine(matrix1);
      Console.WriteLine("Матрица 2:");
      Console.WriteLine(matrix2);

      var sumMatrix = matrix1 + matrix2;
      Console.WriteLine("Сложение матриц:");
      Console.WriteLine(sumMatrix);

      var productMatrix = matrix1 * matrix2;
      Console.WriteLine("Умножение матриц:");
      Console.WriteLine(productMatrix);

      if (matrix1 == matrix2)
      {
        Console.WriteLine("Матрицы равны");
      }
      else
      {
        Console.WriteLine("Матрицы не равны");
      }

      Console.WriteLine("Детерминант матрицы 1: " + matrix1.Determinant());

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