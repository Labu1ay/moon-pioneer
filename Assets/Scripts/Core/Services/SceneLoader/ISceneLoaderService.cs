using System;

namespace MoonPioneer.Core.Services.SceneLoader
{
  public interface ISceneLoaderService {
    string ActiveSceneName { get; }
    void Load(string name, Action callback = null);
  }
}