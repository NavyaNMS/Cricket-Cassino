using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

class ReportScreen : Screen
{
    string dateFrom;
    string dateTo;
    string claimAmount;
    string netPayAmount;
    string totalSale;
    public Calendar fromCalandar;
    public Calendar toCalandar;
    public int gameid;
    [SerializeField] TMP_Text dateFromTxt;
    [SerializeField] TMP_Text dateToTxt;
    [SerializeField] TMP_Text claimTxt;
    [SerializeField] TMP_Text netPayTxt;
    [SerializeField] TMP_Text totalSaleTxt;

    [SerializeField] TMP_Text fromTxt;
    [SerializeField] TMP_Text toTxt;
    [SerializeField] Button backBtn;
    [SerializeField] Button applyBtn;
    [SerializeField] Button fromBtn;
    [SerializeField] Button toBtn;
    public override ScreenName ScreenID => ScreenName.reportScreen;


    public override void Initialize(ScreenController screenController)
    {
        base.Initialize(screenController);
        backBtn.onClick.AddListener(() =>
        {

            if (gameid == 1)
            {
                screenController.Show(ScreenName.criketScreen);
                return;
            }
            screenController.ShowBettingScreen(gameid);
        });
        applyBtn.onClick.AddListener(DateValidation);

        fromCalandar.onClickOnDate = OnClickOnFromCalander;
        toCalandar.onClickOnDate = OnClickOnToCalander;
        fromBtn.onClick.AddListener(fromCalandar.ShowCalendar);
        toBtn.onClick.AddListener(toCalandar.ShowCalendar);

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

    public override void Show(object data = null)
    {
        base.Show();
        if (data != null) gameid = (int)data;
    }

    void FeachData()
    {
        object retailer = new { retailer_id = screenController.dataStorageController.LoginResponseData.data.retailer_id };
        var report = new GenricWebHandler<Report>(retailer);
        report.SendRequestToServer(Constant.REPORT_URL, OnReport);
    }

    void OnReport(Report obj)
    {
        if (obj == null || obj.status != 200)
        {
            commonPopup.Show();
            return;
        }
        dateFrom = obj.data.sale_report_from;
        dateTo = obj.data.sale_data_upto;
        totalSale = obj.data.total_sale.ToString();
        claimAmount = obj.data.claim_amount.ToString();
        netPayAmount = obj.data.net_pay_amount.ToString();
        UpdateUI();
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
        RetailerReport info = new RetailerReport()
        {
            to_date = todateHolder,
            from_date = fromdateHolder,
            retailer_id = screenController.dataStorageController.LoginResponseData.data.retailer_id,
        };
        var obj = new GenricWebHandler<Report>(info);
        obj.SendRequestToServer(Constant.REPORT_URL, OnReport);
    }

    void UpdateUI()
    {
        dateFromTxt.text = "From :" + dateFrom;
        totalSaleTxt.text = "Total Sale:" + totalSale.ToString();
        claimTxt.text = "Claim Amount:" + claimAmount.ToString();
        dateToTxt.text = "To :" + dateTo;
        netPayTxt.text = "Net Pay Amount:" + netPayAmount.ToString();
    }
    public override void Hide()
    {
        base.Hide();
    }
}
public class ReportData
{
    public string sale_report_from;
    public string sale_data_upto;
    public int total_sale;
    public int claim_amount;
    public int net_pay_amount;
}

public class Report
{
    public int status;
    public string message;
    public ReportData data;
}

public class RetailerReport
{
    public string retailer_id;
    public string from_date;
    public string to_date;
}