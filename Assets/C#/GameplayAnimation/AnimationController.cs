using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] Transform animationParent;
    Dictionary<AnimationWindow, GameIds> windows = new Dictionary<AnimationWindow, GameIds>();
    AnimationWindow currentWindow;
    private void Start()
    {
        foreach (Transform animWindow in animationParent)
        {
            AnimationWindow window = animWindow?.GetComponent<AnimationWindow>();
            if (window == null) continue;
            windows.Add(window, window.id);
            window.CloseWindow();

        }
    }
    public void SetWindow(int gameId)
    {
        foreach (var w in windows)
        {
            if ((int)w.Value == gameId)
            {
                currentWindow?.CloseWindow();
                currentWindow = w.Key;
                
                w.Key.OpenWindow();
                break;
            }
        }
    }
    public void PlayAnimation(int gameId)
    {
        currentWindow.Play(gameId);
    }
}
public enum GameIds
{
    cricket = 1,
    football = 2,
    hockey = 3
}