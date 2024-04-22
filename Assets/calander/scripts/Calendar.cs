using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Calendar : MonoBehaviour
{
    public GameObject _calendarPanel;
    public Text _yearNumText;
    public Text _monthNumText;

    public TMP_Dropdown month_dd;
    public List<GameObject> _dateItems = new List<GameObject>();
    const int _totalDateNum = 42;

    private DateTime _dateTime;

    void Start()
    {
        month_dd.onValueChanged.AddListener((index) => { SetMonth(index+1); });
        _calendarPanel.SetActive(false);
    }

    void SetMonth(int monthNo)
    {
        int currentMonth = _dateTime.Month;
        int newMonth = currentMonth > monthNo ? -(currentMonth - monthNo) : (monthNo-currentMonth);
        print($"new month is {newMonth}");
        _dateTime=_dateTime.AddMonths(newMonth);
        CreateCalendar();
    }
    void CreateCalendar()
    {
        DateTime firstDay = _dateTime.AddDays(-(_dateTime.Day - 1));
        int index = GetDays(firstDay.DayOfWeek);
        print(index);
        int date = 0;
        for (int i = 0; i < _totalDateNum; i++)
        {
            Text label = _dateItems[i].GetComponentInChildren<Text>();
            //_dateItems[i].SetActive(false);
            _dateItems[i].GetComponent<Image>().enabled = false;
            _dateItems[i].GetComponentInChildren<Text>().enabled = false;

            if (i >= index)
            {
                DateTime thatDay = firstDay.AddDays(date);
                if (thatDay.Month == firstDay.Month)
                {
                    _dateItems[i].GetComponent<Image>().enabled = true;
                    _dateItems[i].GetComponentInChildren<Text>().enabled = true;

                    label.text = (date + 1).ToString();
                    date++;
                }
            }
        }
        year = _dateTime.Year;
        month = _dateTime.Month;
        _yearNumText.text = _dateTime.Year.ToString();
        _monthNumText.text = GetMonth(_dateTime.Month);
    }
    string GetMonth(int no)
        {
        switch (no)
        {
            case 1: return Year.January.ToString();
            case 2: return Year.February.ToString();
            case 3: return Year.March.ToString();
            case 4: return Year.April.ToString();
            case 5: return Year.May.ToString();
            case 6: return Year.June.ToString();
            case 7: return Year.July.ToString();
            case 8: return Year.August.ToString();
            case 9: return Year.September.ToString();
            case 10: return Year.October.ToString();
            case 11: return Year.November.ToString();
            case 12: return Year.December.ToString();
        }
        return string.Empty;
    }
    int GetDays(DayOfWeek day)
    {
        switch (day)
        {
            case DayOfWeek.Monday: return 1;
            case DayOfWeek.Tuesday: return 2;
            case DayOfWeek.Wednesday: return 3;
            case DayOfWeek.Thursday: return 4;
            case DayOfWeek.Friday: return 5;
            case DayOfWeek.Saturday: return 6;
            case DayOfWeek.Sunday: return 0;
        }

        return 0;
    }
    public void YearPrev()
    {
        _dateTime = _dateTime.AddYears(-1);
        CreateCalendar();
    }

    public void YearNext()
    {
        _dateTime = _dateTime.AddYears(1);
        CreateCalendar();
    }

    public void MonthPrev()
    {
        _dateTime = _dateTime.AddMonths(-1);
        CreateCalendar();
    }

    public void MonthNext()
    {
        _dateTime = _dateTime.AddMonths(1);
        CreateCalendar();
    }

    public void ShowCalendar()
    {
        _calendarPanel.SetActive(true);
        _calendarPanel.transform.position = transform.position;
        _dateTime = DateTime.Now;
        month_dd.SetValueWithoutNotify(_dateTime.Month-1);
        CreateCalendar();
    }

    public Action<int, int, int> onClickOnDate;
    int year;
    int month;
    public void OnDateItemClick(string day)
    {
        int Y = year;
        int D = int.Parse(day);
        int M = month;
        onClickOnDate(D, M, Y);
        _calendarPanel.SetActive(false);
    }
}
public enum Year
{
    January=1,
    February = 2, 
    March=3,
    April=4,
    May=5,
    June=6,
    July=7,
    August=8,
    September=9,
    October=10,
    November=11,
    December=12
}