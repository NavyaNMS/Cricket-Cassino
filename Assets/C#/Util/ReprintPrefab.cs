
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ReprintPrefab : MonoBehaviour
{
   [SerializeField] TMP_Text SrNo;
   [SerializeField] TMP_Text time;
   [SerializeField] TMP_Text ticketId;
   [SerializeField] Button printBtn;
    Printer pr;
    public void Initialize(int srNo,string time, string ticketId,Printer pr)
    {
        SrNo.text = srNo.ToString();
        this.time.text = time;
        this.ticketId.text = ticketId;
        printBtn.onClick.AddListener(pr.Print);
    }
}
