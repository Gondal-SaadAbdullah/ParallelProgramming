
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

			//Write logic to deal with frow interchanging logic if the concerned pivot is already zero

			int N = 3;
			double multiplicationFactor;
			double divisionFactor;


			for (int diagonal = 0; diagonal < N - 1; diagonal++)
			{
				if (A[diagonal, diagonal] == 0)
					continue;
					//do something about it

				divisionFactor = A[diagonal, diagonal];

				for (int i = diagonal + 1; i < N; i++)

				{
					multiplicationFactor = A[i, diagonal];

					if (multiplicationFactor == 0)
						continue;

					if ( multiplicationFactor > 0)

						multiplicationFactor *= -1;

					else

						multiplicationFactor = Math.Abs(multiplicationFactor);


					Console.WriteLine("Multiplication Factor : {0}", multiplicationFactor);


					for (int j = diagonal; j < N + 1; j++)
					{
						if (i == diagonal + 1) 
							A[diagonal, j] = A[diagonal, j] / divisionFactor;
						
						A[i, j] = A[i, j] + (A[diagonal, j] * multiplicationFactor);

					}


					for (int ii = 0; ii < N; Console.WriteLine(), ii++)

						for (int j = 0; j < N + 1; Console.Write("{0}+++", A[ii, j]), j++) ;

				}




				//for (int i = 0; i < N; Console.WriteLine(), i++)

				//    for (int j = 0; j < N; Console.Write("{0}+++", A[i, j]), j++) ;

			}




			Console.WriteLine();




			//for (int i = 0; i < N; Console.WriteLine(), i++)

			//    for (int j = 0; j < N; Console.Write("{0}+++", A[i, j]), j++) ;

		}

	}

}




