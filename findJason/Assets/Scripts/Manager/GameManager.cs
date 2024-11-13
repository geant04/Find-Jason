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

    // Insane ideas doc... people will call me crazy...
    /*
     * - Multiple Jasons????? What?? Gameplay will vary like crazy...
     * 
     * - Squadrons of people, like a crowd following one leader
     * - ^^ fake jason, looks like jason but is not!!! 
     * - secret service?? they travel as a pack
     * 
     * - Parents and children. ^^^ essentially re-use the same system, and you can also use this to make people follow Jason
     * - - also bcuz the tiny capsule pills are funny
     * 
     * - Jason cult people that literally only exist to chase Jason
     * 
     * - Props for people to actually use
     * 
     * - Interactive items in the world that NPCs can randomly use if they feel like it, make a mission for it like "go buy a hot dog"
     * 
     * - Hot dog seller NPC that just sells hot dogs, very klay world, very hot dog.
     * 
     * - construction worker NPC
     * 
     * NPC decorations to add
     * 
     * - cool baseball cap hat
     * - fedora hat, very roblox
     * - shirts, many shirts cool shirts
     * - pants pants pants
     * - something jason esque, i'll have to ask him
     * - sunglasses
     * - accessories for people to hold like an ice cream cone for the kids or something
     * - sunhat
     * - umbrella
     */
}
