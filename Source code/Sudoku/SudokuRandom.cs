
namespace Sudoku
{
	using System;

	public class SudokuRandom : ISudokuRandom
	{
		private Random random;

		public SudokuRandom()
		{
			this.random = new Random();
		}

		public int NextInt(int i)
		{
			return this.random.Next(i);
		}
	}
}

