using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainCanvas : UICanvas
{
    [SerializeField] private Text foamCount;
    [SerializeField] private Text record;

    private int meters; 

    private void OnEnable()
    {
        UIManager.Ins.mainCanvas = this;
        foamCount.text = "x" + DataManager.Ins.Get<int>(AchivementType.FoamCount).ToString();
        meters = 0;
        record.text = meters.ToString() + "m";
    }

    public int GetValue()
    {
        return meters;
    }

    public int SetValue(int newMeters)
    {
        meters = newMeters;
        record.text = meters.ToString() + "m"; // Update UI tại đây
        return meters;
    }

    public void SetFoamCount(int count)
    {
        foamCount.text = "x" + count.ToString();
    }

}
