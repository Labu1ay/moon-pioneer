using System;
using DG.Tweening;
using UnityEngine;

namespace MoonPioneer.UI
{
  public class LoadingCurtain : MonoBehaviour
  {
    [SerializeField] private CanvasGroup _curtain;

    private Tween _tween;

    private void Start() => DontDestroyOnLoad(this);

    public void Show(float fadeDuration, Action action = null)
    {
      gameObject.SetActive(true);
      Fade(1f, fadeDuration, action);
    }

    public void Hide(float fadeDuration, Action action = null)
    {
      Fade(0f, fadeDuration, () =>
      {
        gameObject.SetActive(false);
        action?.Invoke();
      });
    }

    private void Fade(float endValue, float fadeDuration, Action action = null)
    {
      _tween?.Kill();
      _tween = _curtain.DOFade(endValue, fadeDuration).OnComplete(() => action?.Invoke());
    }

    private void OnDestroy()
    {
      _tween?.Kill();
    }
  }
}