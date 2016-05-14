
namespace DTSudoku
{
	using DTLib;
	using AgateLib.DisplayLib;
	using AgateLib.Geometry;
	using DTSudokuLib;
	using System;
	using System.IO;

	public abstract class AgateLibDisplay<T> : IDisplay<T> where T : IAssets
	{
		public AgateLibDisplay()
		{
		}

		public void DrawRectangle(int x, int y, int width, int height, DTColor color, bool fill)
		{
			Color agateLibColor = Color.FromArgb(color.Alpha, color.R, color.G, color.B);

			if (fill)
			{
				Display.FillRect(x, y, width, height, agateLibColor);
			}
			else
			{
				// Unclear why the "- 1" are necessary, but it seems to make the display render better.
				Display.DrawRect(x, y, width - 1, height - 1, agateLibColor);
			}
		}

		public abstract T GetAssets();
	}

	/// <summary>
	/// Requires the image files to be present in the Data/Images folder
	/// </summary>
	public class DTSudokuAgateLibDisplay : AgateLibDisplay<IDTSudokuAssets>
	{
		private DTSudokuAssets assets;

		private class DTSudokuAssets : IDTSudokuAssets
		{
			private string getNum(int i)
			{
				switch (i)
				{
					case 1: return "one";
					case 2: return "two";
					case 3: return "three";
					case 4: return "four";
					case 5: return "five";
					case 6: return "six";
					case 7: return "seven";
					case 8: return "eight";
					case 9: return "nine";
					default: throw new Exception();
				}
			}

			private Surface[] blueArray;
			private Surface[] redArray;
			private Surface[] blackArray;
			private Surface highlightedCell;
			private Surface solvedText;
			private Surface difficulty;
			private Surface easy;
			private Surface normal;
			private Surface hard;
			private Surface highlightedDifficulty;
			private Surface newGame;
			private Surface highlightedNewGame;
			private Surface loading;

			private static string GetImagesDirectory()
			{
				string path = Util.GetExecutablePath();

				// The images are expected to be in the /Data/Images/ folder
				// relative to the executable.
				if (Directory.Exists(path + "/Data/Images"))
					return path + "/Data/Images" + "/";

				// However, if the folder doesn't exist, search for the /Data/Images folder
				// using some heuristic.
				while (true)
				{
					int i = Math.Max(path.LastIndexOf("/", StringComparison.Ordinal), path.LastIndexOf("\\", StringComparison.Ordinal));

					if (i == -1)
						throw new Exception("Cannot find images directory");

					path = path.Substring(0, i);

					if (Directory.Exists(path + "/Data/Images"))
						return path + "/Data/Images" + "/";
				}
			}

			public DTSudokuAssets()
			{
				this.blueArray = new Surface[9];
				for (int i = 0; i < 9; i++)
					this.blueArray[i] = new Surface(GetImagesDirectory() + "blue" + getNum(i + 1) + ".png");
				this.redArray = new Surface[9];
				for (int i = 0; i < 9; i++)
					this.redArray[i] = new Surface(GetImagesDirectory() + "red" + getNum(i + 1) + ".png");
				this.blackArray = new Surface[9];
				for (int i = 0; i < 9; i++)
					this.blackArray[i] = new Surface(GetImagesDirectory() + "black" + getNum(i + 1) + ".png");

				this.highlightedCell = new Surface(GetImagesDirectory() + "highlightedcell.png");
				this.solvedText = new Surface(GetImagesDirectory() + "solvedtext.png");
				this.difficulty = new Surface(GetImagesDirectory() + "difficulty.png");
				this.easy = new Surface(GetImagesDirectory() + "easy.png");
				this.normal = new Surface(GetImagesDirectory() + "normal.png");
				this.hard = new Surface(GetImagesDirectory() + "hard.png");
				this.highlightedDifficulty = new Surface(GetImagesDirectory() + "highlighteddifficulty.png");
				this.newGame = new Surface(GetImagesDirectory() + "newgame.png");
				this.highlightedNewGame = new Surface(GetImagesDirectory() + "highlightednewgame.png");
				this.loading = new Surface(GetImagesDirectory() + "loading.png");
			}

