

namespace DTSudokuLib
{
	using DTLib;

	public class TranslatedDTSudokuDisplay
	{
		private IDisplay<IDTSudokuAssets> display;
		private int xOffset;
		private int yOffset;

		public TranslatedDTSudokuDisplay(IDisplay<IDTSudokuAssets> display, int xOffset, int yOffset)
		{
			this.display = display;
			this.xOffset = xOffset;
			this.yOffset = yOffset;
		}

		public void DrawRectangle(int x, int y, int width, int height, DTColor color, bool fill)
		{
			this.display.DrawRectangle(x + this.xOffset, y + this.yOffset, width, height, color, fill);
		}

		public void DrawThickRectangle(int x, int y, int width, int height, int additionalThickness, DTColor color, bool fill)
		{
			this.display.DrawThickRectangle(x + this.xOffset, y + this.yOffset, width, height, additionalThickness, color, fill);
		}

		public void DrawImage(DTSudokuImage image, int x, int y)
		{
			this.display.GetAssets().DrawImage(image, x + this.xOffset, y + this.yOffset);
		}
	}
}

