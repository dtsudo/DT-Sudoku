
namespace DTSudokuLib
{
	using System;

	using DTLib;
	using Sudoku;

	public class DTSudokuFrame : IFrame<IDTSudokuAssets>
	{
		private DTSudokuLoadingBoardFrameSection.IThreadSafeSudokuRandom random;

		private DTSudokuBoardFrameSection sudokuBoardFrameSection;
		private DTSudokuDifficultySelectionFrameSection sudokuDifficultySelectionFrameSection;
		private DTSudokuNewGameButtonFrameSection sudokuNewGameButtonFrameSection;
		private DTSudokuLoadingBoardFrameSection sudokuLoadingBoardFrameSection;

		public DTSudokuFrame(DTSudokuDifficultyValue initialDifficulty)
		{
			this.random = DTSudokuLoadingBoardFrameSection.GetIThreadSafeSudokuRandomInstance();

			this.sudokuBoardFrameSection = null;
			this.sudokuDifficultySelectionFrameSection = new DTSudokuDifficultySelectionFrameSection(initialDifficulty);
			this.sudokuNewGameButtonFrameSection = new DTSudokuNewGameButtonFrameSection();
			this.sudokuLoadingBoardFrameSection = new DTSudokuLoadingBoardFrameSection(initialDifficulty, this.random);
		}

		public IFrame<IDTSudokuAssets> GetNextFrame(IKeyboard keyboardInput, IMouse mouseInput, IKeyboard previousKeyboardInput, IMouse previousMouseInput)
		{
			if (this.sudokuLoadingBoardFrameSection == null)
			{
				IMouse translatedMouseInput = new TranslatedMouse(mouseInput, -25, -25);
				IMouse translatedPreviousMouseInput = new TranslatedMouse(previousMouseInput, -25, -25);

				this.sudokuBoardFrameSection.ProcessInputs(keyboardInput, translatedMouseInput, previousKeyboardInput, translatedPreviousMouseInput);

				IMouse translatedMouseInput2 = new TranslatedMouse(mouseInput, -25, -555);
				IMouse translatedPreviousMouseInput2 = new TranslatedMouse(previousMouseInput, -25, -555);
				this.sudokuDifficultySelectionFrameSection.ProcessInputs(keyboardInput, translatedMouseInput2, previousKeyboardInput, translatedPreviousMouseInput2);

				IMouse translatedMouseInput3 = new TranslatedMouse(mouseInput, -125, -620);
				IMouse translatedPreviousMouseInput3 = new TranslatedMouse(previousMouseInput, -125, -620);
				this.sudokuNewGameButtonFrameSection.ProcessInputs(keyboardInput, translatedMouseInput3, previousKeyboardInput, translatedPreviousMouseInput3);

				if (this.sudokuNewGameButtonFrameSection.HasStartedNewGame())
				{
					DTSudokuDifficultyValue difficulty = this.sudokuDifficultySelectionFrameSection.GetSelectedDifficulty();

					this.sudokuLoadingBoardFrameSection = new DTSudokuLoadingBoardFrameSection(difficulty, this.random);
				}
			}
			else
			{
				if (this.sudokuLoadingBoardFrameSection.hasFinishedLoading())
				{
					int[,] newBoard = this.sudokuLoadingBoardFrameSection.getLoadedInitialBoard();
					this.sudokuBoardFrameSection = new DTSudokuBoardFrameSection(newBoard);
					this.sudokuLoadingBoardFrameSection = null;
				}
			}

			return this;
		}

		public void Render(IDisplay<IDTSudokuAssets> display)
		{
			if (this.sudokuBoardFrameSection != null)
			{
				TranslatedDTSudokuDisplay translatedDisplay = new TranslatedDTSudokuDisplay(display, 25, 25);
				this.sudokuBoardFrameSection.Render(translatedDisplay);

				if (this.sudokuBoardFrameSection.IsSolved())
				{
					display.GetAssets().DrawImage(DTSudokuImage.SolvedText, 25, 25 + 450 + 15);
				}
			}

			TranslatedDTSudokuDisplay translatedDisplay2 = new TranslatedDTSudokuDisplay(display, 25, 555);
			this.sudokuDifficultySelectionFrameSection.Render(translatedDisplay2);

			TranslatedDTSudokuDisplay translatedDisplay3 = new TranslatedDTSudokuDisplay(display, 125, 620);
			this.sudokuNewGameButtonFrameSection.Render(translatedDisplay3);

			if (this.sudokuLoadingBoardFrameSection != null)
			{
				DTColor semiTransparentGray = new DTColor(0, 0, 0, 100);

				display.DrawRectangle(25, 25, 450, 450, semiTransparentGray, true);
				display.DrawRectangle(25, 555, 451, 51, semiTransparentGray, true);
				display.DrawRectangle(125, 620, 251, 51, semiTransparentGray, true);
				display.GetAssets().DrawImage(DTSudokuImage.LoadingText, 25, 25);
			}
		}
	}
}
