using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartCanvas : UICanvas
{
    [SerializeField] private Button startBtn;
    [SerializeField]
    private Text foamCount, record, money;

    private void OnEnable()
    {
        UpdateDataValue();
    }

    private void Start()
    {
        startBtn.onClick.AddListener(async () =>
        {
            //AudioManager.Ins.PlaySFX(AudioManager.Ins.click);

            await UIManager.Ins.TransitionUI<ChangeUICanvas, StartCanvas>(0.5f,
                () =>
                {
                    UIManager.Ins.OpenUI<UpgradeCanvas>();
                });
        });
    }

    private void UpdateDataValue()
    {
        foamCount.text = "x" + DataManager.Ins.Get<int>(AchivementType.FoamCount).ToString();
        record.text = DataManager.Ins.Get<int>(AchivementType.Record).ToString() + "m";
        money.text = "x" + DataManager.Ins.Get<int>(AchivementType.Money).ToString(); ;
    }
}
