using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class WinCanvas : UICanvas
{
    [SerializeField] private Button collectBtn;
    [SerializeField] private Text dataMoney;

    [SerializeField] private Text recordTxt;
    [SerializeField] private Text moneyTxt;

    [SerializeField] private GameObject moneyCollectedEff;

    private bool isClick;

    private int earnedMoney;
    private bool isDone;

    private void OnEnable()
    {
        //AudioManager.Ins.PlaySFX(AudioManager.Ins.win);
        //Time.timeScale = 0f;
        Display();
    }

    private void OnDisable()
    {
        //Time.timeScale = 1f;
    }

    private void Start()
    {
        collectBtn.onClick.AddListener(() =>
        {
            if (isClick) return;
            isClick = true;
            collectBtn.interactable = false;

            //AudioManager.Ins.PlaySFX(AudioManager.Ins.click);
            moneyCollectedEff.gameObject.SetActive(true);
        });
    }

    private void Display()
    {
        moneyCollectedEff.SetActive(false);
        earnedMoney = UIManager.Ins.mainCanvas.GetValue();
        recordTxt.text = earnedMoney + "m";
        moneyTxt.text = "x" + earnedMoney.ToString();

        collectBtn.interactable = true;
        isDone = false;
        isClick = false;

        dataMoney.text = "x" + DataManager.Ins.Get<int>(AchivementType.Money).ToString();
    }

    public void GetMoney() //Use in UIParticle Attractor
    {
        if (!isDone)
        {
            isDone = true;
            moneyTxt.text = "x0";
            int currentMoney = DataManager.Ins.Get<int>(AchivementType.Money);
            int newMoney = currentMoney + earnedMoney;

            DataManager.Ins.Set(AchivementType.Money, newMoney);
            dataMoney.text = "x" + newMoney.ToString();
        }
    }

    public async void OpenUpgradeCanvas()
    {
        await NewTask();
    }

    private async Task NewTask()
    {
        await Task.Delay(TimeSpan.FromSeconds(2f));

        await UIManager.Ins.TransitionUI<ChangeUICanvas, WinCanvas>(0.6f,
              () =>
              {
                  //LevelManager.Ins.DespawnMap();
                  UIManager.Ins.OpenUI<UpgradeCanvas>();
              });
    }
}
