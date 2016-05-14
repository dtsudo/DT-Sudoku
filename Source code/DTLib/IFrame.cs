
namespace DTLib
{
	public interface IFrame<T> where T : IAssets
	{
		IFrame<T> GetNextFrame(IKeyboard keyboardInput, IMouse mouseInput, IKeyboard previousKeyboardInput, IMouse previousMouseInput);
		void Render(IDisplay<T> display);
	}
}
