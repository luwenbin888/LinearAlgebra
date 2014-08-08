using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            int[,] matrixA = new int[3, 3];
            matrixA[0, 0] = 1;
            matrixA[0, 1] = 2;
            matrixA[0, 2] = 3;

            matrixA[1, 0] = 2;
            matrixA[1, 1] = 2;
            matrixA[1, 2] = 3;

            matrixA[2, 0] = 3;
            matrixA[2, 1] = 2;
            matrixA[2, 2] = 5;

            int factor = matrixA[1, 0]/matrixA[0, 0];

            PrintMatrix(matrixA);
            Console.WriteLine();
            int[,] elemMatrix = InitializeElementaryMatrix(3, 2, 1, factor, Operation.Deduct);

            Console.WriteLine("Deduct row 1 from row 2");
            int[,] result = MatrixMultiplication(elemMatrix, matrixA);

            PrintMatrix(result);
            Console.WriteLine();

            Console.WriteLine("Deduct row 1 from row 3");
            factor = result[2, 0]/result[0, 0];
            int[,] elemMatrix1 = InitializeElementaryMatrix(3, 3, 1, factor, Operation.Deduct);
            int[,] result2 = MatrixMultiplication(elemMatrix1, result);

            PrintMatrix(result2);
            Console.WriteLine();

            Console.WriteLine("Deduct row 2 from row 3");
            factor = result2[2, 1]/result2[1, 1];
            int[,] elemMatrix2 = InitializeElementaryMatrix(3, 3, 2, factor, Operation.Deduct);
            int[,] reducedMatrix = MatrixMultiplication(elemMatrix2, result2);

            PrintMatrix(reducedMatrix);
            
            Console.ReadLine();
        }

        static int[,] InitializePermutationMatrix(int dimension, int exchangeFrom, int exchangeTo)
        {
            if (dimension <= 0) return null;
            int[,] permutationMatrix = InitializeIdentityMatrix(dimension);
            int from = exchangeFrom - 1;
            int to = exchangeTo - 1;

            permutationMatrix[from, from] = 0;
            permutationMatrix[to, to] = 0;

            permutationMatrix[from, to] = 1;
            permutationMatrix[to, from] = 1;

            return permutationMatrix;
        }

        private static int[,] InitializeElementaryMatrix(int dimension, int fromRow, int toRow, int multiply = 1,
            Operation oper = Operation.Deduct)
        {
            if (dimension <= 0) return null;

            int[,] elementaryMatrix = InitializeIdentityMatrix(dimension);
            int multiplier = oper == Operation.Add ? 1 : -1;
            int num = multiplier*multiply;

            elementaryMatrix[fromRow - 1, toRow - 1] = num;

            return elementaryMatrix;
        }

        static int[,] InitializeIdentityMatrix(int dimension)
        {
            if (dimension <= 0) return null;

            int[,] identityMatrix = new int[dimension, dimension];

            for (int m = 0; m < dimension; m++)
            {
                for (int n = 0; n < dimension; n++)
                {
                    if (m == n) identityMatrix[m, n] = 1;
                    else identityMatrix[m, n] = 0;
                }
            }

            return identityMatrix;
        }

        static int[,] MatrixMultiplication(int[,] matrixA, int[,] matrixB)
        {
            int matrixAColumnCount = matrixA.GetLength(1);
            int matrixBRowCount = matrixB.GetLength(0);

            if (matrixAColumnCount != matrixBRowCount)
            {
                throw new Exception("Dimension error");
            }

            int matrixARowCount = matrixA.GetLength(0);
            int matrixBColumnCount = matrixB.GetLength(1);

            int[,] resultMatrix = new int[matrixARowCount, matrixBColumnCount];

            for (int m = 0; m < matrixARowCount; m++)
            {
                for (int n = 0; n < matrixBColumnCount; n++)
                {
                    for (int k = 0; k < matrixAColumnCount; k++)
                    {
                        resultMatrix[m, n] += matrixA[m, k]*matrixB[k, n];
                    }
                }
            }

            return resultMatrix;
        }

        static int[,] InitializeMatrix(int row, int column)
        {
            Random rand = new Random();
            int[,] matrix = new int[row, column];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    matrix[i, j] = rand.Next(10);
                }
            }

            return matrix;
        }

        static void PrintMatrix(int[,] matrix)
        {
            if (matrix == null) return;

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write("{0,5}", matrix[i, j]);
                }
                Console.WriteLine();
            }
        }
    }

    internal enum Operation
    {
        Add = 1,
        Deduct = 2
    }
}
