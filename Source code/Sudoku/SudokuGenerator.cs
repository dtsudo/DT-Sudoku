
namespace Sudoku
{
	using System;

	public class SudokuGenerator : ISudokuGenerator
	{
		private IRandomizedSudokuSolver solver;
		private ISudokuRandom random;

		public SudokuGenerator(IRandomizedSudokuSolver s, ISudokuRandom r)
		{
			this.solver = s;
			this.random = r;
		}

		public int[,] GenerateSudokuPuzzle(SudokuDifficulty difficulty)
		{
			int[,] solvedBoard = solver.SolveForRandomSolution(new int[9, 9], this.random);

			Func<Tuple<int, int, int, int>> getLocation = () =>
				{
					int i1 = this.random.NextInt(9);
					int i2 = this.random.NextInt(9);
					int i3 = 8 - i1;
					int i4 = 8 - i2;
					return new Tuple<int, int, int, int>(i1, i2, i3, i4);
				};

			Func<int[,], int[,]> copyBoard = board =>
				{
					var newBoard = new int[9, 9];
					for (int i = 0; i < 9; i++)
						for (int j = 0; j < 9; j++)
							newBoard[i, j] = board[i, j];

					return newBoard;
				};

			int numRemoved = 0;
			int numTries = 0;

			// Note that maxRemoved could be off by one because we remove two cells at a time
			int maxRemoved = 0;
			int maxNumTries = 0;
			if (difficulty == SudokuDifficulty.Easy)
			{
				maxRemoved = 45;
				maxNumTries = 100;
			}
			else if (difficulty == SudokuDifficulty.Normal)
			{
				maxRemoved = 50;
				maxNumTries = 200;
			}
			else if (difficulty == SudokuDifficulty.Hard)
			{
				maxRemoved = 55;
				maxNumTries = 300;
			}
			else
			{
				throw new Exception("Unrecognized difficulty");
			}

			int[,] currentPuzzle = copyBoard(solvedBoard);

			while (true)
			{
				if (numRemoved >= maxRemoved)
					break;
				if (numTries >= maxNumTries)
					break;

				int[,] proposedPuzzle = copyBoard(currentPuzzle);
				var location = getLocation();
				
				proposedPuzzle[location.Item1, location.Item2] = 0;
				proposedPuzzle[location.Item3, location.Item4] = 0;

				if (this.solver.HasExactlyOneSolution(proposedPuzzle))
				{
					currentPuzzle = copyBoard(proposedPuzzle);
					numRemoved = 0;
					for (int i = 0; i < 9; i++)
					{
						for (int j = 0; j < 9; j++)
						{
							if (currentPuzzle[i, j] == 0)
								numRemoved++;
						}
					}
				}

				numTries++;
			}

			return currentPuzzle;
		}
	}
}

