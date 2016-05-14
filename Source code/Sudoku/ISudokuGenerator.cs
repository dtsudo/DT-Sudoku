
namespace Sudoku
{
	public enum SudokuDifficulty
	{
		Easy,
		Normal,
		Hard
	}

	public interface ISudokuGenerator
	{
		int[,] GenerateSudokuPuzzle(SudokuDifficulty difficulty);
	}
}

