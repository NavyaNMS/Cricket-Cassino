using UnityEngine;
abstract public class AnimationWindow:MonoBehaviour
{
    abstract public GameIds id { get; }
    public void CloseWindow()
    {
        this.gameObject.SetActive(false);
    }
    public void OpenWindow()
    {
        this.gameObject.SetActive(true);
    }
    virtual public void Play(int winNo)
    {

    }
}
