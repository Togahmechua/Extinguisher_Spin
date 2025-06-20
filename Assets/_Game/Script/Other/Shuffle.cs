using System.Collections.Generic;
using UnityEngine;

public class Shuffle : MonoBehaviour
{
    [SerializeField] private List<Transform> blocks; // Gán 6 prefab từ Editor vào
    private Queue<Transform> blockQueue = new Queue<Transform>();

    [SerializeField] private float moveOffset = 5.6f;   // khoảng cách giữa các block
    [SerializeField] private float triggerDistance = 8.9f; // khoảng cách camera cần đi để trigger

    private Vector3 lastCamPos;

    private void Start()
    {
        foreach (Transform block in blocks)
        {
            blockQueue.Enqueue(block);
        }

        lastCamPos = Camera.main.transform.position;
    }

    private void Update()
    {
        Vector3 currentCamPos = Camera.main.transform.position;
        float deltaX = currentCamPos.x - lastCamPos.x;

        if (Mathf.Abs(deltaX) >= triggerDistance)
        {
            if (deltaX > 0f)
            {
                // Đi sang phải → chuyển block đầu lên sau
                Transform firstBlock = blockQueue.Dequeue();
                Transform lastBlock = GetLastBlock();

                Vector3 newPos = lastBlock.position + new Vector3(moveOffset, 0f, 0f);
                firstBlock.position = newPos;

                blockQueue.Enqueue(firstBlock);
            }
            else
            {
                // Đi sang trái → chuyển block cuối lên trước
                Transform[] blockArray = blockQueue.ToArray();
                blockQueue.Clear();

                Transform lastBlock = blockArray[blockArray.Length - 1];
                Vector3 newPos = blockArray[0].position - new Vector3(moveOffset, 0f, 0f);
                lastBlock.position = newPos;

                blockQueue.Enqueue(lastBlock);
                for (int i = 0; i < blockArray.Length - 1; i++)
                {
                    blockQueue.Enqueue(blockArray[i]);
                }
            }

            lastCamPos = currentCamPos;
        }
    }

    private Transform GetLastBlock()
    {
        Transform last = null;
        foreach (Transform block in blockQueue)
        {
            last = block;
        }
        return last;
    }
}
