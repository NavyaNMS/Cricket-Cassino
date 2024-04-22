using Com.BigWin.Frontend.Data;
using UnityEngine;

public class DataStorageController
{
    private LoginResponseData loginResponseData;

    public DataStorageController(Transform screenContainer)
    {
    }


    public string Email
    {
        get
        {
            return ReadData(Constant.EMAIL_DATA_KEY);
        }
    }

    public string Password
    {
        get
        {
            return ReadData(Constant.PASSWORD_DATA_KEY);
        }
    }

    public LoginResponseData LoginResponseData
    {
        get
        {
            return loginResponseData;
        }
        set
        {
            loginResponseData = value;
        }
    }

    public void SaveCredentials(string email, string password)
    {
        if (!string.IsNullOrWhiteSpace(email))
            SaveData(email, Constant.EMAIL_DATA_KEY);

        if (!string.IsNullOrWhiteSpace(password))
            SaveData(password, Constant.PASSWORD_DATA_KEY);
    }

    public void DeleteCredentials()
    {
        DeleteData(Constant.EMAIL_DATA_KEY);
        DeleteData(Constant.PASSWORD_DATA_KEY);
    }

    public void SaveData(string obj, string fileName)
    {
        PlayerPrefs.SetString(fileName, obj);
        PlayerPrefs.Save();
    }

    public string ReadData(string keyName)
    {
        if (PlayerPrefs.HasKey(keyName))
            return PlayerPrefs.GetString(keyName);
        return null;
    }

    public void DeleteData(string keyName)
    {
        PlayerPrefs.DeleteKey(keyName);
    }
}