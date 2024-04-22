
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Linq;
using System;
using System.Collections.Generic;

class HistoryScreen : Screen
{
    public int gameId = 2;

    [SerializeField] Button backBtn;
    [SerializeField] Button applyBtn;
    [SerializeField] Button fromBtn;
    [SerializeField] Button toBtn;
    [SerializeField] TMP_InputField[] toDate;
    [SerializeField] TMP_InputField[] fromDate;
    [SerializeField] TMP_Text msg;
    [SerializeField] TMP_Text fromTxt;
    [SerializeField] TMP_Text toTxt;

    [SerializeField] GameObject contenPerfab;
    [SerializeField] GameObject content;

    [SerializeField] Calendar fromCalandar;
    [SerializeField] Calendar toCalandar;


    public int to_date;
    public int toMonth;
    public int toYear;
    public int fromYear;
    public int fromMonth;
    public int from_date;
    public override ScreenName ScreenID => ScreenName.historyScreen;

    public override void Hide()
    {
        base.Hide();
        print("1 chile count " + content.transform.childCount);
        if (0 < temp.Count)
        {
            foreach (var oldData in temp)
            {
                GameObject.Destroy(oldData.gameObject);
            }
            temp.Clear();
        }
    }

    public override void Initialize(ScreenController screenController)
    {
        base.Initialize(screenController);
        AddListners();
    }
    void AddListners()
    {
        fromCalandar.onClickOnDate = OnClickOnFromCalander;
        toCalandar.onClickOnDate = OnClickOnToCalander;
        fromBtn.onClick.AddListener(fromCalandar.ShowCalendar);
        toBtn.onClick.AddListener(toCalandar.ShowCalendar);

        toDate[0].onValueChanged.AddListener((value) => { if (string.IsNullOrEmpty(value)) return; to_date = int.Parse(value); });
        toDate[0].onSubmit.AddListener((value) => { if (string.IsNullOrEmpty(value)) return; to_date = int.Parse(value); });

        toDate[1].onValueChanged.AddListener((value) => { if (string.IsNullOrEmpty(value)) return; toMonth = int.Parse(value); });
        toDate[1].onSubmit.AddListener((value) => { if (string.IsNullOrEmpty(value)) return; toMonth = int.Parse(value); });

        toDate[2].onValueChanged.AddListener((value) => { if (string.IsNullOrEmpty(value)) return; toYear = int.Parse(value); });
        toDate[2].onSubmit.AddListener((value) => { if (string.IsNullOrEmpty(value)) return; toYear = int.Parse(value); });


        fromDate[0].onValueChanged.AddListener((value) => { if (string.IsNullOrEmpty(value)) return; from_date = int.Parse(value); });
        fromDate[0].onSubmit.AddListener((value) => { if (string.IsNullOrEmpty(value)) return; from_date = int.Parse(value); });

        fromDate[1].onValueChanged.AddListener((value) => { if (string.IsNullOrEmpty(value)) return; fromMonth = int.Parse(value); });
        fromDate[1].onSubmit.AddListener((value) => { if (string.IsNullOrEmpty(value)) return; fromMonth = int.Parse(value); });

        fromDate[2].onValueChanged.AddListener((value) => { if (string.IsNullOrEmpty(value)) return; fromYear = int.Parse(value); });
        fromDate[2].onSubmit.AddListener((value) => { if (string.IsNullOrEmpty(value)) return; fromYear = int.Parse(value); });


        applyBtn.onClick.AddListener(DateValidation);
        backBtn.onClick.AddListener(() =>
        {
            if (gameId == 1)
            {
                screenController.Show(ScreenName.criketScreen);
                return;
            }
            screenController.ShowBettingScreen(gameId);
        });
    }
    DateTime fromdate;
    string todateHolder;
    string fromdateHolder;
    void OnClickOnFromCalander(int Day, int Month, int Year)
    {
        string tmp_d = Day.ToString();
        string tmp_m = Month.ToString();
        print($"{Day}/{Month}/{Year}");
        if (Day < 10 || Month < 10)
        {
            if (Month < 10) tmp_m = $"0{Month}";
            if (Day < 10) tmp_d = $"0{Day}";
            fromdateHolder = $"{Year}-{tmp_m}-{tmp_d}";
            fromTxt.text = $"{Year}-{tmp_m}-{tmp_d}";
        }
        else
        {
            fromTxt.text = $"{Year}-{Month}-{Day}";
            fromdateHolder = $"{Year}-{Month}-{Day}";
        }

        fromdate = new DateTime(Year, Month, Day);
    }
    DateTime todate;
    void OnClickOnToCalander(int Day, int Month, int Year)
    {
        string tmp_d = Day.ToString();
        string tmp_m = Month.ToString();
        print($"{Day}/{Month}/{Year}");
        if (Day < 10 || Month < 10)
        {
            if (Month < 10) tmp_m = $"0{Month}";

            if (Day < 10) tmp_d = $"0{Day}";
            todateHolder = $"{Year}-{tmp_m}-{tmp_d}";
            toTxt.text = $"{Year}-{tmp_m}-{tmp_d}";
        }
        else
        {
            todateHolder = $"{Year}-{Month}-{Day}";
            toTxt.text = $"{Year}-{Month}-{Day}";
        }
        todate = new DateTime(Year, Month, Day);
    }

