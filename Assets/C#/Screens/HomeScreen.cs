using UnityEngine;
using UnityEngine.UI;

public class HomeScreen : Screen
{
    [SerializeField] private Button cricketBtn;
    [SerializeField] private Button hockeyBtn;
    [SerializeField] private Button footballBtn;

    public override ScreenName ScreenID => ScreenName.homeScreen;

    public override void Back()
    {
        base.Back();
    }

    public override void Hide()
    {
        base.Hide();
    }

    public override void Initialize(ScreenController screenController)
    {
        base.Initialize(screenController);
        cricketBtn.onClick.AddListener(() => screenController.Show(ScreenName.criketScreen));
        footballBtn.onClick.AddListener(() => screenController.ShowBettingScreen(2));
        hockeyBtn.onClick.AddListener(() => screenController.ShowBettingScreen(3));
    }

    public override void Show(object data = null)
    {
        base.Show();
    }
}
