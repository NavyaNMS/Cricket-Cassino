using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using ApiPrototype;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using FootballPrototype2;
using System;
using FootballPrototype3;

class CricketScreen : Screen
{
    [SerializeField] Button cancleBtn;
    [SerializeField] Button playBtn;
    [SerializeField] Button clearBtn;
    [SerializeField] Button infoBtn;
    [SerializeField] Button reportBtn;
    [SerializeField] Button claimBtn;
    [SerializeField] Button reprintBtn;
    [SerializeField] Button lobbyBtn;
    [SerializeField] Button greenBtn;
    [SerializeField] Button claimCrossBtn;
    [SerializeField] Button advanceBettingBtn;
    [SerializeField] Button advanceBettingCrossBtn;
    [SerializeField] Button enterClaimBtn;
    [SerializeField] Button refreshBtn;
    [SerializeField] Button testBtn;

    [SerializeField] TMP_InputField claimInputFiel;
    [SerializeField] TMP_InputField[] betInputs;

    [SerializeField] TMP_Text totalSelectedSpot;
    [SerializeField] TMP_Text totalAmount;
    [SerializeField] TMP_Text poolPrizetxt;
    [SerializeField] TMP_Text serviceChargetxt;
    [SerializeField] TMP_Text timer;
    [SerializeField] TMP_Text limit;
    [SerializeField] TMP_Text dateAndTime;

    [SerializeField] Toggle twoRsToggle;
    [SerializeField] Toggle fourRsToggle;
    [SerializeField] Toggle sixRsToggle;
    [SerializeField] Toggle eightRsToggle;

    [SerializeField] Toggle[] timers;

    [SerializeField] CricketAnimationController cricketAnimationController;
    [SerializeField] GameObject claimPopup;
    [SerializeField] GameObject advanceBettingPopup;
    Dictionary<string, BetInfo> betsContainer = new Dictionary<string, BetInfo>();
    int gameId = 1;
    char[] bettingSerise = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J' };

    public override ScreenName ScreenID => ScreenName.criketScreen;

    public override void Initialize(ScreenController screenController)
    {
        base.Initialize(screenController);
        AddListner();
    }
    void AddListner()
    {
        SetupInputField();
        playBtn.onClick.AddListener(OnPlay);
        reportBtn.onClick.AddListener(() => screenController.ShowReportScreen(gameId));
        clearBtn.onClick.AddListener(() => ClearAllBets());
        reprintBtn.onClick.AddListener(() => screenController.ShowReprinttScreen(gameId));
        infoBtn.onClick.AddListener(() => screenController.ShowHistoryScreen(gameId));
        lobbyBtn.onClick.AddListener(screenController.ShowHomeScreen);
        claimBtn.onClick.AddListener(() => claimPopup.SetActive(true));
        cancleBtn.onClick.AddListener(OnCancleOrder);
        claimCrossBtn.onClick.AddListener(() => claimPopup.SetActive(false));
        testBtn.onClick.AddListener(() => cricketAnimationController.Play(0));
        advanceBettingBtn.onClick.AddListener(() =>
        {
            if (!isDataLoaded) return;

            advanceBettingPopup.SetActive(true);
            OnAdvanceBetting();

        });
        advanceBettingCrossBtn.onClick.AddListener(() =>
        {
            advanceBettingPopup.SetActive(false);
        });
        enterClaimBtn.onClick.AddListener(OnClaim);
        twoRsToggle.onValueChanged.AddListener((isOn) =>
        {
            if (isOn) oneTicketPrize = 2;
        });
        fourRsToggle.onValueChanged.AddListener((isOn) =>
        {
            if (isOn) oneTicketPrize = 4;
        });
        sixRsToggle.onValueChanged.AddListener((isOn) =>
        {
            if (isOn) oneTicketPrize = 6;
        });
        eightRsToggle.onValueChanged.AddListener((isOn) =>
        {
            if (isOn) oneTicketPrize = 8;
        });

        for (int i = 0; i < timers.Length; i++)
        {
            var togleBtn = timers[i].transform;
            timers[i].onValueChanged.AddListener((isOn) =>
            {
                if (!isOn) return;
                customizeDrawTime = togleBtn.GetChild(1).GetComponent<Text>().text;
                isCustomizedTimeOn = true;
                advanceBettingPopup.SetActive(false);
            });
        }
    }
    void OnAdvanceBetting()
    {
        bool flag = false;
        int index = 0;
        for (int i = 0; i < timers.Length; i++)
        {
            if (timers[i].transform.GetChild(1).GetComponent<Text>().text.Trim()
                == drawTime.Trim())
            {
                flag = true;
                index = i;
            }
            if (flag)
            {
                timers[i].interactable = true; ;
            }
            else
            {
                timers[i].interactable = false;

            }

        }

        timers[index].isOn=true;
        timers[index].Select();
    }
    void SetupInputField()
    {
        for (int i = 0; i < betInputs.Length; i++)
        {
            TMP_InputField betInputField = betInputs[i];
            int spotNo = i;
            betInputField.onValueChanged.AddListener((value) =>
            {
                if (Input.anyKey)
                    OnAddBet(betInputField, value, spotNo);
            });
        }
    }
    int totalBets;
    int totalSpots;
    int oneTicketPrize;
    void OnAddBet(TMP_InputField selectedSpot, string betamount, int spotNo)
    {

        TMP_Text placeholder = selectedSpot.transform.GetChild(0).GetChild(1).GetComponent<TMP_Text>();
        string spotName = placeholder.text;
        print("on add");
        if (!isDataLoaded) return;
        if (string.IsNullOrEmpty(betamount) & betsContainer.ContainsKey(spotName))
        {
            betsContainer.Remove(spotName);
            totalBets = betsContainer.Values.Sum(x => x.amount);
            selectedSpot.GetComponent<Image>().color = Color.white;
            UpdateUi();
            return;
        }
        bool isInvalidAmount = betamount == string.Empty || betamount == 0.ToString() || betamount == 00.ToString() || betamount == 000.ToString();
        if (isInvalidAmount)
        {
            selectedSpot.text = string.Empty;
            return;
        }
        int bet = int.Parse(betamount.Trim()) * oneTicketPrize;

        if (bet + totalBets > balance)
        {
            commonPopup.Show("Insufficient balance");
            return;
        }
        BetInfo betObj = new BetInfo()
        {
            amount = bet,
            serise = bettingSerise[spotNo],
            ticketValue = betamount,
            spotNo = spotNo
        };

        if (betsContainer.ContainsKey(spotName))
        {
            betsContainer[spotName] = betObj;
        }
        else
        {
            betsContainer.Add(spotName, betObj);
        }
        selectedSpot.GetComponent<Image>().color = Color.yellow;
        totalBets = betsContainer.Values.Sum(x => x.amount);
        UpdateUi();
    }

