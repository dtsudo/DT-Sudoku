
namespace DTSudokuLib
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using DTLib;

	public enum DTSudokuImage
	{
		BlueOne,
		BlueTwo,
		BlueThree,
		BlueFour,
		BlueFive,
		BlueSix,
		BlueSeven,
		BlueEight,
		BlueNine,

		RedOne,
		RedTwo,
		RedThree,
		RedFour,
		RedFive,
		RedSix,
		RedSeven,
		RedEight,
		RedNine,

		BlackOne,
		BlackTwo,
		BlackThree,
		BlackFour,
		BlackFive,
		BlackSix,
		BlackSeven,
		BlackEight,
		BlackNine,

		HighlightedCell,
		SolvedText,
		DifficultyText,
		EasyText,
		NormalText,
		HardText,
		HighlightedDifficulty,
		NewGameText,
		HighlightedNewGame,
		LoadingText
	}

	public interface IDTSudokuAssets : IAssets
	{
		void DrawImage(DTSudokuImage image, int x, int y);
	}
}
