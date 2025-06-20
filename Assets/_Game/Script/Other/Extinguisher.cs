using UnityEngine;

public class Extinguisher : MonoBehaviour
{
        public Rigidbody2D extinguisherBody; // gán từ Inspector
        public float thrustForce = 3f;
        public Transform nozzlePoint;

        void Update()
        {
        if (Input.GetMouseButton(0))
        {
            Vector2 forceDir = nozzlePoint.up;

            // Thêm lực đẩy chính giữa bình
            extinguisherBody.AddForce(forceDir * thrustForce, ForceMode2D.Force);

            // Cộng thêm torque nhẹ để tạo xoay vừa phải
            extinguisherBody.AddTorque(0.5f, ForceMode2D.Force);
        }

    }
}
