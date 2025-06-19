using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeCanvas : UICanvas
{
    [Header("===Button===")]
    [SerializeField] private Button startBtn;
    [SerializeField] private Button ammountBtn;
    [SerializeField] private Button powerBtn;
    [SerializeField] private Button firstShotBtn;

    [Header("===Data===")]
    [SerializeField] private Text foamCount;
    [SerializeField] private Text record;
    [SerializeField] private Text money;

    [SerializeField] private List<UpgradeBtn> upgradeList = new List<UpgradeBtn>();

    private void OnEnable()
    {
        UIManager.Ins.upgradeCanvas = this;
        UpdateDataValue();
        UpgradeBtn.OnPressBtn += UpdateButton;
    }

    private void OnDisable()
    {
        UpgradeBtn.OnPressBtn -= UpdateButton;
    }

    private void Start()
    {
        startBtn.onClick.AddListener(async () =>
        {
            //AudioManager.Ins.PlaySFX(AudioManager.Ins.click);

            await UIManager.Ins.TransitionUI<ChangeUICanvas, UpgradeCanvas>(0.5f,
                () =>
                {
                    UIManager.Ins.OpenUI<MainCanvas>();
                });
        });
    }

    public void UpdateDataValue()
    {
        foamCount.text = "x" + DataManager.Ins.Get<int>(AchivementType.FoamCount).ToString();
        record.text = DataManager.Ins.Get<int>(AchivementType.Record).ToString() + "m";
        money.text = "x" + DataManager.Ins.Get<int>(AchivementType.Money).ToString(); ;
    }

    private void UpdateButton()
    {
        foreach (var item in upgradeList)
        {
            item.RefreshButtonState();
        }
    }

    private void OnDrawGizmosSelected()
    {
        upgradeList.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            UpgradeBtn upgradeBtn = transform.GetChild(i).GetComponent<UpgradeBtn>();

            if (upgradeBtn != null)
            {
                upgradeList.Add(upgradeBtn);
            }
        }
    }
}
