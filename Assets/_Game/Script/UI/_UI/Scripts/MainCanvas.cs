using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainCanvas : UICanvas
{
    [SerializeField] private Button pauseBtn;
    [SerializeField] private Text foamCount;
    [SerializeField] private Text record;

    private int meters; 

    private void OnEnable()
    {
        UIManager.Ins.mainCanvas = this;
        foamCount.text = "x" + DataManager.Ins.Get<int>(AchivementType.FoamCount).ToString();
        meters = 100;
        record.text = meters.ToString() + "m";
    }

    private void Start()
    {
        pauseBtn.onClick.AddListener(() =>
        {
            //AudioManager.Ins.PlaySFX(AudioManager.Ins.click);
            UIManager.Ins.OpenUI<PauseCanvas>();
            UIManager.Ins.CloseUI<MainCanvas>();
        });
    }

    public int GetValue()
    {
        return meters;
    }

    public int SetValue(int newMeters)
    {
        meters = newMeters;
        return meters;
    }
}
