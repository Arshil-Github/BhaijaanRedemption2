using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WorldElements : MonoBehaviour
{
    //The function is to move all its children down by certain speed
    //When reached a certian y value destroy them
    //Switch to pause unpause
    //Spawning Chunks

    public static WorldElements Instance;

    [SerializeField] [Tooltip("Give Chunks in order of spawn")] private List<GameObject> pf_chunks;
    [SerializeField] public float chunkDestroyY = -12;


    private int _currentChunkIndex = 0;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        SpawnChunk();
    }

    private void AssignVelocity(float vel)
    {
        //For assigning Velocity to a chunk's Elements
        foreach (Transform child in transform)
        {
            //Getting Rigidbody
            Rigidbody2D rb;
            if(child.TryGetComponent(out rb)) rb = child.GetComponent<Rigidbody2D>();
            else rb = child.AddComponent<Rigidbody2D>();


            int forceDir = -1;//For Controlling Direction

            rb.gravityScale = 0;
            rb.velocity = Vector3.zero;
            rb.velocity = forceDir * vel * transform.up;
        }
    }

    private void SpawnChunk()
    {
        Chunk chunkInfo = pf_chunks[_currentChunkIndex].GetComponent<Chunk>();

        //Decide how many screens up should it spawn
        int spawnPosScreen = chunkInfo.spawnScreenPos;
        Vector2 spawnPos = new Vector2(0, Camera.main.orthographicSize * (spawnPosScreen + 1));

        GameObject spawnedChunk = Instantiate(pf_chunks[_currentChunkIndex], spawnPos, Quaternion.identity);
        spawnedChunk.transform.parent = transform;

        AssignVelocity(chunkInfo.velocityOfChunk);

        _currentChunkIndex ++;

    }

    public void DestroyChunk(Chunk chunkToDestroy)
    {
        Destroy(chunkToDestroy.gameObject);

        if(_currentChunkIndex < pf_chunks.Count)
        {
            SpawnChunk();
        }
        else
        {
            Debug.Log("Level Complete");
        }
    }
}
