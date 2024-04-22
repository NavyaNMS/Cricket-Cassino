using Com.BigWin.Frontend.Data;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Com.BigWin.Frontend
{
    public class LoginScreen : Screen
    {
        [SerializeField] private TMP_InputField userNameInput;
        [SerializeField] private TMP_InputField passwordInput;
        [SerializeField] private Toggle rememberPasswordToggle;
        [SerializeField] private Button loginBtn;
        [SerializeField] private Button exitBtn;

        public override void Initialize(ScreenController screenController)
        {
            base.Initialize(screenController);
            AddListeners();
        }

        private void AddListeners()
        {
            loginBtn.onClick.AddListener(OnClickLoginButton);
            exitBtn.onClick.AddListener(Application.Quit);
            passwordInput.onSubmit.AddListener((vslue)=> { OnClickLoginButton(); });
        }

        public override void Show(object data = null)
        {
            base.Show();
            userNameInput.text = screenController.dataStorageController.Email?.Replace("Fun", string.Empty);
            passwordInput.text = screenController.dataStorageController.Password;
        }

        public override ScreenName ScreenID => ScreenName.loginScreen;


        private void OnClickLoginButton()
        {
            if (userNameInput.text.Trim() == "" && passwordInput.text.Trim() == "")
            {
                print("Emplty Field");
                commonPopup.Show("Empty field");
                return;
            }
            LoginForm form = new LoginForm(userNameInput.text.Trim(), passwordInput.text.Trim());
            WebRequestHandler.instance.Post(Constant.LOGIN_URL, JsonUtility.ToJson(form), OnLoginRequestProcessed);
        }

        private void OnLoginRequestProcessed(string json, bool success)
        {
            LoginResponseData loginResponseData = JsonUtility.FromJson<LoginResponseData>(json);
            if (loginResponseData.status == 401)
            {

                commonPopup.Show(loginResponseData.message);

                return;
            }
            if (loginResponseData.status == 200)
            {
                if (rememberPasswordToggle.isOn)
                {
                    screenController.dataStorageController.SaveCredentials("Fun" + userNameInput.text, passwordInput.text);
                }
                else
                {
                    screenController.dataStorageController.DeleteCredentials();
                }

                screenController.dataStorageController.LoginResponseData = loginResponseData;
                screenController.ShowHomeScreen();
                return;
            }
            else
            {
                if (loginResponseData.status == 202)
                {
                    commonPopup.Show(loginResponseData.message, okButtonMsg: "Force Login");

                }
            }

            commonPopup.Show(loginResponseData.message);
        }


    }
}
[Serializable]
public class ForceLogin
{
    public string user_id;
    public string password;
    public string device;
}
public class UserData
{
    public string message;
    public string status;
    public string coins;
    public string user_id;
    public string device;
}
[Serializable]
public class ForceLoginResponse
{
    public UserData user_data;
}

public class e
{
    public string message { get; set; }
    public string status { get; set; }
}
[Serializable]
public class Invalide
{
    public e user_data { get; set; }
}