    void DateValidation()
    {
        if (string.IsNullOrEmpty(fromdateHolder) || string.IsNullOrEmpty(todateHolder))
        {
            commonPopup.Show("enter valid date");
            return;
        }
        if (todate == null || fromdate == null)
        {
            commonPopup.Show("enter valid date");
            return;
        }
        int areDatesValid = DateTime.Compare(fromdate, todate);
        if (areDatesValid == 1)
        {
            commonPopup.Show("Invalid date");
            return;
        }
        Info info = new Info()
        {
            to_date = todateHolder,
            from_date = fromdateHolder,
            retailer_id = screenController.dataStorageController.LoginResponseData.data.retailer_id,
            game_id = gameId.ToString()
        };

        foreach (Transform item in content.transform)
        {
            Destroy(item.gameObject);
        }

        if (gameId == (int)GameIds.cricket)
        {
            var obj = new GenricWebHandler<CricketHistory>(info);
            obj.SendRequestToServer(Constant.GAME_HISTORY_URL, OnCricketHistory);
        }
        else
        {
            var obj = new GenricWebHandler<GameHistory>(info);
            obj.SendRequestToServer(Constant.GAME_HISTORY_URL, OnHistory);
        }
    }
    List<GameObject> temp = new List<GameObject>();
    void OnHistory(GameHistory obj)
    {
        if (obj == null)
        {
            commonPopup.Show();
            return;
        }
        if (obj.status != 200)
        {
            commonPopup.Show(obj.message);
            return;
        }

        print("1 chile count " + content.transform.childCount);
        if (0 < temp.Count)
        {
            foreach (var oldData in temp)
            {
                GameObject.Destroy(oldData.gameObject);
            }
            temp.Clear();
        }
        print("2 chile count " + content.transform.childCount);
        for (int i = 0; i < obj.data.game_report.Count; i++)
        {
            var o = Instantiate(contenPerfab, content.transform);
            o.GetComponent<GameHistoryMono>().SetData(obj.data.game_report[i]);
            temp.Add(o);
        }
        print("3 chile count " + content.transform.childCount);


    }

    public GameObject cricketPrefab;
    public void OnCricketHistory(CricketHistory obj)
    {
        if (obj == null)
        {
            commonPopup.Show();
            return;
        }
        if (obj.status != 200)
        {
            commonPopup.Show(obj.message);
            return;
        }

        print("1 chile count " + content.transform.childCount);
        if (0 < temp.Count)
        {
            foreach (var oldData in temp)
            {
                GameObject.Destroy(oldData.gameObject);
            }
            temp.Clear();
        }

        print("2 chile count " + content.transform.childCount);
        for (int i = 0; i < obj.data.game_report.Count; i++)
        {
            var o = Instantiate(cricketPrefab, content.transform);
            string drawData = $"{i + 1}.                    {obj.data.game_report[i].date} | {obj.data.game_report[i].draw_time}                    <b> {GetCricketDrawAlphabet(int.Parse(obj.data.game_report[i].winning_no))}";
            o.GetComponentInChildren<TMP_Text>().text = drawData;
            temp.Add(o);
        }
        print("3 chile count " + content.transform.childCount);

    }
    string GetCricketDrawAlphabet(int index)
    {
        switch (index)
        {
            case 0: return "A00";
            case 1: return "B01";
            case 2: return "C02";
            case 3: return "D03";
            case 4: return "E04";
            case 5: return "F05";
            case 6: return "G06";
            case 7: return "H07";
            case 8: return "I08";
            case 9: return "J09";
            default:
                break;
        }
        return string.Empty;
    }
    void InvadlidDateMsg(string m = "invalid Date") => msg.text = m;
    void VadlidDate() => msg.text = "";
    void DefaultData()
    {
        todateHolder = string.Empty;
        fromdateHolder = string.Empty;

    }
    public override void Show(object data = null)
    {
        base.Show();
        if (data != null) gameId = (int)data;
        DefaultData();
        VadlidDate();
    }

}

public class Info
{
    public string retailer_id;
    public string game_id;
    public string from_date;
    public string to_date;
}

public class GameReport
{
    public string draw_time;
    public string date;
    public string win_no_A;
    public string win_no_B;
    public string win_no_C;
    public string win_no_D;
    public string win_no_E;
    public string win_no_F;
    public string win_no_G;
    public string win_no_H;
    public string win_no_I;
    public string win_no_J;
    public string created;
}

public class HistoryData
{
    public List<GameReport> game_report;
}

public class GameHistory
{
    public int status;
    public string message;
    public HistoryData data;
}




public class CricketGameReport
{
    public string draw_time { get; set; }
    public string winning_no { get; set; }
    public string date { get; set; }
}

public class CricketHistoryData
{
    public List<CricketGameReport> game_report { get; set; }
}

public class CricketHistory
{
    public int status;
    public string message;
    public CricketHistoryData data;
}

