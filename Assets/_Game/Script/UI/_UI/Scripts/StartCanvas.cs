using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class StartCanvas : UICanvas
{
    [SerializeField] private Button startBtn;
    [SerializeField] private Text foamCount, record, money;

    [Header("---Music Button---")]
    [SerializeField] private Button soundBtn;
    [SerializeField] private Sprite[] spr;

    private CancellationTokenSource cts;

    private void OnEnable()
    {
        cts = new CancellationTokenSource();
        UpdateDataValue();
    }

    private void OnDestroy()
    {
        cts?.Cancel();
        cts?.Dispose();
    }

    private void Start()
    {
        startBtn.onClick.AddListener(async () =>
        {
            AudioManager.Ins.PlaySFX(AudioManager.Ins.click);

            await UIManager.Ins.TransitionUI<ChangeUICanvas, StartCanvas>(0.5f,
                () =>
                {
                    UIManager.Ins.OpenUI<UpgradeCanvas>();
                },
                cts.Token  // truyền token vào để có thể huỷ task
            );
        });

        soundBtn.onClick.AddListener(() =>
        {
            AudioManager.Ins.PlaySFX(AudioManager.Ins.click);

            if (AudioManager.Ins.IsMuted)
                AudioManager.Ins.TurnOn();
            else
                AudioManager.Ins.TurnOff();

            UpdateSoundIcon();
        });
    }

    private void UpdateDataValue()
    {
        foamCount.text = "x" + DataManager.Ins.Get<int>(AchivementType.FoamCount).ToString();
        record.text = DataManager.Ins.Get<int>(AchivementType.Record).ToString() + "m";
        money.text = "x" + DataManager.Ins.Get<int>(AchivementType.Money).ToString();
    }

    private void UpdateSoundIcon()
    {
        soundBtn.image.sprite = spr[AudioManager.Ins.IsMuted ? 1 : 0];
    }
}
