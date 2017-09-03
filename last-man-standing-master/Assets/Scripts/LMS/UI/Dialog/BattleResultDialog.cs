using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleResultDialog : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI TextResult;


    public void ShowDialog(bool isWin)
    {
        TextResult.text = isWin ? "WIN" : "LOSE";
        gameObject.SetActive(true);
    }

}
