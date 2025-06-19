using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseCanvas : UICanvas
{
    [Header("---Other Button---")]
    [SerializeField] private Button continueBtn;
    [SerializeField] private Button menuBtn;
    [SerializeField] private Button retryBtn;

    [Header("---Music Button---")]
    [SerializeField] private Button soundBtn;
    [SerializeField] private Sprite[] spr;

    private bool isClick;

    private void OnEnable()
    {
        UpdateSoundIcon();
        Time.timeScale = 0f;
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
    }


    private void Start()
    {
        continueBtn.onClick.AddListener(() =>
        {
            //AudioManager.Ins.PlaySFX(AudioManager.Ins.click);
            UIManager.Ins.CloseUI<PauseCanvas>();
            UIManager.Ins.OpenUI<WinCanvas>();
        });

        menuBtn.onClick.AddListener(async () =>
        {
            //AudioManager.Ins.PlaySFX(AudioManager.Ins.click);

            await UIManager.Ins.TransitionUI<ChangeUICanvas, PauseCanvas>(0.6f,
                () =>
                {
                    //LevelManager.Ins.DespawnMap();
                    UIManager.Ins.OpenUI<StartCanvas>();
                });
        });

        retryBtn.onClick.AddListener(async () =>
        {
            //AudioManager.Ins.PlaySFX(AudioManager.Ins.click);

            await UIManager.Ins.TransitionUI<ChangeUICanvas, PauseCanvas>(0.6f,
                () =>
                {
                    UIManager.Ins.CloseUI<PauseCanvas>();
                    UIManager.Ins.OpenUI<MainCanvas>();
                });
        });

        soundBtn.onClick.AddListener(() =>
        {
            // AudioManager.Ins.PlaySFX(AudioManager.Ins.click);

            if (AudioManager.Ins.IsMuted)
                AudioManager.Ins.TurnOn();
            else
                AudioManager.Ins.TurnOff();

            UpdateSoundIcon();
        });
    }

    private void UpdateSoundIcon()
    {
        soundBtn.image.sprite = spr[AudioManager.Ins.IsMuted ? 1 : 0];
    }
}