    void OnPlay()
    {

        if (minuteLeft < 1)
        {
            if (secLeft < 30)
            {
                commonPopup.Show("cannot place bets in last 30 sec");
                ClearAllBets();
                return;
            }
        }
        if (betsContainer.Sum(x => x.Value.amount) == 0)
        {
            commonPopup.Show("No bets");
            return;
        }
        PostBets();
    }
    private void OnCancleOrder()
    {
        if (string.IsNullOrEmpty(currentTicket))
        {
            commonPopup.Show("No tickets found");
            return;
        }

        object ticketNo = new { ticket_no = currentTicket };
        var cancleaTicket = new GenricWebHandler<Ticket>(ticketNo);
        cancleaTicket.SendRequestToServer(Constant.CANCLE_TICKET_URL, (ticketInfo) =>
        {
            if (ticketInfo == null || ticketInfo.status != 200)
            {
                commonPopup.Show(ticketInfo.message);
                commonPopup.OnClickOk = () => Reload();
                return;
            }
            commonPopup.Show(ticketInfo.message);
            balance = ticketInfo.data.retailer_balance;
            recentPrint = null;
            UpdateUi();
        });

    }
    void PostBets()
    {
        List<Bet> bets;
        int totalBet;
        print(JsonConvert.SerializeObject(betsContainer));
        var betInfo = CalculateBets();

        bets = betInfo.Item1;
        totalBet = betInfo.Item2;
        if (totalBet == 0)
        {
            commonPopup.Show("No bets added");
            return;
        }
        BetsDetail b = new BetsDetail()
        {
            retailer_id = retailer.retailer_id,
            game_id = gameId.ToString(),
            bet = bets,
            draw_time = isCustomizedTimeOn ? customizeDrawTime : drawTime,
            total_points = totalBet.ToString()
        };
        var postBets = new GenricWebHandler<BetValidation>(b);
        if (!isPreviousOrderDone)
        {
            Toast.instance.Show("please wait");
            return;
        }
        isPreviousOrderDone = false;
        postBets.SendRequestToServer(Constant.SHOWCURRENT_BET_URL, OnBetValidation);
    }
    (List<Bet>, int) CalculateBets()
    {
        List<Bet> bets = new List<Bet>();
        int totalBet = 0;
        foreach (var item in betsContainer)
        {
            var temp = item.Key.ToCharArray();
            string serise = temp[0].ToString();
            string no = (temp[1].ToString() + temp[2].ToString());
            string value = item.Value.amount.ToString();
            Bet bet = new Bet() { bet_no = no, series = serise, points = value };
            totalBet += item.Value.amount;
            bets.Add(bet);
        }
        return (bets, totalBet);
    }
    private void OnClaim()
    {
        string claimId = claimInputFiel.text;
        if (string.IsNullOrEmpty(claimId) || !claimId.All(char.IsDigit))
        {
            commonPopup.Show("Please Enter valid id");
            return;
        }
        object tickect = new { ticket_no = claimId };
        var claim = new GenricWebHandler<Claim>(tickect);
        claim.SendRequestToServer(Constant.CLAIM_TICKET_URL, (claimObj) =>
        {
            if (claimObj.status == null || claimObj.status != 200)
            {
                commonPopup.Show("something went wrong");
                commonPopup.OnClickOk = () => Reload();
            }
            commonPopup.Show(claimObj.message);
        });

    }
    void OnBetValidation(BetValidation bet)
    {
        if (bet.status != 200)
        {
            commonPopup.Show("Unable to place bets please try again");
            Reload();
        }
        else
        {
            Toast.instance.Show("Bets Placed Successfully");
            currentTicket = bet.data.barcode_no;
            canStopTimer = true;
            string DrawTime = isCustomizedTimeOn ? customizeDrawTime : drawTime;

            Printer print = new Printer(currentTime, date,
                totalSpots.ToString(), DrawTime,
                currentTicket,
                betsContainer, poolPrize, serviceCharge,(GameIds)gameId);
            recentPrint = print;
            balance = bet.data.cash_balance;
            print.Print();
            object gameIdObj = new { game_id = gameId };
            var currentRount = new GenricWebHandler<CurrentTimer>(gameIdObj);
            currentRount.SendRequestToServer(Constant.GET_TIMER_URL, OnCurrentTimer);
        }
        isPreviousOrderDone = true;
        UpdateUi();
        ClearAllBets();
    }
    void ClearAllBets()
    {

        isCustomizedTimeOn = false;
        foreach (var item in betInputs)
        {
            item.text = string.Empty;
            item.GetComponent<Image>().color = Color.white;
        }
        betsContainer.Clear();
        totalBets = betsContainer.Values.Sum(x => x.amount);
        UpdateUi();
    }
    public override void Hide()
    {
        base.Hide();

    }

