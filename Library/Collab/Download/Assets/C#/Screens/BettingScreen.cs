using ApiPrototype;
using FootballPrototype2;
using FootballPrototype3;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// this script is for bot football and hockey
/// </summary>
public class BettingScreen : Screen
{
    private int gameId = 2;

    [SerializeField] Transform bettingGrid;
    Transform gameGrid;
    [SerializeField] Transform gameGridfootball;
    [SerializeField] Transform gameGridhockey;
    [SerializeField] Transform lastroundsResults;
    [SerializeField] Button cancleBtn;
    [SerializeField] Button playBtn;
    [SerializeField] Button clearBtn;
    [SerializeField] Button infoBtn;
    [SerializeField] Button advanceBettingBtn;
    [SerializeField] Button advanceBettingCrossBtn;
    [SerializeField] Button reportBtn;
    [SerializeField] Button claimBtn;
    [SerializeField] Button reprintBtn;
    [SerializeField] Button lobbyBtn;
    [SerializeField] Button greenBtn;
    [SerializeField] Button claimCrossBtn;
    [SerializeField] Button enterClaimBtn;
    [SerializeField] Button refreshBtn;

    [SerializeField] Toggle[] toggleSerise;
    [SerializeField] Toggle[] drawTimeToggles;
    [SerializeField] Toggle evenToggle;
    [SerializeField] Toggle oddToggle;
    [SerializeField] Toggle noneToggle;
    [SerializeField] Toggle allSeriseToggle;
    [SerializeField] Toggle twoRsToggle;
    [SerializeField] Toggle fourRsToggle;
    [SerializeField] Toggle sixRsToggle;
    [SerializeField] Toggle eightRsToggle;
    [SerializeField] TMP_InputField claimInputFiel;

    [SerializeField] TMP_Text totalSelectedSpot;
    [SerializeField] TMP_Text _serviceCharge;
    [SerializeField] TMP_Text totalAmount;
    [SerializeField] TMP_Text poolPrizetxt;
    [SerializeField] TMP_Text serviceChargetxt;
    [SerializeField] TMP_Text timer;
    [SerializeField] TMP_Text limit;
    [SerializeField] TMP_Text dateAndTime;

    [SerializeField] GameObject claimScreen;
    [SerializeField] GameObject advanceBettingPopup;

    [SerializeField] Image goalKeeperImg;
    [SerializeField] AnimationController animController;

    Retailer retailer;
    Dictionary<string, BetInfo> betsContainer;
    Printer recentPrint;
    string currentRound;
    string currentTicket;
    string currentTime;
    string date;
    char[] bettingSerise = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J' };
    int currentSerise = 0;
    int totalSpots;
    int totalBets;
    int balance;
    float poolPrizePc;
    int oneTicketPrize = 2;
    bool isDataLoaded;
    bool isPreviousOrderDone;
    bool isAllSeriseSelected;
    public string drawTime;
    public string coustomizeDrawTime;
    public bool isCoustmaizeTimeOn;
    public override ScreenName ScreenID => ScreenName.bettingScreen;


    #region intialSetup
    public Button test;
    //this will run only onces for the very first time
    public override void Initialize(ScreenController screenController)
    {

        base.Initialize(screenController);
        DefaulValue();
        //test.onClick.AddListener(() => screenController.Show(ScreenName.FootballScreen));
        AddListener();
    }
    void DefaulValue()
    {
        currentSerise = 0;
        betsContainer = new Dictionary<string, BetInfo>();

    }
    void AddListener()
    {
        SetupInputFild();
        OnChangeSerise();
        reportBtn.onClick.AddListener(() => screenController.ShowReportScreen(gameId)); ;
        reprintBtn.onClick.AddListener(() => screenController.ShowReprinttScreen(gameId));
        clearBtn.onClick.AddListener(() => ClearAllBets());
        playBtn.onClick.AddListener(() => OnPlay());
        claimBtn.onClick.AddListener(() => claimScreen.SetActive(true));
        enterClaimBtn.onClick.AddListener(() => OnClaim());
        claimCrossBtn.onClick.AddListener(() => claimScreen.SetActive(false));
        advanceBettingBtn.onClick.AddListener(() =>
        {
            if (!isDataLoaded) return;
                advanceBettingPopup.SetActive(true);
        });
        advanceBettingCrossBtn.onClick.AddListener(() =>
        {
            advanceBettingPopup.SetActive(false);
        });
        cancleBtn.onClick.AddListener(() => OnCancleOrder());
        infoBtn.onClick.AddListener(() => screenController.ShowHistoryScreen(gameId));
        lobbyBtn.onClick.AddListener(screenController.ShowHomeScreen);
        refreshBtn.onClick.AddListener(Reload);
        SetToggleBtns();

        for (int i = 0; i < drawTimeToggles.Length; i++)
        {
            var togleBtn = drawTimeToggles[i].transform;
            drawTimeToggles[i].onValueChanged.AddListener(OnTimerChange(i, togleBtn));
        }
    }

