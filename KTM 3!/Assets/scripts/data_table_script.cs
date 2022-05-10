using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class data_table_script : MonoBehaviour
{
    public TextMeshProUGUI a; // times
    public TextMeshProUGUI b; // last moves

    private void Start()
    {
        data_script data = FindObjectOfType<data_script>();
        a.text = string.Join(",", data.times);
        b.text = string.Join(" ", data.lastmoves);
    }
}
