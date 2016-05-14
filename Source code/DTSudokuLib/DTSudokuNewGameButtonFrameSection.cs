
namespace DTSudokuLib
{
	using DTLib;

	public class DTSudokuNewGameButtonFrameSection
	{
		private bool isHoveringOverNewGame;
		private bool hasStartedNewGame;

		public DTSudokuNewGameButtonFrameSection()
		{
			this.isHoveringOverNewGame = false;
			this.hasStartedNewGame = false;
		}

		public bool HasStartedNewGame()
		{
			return this.hasStartedNewGame;
		}

		public void ProcessInputs(IKeyboard keyboardInput, IMouse mouseInput, IKeyboard previousKeyboardInput, IMouse previousMouseInput)
		{
			int x = mouseInput.GetX();
			int y = mouseInput.GetY();

			this.isHoveringOverNewGame = x >= 0
				&& x <= 250
				&& y >= 0
				&& y <= 50;

			this.hasStartedNewGame = this.isHoveringOverNewGame
				&& mouseInput.IsLeftMouseButtonPressed()
				&& !previousMouseInput.IsLeftMouseButtonPressed();
		}

		public void Render(TranslatedDTSudokuDisplay display)
		{
			DTColor black = new DTColor(0, 0, 0);

			display.DrawImage(DTSudokuImage.NewGameText, 1, 1);

			if (this.isHoveringOverNewGame)
				display.DrawImage(DTSudokuImage.HighlightedNewGame, 1, 1);

            display.DrawRectangle(0, 0, 251, 51, black, false);
		}
	}
}
