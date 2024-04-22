using UnityEngine;

public class PrintGuiText : MonoBehaviour
{
    string sample;
    public static PrintGuiText instance;
    private void Start()
    {
        instance = this;
    }
    //GUIStyle style = new GUIStyle();
    //private void OnGUI()
    //{
    //    style.fontSize = 50;
    //    style.normal.textColor = Color.white;
    //    GUI.Label(new Rect(100, 100, 100, 20), "test: " + sample, style);
    //}
    public void Print(string s)
    {
        sample = s;
    }
}