    private UnityAction<bool> OnTimerChange(int i, Transform togleBtn)
    {
        return (isOn) =>
        {
            int index = i;
            if (!isOn && !drawTimeToggles[index].transform.gameObject.activeSelf) return;
            coustomizeDrawTime = togleBtn.GetChild(1).GetComponent<Text>().text;
            isCoustmaizeTimeOn = true;
            //advanceBettingPopup.SetActive(false);

        };
    }

    public Transform advanceTimerGameobject;
    void OnAdvanceBetting()
    {
        advanceBettingPopup.SetActive(true);

        bool flag = false;
        int index = 0;

        for (int i = 0; i < drawTimeToggles.Length; i++)
        {
            if (drawTimeToggles[i].transform.GetChild(1).GetComponent<Text>().text.Trim()
                == drawTime.Trim())
            {
                flag = true;
                index = i;
            }
            if (flag)
            {
                drawTimeToggles[i].interactable = true;
            }
            else
            {
                drawTimeToggles[i].interactable = false;
            }

        }
        drawTimeToggles[index].isOn = true;
        drawTimeToggles[index].Select();
        advanceBettingPopup.SetActive(false);

    }


    private void SetToggleBtns()
    {
        evenToggle.onValueChanged.AddListener((isOn) =>
        {
            OnToggleEvenOddNone(isOn, "E");
        });
        oddToggle.onValueChanged.AddListener((isOn) =>
        {
            OnToggleEvenOddNone(isOn, "O");
        });
        noneToggle.onValueChanged.AddListener((isOn) =>
        {
            OnToggleEvenOddNone(isOn, "N");
        });

        allSeriseToggle.onValueChanged.AddListener((isOn) =>
        {
            if (isOn)
            {
                isAllSeriseSelected = true;
                for (int i = 0; i < toggleSerise.Length; i++)
                {
                    toggleSerise[i].transform.GetChild(0).GetChild(1).transform.gameObject.SetActive(true);
                }
            }
            else
            {
                isAllSeriseSelected = false;
                for (int i = 0; i < toggleSerise.Length; i++)
                {
                    toggleSerise[i].transform.GetChild(0).GetChild(1).transform.gameObject.SetActive(false);
                }
            }

        });
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
    }

    void OnToggleEvenOddNone(bool isOn, string type)
    {
        if (!isOn) return;

        int reminder = 0;

        if (type == "O") reminder = 1;
        if (type == "E") reminder = 0;
        if (type == "N") reminder = -1;
        for (int i = 0; i < bettingGrid.transform.childCount; i++)
        {
            if (reminder == -1)
            {
                bettingGrid.transform.GetChild(i).GetComponent<TMP_InputField>().interactable = true;
                continue;
            }
            if (i % 2 == reminder)
                bettingGrid.transform.GetChild(i).GetComponent<TMP_InputField>().interactable = false;
            else
                bettingGrid.transform.GetChild(i).GetComponent<TMP_InputField>().interactable = true;
        }

    }
    private void OnPlay()
    {
        if (miniutsLeft < 1)
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
        ValidateOrder();

    }
    [SerializeField] List<TMP_InputField> betsInputFieldHolder = new List<TMP_InputField>();

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
    private void OnChangeSerise()
    {
        for (int i = 0; i < toggleSerise.Length; i++)
        {
            int seriseIndex = i;
            toggleSerise[i].onValueChanged.AddListener((isOn) =>
            {
                if (!isOn) return;
                currentSerise = seriseIndex;
                char letter = bettingSerise[seriseIndex];
                for (int j = 0; j < gameGrid.childCount; j++)
                {
                    if (j < 10)
                    {
                        gameGrid.GetChild(j).GetChild(0).GetComponent<TMP_Text>().text = $"{letter}\n0{j}";
                        bettingGrid.GetChild(j).GetChild(0).GetChild(1).GetComponent<TMP_Text>().text = $"{letter}0{j}";
                    }
                    else
                    {
                        gameGrid.GetChild(j).GetChild(0).GetComponent<TMP_Text>().text = $"{letter}\n{j}";
                        bettingGrid.GetChild(j).GetChild(0).GetChild(1).GetComponent<TMP_Text>().text = $"{letter}{j}";
                    }
                    bettingGrid.GetChild(j).GetComponent<TMP_InputField>().text = string.Empty;
                    bettingGrid.GetChild(j).GetComponent<Image>().color = Color.white;
                }
                if (betsContainer.Count > 0)
                    DisplayBetsOnToggle();

            });
        }
    }
    /// <summary>
    /// it will display bets of respective serise and change the seris
    /// it will take the value from the betContainer
    /// </summary>
    void DisplayBetsOnToggle()
    {
        foreach (var bet in betsContainer)
        {
            if (bet.Value.serise != bettingSerise[currentSerise])
            {
                continue;
            }
            bettingGrid.GetChild(bet.Value.spotNo).
                GetComponent<TMP_InputField>().text = bet.Value.ticketValue;
            bettingGrid.GetChild(bet.Value.spotNo).
                GetComponent<Image>().color = Color.yellow;
        }
    }
    void SetupInputFild()
    {

        var serise = bettingGrid.transform;
        for (int i = 0; i < serise.childCount; i++)
        {
            TMP_InputField betInputField = serise.GetChild(i).GetComponent<TMP_InputField>();
            int spotNo = i;
            betInputField.onValueChanged.AddListener((value) =>
            {
                if (Input.anyKey)
                    OnAddBet(betInputField, value, spotNo);
            });
        }

    }
    #endregion