			public void DrawImage(DTSudokuImage image, int x, int y)
			{
				Surface surface;
				if (image == DTSudokuImage.BlueOne) surface = this.blueArray[0];
				else if (image == DTSudokuImage.BlueTwo) surface = this.blueArray[1];
				else if (image == DTSudokuImage.BlueThree) surface = this.blueArray[2];
				else if (image == DTSudokuImage.BlueFour) surface = this.blueArray[3];
				else if (image == DTSudokuImage.BlueFive) surface = this.blueArray[4];
				else if (image == DTSudokuImage.BlueSix) surface = this.blueArray[5];
				else if (image == DTSudokuImage.BlueSeven) surface = this.blueArray[6];
				else if (image == DTSudokuImage.BlueEight) surface = this.blueArray[7];
				else if (image == DTSudokuImage.BlueNine) surface = this.blueArray[8];
				else if (image == DTSudokuImage.RedOne) surface = this.redArray[0];
				else if (image == DTSudokuImage.RedTwo) surface = this.redArray[1];
				else if (image == DTSudokuImage.RedThree) surface = this.redArray[2];
				else if (image == DTSudokuImage.RedFour) surface = this.redArray[3];
				else if (image == DTSudokuImage.RedFive) surface = this.redArray[4];
				else if (image == DTSudokuImage.RedSix) surface = this.redArray[5];
				else if (image == DTSudokuImage.RedSeven) surface = this.redArray[6];
				else if (image == DTSudokuImage.RedEight) surface = this.redArray[7];
				else if (image == DTSudokuImage.RedNine) surface = this.redArray[8];
				else if (image == DTSudokuImage.BlackOne) surface = this.blackArray[0];
				else if (image == DTSudokuImage.BlackTwo) surface = this.blackArray[1];
				else if (image == DTSudokuImage.BlackThree) surface = this.blackArray[2];
				else if (image == DTSudokuImage.BlackFour) surface = this.blackArray[3];
				else if (image == DTSudokuImage.BlackFive) surface = this.blackArray[4];
				else if (image == DTSudokuImage.BlackSix) surface = this.blackArray[5];
				else if (image == DTSudokuImage.BlackSeven) surface = this.blackArray[6];
				else if (image == DTSudokuImage.BlackEight) surface = this.blackArray[7];
				else if (image == DTSudokuImage.BlackNine) surface = this.blackArray[8];
				else if (image == DTSudokuImage.HighlightedCell)
					surface = this.highlightedCell;
				else if (image == DTSudokuImage.SolvedText)
					surface = this.solvedText;
				else if (image == DTSudokuImage.DifficultyText)
					surface = this.difficulty;
				else if (image == DTSudokuImage.EasyText)
					surface = this.easy;
				else if (image == DTSudokuImage.NormalText)
					surface = this.normal;
				else if (image == DTSudokuImage.HardText)
					surface = this.hard;
				else if (image == DTSudokuImage.HighlightedDifficulty)
					surface = this.highlightedDifficulty;
				else if (image == DTSudokuImage.NewGameText)
					surface = this.newGame;
				else if (image == DTSudokuImage.HighlightedNewGame)
					surface = this.highlightedNewGame;
				else if (image == DTSudokuImage.LoadingText)
					surface = this.loading;
				else
					throw new Exception();

				// Unclear why the "- 1" is necessary, but it seems to make the image render better.
				surface.Draw(x - 1, y);
			}
		}

		public DTSudokuAgateLibDisplay()
		{
			this.assets = new DTSudokuAssets();
		}

		public override IDTSudokuAssets GetAssets()
		{
			return this.assets;
		}
	}
}
