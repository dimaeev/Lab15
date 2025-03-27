using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class SquareMatrix
{
  private int[,] matrix;
  public int Size { get; set; }

  public SquareMatrix(int size)
  {
    if (size <= 0)
      throw new ArgumentException("Размер матрицы должен быть положительным");

    Size = size;
    matrix = new int[size, size];

    Random random = new Random();
    for (int i = 0; i < size; i++)
      for (int j = 0; j < size; j++)
        matrix[i, j] = random.Next(-10, 11);
  }

  public SquareMatrix(int[,] values)
  {
    int size = values.GetLength(0);
    if (size != values.GetLength(1))
      throw new ArgumentException("Матрица должна быть квадратной");

    Size = size;
    matrix = (int[,])values.Clone();
  }

  public override string ToString()
  {
    string result = "";
    for (int i = 0; i < Size; i++)
    {
      for (int j = 0; j < Size; j++)
        result += $"{matrix[i, j],4}";
      result += "\n";
    }
    return result;
  }
}

  public static SquareMatrix operator +(SquareMatrix a, SquareMatrix b)
  {
    if (a.Size != b.Size)
      throw new ArgumentException("Матрица должны быть одинакового размера ");

    int size = a.Size;
    int[,] result = new int[size, size];

    for (int i = 0; i < size; i++)
      for (int j = 0;j < size; j++)
        result[i, j] = a.matrix[i, j] + b.matrix[i, j];

    return new SquareMatrix(result);
  }

  public static SquareMatrix operator *(SquareMatrix a, SquareMatrix b)
  {
    if (a.Size != b.Size)
      throw new ArgumentException("Матрицы должны быть одинакового размера");

    int size = a.Size;
    int[,] result = new int[size,size];

    for (int i = 0;i < size;i++)
      for (int j = 0; j < size;j++)
      {
        result[i, j] = 0;
        for (int k = 0; k < size; k++)
          result[i, j] += a.matrix[i, k] * b.matrix[k, j];
      }

    return new SquareMatrix(result);
  }



class Program
{
  static void Main()
  {

  }
}
