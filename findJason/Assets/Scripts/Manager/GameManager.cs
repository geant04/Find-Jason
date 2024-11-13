using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject Dynamic;
    public GameObject NPC;
    public GameObject CameraController;
    public static GameObject Jason;

    private NPCSpawner npcSpawner;

    public static void AssignJason(GameObject j)
    {
        Jason = j;
    }

    void Start()
    {
        npcSpawner = new NPCSpawner(Dynamic.transform, this);
        npcSpawner.SpawnFriendsAndJason(90);
        npcSpawner.SpawnIdleCrowds(4);
    }

    void Update()
    {
        
    }
}
