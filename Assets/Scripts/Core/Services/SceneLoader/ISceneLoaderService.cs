using System;
using Cysharp.Threading.Tasks;

namespace MoonPioneer.Core.Services.SceneLoader
{
  public interface ISceneLoaderService {
    string ActiveSceneName { get; }
    UniTaskVoid Load(string name, Action callback = null);
  }
}