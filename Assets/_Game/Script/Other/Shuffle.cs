using System.Collections.Generic;
using UnityEngine;

public class Shuffle : MonoBehaviour
{
    [SerializeField] private List<Transform> blocks;
    private Queue<Transform> blockQueue = new Queue<Transform>();

    [SerializeField] private float moveOffset = 5.6f;

    private Camera cam;

    private void Start()
    {
        cam = Camera.main;

        foreach (Transform block in blocks)
        {
            blockQueue.Enqueue(block);
        }
    }

    private void Update()
    {
        float camLeftEdge = cam.transform.position.x - cam.orthographicSize * cam.aspect;
        float camRightEdge = cam.transform.position.x + cam.orthographicSize * cam.aspect;

        // Kiểm tra block đầu tiên nếu nằm ngoài bên trái quá xa → dịch sang phải
        while (blockQueue.Peek().position.x + moveOffset < camLeftEdge)
        {
            Transform firstBlock = blockQueue.Dequeue();
            Transform lastBlock = GetLastBlock();

            Vector3 newPos = lastBlock.position + new Vector3(moveOffset, 0f, 0f);
            firstBlock.position = newPos;
            blockQueue.Enqueue(firstBlock);
        }

        // Ngược lại nếu cần (nếu đi trái cũng cần loop lại block)
        while (GetLastBlock().position.x - moveOffset > camRightEdge)
        {
            // Di chuyển phần tử cuối về trước
            Transform[] arr = blockQueue.ToArray();
            blockQueue.Clear();

            Transform last = arr[arr.Length - 1];
            Vector3 newPos = arr[0].position - new Vector3(moveOffset, 0f, 0f);
            last.position = newPos;

            blockQueue.Enqueue(last);
            for (int i = 0; i < arr.Length - 1; i++)
                blockQueue.Enqueue(arr[i]);
        }
    }

    private Transform GetLastBlock()
    {
        Transform last = null;
        foreach (var b in blockQueue)
        {
            last = b;
        }
        return last;
    }
}
