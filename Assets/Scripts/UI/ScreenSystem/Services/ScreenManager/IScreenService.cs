namespace MoonPioneer.UI.ScreenSystem.Services.ScreenManager
{
  public interface IScreenService
  {
    void ShowScreen<T>() where T : Screen;
    void HideScreen<T>() where T : Screen;
    void DestroyScreen<T>() where T : Screen;
  }
}