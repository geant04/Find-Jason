using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [Header("Sound Effects")]
    public SoundData hurtSound;
    public SoundData deathSound;

    public int health;
    public float speed, turnSpeed;
    [HideInInspector] public Vector3 destination;
    [HideInInspector] public UnityEngine.AI.NavMeshAgent agent;

    protected float cTurnSpeed;
    protected bool isDead;
    protected GameObject player;
    protected Rigidbody playerRB;
    protected Player playerScript;

    public void TakeDamage(int amt)
    {
        health = Mathf.Max(0, health - amt);
        SoundManager.Instance.CreateSound()
                    .WithSoundData(hurtSound)
                    .WithPosition(transform.position)
                    .Play();
        Debug.Log(transform.name + " wuz hit, hp: " + health);
        if (health <= 0)
        {
            SoundManager.Instance.CreateSound()
                    .WithSoundData(deathSound)
                    .WithPosition(transform.position)
                    .Play();
            Kill();
        }
    }
    protected void Kill()
    {
        if (isDead) return;

        Debug.Log("deceased");
        isDead = true;
        Destroy(gameObject);
    }

    protected float TimerF(float val)
    {
        if (val > 0)
        {
            val -= Time.deltaTime;
            if (val <= 0) val = 0;
        }
        return val;
    }

    protected void findPlayer() {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) {
            Debug.Log("Could not find player");
        } else {
            playerScript = player.GetComponent<Player>();
            playerRB = playerScript.myRB;
        }
    }
    protected float getDist() {
        return playerRB ? Vector3.Distance(transform.position, playerRB.position) : float.NaN;
    }
    protected bool lineOfSightCheck() {
        if (playerRB == null) {
            return false;
        }
        return !Physics.Raycast(playerRB.position, transform.position - playerRB.position, getDist() - 1, 1 << 3);
    }
}
