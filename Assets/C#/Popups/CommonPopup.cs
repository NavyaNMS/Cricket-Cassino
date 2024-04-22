using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CommonPopup : PopUp
{
    [SerializeField] private Button okButton;
    [SerializeField] private TextMeshProUGUI okButtonText;
    [SerializeField] private TextMeshProUGUI messageTextUI;

    private void Start()
    {
        okButton.onClick.AddListener(() => OnClickOk());
        this.gameObject.SetActive(false);
    }

    public void Show(string msg= "SomeThing went wrong", bool showOK = true, string okButtonMsg = "OK")
    {
        base.Show(showOK);
        messageTextUI.text = msg;
        this.okButtonText.text = okButtonMsg;
        okButton.gameObject.SetActive(showOK);
        okButton.onClick.AddListener(Hide);
    }

    public new void Hide()
    {
        base.Hide();
    }
}
