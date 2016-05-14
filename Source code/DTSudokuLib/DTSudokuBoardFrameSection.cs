
namespace DTSudokuLib
{
	using System;

	using DTLib;
	using Sudoku;

	/// <summary>
	/// Represents the actual sudoku board part of the game.
	/// (i.e. doesn't include the difficulty selector or other
	/// parts of the UI)
	/// </summary>
	public class DTSudokuBoardFrameSection
	{
		private const int WIDTH = 50;

		private int[,] initialBoard;
		private int[,] currentBoard;
		private int[,] solvedBoard;

		private Tuple<int, int> selectedCell;

		public DTSudokuBoardFrameSection(int[,] initialBoard)
		{
			ISudokuSolver solver = new RandomizedSudokuSolver();

			this.initialBoard = copyBoard(initialBoard);
			this.currentBoard = copyBoard(initialBoard);
			this.solvedBoard = copyBoard(solver.SolveForFirstSolution(initialBoard));

			this.selectedCell = null;
		}

		public void SetNewInitialBoard(int[,] initialBoard)
		{
			ISudokuSolver solver = new RandomizedSudokuSolver();

			this.initialBoard = copyBoard(initialBoard);
			this.currentBoard = copyBoard(initialBoard);
			this.solvedBoard = copyBoard(solver.SolveForFirstSolution(initialBoard));

			this.selectedCell = null;
		}

		public bool IsSolved()
		{
			for (int i = 0; i < 9; i++)
			{
				for (int j = 0; j < 9; j++)
				{
					if (this.currentBoard[i, j] == 0)
						return false;
					if (this.currentBoard[i, j] != this.solvedBoard[i, j])
						return false;
				}
			}

			return true;
		}

		public void ProcessInputs(IKeyboard keyboardInput, IMouse mouseInput, IKeyboard previousKeyboardInput, IMouse previousMouseInput)
		{
			Func<Key, bool> isPressed = key => keyboardInput.IsPressed(key) && !previousKeyboardInput.IsPressed(key);

			if (this.selectedCell != null)
			{
				if (isPressed(Key.UpArrow) && this.selectedCell.Item2 > 0)
					this.selectedCell = new Tuple<int, int>(this.selectedCell.Item1, this.selectedCell.Item2 - 1);
				if (isPressed(Key.DownArrow) && this.selectedCell.Item2 < 8)
					this.selectedCell = new Tuple<int, int>(this.selectedCell.Item1, this.selectedCell.Item2 + 1);
				if (isPressed(Key.LeftArrow) && this.selectedCell.Item1 > 0)
					this.selectedCell = new Tuple<int, int>(this.selectedCell.Item1 - 1, this.selectedCell.Item2);
				if (isPressed(Key.RightArrow) && this.selectedCell.Item1 < 8)
					this.selectedCell = new Tuple<int, int>(this.selectedCell.Item1 + 1, this.selectedCell.Item2);
			}

			if (mouseInput.IsLeftMouseButtonPressed() && !previousMouseInput.IsLeftMouseButtonPressed())
			{
				int x = mouseInput.GetX() / WIDTH;
				int y = mouseInput.GetY() / WIDTH;

				if (x >= 0 && x < 9 && y >= 0 && y < 9 && mouseInput.GetX() >= 0 && mouseInput.GetY() >= 0)
					this.selectedCell = new Tuple<int, int>(x, y);
				else
					this.selectedCell = null;
			}

			if (this.selectedCell != null)
			{
				int x = this.selectedCell.Item1;
				int y = this.selectedCell.Item2;

				if (this.initialBoard[x, y] == 0)
				{
					if (isPressed(Key.One))
						this.currentBoard[x, y] = 1;
					if (isPressed(Key.Two))
						this.currentBoard[x, y] = 2;
					if (isPressed(Key.Three))
						this.currentBoard[x, y] = 3;
					if (isPressed(Key.Four))
						this.currentBoard[x, y] = 4;
					if (isPressed(Key.Five))
						this.currentBoard[x, y] = 5;
					if (isPressed(Key.Six))
						this.currentBoard[x, y] = 6;
					if (isPressed(Key.Seven))
						this.currentBoard[x, y] = 7;
					if (isPressed(Key.Eight))
						this.currentBoard[x, y] = 8;
					if (isPressed(Key.Nine))
						this.currentBoard[x, y] = 9;
					if (isPressed(Key.Zero) || isPressed(Key.Backspace) || isPressed(Key.Delete))
						this.currentBoard[x, y] = 0;
				}
			}
		}

