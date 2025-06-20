using System;
using System.Threading;
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
    private bool flag;

    private CancellationTokenSource cts;

    private void OnEnable()
    {
        cts = new CancellationTokenSource();

        AudioManager.Ins.PlaySFX(AudioManager.Ins.win);

        Display();
        LevelManager.Ins.DespawmLevel();
    }

    private void OnDestroy()
    {
        cts?.Cancel();
        cts?.Dispose();
        cts = null;
    }


    private void Start()
    {
        collectBtn.onClick.AddListener(() =>
        {
            if (isClick) return;
            isClick = true;
            collectBtn.interactable = false;

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
        flag = true;

        dataMoney.text = "x" + DataManager.Ins.Get<int>(AchivementType.Money).ToString();
    }

    public void GetMoney() // Use in UIParticle Attractor
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
        if (cts == null || cts.IsCancellationRequested)
        {
            Debug.LogWarning("WinCanvas.NewTask() bị gọi sau khi đã bị huỷ hoặc dispose.");
            return;
        }

        try
        {
            await Task.Delay(TimeSpan.FromSeconds(4f), cts.Token);

            if (cts == null || cts.IsCancellationRequested)
            {
                Debug.Log("WinCanvas transition bị huỷ giữa chừng.");
                return;
            }

            await UIManager.Ins.TransitionUI<ChangeUICanvas, WinCanvas>(0.6f,
                () =>
                {
                    if (cts == null || cts.IsCancellationRequested) return;

                    LevelManager.Ins.DespawmLevel();
                    UIManager.Ins.OpenUI<UpgradeCanvas>();
                },
                cts.Token
            );
        }
        catch (TaskCanceledException)
        {
            Debug.Log("WinCanvas task was canceled.");
        }
        catch (ObjectDisposedException)
        {
            Debug.LogWarning("WinCanvas task bị gọi sau khi CTS đã Dispose.");
        }
    }


    public void PlaySFX()
    {
        if (!flag)
            return;
        flag = false;
        AudioManager.Ins.PlaySFX(AudioManager.Ins.coin);
    }
}
