using UnityEngine;

public class Level : MonoBehaviour
{
    public Extinguisher extinguisher;

    private float startX;
    private float maxDistance;

    private void OnEnable()
    {
        CameraController.Ins.extinguisher = extinguisher.transform;
        startX = extinguisher.transform.position.x;
        maxDistance = 0f;
    }

    private void Update()
    {
        float currentX = extinguisher.transform.position.x;
        float distance = currentX - startX;

        // Chỉ cập nhật nếu tiến về phía trước (x tăng)
        if (distance > maxDistance)
        {
            maxDistance = distance;

            // Làm tròn và quy đổi ra "mét"
            int meters = Mathf.FloorToInt(maxDistance);
            UIManager.Ins.mainCanvas.SetValue(meters);

            // Nếu vượt qua kỷ lục cũ thì lưu và cập nhật UI
            int currentRecord = DataManager.Ins.Get<int>(AchivementType.Record);
            if (meters > currentRecord)
            {
                DataManager.Ins.Set(AchivementType.Record, meters);
            }
        }
    }
}
 