using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public float velocityOfChunk = 5; //Velocity of this chunk
    public int spawnScreenPos = 1;

    public Transform destroyDetector;

    private void Update()
    {
        if(destroyDetector.position.y < WorldElements.Instance.chunkDestroyY)
        {
            WorldElements.Instance.DestroyChunk(this);
        }
    }
}
