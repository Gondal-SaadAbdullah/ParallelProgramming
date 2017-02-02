using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace GaussiaonEliminationSequential
{
	class Program
	{
		static void Main(string[] args)
		{
			Random num = new Random();
			int rows = 1000, col = 1001;
			double[,] A = new double[rows, col];
			//{
			//	{5,6,8,5},
			//	{3,-6,1,7},
			//	{4,9,7,10}
			//};

			for (int i = 0; i < rows; i++)
			{
				for (int j = 0; j < col; j++)
				{
					A[i, j] = num.Next(0, 999);
					//Console.Write(A[i,j] + ", ");
				}
				//Console.WriteLine();
			}
			Console.WriteLine(A[0,rows]);

			//Write logic to deal with row interchanging logic if the concerned pivot is already zero
			int N = rows;
			int nextNonzeroRowAtDiagonal;
			int sizeOfDouble = sizeof(double);
			double[] tempRowToBeReplaced = new double[N + 1];
			double[] result = new double[N];
			Task[] task;
			Stopwatch watch = Stopwatch.StartNew();

			for (int diagonal = 0; diagonal < N - 1; diagonal++)
			{

				//do something about it - which is to switch the row with immediate next row,
				//having non zero value in the position of the diagonal
				if (A[diagonal, diagonal] == 0)
				{
					//Console.WriteLine("Switching rows");
					
					for (nextNonzeroRowAtDiagonal = diagonal + 1; nextNonzeroRowAtDiagonal < N && A[nextNonzeroRowAtDiagonal, diagonal] == 0; nextNonzeroRowAtDiagonal++) ;

					//copying diagonal row to a temp array
					Buffer.BlockCopy(A, sizeOfDouble * (N + 1) * (diagonal), tempRowToBeReplaced, 0, (N + 1) * sizeOfDouble);

					//copying first non-zero row to diagonal row
					Buffer.BlockCopy(A, sizeOfDouble * (N + 1) * (nextNonzeroRowAtDiagonal), A, sizeOfDouble * (N + 1) * (diagonal), sizeOfDouble * (N + 1));

					//copying temp array to first non-zero row
					Buffer.BlockCopy(tempRowToBeReplaced, 0, A, sizeOfDouble * (N + 1) * (nextNonzeroRowAtDiagonal), sizeOfDouble * (N + 1));
				}

				task = new Task[N - 1 - diagonal];

				for (int i = diagonal + 1, j = 0; i < N; i++, j++)
				{
					task[j] = new Task((rowObject) => {
						int row = (int)rowObject;

						//Console.WriteLine("i I received {0}", row);

						double localMultiplicationFactor = A[row, diagonal];

						//Means no elimination operation is required for the current row,
						//because the column under the diagonal for this zero is already zero,
						//so just move to the new row
						if (localMultiplicationFactor == 0)
							return;

						if (localMultiplicationFactor > 0)
							localMultiplicationFactor *= -1;
						else
							localMultiplicationFactor = Math.Abs(localMultiplicationFactor);

						//Console.WriteLine("Multiplication Factor : {0}", localMultiplicationFactor);

						Parallel.For(diagonal, N + 1, (int index) =>
						{
							//Console.WriteLine("row {0}, index {1}", row, index);
							A[row, index] = A[row, index] + (A[diagonal, index] * localMultiplicationFactor / A[diagonal, diagonal]);
						});
					}, i);

					task[j].Start();
				}

				Task.WaitAll(task);

				//for (int ii = 0; ii < N; Console.WriteLine(), ii++)
				//	for (int j = 0; j < N + 1; Console.Write("{0}+++", A[ii, j]), j++) ;
			}

			//Console.WriteLine();
			watch.Stop();
			Console.WriteLine("time >>>>> " + watch.ElapsedMilliseconds);
			Console.WriteLine("time ticks >>>>> " + watch.ElapsedTicks);
			watch.Reset();
			watch.Start();

			//Console.WriteLine("Substitution, final results");

			result[N - 1] = A[N - 1, N] / A[N - 1, N - 1];

			int columns = N;
			N = N - 2;
			double temp;

			for (int diagonal = N; diagonal > -1; diagonal--)
			{
				temp = 0;

				for (int j = N + 1; j > diagonal; j--)
					temp += A[diagonal, j] * result[j];

				result[diagonal] = (temp * -1) + A[diagonal, columns];

				result[diagonal] /= A[diagonal, diagonal];
			}

			for (int i = 0; i < 3; i++)
				Console.WriteLine(result[i]);

			//validating values of variables
			double finalResult = 0;
			N = rows;
			for (col = 0; col < N; col++)
			{
				finalResult += A[0, col] * result[col];
			}
			Console.WriteLine("Final Result >>>>> " + finalResult);

			watch.Stop();
			Console.WriteLine("eq time >>>> " + watch.ElapsedMilliseconds);
			Console.WriteLine("eq time ticks >>>> " + watch.ElapsedTicks);

			Console.ReadLine();
		}
	}
}




