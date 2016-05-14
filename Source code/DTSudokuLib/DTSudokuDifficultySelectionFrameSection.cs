
namespace DTSudokuLib
{
	using System;

	using DTLib;

	public class DTSudokuDifficultySelectionFrameSection
	{
		private DTSudokuDifficultyValue currentlySelectedDifficultyValue;

		public DTSudokuDifficultySelectionFrameSection(DTSudokuDifficultyValue initialDifficulty)
		{
			this.currentlySelectedDifficultyValue = initialDifficulty;
		}

		public DTSudokuDifficultyValue GetSelectedDifficulty()
		{
			return this.currentlySelectedDifficultyValue;
		}

		public void ProcessInputs(IKeyboard keyboardInput, IMouse mouseInput, IKeyboard previousKeyboardInput, IMouse previousMouseInput)
		{
			int x = mouseInput.GetX();
			int y = mouseInput.GetY();

			if (mouseInput.IsLeftMouseButtonPressed() && !previousMouseInput.IsLeftMouseButtonPressed())
			{
				if (y >= 0 && y <= 50)
				{
					if (x >= 150 && x < 250)
						this.currentlySelectedDifficultyValue = DTSudokuDifficultyValue.Easy;
					if (x >= 250 && x < 350)
						this.currentlySelectedDifficultyValue = DTSudokuDifficultyValue.Normal;
					if (x >= 350 && x < 450)
						this.currentlySelectedDifficultyValue = DTSudokuDifficultyValue.Hard;
				}
			}
		}

		public void Render(TranslatedDTSudokuDisplay display)
		{
			DTColor black = new DTColor(0, 0, 0);

			display.DrawImage(DTSudokuImage.DifficultyText, 0, 0);

			display.DrawImage(DTSudokuImage.EasyText, 151, 1);
			display.DrawImage(DTSudokuImage.NormalText, 251, 1);
			display.DrawImage(DTSudokuImage.HardText, 351, 1);

			if (this.currentlySelectedDifficultyValue == DTSudokuDifficultyValue.Easy)
				display.DrawImage(DTSudokuImage.HighlightedDifficulty, 151, 1);
			else if (this.currentlySelectedDifficultyValue == DTSudokuDifficultyValue.Normal)
				display.DrawImage(DTSudokuImage.HighlightedDifficulty, 251, 1);
			else if (this.currentlySelectedDifficultyValue == DTSudokuDifficultyValue.Hard)
				display.DrawImage(DTSudokuImage.HighlightedDifficulty, 351, 1);
			else
				throw new Exception();

            display.DrawRectangle(150, 0, 101, 51, black, false);
            display.DrawRectangle(250, 0, 101, 51, black, false);
            display.DrawRectangle(350, 0, 101, 51, black, false);
		}
	}
}

