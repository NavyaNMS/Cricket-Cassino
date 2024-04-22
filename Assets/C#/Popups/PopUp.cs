using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class PopUp : MonoBehaviour
{
    protected GameObject popUpDialog;
    public Action OnDialogShow = delegate { };
    public Action OnDialogHide = delegate { };
    public Action OnClickOk = delegate { };
    [SerializeField] protected Button hideDialog;
    private void Start()
    {
        gameObject.SetActive(false);
    }
    public bool IsVisible { get; set; }

    protected virtual void Show(bool showOK)
    {
        popUpDialog = this.gameObject;
        IsVisible = true;
        hideDialog.gameObject.SetActive(true);
        popUpDialog.SetActive(true);
        if (showOK) hideDialog.onClick.AddListener(Hide);
        OnDialogShow();
        OnDialogShow = () => { };
    }

    protected virtual void Hide()
    {
        IsVisible = false;
        popUpDialog.SetActive(false);
        hideDialog.gameObject.SetActive(false);
        hideDialog.onClick.RemoveListener(Hide);
        OnDialogHide();
        OnDialogHide = () => { };

    }
}
