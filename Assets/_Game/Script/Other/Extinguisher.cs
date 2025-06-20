using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class Extinguisher : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float thrustForce;
    [SerializeField] private float rotateSpeed = 150f; // Tốc độ xoay độ/giây
    [SerializeField] private GameObject smokeEff;
    [SerializeField] private int foamCount;

    private bool hasFiredFirstShot = false;
    private bool isWin;
    private CancellationTokenSource cts;

    private void Start()
    {
        foamCount = DataManager.Ins.Get<int>(AchivementType.FoamCount);
        thrustForce = DataManager.Ins.Get<int>(AchivementType.Power);
        smokeEff.gameObject.SetActive(false);
        cts = new CancellationTokenSource();
    }

    private void OnDestroy()
    {
        cts?.Cancel();
        cts?.Dispose();
    }

    async void Update()
    {
        float speedFactor = rb.velocity.magnitude;
        float currentRotateSpeed = rotateSpeed;

        // Nếu đang bay (có vận tốc đáng kể) thì tăng tốc độ xoay
        if (speedFactor > 0.1f)
        {
            currentRotateSpeed += speedFactor * 5f;
        }

        transform.Rotate(0f, 0f, currentRotateSpeed * Time.deltaTime);

        // Xử lý bắn nếu chạm hoặc click
        if (IsClicked() && foamCount > 0)
        {
            AddThrust();
            foamCount--;

            AudioManager.Ins.PlaySFX(AudioManager.Ins.spray);
            smokeEff.SetActive(true);
            Invoke(nameof(DisableSmokeEff), 0.5f); // Tắt hiệu ứng sau 0.5 giây

            UIManager.Ins.mainCanvas.SetFoamCount(foamCount);
            Debug.Log("Foam còn lại: " + foamCount);
        }

        if (foamCount == 0 && !isWin)
        {
            await DelayTask(cts.Token);
            isWin = true;
        }
    }

    private bool IsClicked()
    {
        // PC
        if (Input.GetMouseButtonDown(0)) return true;

        // Mobile
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) return true;

        return false;
    }


    private void DisableSmokeEff()
    {
        smokeEff.SetActive(false);
    }

    private async Task DelayTask(CancellationToken token)
    {
        try
        {
            await Task.Delay(TimeSpan.FromSeconds(4f), token);
            UIManager.Ins.CloseUI<MainCanvas>();
            UIManager.Ins.OpenUI<WinCanvas>();
        }
        catch (TaskCanceledException)
        {
            Debug.Log("DelayTask in Extinguisher was canceled.");
        }
    }

    private void AddThrust()
    {
        float angleZ = transform.eulerAngles.z;
        float angleRad = angleZ * Mathf.Deg2Rad;

        Vector2 forceDir = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));

        // Lực từ DataManager
        int power = hasFiredFirstShot
            ? DataManager.Ins.Get<int>(AchivementType.Power)
            : DataManager.Ins.Get<int>(AchivementType.FirstShotPower);

        Vector2 force = forceDir * power;

        rb.AddForce(force, ForceMode2D.Impulse);
        rb.gravityScale = 1;

        hasFiredFirstShot = true; // đánh dấu là đã bắn phát đầu

        Debug.Log($"Góc Z: {angleZ:F1} | Lực phun: {force}");
        Debug.DrawRay(transform.position, force, Color.red);
    }

}
