using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeBtn : MonoBehaviour
{
    [SerializeField] private AchivementType achivementType;
    [SerializeField] private int cost; 

    private Text childTxt;
    private Button thisBtn;
    private bool initialized;

    public static event Action OnPressBtn;

    private void OnEnable()
    {
        if (initialized)
        {
            RefreshButtonState();
        }
    }

    private void Start()
    {
        initialized = true;

        thisBtn = GetComponent<Button>();
        childTxt = GetComponentInChildren<Text>();

        if (childTxt != null)
        {
            int.TryParse(childTxt.text, out cost);
        }

        RefreshButtonState();

        thisBtn.onClick.AddListener(() =>
        {
            Buy();
        });
    }

    public void RefreshButtonState()
    {
        int currentMoney = DataManager.Ins.Get<int>(AchivementType.Money);
        thisBtn.interactable = currentMoney >= cost;
    }

    private void Buy()
    {
        int curMoney = DataManager.Ins.Get<int>(AchivementType.Money);
        if (curMoney < cost) return;

        curMoney -= cost;
        DataManager.Ins.Set(AchivementType.Money, curMoney);

        switch (achivementType)
        {
            case AchivementType.FoamCount:
                int foamCount = DataManager.Ins.Get<int>(AchivementType.FoamCount);
                foamCount += 1;
                DataManager.Ins.Set(AchivementType.FoamCount, foamCount);
                break;

            case AchivementType.Power:
                int power = DataManager.Ins.Get<int>(AchivementType.Power);
                power += 5;
                DataManager.Ins.Set(AchivementType.Power, power);
                break;

            case AchivementType.FirstShotPower:
                int fPower = DataManager.Ins.Get<int>(AchivementType.FirstShotPower);
                fPower += 5;
                DataManager.Ins.Set(AchivementType.FirstShotPower, fPower);
                break;
        }

        UIManager.Ins.upgradeCanvas.UpdateDataValue();
        OnPressBtn?.Invoke();
    }

}


public enum EPower
{
    Amount,
    Power,
    FirstShotPower
}