    public override void Show(object data = null)
    {
        base.Show(data);
        LoadData();
    }

    Retailer retailer;
    #region Helper Funtions
    void LoadData()
    {
        DefaulValue();
        twoRsToggle.isOn = true;

        twoRsToggle.Select();
        advanceBettingPopup.SetActive(false);
        claimPopup.SetActive(false);
        StopAllCoroutines();
        retailer = new Retailer()
        {
            game_id = gameId.ToString(),
            retailer_id =
           screenController.dataStorageController.LoginResponseData.data.retailer_id
        };
        isDataLoaded = false;
        StartCoroutine(DisplayDate());

        var obj = new GenricWebHandler<CricketCurrentRound>(retailer);
        obj.SendRequestToServer(Constant.CURRENT_ROUND_URL, OnCurrentRound);
        var poolPrizeObj = new GenricWebHandler<PoolPrize>();
        poolPrizeObj.SendGetRequestToServer(Constant.POOL_PRIZE_URL, OnPoolPrize);

    }
    float poolPrizePc;
    void OnPoolPrize(PoolPrize p)
    {
        if (p == null) return;
        if (p.status != 200)
        {
            return;
        }

        poolPrizePc = p.data.pool_prize_percentage;
        UpdateUi();
    }
    IEnumerator DisplayDate()
    {
        date = $"{DateTime.Now.Day}/{DateTime.Now.Month}/{DateTime.Now.Year}";
        dateAndTime.text = DateTime.Now.ToShortTimeString().ToString() + " | " + date;
        currentTime = DateTime.Now.ToShortTimeString().ToString();
        yield return new WaitForSeconds(60 - DateTime.Now.Second);
        StartCoroutine(DisplayDate());
    }
    void DefaulValue()
    {
        isPreviousOrderDone = true;
        isCustomizedTimeOn = false;
        oneTicketPrize = 2;
        betsContainer = new Dictionary<string, BetInfo>();
    }
    float poolPrize = 0;
    float serviceCharge = 0;
    void UpdateUi()
    {
        int totalBets = betsContainer.Sum(x => x.Value.amount);
        serviceCharge = (totalBets * (poolPrizePc / 100));
        poolPrize = totalBets - serviceCharge;
        totalAmount.text = totalBets.ToString();
        totalSelectedSpot.text = betsContainer.Count.ToString();
        limit.text = "Limit:" + balance.ToString();
        poolPrizetxt.text = poolPrize.ToString();
        serviceChargetxt.text = serviceCharge.ToString();

    }
    (int, int) DrawtimeCalculator(int min, int hr)
    {
        int minutes;
        if (min <= 15)
        {
            return (hr, 15);
        }

        if (min <= 30)
        {
            return (hr, 30);
        }

        if (min <= 45)
        {
            return (hr, 45);
        }

        if (min >= 45)
        {
            minutes = hr == 24 ? hr = 1 : ++hr;
            return (minutes, 00);
        }
        return (0, 0);
    }
    void Reload()
    {
        ClearAllBets();
        print("something went wrong");
        StopCoroutine(DisplayDate());
        StopCoroutine(DisplayCountDown());
        LoadData();
    }
    #endregion