    #region GameFlow
    void OnCurrentRound(CurrentRound cr)
    {
        if (cr == null)
        {
            print("obj is null");
            Reload();
            return;
        }
        if (cr.status != 200)
        {
            Reload();
            return;
        }
        var temp = cr.data.current_draw.Split(':');
        int sec = int.Parse(temp[1]);
        int min = int.Parse(temp[0]);
        balance = int.Parse(cr.data.retailer_bal);
        if (cr.data.last_draw_report.Count > 0)
        {

            var obj = cr.data.last_draw_report[0];
            lastroundsResults.GetChild(0).GetChild(1).GetComponent<TMP_Text>().text = "A-" + obj.win_no_A;
            lastroundsResults.GetChild(1).GetChild(1).GetComponent<TMP_Text>().text = "B-" + obj.win_no_B;
            lastroundsResults.GetChild(2).GetChild(1).GetComponent<TMP_Text>().text = "C-" + obj.win_no_C;
            lastroundsResults.GetChild(3).GetChild(1).GetComponent<TMP_Text>().text = "D-" + obj.win_no_D;
            lastroundsResults.GetChild(4).GetChild(1).GetComponent<TMP_Text>().text = "E-" + obj.win_no_E;
            lastroundsResults.GetChild(5).GetChild(1).GetComponent<TMP_Text>().text = "F-" + obj.win_no_F;
            lastroundsResults.GetChild(6).GetChild(1).GetComponent<TMP_Text>().text = "G-" + obj.win_no_G;
            lastroundsResults.GetChild(7).GetChild(1).GetComponent<TMP_Text>().text = "H-" + obj.win_no_H;
            lastroundsResults.GetChild(8).GetChild(1).GetComponent<TMP_Text>().text = "I-" + obj.win_no_I;
            lastroundsResults.GetChild(9).GetChild(1).GetComponent<TMP_Text>().text = "J-" + obj.win_no_J;

            string[] tempTime = obj.draw_time.Split(':');
            string lastDrawTime = tempTime[0] + ":" + tempTime[1];
            lastroundsResults.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = lastDrawTime;
            lastroundsResults.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = lastDrawTime;
            lastroundsResults.GetChild(2).GetChild(0).GetComponent<TMP_Text>().text = lastDrawTime;
            lastroundsResults.GetChild(3).GetChild(0).GetComponent<TMP_Text>().text = lastDrawTime;
            lastroundsResults.GetChild(4).GetChild(0).GetComponent<TMP_Text>().text = lastDrawTime;
            lastroundsResults.GetChild(5).GetChild(0).GetComponent<TMP_Text>().text = lastDrawTime;
            lastroundsResults.GetChild(6).GetChild(0).GetComponent<TMP_Text>().text = lastDrawTime;
            lastroundsResults.GetChild(7).GetChild(0).GetComponent<TMP_Text>().text = lastDrawTime;
            lastroundsResults.GetChild(8).GetChild(0).GetComponent<TMP_Text>().text = lastDrawTime;
            lastroundsResults.GetChild(9).GetChild(0).GetComponent<TMP_Text>().text = lastDrawTime;


        }
        var time = DrawtimeCalculator(sec, min);
        if (time.Item1 == 0 && time.Item2 == 0)
        {
            print("something went wrong");
            return;
        }
        drawTime = cr.data.current_draw;
        //drawTime = time.Item1.ToString() + ":" + time.Item2.ToString();

        object gameIdObj = new { game_id = gameId };
        var currentRount = new GenricWebHandler<CurrentTimer>(gameIdObj);
        currentRount.SendRequestToServer(Constant.GET_TIMER_URL, OnCurrentTimer);
        UpdateUi();
    }