		public void Render(TranslatedDTSudokuDisplay display)
		{
			DTColor black = new DTColor(0, 0, 0);

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (this.currentBoard[i, j] != 0)
                    {
                        CellStatus cellStatus = CellStatus.Given;
                        if (this.currentBoard[i, j] != this.initialBoard[i, j])
                            cellStatus = CellStatus.Correct;
                        if (this.currentBoard[i, j] != this.solvedBoard[i, j])
                            cellStatus = CellStatus.Wrong;
                        this.DrawNumber(this.currentBoard[i, j], cellStatus, WIDTH * i + 1, WIDTH * j + 1, display);
                    }
                }
            }

			if (this.selectedCell != null)
			{
				int x = this.selectedCell.Item1;
				int y = this.selectedCell.Item2;
				display.DrawImage(DTSudokuImage.HighlightedCell, WIDTH * x + 1, WIDTH * y + 1);
            }

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    display.DrawRectangle(WIDTH * i, WIDTH * j, WIDTH + 1, WIDTH + 1, black, false);
                }
            }

			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					int xOffset = i * WIDTH * 3;
					int yOffset = j * WIDTH * 3;

					display.DrawThickRectangle(xOffset, yOffset, WIDTH * 3 + 1, WIDTH * 3 + 1, 2, black, false);
				}
			}
		}

		private void DrawNumber(int num, CellStatus cellStatus, int x, int y, TranslatedDTSudokuDisplay display)
		{
			DTSudokuImage image;

			if (cellStatus == CellStatus.Given)
			{
				if (num == 1)
					image = DTSudokuImage.BlackOne;
				else if (num == 2)
					image = DTSudokuImage.BlackTwo;
				else if (num == 3)
					image = DTSudokuImage.BlackThree;
				else if (num == 4)
					image = DTSudokuImage.BlackFour;
				else if (num == 5)
					image = DTSudokuImage.BlackFive;
				else if (num == 6)
					image = DTSudokuImage.BlackSix;
				else if (num == 7)
					image = DTSudokuImage.BlackSeven;
				else if (num == 8)
					image = DTSudokuImage.BlackEight;
				else if (num == 9)
					image = DTSudokuImage.BlackNine;
				else
					throw new Exception();
			}
			else if (cellStatus == CellStatus.Correct)
			{
				if (num == 1)
					image = DTSudokuImage.BlueOne;
				else if (num == 2)
					image = DTSudokuImage.BlueTwo;
				else if (num == 3)
					image = DTSudokuImage.BlueThree;
				else if (num == 4)
					image = DTSudokuImage.BlueFour;
				else if (num == 5)
					image = DTSudokuImage.BlueFive;
				else if (num == 6)
					image = DTSudokuImage.BlueSix;
				else if (num == 7)
					image = DTSudokuImage.BlueSeven;
				else if (num == 8)
					image = DTSudokuImage.BlueEight;
				else if (num == 9)
					image = DTSudokuImage.BlueNine;
				else
					throw new Exception();
			}
			else if (cellStatus == CellStatus.Wrong)
			{
				if (num == 1)
					image = DTSudokuImage.RedOne;
				else if (num == 2)
					image = DTSudokuImage.RedTwo;
				else if (num == 3)
					image = DTSudokuImage.RedThree;
				else if (num == 4)
					image = DTSudokuImage.RedFour;
				else if (num == 5)
					image = DTSudokuImage.RedFive;
				else if (num == 6)
					image = DTSudokuImage.RedSix;
				else if (num == 7)
					image = DTSudokuImage.RedSeven;
				else if (num == 8)
					image = DTSudokuImage.RedEight;
				else if (num == 9)
					image = DTSudokuImage.RedNine;
				else
					throw new Exception();
			}
			else
				throw new Exception();

			display.DrawImage(image, x, y);
		}

		private int[,] copyBoard(int[,] board)
		{
			int[,] newBoard = new int[9, 9];
			for (int i = 0; i < 9; i++)
				for (int j = 0; j < 9; j++)
					newBoard[i, j] = board[i, j];

			return newBoard;
		}

		private enum CellStatus
		{
			Given,
			Correct,
			Wrong
		}
	}
}