    #region gameflow
    [SerializeField] GameObject[] lastrounds;
    [SerializeField] Sprite[] WinsScreenShorts;
    [SerializeField] Sprite wss;

    int balance;
    public string drawTime;
    public string customizeDrawTime;
    public bool isCustomizedTimeOn;
    public bool isDataLoaded;
    private bool isPreviousOrderDone;
    private Printer recentPrint;
    private string currentTicket;
    private string currentTime;
    private string date;

    void OnCurrentRound(CricketCurrentRound obj)
    {
        if (obj == null)
        {
            print("obj is null");
            Reload();
            return;
        }

        if (obj.status != 200)
        {
            Reload();
            return;
        }


        //this block will display last ten rounds win Numbers
        for (int i = 0; i < obj.data.last_draw_report.Count; i++)
        {
            if (lastrounds.Length - 1 < i) break;
            int index = int.Parse(obj.data.last_draw_report[i].win_no);
            lastrounds[i].transform.GetChild(0)
                .GetChild(0)
                .GetComponent<TMP_Text>().text = bettingSerise[index] + obj.data.last_draw_report[i].win_no
                + "\n\n" +
                obj.data.last_draw_report[i].draw_time;

            int winIndex = int.Parse(obj.data.last_draw_report[i].win_no);
            lastrounds[i].transform.GetChild(1).GetComponent<Image>().sprite = WinsScreenShorts[winIndex];
        }

        var temp = obj.data.current_draw.Split(':');
        int sec = int.Parse(temp[1]);
        int min = int.Parse(temp[0]);
        balance = int.Parse(obj.data.retailer_bal);
        var time = DrawtimeCalculator(sec, min);

        drawTime = time.Item1.ToString() + ":" + time.Item2.ToString();
        object gameIdObj = new { game_id = gameId };
        var currentRount = new GenricWebHandler<CurrentTimer>(gameIdObj);
        currentRount.SendRequestToServer(Constant.GET_TIMER_URL, OnCurrentTimer);
        UpdateUi();
    }
    private void OnCurrentTimer(CurrentTimer obj)
    {
        if (obj.status != "200")
        {
            Reload();
            return;
        }
        var temp = obj.timer.Split(':');
        int sec = 60 - int.Parse(temp[1]);
        int min = 14 - int.Parse(temp[0]);
        isDataLoaded = true;
        canStopTimer = false;

        StopCoroutine(DisplayCountDown());
        StartCoroutine(DisplayCountDown(sec, min));
    }
    #endregion

    int minuteLeft = 0;
    int secLeft = 0;
    bool canStopTimer;
    IEnumerator DisplayCountDown(int sec = 30, int min = 1)
    {
        while (min > -1)
        {
            if (canStopTimer)
            {
                yield break;
            }
            minuteLeft = min;
            secLeft = sec;
            if (sec.ToString().ToCharArray().Length == 1)
                timer.text = $"{min}:0{sec}";
            else
                timer.text = $"{min}:{sec}";
            yield return new WaitForSecondsRealtime(1);
            if (sec < 1)
            {
                sec = 60;
                min--;
            }
            sec--;
        }
        timer.text = $"0:00";
        RoundInfo round = new RoundInfo()
        {
            game_id = gameId.ToString(),
            draw_time = drawTime
        };
        //restart round
        var winNo = new GenricWebHandler<CricketWinNo>(round);
        winNo.SendRequestToServer(Constant.WIN_NO_URL, OnDrawResult);
    }
    void OnDrawResult(CricketWinNo obj)
    {

        if (obj == null)
        {
            Reload();
            return;
        }

        if (obj.status != 200)
        {
            Reload();
            return;
        }
        int winNo = int.Parse(obj.Data.winning_no);
        cricketAnimationController.Play(winNo);
        LoadData();

    }
}


public class LastDrawReport
{
    public string draw_time;
    public string win_no;
}

public class CricketData
{
    public string current_draw;
    public string retailer_bal;
    public List<LastDrawReport> last_draw_report;
}

public class CricketCurrentRound
{
    public int status;
    public string message;
    public CricketData data;
}

public class CricketWinNoData
{
    public string draw_time;
    public string winning_no;
}

public class CricketWinNo
{
    public int status;
    public string message;
    public CricketWinNoData Data;
}
