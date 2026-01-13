using Cysharp.Threading.Tasks;

namespace MoonPioneer.Game.Tooltip.Services.TooltipSystem
{
  public interface ITooltipService
  {
    UniTask TryShowTooltip(string tooltipText, float delaySeconds = 5f);
    UniTask TryShowTemporaryTooltip(string tooltipText, float delaySeconds = 0f, float durationSeconds = 1f);
    void HideTooltip();
  }
}