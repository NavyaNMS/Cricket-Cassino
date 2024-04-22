
using System;
using System.Collections;
using UnityEngine;

public abstract class Screen : MonoBehaviour
{
    protected GameObject screenObj;
    protected ScreenController screenController;
    protected CommonPopup commonPopup;
    protected bool isActive;//check the screen if currently active

    public virtual void Initialize(ScreenController screenController)
    {
        this.screenController = screenController;
        commonPopup = screenController.commonPopup;
        screenObj = this.gameObject;
    }

    public abstract ScreenName ScreenID { get; }

    public virtual void Show(object data=null)
    {
        ActivateScreen(true);
        screenObj.transform.SetAsLastSibling();
    }

    public virtual void Back()
    {
        screenController.Back();
    }

    public virtual void Hide()
    {
        ActivateScreen(false);
    }

    public virtual void ActivateScreen(bool state)
    {
        screenObj.SetActive(state);
    }

    protected virtual IEnumerator CallFuntionWithDelay(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);
        action();
    }

    public Action OnStartGame;
}
