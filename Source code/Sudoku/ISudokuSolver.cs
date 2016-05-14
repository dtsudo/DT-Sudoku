
namespace Sudoku
{
	public interface ISudokuSolver
	{
		/// <summary>
		/// Given a board that has one or more solutions, returns one of the
		/// solutions.
		/// 
		/// If the board has more than one solution, there are no guarantees
		/// on which solution will be returned.
		/// 
		/// Undefined behavior if the board does not have at least one solution.
		/// </summary>
		int[,] SolveForFirstSolution(int[,] board);

		bool HasAtLeastOneSolution(int[,] board);

		bool HasExactlyOneSolution(int[,] board);
	}
}
