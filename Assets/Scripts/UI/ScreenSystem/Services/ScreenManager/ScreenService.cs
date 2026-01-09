using MoonPioneer.UI.ScreenSystem.Factory;

namespace MoonPioneer.UI.ScreenSystem.Services.ScreenManager
{
  public class ScreenService : IScreenService
  {
    private readonly IScreenFactory _screenFactory;

    public ScreenService(IScreenFactory screenFactory)
    {
      _screenFactory = screenFactory;
    }

    public void ShowScreen<T>() where T : Screen
    {
      var screen = _screenFactory.GetOrCreate<T>();
      screen.gameObject.SetActive(true);
    }

    public void HideScreen<T>() where T : Screen
    {
      var screen = _screenFactory.GetOrCreate<T>();
      screen.gameObject.SetActive(false);
    }

    public void DestroyScreen<T>() where T : Screen => 
      _screenFactory.Destroy<T>();
  }
}