    void OnCurrentTimer(CurrentTimer obj)
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
        //this will set the advance bet toggle bet on the current draw time

        OnAdvanceBetting();
    }



    void OnAddBet(TMP_InputField selectedSpot, string betamount, int spotIndex)
    {
        TMP_Text placeholder = selectedSpot.transform.GetChild(0).GetChild(1).GetComponent<TMP_Text>();
        string spotName = placeholder.text;
        if (!isDataLoaded) return;
        //print($"spot name is: {spotName}  and spot No is {spotIndex} and amount is {betamount}");
        if (isAllSeriseSelected)
        {

            PlaceBetsOnAllSerise(selectedSpot, betamount, spotIndex, spotName);
            return;
        }

        if (string.IsNullOrEmpty(betamount) & betsContainer.ContainsKey(spotName))
        {
            betsContainer.Remove(spotName);
            betsInputFieldHolder.Remove(selectedSpot);
            totalSpots = betsInputFieldHolder.Count;
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
            serise = bettingSerise[currentSerise],
            ticketValue = betamount,
            spotNo = spotIndex
        };
        if (betsContainer.ContainsKey(spotName))
        {
            betsContainer[spotName] = betObj;
        }
        else
        {

            betsContainer.Add(spotName, betObj);
        }

        if (!betsInputFieldHolder.Contains(selectedSpot))
            betsInputFieldHolder.Add(selectedSpot);
        selectedSpot.GetComponent<Image>().color = Color.yellow;

        totalSpots = betsInputFieldHolder.Count;
        totalBets = betsContainer.Values.Sum(x => x.amount);
        UpdateUi();
    }
    void PlaceBetsOnAllSerise(TMP_InputField selectedSpot, string betamount, int spotIndex, string spotName)
    {
        if (string.IsNullOrEmpty(betamount) & betsContainer.ContainsKey(spotName))
        {
            for (int i = 0; i < bettingSerise.Length; i++)
            {
                //place bet on all the serise on same spot
                string spot_Name = string.Empty;
                if (spotIndex < 10)
                {
                    spot_Name = $"{bettingSerise[i]}0{spotIndex}";
                }
                else
                {
                    spot_Name = $"{bettingSerise[i]}{spotIndex}";
                }
                if (betsContainer.ContainsKey(spot_Name))
                    betsContainer.Remove(spot_Name);

            }
            if (betsInputFieldHolder.Contains(selectedSpot))
                betsInputFieldHolder.Remove(selectedSpot);
            totalSpots = betsContainer.Count;
            totalBets = betsContainer.Values.Sum(x => x.amount);
            selectedSpot.GetComponent<Image>().color = Color.white;
            UpdateUi();
            return;
        }

        int bet = int.Parse(betamount) * oneTicketPrize;
        for (int i = 0; i < bettingSerise.Length; i++)
        {
            //place bet on all the serise on same spot
            string spot_Name = string.Empty;
            if (spotIndex < 10)
            {
                spot_Name = $"{bettingSerise[i]}0{spotIndex}";
            }
            else
            {
                spot_Name = $"{bettingSerise[i]}{spotIndex}";
            }
            char serise = bettingSerise[i];
            BetInfo obj = new BetInfo()
            {
                amount = bet,
                serise = serise,
                ticketValue = betamount,
                spotNo = spotIndex
            };
            if (betsContainer.ContainsKey(spot_Name))
            {
                betsContainer[spot_Name] = obj;
            }
            else
            {
                betsContainer.Add(spot_Name, obj);
            }
        }
        if (!betsInputFieldHolder.Contains(selectedSpot))
            betsInputFieldHolder.Add(selectedSpot);
        selectedSpot.GetComponent<Image>().color = Color.yellow;
        totalSpots = betsContainer.Count;
        totalBets = betsContainer.Values.Sum(x => x.amount);
        UpdateUi();
    }
    void OnDrawResult(WinNo obj)
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
        string currentLetter = bettingSerise[currentSerise].ToString();
        int? winIndex = GetWinNoPoint(obj, currentLetter);
        if (winIndex == null)
        {
            Reload();
            return;
        }
        var data = obj.data;
        int[] winNos = new int[]{ data.win_no_A,data.win_no_B,data.win_no_C,data.win_no_D,data.win_no_E
        ,data.win_no_F,data.win_no_G,data.win_no_H,data.win_no_I,data.win_no_J};

        StopAllCoroutines();
        if (gameId == (int)GameIds.football)
        {
            screenController.Show(ScreenName.FootballScreen, data: winNos);
        }
        else
        {
            screenController.Show(ScreenName.HockeyScreen, data: winNos);
        };
    }
    int? GetWinNoPoint(WinNo obj, string currentLetter)
    {
        print("current letter " + currentLetter);
        if (currentLetter == "A")
        {
            return obj.data.win_no_A;
        }
        if (currentLetter == "B")
        {
            return obj.data.win_no_B;
        }
        if (currentLetter == "C")
        {
            return obj.data.win_no_C;
        }
        if (currentLetter == "D")
        {
            return obj.data.win_no_D;
        }
        if (currentLetter == "E")
        {
            return obj.data.win_no_E;
        }
        if (currentLetter == "F")
        {
            return obj.data.win_no_F;
        }
        if (currentLetter == "G")
        {
            return obj.data.win_no_G;
        }
        if (currentLetter == "H")
        {
            return obj.data.win_no_H;
        }
        if (currentLetter == "I")
        {
            return obj.data.win_no_I;
        }
        if (currentLetter == "J")
        {
            return obj.data.win_no_J;
        }
        return null;
    }
    void ValidateOrder()
    {
        List<Bet> bets;
        int totalBet;
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
            draw_time = isCoustmaizeTimeOn ? coustomizeDrawTime : drawTime,
            total_points = totalBet.ToString()
        };
        if (!isPreviousOrderDone)
        {
            Toast.instance.Show("please wait");
            return;
        }
        isPreviousOrderDone = false;
        //post Bets
        var postBets = new GenricWebHandler<BetValidation>(b);
        postBets.SendRequestToServer(Constant.SHOWCURRENT_BET_URL, OnBetValidation);
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
            print("printing");
            canStopTimer = true;
            PrintTicket(bet);
            print("printed");
            object gameIdObj = new { game_id = gameId };
            var currentRount = new GenricWebHandler<CurrentTimer>(gameIdObj);
            currentRount.SendRequestToServer(Constant.GET_TIMER_URL, OnCurrentTimer);
        }
        isPreviousOrderDone = true;
        ClearAllBets();
    }

    private void PrintTicket(BetValidation bet)
    {
        string DrawTime = isCoustmaizeTimeOn ? coustomizeDrawTime : drawTime;
        currentTicket = bet.data.barcode_no;
        Printer print = new Printer(currentTime, date,
            totalSpots.ToString(), DrawTime, currentTicket
            ,
            betsContainer, poolPrize, serviceCharge, (GameIds)gameId);
        recentPrint = print;
        balance = bet.data.cash_balance;
        print.Print();
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
    #endregion

    #region HelperFuctions
    void ClearAllBets()
    {
        foreach (var holder in betsInputFieldHolder.ToList())
        {
            holder.text = string.Empty;
            holder.GetComponent<Image>().color = Color.white;
        }
        isCoustmaizeTimeOn = false;
        betsInputFieldHolder.Clear();
        betsContainer.Clear();
        totalSpots = betsInputFieldHolder.Count;
        totalBets = betsContainer.Values.Sum(x => x.amount);

        OnAdvanceBetting();
        UpdateUi();
    }

    /// <summary>
    /// This will set the advance bets dynimcally
    /// </summary>
    private void SetTimers()
    {
        if (gameId == (int)GameIds.football)
        {
            int hr = 10;
            int min = 5;
            int offset = 15;
            int wholeHr = 60;
            foreach (Transform item in advanceTimerGameobject)
            {
                Text text = item.GetChild(1).GetComponent<Text>();

                string time = $"{hr}:{min}";
                if (min.ToString().ToArray().Length < 2)
                {
                    time = $"{hr}:0{min}";
                }
                text.text = time;
                min += offset;
                if (min > wholeHr)
                {
                    hr++;
                    min = 5;
                }
            }

        }

        if (gameId == (int)GameIds.hockey)
        {
            int hr = 10;
            int min = 10;
            int offset = 15;
            int wholeHr = 60;
            foreach (Transform item in advanceTimerGameobject)
            {
                Text text = item.GetChild(1).GetComponent<Text>();
                string time = $"{hr}:{min}";
                if (min.ToString().ToArray().Length < 2)
                {
                    time = $"{hr}:0{min}";
                }
                text.text = time;
                min += offset;
                if (min > wholeHr)
                {
                    hr++;
                    min = 10;
                }
            }

        }
    }
    float serviceCharge = 0;
    float poolPrize = 0;
    void UpdateUi()
    {
        int totalBets = betsContainer.Sum(x => x.Value.amount);
        serviceCharge = (totalBets * (poolPrizePc / 100));
        poolPrize = totalBets - serviceCharge;

        totalSelectedSpot.text = totalSpots.ToString();
        totalAmount.text = betsContainer.Values.Sum(x => x.amount).ToString();
        limit.text = "Limit: " + balance.ToString();
        poolPrizetxt.text = poolPrize.ToString();
        serviceChargetxt.text = serviceCharge.ToString();
    }
    void PrintBill()
    {
        if (recentPrint == null)
        {
            commonPopup.Show("No recent Ticket available");
            return;
        }
        recentPrint.Print();
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
        print("something went wrong");
        StopAllCoroutines();
        StopCoroutine(DisplayDate());
        StopCoroutine(DisplayCountDown());
        LoadDefaultData();
    }
    #endregion

    #region Coroutines
    IEnumerator DisplayDate()
    {
        date = $"{DateTime.Now.Day}/{DateTime.Now.Month}/{DateTime.Now.Year}";
        dateAndTime.text = DateTime.Now.ToShortTimeString().ToString() + " | " + date;
        currentTime = DateTime.Now.ToShortTimeString().ToString();
        yield return new WaitForSeconds(60 - DateTime.Now.Second);
        StartCoroutine(DisplayDate());
    }

    float miniutsLeft;
    float secLeft;
    bool canStopTimer;
    IEnumerator DisplayCountDown(int sec = 30, int min = 1, bool canStop = false)
    {
        while (min > -1)
        {
            if (canStopTimer)
            {
                print("can stop " + canStopTimer);
                yield break;
            }
            miniutsLeft = min;
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
        var winNo = new GenricWebHandler<WinNo>(round);
        winNo.SendRequestToServer(Constant.WIN_NO_URL, OnDrawResult);
    }
    #endregion

    void LoadDefaultData()
    {
        retailer = new Retailer()
        {
            game_id = gameId.ToString(),
            retailer_id =
            screenController.dataStorageController.LoginResponseData.data.retailer_id
        };
        totalSelectedSpot.text = totalSpots.ToString();
        recentPrint = null;
        isDataLoaded = false;
        isPreviousOrderDone = true;
        isAllSeriseSelected = false;
        currentTicket = string.Empty;
        toggleSerise[0].Select();
        toggleSerise[0].isOn = true;
        ClearAllBets();
        claimScreen.SetActive(false);
        advanceBettingPopup.SetActive(false);
        WebRequestHandler.instance.Post(Constant.CURRENT_ROUND_URL, JsonConvert.SerializeObject(retailer), (json, status) =>
        {
            if (!status)
            {
                print("somethng wnt wrng");
            }
            CurrentRound obj = JsonConvert.DeserializeObject<CurrentRound>(json);
            OnCurrentRound(obj);
        });
        var poolPrizeObj = new GenricWebHandler<PoolPrize>();
        poolPrizeObj.SendGetRequestToServer(Constant.POOL_PRIZE_URL, OnPoolPrize);

    }
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
    public override void Hide()
    {
        base.Hide();
        StopAllCoroutines();
    }
    public override void Show(object data = null)
    {
        base.Show();
        if (data != null)
        {
            gameId = (int)data;

        }
        if (gameId == 2) gameGrid = gameGridfootball;
        if (gameId == 3) gameGrid = gameGridhockey;
        SetTimers();
        animController.SetWindow(gameId);
        LoadDefaultData();
        if (retailer == null) return;
        StartCoroutine(DisplayDate());
    }

}


public class BetInfo
{
    public int amount;
    public string ticketValue;
    public char serise;
    public int spotNo;
}