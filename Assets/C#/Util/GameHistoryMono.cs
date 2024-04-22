using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameHistoryMono : MonoBehaviour
{
    public TMP_Text date;
    public TMP_Text A;
    public TMP_Text B;
    public TMP_Text C;
    public TMP_Text D;
    public TMP_Text E;
    public TMP_Text F;
    public TMP_Text G;
    public TMP_Text H;
    public TMP_Text I;
    public TMP_Text J;

    public void SetData(GameReport obj)
    {
        date.text = obj.date+" | "+obj.draw_time;
        A.text = "A: "+obj.win_no_A;
        B.text = "B: "+obj.win_no_B;
        C.text = "C: "+obj.win_no_C;
        D.text = "D: "+obj.win_no_D;
        E.text = "E: "+obj.win_no_E;
        F.text = "F: "+obj.win_no_F;
        G.text = "G: "+obj.win_no_G;
        H.text = "H: "+obj.win_no_H;
        I.text = "I: "+obj.win_no_I;
        J.text = "J: " + obj.win_no_J;
    }
}
