using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    class Program
    {
        private static void Main(string[] args)
        {
            int[,] matrixA = InitializeMatrix(1000, 1000);
            int[,] matrixB = InitializeMatrix(1000, 3000);

            /*
            PrintMatrix(matrixA);
            Console.WriteLine();
            int[] vector = {2, 3, 4, 5, 6};
            int[] resultVector = MatrixTimesVector(matrixA, vector);
            PrintVector(resultVector);
            Console.ReadLine();
             * */

            /*
            PrintMatrix(matrixA);
            Console.WriteLine();
            PrintVector(GetColumnVector(matrixA, 3));
             

            PrintMatrix(matrixA);
            Console.WriteLine();
            PrintMatrix(matrixB);
            Console.WriteLine();
             * * */

            int[,] resultMatrix;
            int[,] resultMatrix2;

            using (TimeMeasure measure1 = new TimeMeasure("MultiplyByLinearCombinationOfColumnsParallelAlgorithm"))
            {
                resultMatrix = MatrixMultiplyParallel(matrixA, matrixB);
            }

            using (TimeMeasure measure2 = new TimeMeasure("MultiplyByRowTimesColumns"))
            {
                resultMatrix2 = MatrixMultiply2(matrixA, matrixB);
            }

            if (CheckTwoMatrixEqual(resultMatrix, resultMatrix2))
            {
                Console.WriteLine("Two result equal");
            }

            Console.ReadLine();
        }

        static bool CheckTwoMatrixEqual(int[,] matrixA, int[,] matrixB)
        {
            if (matrixA.GetLength(0) != matrixB.GetLength(0)) return false;
            if (matrixA.GetLength(1) != matrixB.GetLength(1)) return false;

            int rowCount = matrixA.GetLength(0);
            int columnCount = matrixB.GetLength(1);

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    if (matrixA[i, j] != matrixB[i, j]) return false;
                }
            }

            return true;
        }

        static int[,] InitializeMatrix(int m, int n)
        {
            int[,] matrix = new int[m, n];

            Random rand = new Random();
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    matrix[i, j] = rand.Next(100);
                }
            }

            return matrix;
        }

        static int[,] MatrixMultiply(int[,] matrixA, int[,] matrixB)
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

            for (int i = 0; i < matrixBColumnCount; i++)
            {
                int[] columnVector = GetColumnVector(matrixB, i);
                int[] tempResult = MatrixTimesVector(matrixA, columnVector);

                for (int j = 0; j < matrixARowCount; j++)
                {
                    resultMatrix[j, i] = tempResult[j];
                }
            }

            return resultMatrix;
        }

        static int[,] MatrixMultiplyParallel(int[,] matrixA, int[,] matrixB)
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

            Parallel.For(0, matrixBColumnCount, colBIdx =>
            {
                int[] columnVector = GetColumnVector(matrixB, colBIdx);
                int[] tempResult = MatrixTimesVector(matrixA, columnVector);

                for (int j = 0; j < matrixARowCount; j++)
                {
                    resultMatrix[j, colBIdx] = tempResult[j];
                }
            });

            return resultMatrix;
        }

        static int[,] MatrixMultiply2(int[,] matrixA, int[,] matrixB)
        {
            int matrixAColumnCount = matrixA.GetLength(1);
            int matrixBRowCount = matrixB.GetLength(0);

            if (matrixAColumnCount != matrixBRowCount)
            {
                throw new Exception("Dimension error");
            }

            int matrixARowCount = matrixA.GetLength(0);
            int matrixBColumnCount = matrixB.GetLength(1);

            int[,] result = new int[matrixARowCount, matrixBColumnCount];

            for (int i = 0; i < matrixARowCount; i++)
            {
                for (int j = 0; j < matrixBColumnCount; j++)
                {
                    for (int k = 0; k < matrixAColumnCount; k++)
                    {
                        result[i, j] += matrixA[i, k]*matrixB[k, j];
                    }
                }
            }

            return result;
        }

        static int[] MatrixTimesVector(int[,] matrix, int[] vector)
        {
            int matrixColumnCount = matrix.GetLength(1);
            if (matrixColumnCount != vector.Length)
            {
                throw new Exception("Dimension error");
            }

            int matrixRowCount = matrix.GetLength(0);

            int[] resultVector = new int[matrixRowCount];
            InitializeVector(resultVector);

            for (int i = 0; i < matrixColumnCount; i++)
            {
                for (int j = 0; j < matrixRowCount; j++)
                {
                    resultVector[j] += matrix[j, i]*vector[i];
                }
            }

            return resultVector;
        }

        static int[] GetColumnVector(int[,] matrix, int colIdx)
        {
            int rowCount = matrix.GetLength(0);
            int[] columnVector = new int[rowCount];

            for (int i = 0; i < rowCount; i++)
            {
                columnVector[i] = matrix[i, colIdx];
            }

            return columnVector;
        }

        static void InitializeVector(int[] vector)
        {
            if (vector == null) return;
            for (int i = 0; i < vector.Length; i++)
            {
                vector[i] = 0;
            }
        }

        static void PrintMatrix(int[,] matrix)
        {
            if (matrix == null) return;

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write("{0,10}", matrix[i, j]);
                }
                Console.WriteLine();
            }
        }

        static void PrintVector(int[] vector)
        {
            for (int i = 0; i < vector.Length; i++)
            {
                Console.Write("{0,5}", vector[i]);
            }
        }
    }

    class TimeMeasure:IDisposable
    {
        public String MeasureName { get; set; }

        private Stopwatch watch = new Stopwatch();

        public TimeMeasure(String measureName)
        {
            MeasureName = measureName;
            watch.Start();
        }

        public void Dispose()
        {
            watch.Stop();
            Console.WriteLine("{0} costed {1} milliseconds", MeasureName, watch.ElapsedMilliseconds);
        }
    }
}
