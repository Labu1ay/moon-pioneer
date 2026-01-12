using System;
using Cysharp.Threading.Tasks;

namespace MoonPioneer.Core.Services.CurtainProvider
{
  public interface ILoadingCurtainProvider
  {
    UniTaskVoid Show(float fadeDuration = 1f, Action action = null);
    void Hide(float duration = 1f, Action action = null);
  }
}