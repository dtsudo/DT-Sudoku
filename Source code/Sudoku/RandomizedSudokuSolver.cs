
namespace Sudoku
{
	using System;
	using System.Collections.Generic;

	public class RandomizedSudokuSolver : IRandomizedSudokuSolver
	{
		public RandomizedSudokuSolver()
		{
		}

		private class InnerRandom : ISudokuRandom
		{
			public int NextInt(int i)
			{
				return 0;
			}
		}

		public int[,] SolveForFirstSolution(int[,] board)
		{
			return this.SolveForRandomSolution(board, new InnerRandom());
		}

		public bool HasAtLeastOneSolution(int[,] board)
		{
			return this.GetSolutions(board, 1, new InnerRandom()).Count == 1;
		}

		public bool HasExactlyOneSolution(int[,] board)
		{
			return this.GetSolutions(board, 2, new InnerRandom()).Count == 1;
		}

		public int[,] SolveForRandomSolution(int[,] board, ISudokuRandom random)
		{
			var solution = this.GetSolutions(board, 1, random);
			if (solution.Count == 0)
			{
				throw new Exception("Undefined behavior - board must have at least one solution");
			}
			return solution[0];
		}

		// Given a board, returns up to numSolutions solutions, chosen randomly.
		// If there are less than numSolutions solutions, the returned list's
		// length is the number of solutions the board has.
		private List<int[,]> GetSolutions(int[,] board, int numSolutions, ISudokuRandom random)
		{
			if (numSolutions == 0)
			{
				return new List<int[,]>();
			}

			if (!this.NoConflicts(board))
			{
				return new List<int[,]>();
			}

			Tuple<int, int> mostConstrainedEmptyCell = this.GetMostConstrainedEmptyCell(board);

			if (mostConstrainedEmptyCell == null)
			{
				var list = new List<int[,]>();
				list.Add(this.CopyBoard(board));
				return list;
			}

			List<int> valuesToTry = new List<int>();
			for (int k = 1; k <= 9; k++)
			{
				valuesToTry.Add(k);
			}

			var solutions = new List<int[,]>();
			while (valuesToTry.Count > 0)
			{
				int nextIndex = random.NextInt(valuesToTry.Count);
				int nextValueToTry = valuesToTry[nextIndex];
				valuesToTry.RemoveAt(nextIndex);

				int[,] boardCopy = this.CopyBoard(board);
				boardCopy[mostConstrainedEmptyCell.Item1, mostConstrainedEmptyCell.Item2] = nextValueToTry;

				int numSolutionsNeeded = numSolutions - solutions.Count;

				List<int[,]> subsolutions = this.GetSolutions(boardCopy, numSolutionsNeeded, random);

				foreach (int[,] subsolution in subsolutions)
				{
					solutions.Add(subsolution);
				}

				if (solutions.Count == numSolutions)
				{
					return solutions;
				}
			}

			return solutions;
		}

		private int[,] CopyBoard(int[,] board)
		{
			int[,] copy = new int[9, 9];

			for (int i = 0; i < 9; i++)
			{
				for (int j = 0; j < 9; j++)
				{
					copy[i, j] = board[i, j];
				}
			}

			return copy;
		}

		// Returns null if board has no empty cells
		private Tuple<int, int> GetMostConstrainedEmptyCell(int[,] board)
		{
			HashSet<int>[,] invalid = new HashSet<int>[9, 9];
			for (int i = 0; i < 9; i++)
			{
				for (int j = 0; j < 9; j++)
				{
					invalid[i, j] = new HashSet<int>();
				}
			}

			for (int i = 0; i < 9; i++)
			{
				for (int j = 0; j < 9; j++)
				{
					int value = board[i, j];

					if (value != 0)
					{
						for (int k = 1; k <= 9; k++)
						{
							invalid[i, j].Add(k);
						}

						for (int k = 0; k < 9; k++)
						{
							invalid[i, k].Add(value);
							invalid[k, j].Add(value);
						}

						int xBox = (i / 3) * 3;
						int yBox = (j / 3) * 3;
						invalid[xBox, yBox].Add(value);
						invalid[xBox, yBox+1].Add(value);
						invalid[xBox, yBox+2].Add(value);
						invalid[xBox+1, yBox].Add(value);
						invalid[xBox+1, yBox+1].Add(value);
						invalid[xBox+1, yBox+2].Add(value);
						invalid[xBox+2, yBox].Add(value);
						invalid[xBox+2, yBox+1].Add(value);
						invalid[xBox+2, yBox+2].Add(value);
					}
				}
			}

			Tuple<int, int> mostConstrainedCell = null;
			int? mostConstrainedCellNumPossibleValues = null;

			for (int i = 0; i < 9; i++)
			{
				for (int j = 0; j < 9; j++)
				{
					if (board[i, j] == 0)
					{
						int numPossibleValues = 9 - invalid[i, j].Count;
						if (mostConstrainedCell == null || ((int)mostConstrainedCellNumPossibleValues > numPossibleValues))
						{
							mostConstrainedCell = new Tuple<int, int>(i, j);
							mostConstrainedCellNumPossibleValues = numPossibleValues;
						}
					}
				}
			}

			return mostConstrainedCell;
		}

		// Returns true iff the board does not have any immediate conflicts
		private bool NoConflicts(int[,] board)
		{
			// Check rows
			for (int i = 0; i < 9; i++)
			{
				if (!this.NoConflicts(board[i, 0],
									  board[i, 1],
									  board[i, 2],
									  board[i, 3],
									  board[i, 4],
									  board[i, 5],
									  board[i, 6],
									  board[i, 7],
									  board[i, 8]))
				{
					return false;
				}
			}

			// Check columns
			for (int j = 0; j < 9; j++)
			{
				if (!this.NoConflicts(board[0, j],
									  board[1, j],
									  board[2, j],
									  board[3, j],
									  board[4, j],
									  board[5, j],
									  board[6, j],
									  board[7, j],
									  board[8, j]))
				{
					return false;
				}
			}

			// Check 3x3 boxes
			for (int i = 0; i < 9; i = i + 3)
			{
				for (int j = 0; j < 9; j = j + 3)
				{
					if (!this.NoConflicts(board[i, j],
										  board[i, j+1],
										  board[i, j+2],
										  board[i+1, j],
										  board[i+1, j+1],
										  board[i+1, j+2],
										  board[i+2, j],
										  board[i+2, j+1],
										  board[i+2, j+2]))
					{
						return false;
					}
				}
			}

			return true;
		}

		private bool NoConflicts(int i1, int i2, int i3, int i4, int i5, int i6, int i7, int i8, int i9)
		{
			int[] array = new int[10];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = 0;
			}
			array[i1] = array[i1] + 1;
			array[i2] = array[i2] + 1;
			array[i3] = array[i3] + 1;
			array[i4] = array[i4] + 1;
			array[i5] = array[i5] + 1;
			array[i6] = array[i6] + 1;
			array[i7] = array[i7] + 1;
			array[i8] = array[i8] + 1;
			array[i9] = array[i9] + 1;
			return array[1] <= 1 && array[2] <= 1 && array[3] <= 1
				&& array[4] <= 1 && array[5] <= 1 && array[6] <= 1
				&& array[7] <= 1 && array[8] <= 1 && array[9] <= 1;
		}
	}
}
