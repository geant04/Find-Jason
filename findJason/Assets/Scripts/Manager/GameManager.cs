using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject Dynamic;
    public GameObject NPC;
    public static GameObject Jason;
    private List<GameObject> NPCs;

    void SpawnFriends()
    {
        int n = 30;
        int rand = Random.Range(0, n);

        for (int i = 0; i < n; i++)
        {
            GameObject npc = Instantiate(NPC, Dynamic.transform);
            Transform capsule = npc.transform.Find("Capsule");

            Vector3 randPos = Random.insideUnitSphere * 40.0f;
            randPos.y = 20.0f;
            npc.transform.position = randPos;

            Color randomColor = new Color();
            randomColor.r = Random.Range(0.0f, 1.0f);
            randomColor.g = Random.Range(0.0f, 1.0f);
            randomColor.b = Random.Range(0.0f, 1.0f);

            capsule.GetComponent<MeshRenderer>().material.SetColor("_Color", randomColor);

            if (i == rand)
            {
                Jason = npc;
                npc.GetComponent<NPC>().IsJason = true;
                npc.GetComponent<NPC>().Decorate();
                npc.name = "Jason";

                //capsule.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.blue);
            }

            NPCs.Add(npc);
        }
    }

    void Start()
    {
        NPCs = new List<GameObject>();
        SpawnFriends();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
