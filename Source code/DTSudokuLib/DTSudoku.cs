
namespace DTSudokuLib
{
	using DTLib;
	using Sudoku;

	public class DTSudoku
	{
		public static IFrame<IDTSudokuAssets> GetFirstFrame()
		{
			// The sudoku game starts at normal difficulty as an arbitrarily chosen default.
			var frame = new DTSudokuFrame(DTSudokuDifficultyValue.Normal);
			return frame;
		}
	}
}
