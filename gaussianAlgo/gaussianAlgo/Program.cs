using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaussiaonEliminationSequential
{
	class Program
	{
		static void Main(string[] args)
		{
			double[,] A = new double[,]
			{
				{5,6,8,5},
				{3,-6,1,7},
				{4,9,7,10}
			};

			//Write logic to deal with row interchanging logic if the concerned pivot is already zero
			int N = 3;
			int nextNonzeroRowAtDiagonal;
			double multiplicationFactor;
			int sizeOfDouble = sizeof(double);
			double[] tempRowToBeReplaced = new double[N + 1];
			double[] result = new double[N];

			for (int diagonal = 0; diagonal < N - 1; diagonal++)
			{

				//do something about it - which is to switch the row with immediate next row,
				//having non zero value in the position of the diagonal
				if (A[diagonal, diagonal] == 0)
				{
					for (nextNonzeroRowAtDiagonal = diagonal + 1; nextNonzeroRowAtDiagonal < N && A[nextNonzeroRowAtDiagonal, diagonal] == 0; nextNonzeroRowAtDiagonal++) ;

					//copying diagonal row to a temp array
					Buffer.BlockCopy(A, sizeOfDouble * (N + 1) * (diagonal), tempRowToBeReplaced, 0, (N + 1) * sizeOfDouble);

					//copying first non-zero row to diagonal row
					Buffer.BlockCopy(A, sizeOfDouble * (N + 1) * (nextNonzeroRowAtDiagonal), A, sizeOfDouble * (N + 1) * (diagonal), sizeOfDouble * (N + 1));

					//copying temp array to first non-zero row
					Buffer.BlockCopy(tempRowToBeReplaced, 0, A, sizeOfDouble * (N + 1) * (nextNonzeroRowAtDiagonal), sizeOfDouble * (N + 1));
				}

				for (int i = diagonal + 1; i < N; i++)
				{
					multiplicationFactor = A[i, diagonal];

					//Means no elimination operation is required for the current row,
					//because the column under the diagonal for this zero is already zero,
					//so just move to the new row
					if (multiplicationFactor == 0)
						continue;

					if (multiplicationFactor > 0)
						multiplicationFactor *= -1;

					else
						multiplicationFactor = Math.Abs(multiplicationFactor);

					Console.WriteLine("Multiplication Factor : {0}", multiplicationFactor);

					for (int j = diagonal; j < N + 1; j++)
					{
						A[i, j] = A[i, j] + (A[diagonal, j] * multiplicationFactor / A[diagonal, diagonal]);
					}

					for (int ii = 0; ii < N; Console.WriteLine(), ii++)
						for (int j = 0; j < N + 1; Console.Write("{0}+++", A[ii, j]), j++) ;
				}
			}

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

			for (int i = 0; i < columns; i++)
				Console.WriteLine(result[i]);
		}
	}
}




