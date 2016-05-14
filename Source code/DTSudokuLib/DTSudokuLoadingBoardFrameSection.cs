
namespace DTSudokuLib
{
	using System;
	using System.Threading;

	using Sudoku;

	public class DTSudokuLoadingBoardFrameSection
	{
		/// <summary>
		/// An interface marking an implementation of ISudokuRandom
		/// as being thread-safe.
		/// </summary>
		public interface IThreadSafeSudokuRandom : ISudokuRandom
		{
		}

		private class ThreadSafeSudokuRandom : IThreadSafeSudokuRandom
		{
			private Object randomLock = new Object();

			private ISudokuRandom random;

			public ThreadSafeSudokuRandom()
			{
				this.random = new SudokuRandom();
			}

			public int NextInt(int i)
			{
				int nextValue;

				lock (this.randomLock)
				{
					nextValue = this.random.NextInt(i);
				}

				return nextValue;
			}
		}

		public static IThreadSafeSudokuRandom GetIThreadSafeSudokuRandomInstance()
		{
			return new ThreadSafeSudokuRandom();
		}

		private Object locker = new Object();

		private DTSudokuDifficultyValue difficulty;
		private IThreadSafeSudokuRandom random;
		private int[,] newSudokuBoard;
		private bool isThreadDone;

		public DTSudokuLoadingBoardFrameSection(DTSudokuDifficultyValue difficulty, IThreadSafeSudokuRandom random)
		{
			lock (this.locker)
			{
				this.difficulty = difficulty;
				this.random = random;
				this.newSudokuBoard = null;
				this.isThreadDone = false;
			}

			Thread thread = new Thread(new ThreadStart(this.generateNewSudokuBoard));
			thread.IsBackground = true;
			thread.Start();
		}

		private void generateNewSudokuBoard()
		{
			IThreadSafeSudokuRandom random;
			DTSudokuDifficultyValue difficulty;

			lock (this.locker)
			{
				random = this.random;
				difficulty = this.difficulty;
			}

			ISudokuGenerator generator = new SudokuGenerator(new RandomizedSudokuSolver(), random); 

			int[,] newBoard;

			if (difficulty == DTSudokuDifficultyValue.Easy)
				newBoard = generator.GenerateSudokuPuzzle(SudokuDifficulty.Easy);
			else if (difficulty == DTSudokuDifficultyValue.Normal)
				newBoard = generator.GenerateSudokuPuzzle(SudokuDifficulty.Normal);
			else if (difficulty == DTSudokuDifficultyValue.Hard)
				newBoard = generator.GenerateSudokuPuzzle(SudokuDifficulty.Hard);
			else
				throw new Exception();

			lock (this.locker)
			{
				this.newSudokuBoard = newBoard;
				this.isThreadDone = true;
			}
		}

		public bool hasFinishedLoading()
		{
			bool isThreadDone;

			lock (this.locker)
			{
				isThreadDone = this.isThreadDone;
			}

			return isThreadDone;
		}

		public int[,] getLoadedInitialBoard()
		{
			bool isThreadDone;
			int[,] newBoard;

			lock (this.locker)
			{
				isThreadDone = this.isThreadDone;
				newBoard = this.newSudokuBoard;
			}

			if (!isThreadDone)
				throw new Exception();

			return newBoard;
		}
	}
}
