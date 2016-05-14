
namespace DTSudoku
{
	using DTLib;
	using AgateLib;
	using AgateLib.DisplayLib;
	using AgateLib.Geometry;
	using DTSudokuLib;

	public class Initializer
	{
		public static void Start()
		{
			using (AgateSetup setup = new AgateSetup())
			{
				setup.InitializeAll();

				if (setup.WasCanceled)
					return;

				DisplayWindow window = DisplayWindow.CreateWindowed("DT Sudoku", 500, 700);

				IFrame<IDTSudokuAssets> frame = DTSudoku.GetFirstFrame();

				IKeyboard agateLibKeyboard = new AgateLibKeyboard();
				IMouse agateLibMouse = new AgateLibMouse();
				IDisplay<IDTSudokuAssets> display = new DTSudokuAgateLibDisplay();
				IKeyboard prevKeyboard = new EmptyKeyboard();
				IMouse prevMouse = new EmptyMouse();
				
				double elapsedTimeMs = 0.0;

				while (Display.CurrentWindow.IsClosed == false)
				{
					Display.BeginFrame();

					Display.Clear(Color.White);

				    frame.Render(display);

					elapsedTimeMs += Display.DeltaTime;

					// Run at 60 frames per second.

					// If for whatever reason, we're really behind, we'll try to catch up,
					// but only for a maximum of 5 consecutive frames.
					if (elapsedTimeMs > 1000.0 / 60.0 * 5)
						elapsedTimeMs = 1000.0 / 60.0 * 5;

					if (elapsedTimeMs > 1000.0 / 60.0)
					{
						elapsedTimeMs = elapsedTimeMs - 1000.0 / 60.0;
						IKeyboard currentKeyboard = new CopiedKeyboard(agateLibKeyboard);
						IMouse currentMouse = new CopiedMouse(agateLibMouse);
						frame = frame.GetNextFrame(currentKeyboard, currentMouse, prevKeyboard, prevMouse);
						prevKeyboard = new CopiedKeyboard(currentKeyboard);
						prevMouse = new CopiedMouse(currentMouse);
					}

					Display.EndFrame();

					Core.KeepAlive();
				}
			}
		}
	}
}
