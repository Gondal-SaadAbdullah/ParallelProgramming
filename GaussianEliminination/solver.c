/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/*
 * File:   gauss_algo.c
 * Author: saadabdullahgondal
 *
 * Created on January 17, 2017, 6:41 PM
 */

#include <stdio.h>
#include <stdlib.h>
#include <math.h>
#include <omp.h>

/*
 *
 */

int main(int argc, char** argv)

    {
    double A[4][4] =
	{
	    {
	    0, 6, 8, 5
	    },
	    {
	    3, -6, 1, 7
	    },
	    {
	    4, 9, 7, 10
	    }
	};

    int N = 3;
    int nextNonzeroRowAtDiagonal;
    double multiplicationFactor;

    int sizeOfDouble = sizeof(double);

    double zerothRow[N + 1];

    double result[N];

    for (int diagonal = 0; diagonal < N - 1; diagonal++)

	{
	//do something about it - which is to switch the row with immediate next row,
	//having non zero value in the position of the diagonal

	if (A[diagonal][diagonal] == 0)
	    {
	    printf("Switching rows\n");

	    for (nextNonzeroRowAtDiagonal = diagonal + 1; nextNonzeroRowAtDiagonal < N && A[nextNonzeroRowAtDiagonal, diagonal] == 0;
		    nextNonzeroRowAtDiagonal++)
		;

	    //copying diagonal row to a temp array
	    memcpy(zerothRow, A[diagonal], (N + 1) * sizeOfDouble);

	    //copying first non-zero row to diagonal row
	    memcpy(A[diagonal], A[nextNonzeroRowAtDiagonal], (N + 1) * sizeOfDouble);

	    //copying temp array to first non-zero row
	    memcpy(A[nextNonzeroRowAtDiagonal], zerothRow, (N + 1) * sizeOfDouble);
	    }

	multiplicationFactor = A[diagonal][diagonal];

	printf("Division Factor : %f\n", multiplicationFactor);

	if (multiplicationFactor != 1)
	    {
	    for (int j = diagonal; j < N + 1; j++)
		A[diagonal][j] = A[diagonal][j] / multiplicationFactor;
	    }

#pragma omp parallel num_threads(4)
	{
	printf("\nthread number ::: %d\n", omp_get_thread_num());
#pragma omp for
	for (int i = diagonal + 1; i < N; i++)
	    {
	    multiplicationFactor = A[i][diagonal];

	    //Means no elimination operation is required for the current row,
	    //because the column under the diagonal for this zero is already zero,
	    //so just move to the new row

	    if (multiplicationFactor == 0)
		continue;

	    if (multiplicationFactor > 0)
		multiplicationFactor *= -1;
	    else
		multiplicationFactor = abs(multiplicationFactor);

	    printf("Multiplication Factor : %f\n", multiplicationFactor);

	    for (int j = diagonal; j < N + 1; j++)
		A[i][j] = A[i][j] + (A[diagonal][j] * multiplicationFactor);

//	    for (int ii = 0; ii < N; printf("\n"), ii++)
//		for (int j = 0; j < N + 1; printf("%f +++", A[ii][j]), j++)
//		    ;
	    }
	}

	for (int ii = 0; ii < N; printf("\n"), ii++)
	    for (int j = 0; j < N + 1; printf("%f +++", A[ii][j]), j++)
		;
	}

    printf("\n");
    printf("Substitution, final results\n");
    result[N - 1] = A[N - 1][N] / A[N - 1][N - 1];
    N = N - 2;
    double temp;

    for (int diagonal = N; diagonal > -1; diagonal--)
	{
	temp = 0;
	for (int j = N + 1; j > diagonal; j--)
	    temp += A[diagonal][j] * result[diagonal + 1];

	result[diagonal] += result[diagonal] + (temp * -1);
	}

    for (int i = 0; i < 3; i++)
	printf("%f\n", result[i]);

    return (EXIT_SUCCESS);
    }

