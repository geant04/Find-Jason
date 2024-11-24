using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject Dynamic;
    public GameObject NPC;
    public GameObject CameraController;
    private CameraTargetDetect CameraTargeter;
    public static GameObject Jason;

    private NPCSpawner npcSpawner;
    private static Timer timer;
    public UIController uiController;
    public bool run = true;

    public void Click()
    {
        timer.SetActive(false);
        float time = timer.GetTime();

        AudioClass.Instance.sfxSource.volume = 0.25f;
        AudioClass.Instance.sfxSource.PlayOneShot(AudioClips.Instance.cameraClick);
        AudioClass.Instance.sfxSource.volume = 1.00f;
        NPC[] npcs = Dynamic.GetComponentsInChildren<NPC>();

        for (int i = 0; i < npcs.Length; i++)
        {
            npcs[i].disable = true;
            npcs[i].NavMeshAgent.speed = 0.0f;
        }

        Debug.Log("Done, found jason in " + time + " seconds");

        StartCoroutine(ClickCooldown());
    }

    public void ClearDynamic()
    {
        for (int i = Dynamic.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(Dynamic.transform.GetChild(i).gameObject);
        }
    }

    public void GenerateWorld()
    {
        bool debug = false;
        int n = !debug ? 90 : 2;
        int ic = !debug ? 4 : 0;

        npcSpawner = new NPCSpawner(Dynamic.transform, this);
        npcSpawner.SpawnFriendsAndJason(n);
        npcSpawner.SpawnIdleCrowds(ic);

        if (Jason != null && Jason.GetComponent<NPC>())
        {
            Color color = Jason.GetComponent<NPC>().color;
            color *= 0.8f; // to account for the post-processing unfortunately
            color.a = 1.0f;
            uiController.SetColor(color);
        }
    }

    public IEnumerator ClickCooldown()
    {
        float cooldown = 0.02f;
        while (cooldown > 0.0f)
        {
            cooldown -= Time.deltaTime;
            yield return null;
        }

        run = false;
        yield return null;
    }

    public void CreateRound()
    {
        ClearDynamic();
        Jason = null;

        GenerateWorld();

        if (timer == null && uiController != null) timer = new Timer(uiController);
        timer.Reset();
        timer.SetActive(true);
    }

    public static void AssignJason(GameObject j)
    {
        Jason = j;
    }

    void Start()
    {
        CreateRound();
        CameraTargeter = CameraController.GetComponent<CameraTargetDetect>();
    }

    void Update()
    {
        if (!run && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0)))
        {
            run = true;
            CameraTargeter.Reset();
            CreateRound();
        }
        if (timer != null ) timer.Update();
    }

    // Insane ideas doc... people will call me crazy...
    /*
     * - Multiple Jasons????? What?? Gameplay will vary like crazy... (This will actually be pretty sick, do this if you have time)
     * 
     * - Squadrons of people, like a crowd following one leader (Should be a giveaway that you shouldn't focus too much on them)
     * - ^^ fake jason, looks like jason but is not!!! 
     * - secret service?? they travel as a pack
     * 
     * - Parents and children. ^^^ essentially re-use the same system, and you can also use this to make people follow Jason
     * - - also bcuz the tiny capsule pills are funny (Under the same class of NPCs that won't help you find Jason so you should ignore them)
     * 
     * - Jason cult people that literally only exist to chase Jason (Helps player identify jason if you follow the crowd -- it'll eventually reach him)
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

    /* Some gameplay notes
     * - Idk if having the static post-process improved gameplay, but it might help making the player avoid distractions?
     * - Randomizing jason's color entirely changed gameplay experience in addition to having the progress bar before you can take a photo
     * -> Not only makes the game harder but makes the player spend more time exploring and carefully analyzing every NPC whereas before you'd just
     * -> ignore everyone else and just look for Jason's color
     * - Making the progress bar flash when it's full + sfx makes user feedback experience much nicer
     * 
     * To do next then:
     * -> make some nicer hats and NPCs so that the player can spot patterns and that should make Jason-hunting much easier
     */
}

public class Timer
{
    public float time = 0.0f;
    public bool active = false;
    private UIController uiController;

    public Timer(UIController controller) 
    {
        uiController = controller;
    }

    public void SetActive(bool value)
    {
        active = value;
    }

    public float GetTime()
    {
        return time;
    }

    public void Reset()
    {
        time = 0.0f;
        active = false;
    }

    public void Update()
    {
        if (active)
        {
            time += Time.deltaTime;
            uiController.SetTimerText(time);
        }
    }
}

