using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RetailerReportScreen : Screen
{
    string dateFrom;
    string dateTo;
    string claimAmount;
    string netPayAmount;
    string totalSale;

    public int gameid;
    [SerializeField] TMP_Text dateFromTxt;
    [SerializeField] TMP_Text dateToTxt;
    [SerializeField] TMP_Text claimTxt;
    [SerializeField] TMP_Text netPayTxt;
    [SerializeField] TMP_Text totalSaleTxt;

    [SerializeField] Button backBtn;
    public override ScreenName ScreenID => ScreenName.RetailerReport;

    public override void Hide()
    {
        base.Hide();
    }

    public override void Initialize(ScreenController screenController)
    {
        base.Initialize(screenController);
    }

   
    void UpdateUI()
    {
        dateFromTxt.text = "From :" + dateFrom;
        totalSaleTxt.text = "Total Sale:" + totalSale.ToString();
        claimTxt.text = "Claim Amount:" + claimAmount.ToString();
        dateToTxt.text = "To :" + dateTo;
        netPayTxt.text = "Net Pay Amount:" + netPayAmount.ToString();
    }
    public override void Show(object data = null)
    {
        base.Show(data);
        if (data != null) gameid = (int)data;
        FeachData();
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

}
