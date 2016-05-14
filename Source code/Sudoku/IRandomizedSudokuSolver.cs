
namespace Sudoku
{
	public interface IRandomizedSudokuSolver : ISudokuSolver
	{
		/// <summary>
		/// Given a sudoku board that has one or more solutions, returns
		/// a random solution.
		/// 
		/// The input board must have at least one solution.  (Undefined behavior
		/// if the input board does not have any solutions.)
		/// 
		/// If the input board has more than one solution, returns one of the
		/// solutions randomly.
		/// 
		/// Note that this method can be used to generate a random solved sudoku board
		/// if you pass in an empty 9x9 array as the input.
		/// </summary>
		int[,] SolveForRandomSolution(int[,] board, ISudokuRandom random);
	}
}
