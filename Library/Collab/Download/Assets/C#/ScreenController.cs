using System.Collections.Generic;
using UnityEngine;

public class ScreenController : MonoBehaviour
{
    private Stack<ScreenName> screensStack;
    private ScreenName lastSubScreenID, currentSubScreenId;
    private ScreenName currentScreenId, lastScreenId;
    private Dictionary<ScreenName, Screen> screensCollection;
    private Transform screensContainer;
    public DataStorageController dataStorageController;
    public CommonPopup commonPopup;

    public void Start()
    {
        commonPopup.gameObject.SetActive(true);
        screensStack = new Stack<ScreenName>();
        lastSubScreenID = ScreenName.NONE;
        lastSubScreenID = ScreenName.NONE;
        screensCollection = new Dictionary<ScreenName, Screen>();
        dataStorageController = new DataStorageController(screensContainer);
        LoadScreens();
        ShowLoginScreen();
        //Show(ScreenName.historyScreen);
    }

    private void LoadScreens()
    {
        Screen[] s = this.GetComponentsInChildren<Screen>(true);
        foreach (Screen screen in s)
        {
            screen.Initialize(this);
            screen.gameObject.SetActive(true);
            screensCollection.Add(screen.ScreenID, screen);
            screen.Hide();
        }

    }

    public void ShowLoginScreen() => Show(ScreenName.loginScreen);
    public void ShowHomeScreen() => Show(ScreenName.homeScreen);
    public void ShowBettingScreen(object gameId=null) => Show(ScreenName.bettingScreen,data:gameId);
    public void ShowHistoryScreen(object gameId=null) => Show(ScreenName.historyScreen, data: gameId);
    public void ShowReportScreen(object gameId = null) => Show(ScreenName.reportScreen, data: gameId);
    public void ShowReprinttScreen(object gameId = null) => Show(ScreenName.reprintScreen,data:gameId);
    private void PrintAllScreens()
    {
        foreach (KeyValuePair<ScreenName, Screen> pair in screensCollection)
            Debug.Log(pair.Key + " , " + pair.Value);
    }

    public void Show(ScreenName id, bool ShowAsSubScreen = false,object data=null)
    {
        if (ShowAsSubScreen)
        {
            lastSubScreenID = currentSubScreenId;
            currentSubScreenId = id;
        }
        else
        {
            lastScreenId = currentScreenId;
            currentScreenId = id;
        }
        Push(id, ShowAsSubScreen);
        Hide(ShowAsSubScreen);
        screensCollection[id].Show(data);
    }

    public ScreenName Hide(bool subScreen = false)
    {
        if (!subScreen)
        {
            // Normal screens
            if (screensStack.Count > 0)
            {
                ScreenName screenID = Pop();
                screensCollection[lastScreenId].Hide();
                return screenID;
            }
        }
        else
        {
            // Child Screens under Screens
            if (lastSubScreenID != ScreenName.NONE)
            {
                screensCollection[lastSubScreenID].Hide();
                lastSubScreenID = ScreenName.NONE;
            }
        }
        return ScreenName.NONE;
    }

    public void Back()
    {
        ScreenName currentScreenID = Hide();
        ScreenName lastScreenID = currentScreenID - 1;
        Push(lastScreenID);
        screensCollection[lastScreenID].ActivateScreen(true);
    }

    private void Push(ScreenName screenID, bool subScreen = false)
    {
        if (!subScreen) screensStack.Push(screenID);
        else lastScreenId = screenID;
    }

    private ScreenName Pop()
    {
        return screensStack.Pop();
    }

    public ScreenName Peek()
    {
        return screensStack.Peek();
    }
}

public enum ScreenName
{
    loginScreen,
    accountScreen,
    homeScreen,
    NONE,
    bettingScreen,
    reportScreen,
    historyScreen,
    reprintScreen,
    criketScreen,
    RetailerReport,
    FootballScreen,
    HockeyScreen
}