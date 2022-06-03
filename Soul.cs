using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Soul : MonoBehaviour
{
    public int scoreModifer = 0;
    public int healthModifier = 20;
    public bool revealed = false;

    [SerializeField] private GameObject neutral;
    [SerializeField] private GameObject red;
    [SerializeField] private GameObject blue;
    [SerializeField] private GameObject redDeath;
    [SerializeField] private GameObject blueDeath;
    [SerializeField] private float speed;
    [SerializeField] private float chaseSpeed;
    [SerializeField] private float wanderRadius;
    [SerializeField] private float wanderTimer;
    [SerializeField] private int colorRNGMin;
    [SerializeField] private int colorRNGMax;
    [SerializeField] private int redMax;
    [SerializeField] private int blueMin;

    private GameManager gameManager;
    private NavMeshAgent agent;
    private float timer;
    private int color = 0;
    private SphereCollider sCollider;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        sCollider = GetComponent<SphereCollider>();
        //Color Script
        color = Random.Range(colorRNGMin, colorRNGMax);

        //Wander Script
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        timer = wanderTimer;
    }

    void Update()
    {
        if(revealed)
        {
            agent.SetDestination(gameManager.player.transform.position);
            agent.speed = chaseSpeed;
        }
        else { Wander(); }

        if (gameManager.gameOver) { Die(); }
    }

    //Die when Hit SoulTrigger if Revealed
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("SoulTrigger") && revealed)
        {
            sCollider.enabled = false;
            neutral.SetActive(false);
            red.SetActive(false);
            blue.SetActive(false);
            if (color < redMax)
            {
                redDeath.SetActive(true);
                gameManager.UpdateHealth(-healthModifier);
            }
            if(color > blueMin)
            {
                blueDeath.SetActive(true);
                gameManager.UpdateHealth(healthModifier);
            }
        }
    }

    //Resets wanderTimer, Gets Random Position, Sets Position as Destination
    public void Wander()
    {
        agent.speed = speed;
        timer += Time.deltaTime;

        if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            timer = 0;
        }
    }

    //Get Distance to Player
    public float PlayerDistance()
    {
        return Vector3.Distance(gameManager.player.transform.position, transform.position);
    }

    //Find a Random Point in a Sphere that has NavMesh
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        return navHit.position;
    }

    //Called from FlashLight, Disables Neutral Particles and Enables True Colors
    public void RevealColor()
    {
        if (!revealed)
        {
            revealed = true;
            if (color < redMax)
            {
                neutral.SetActive(false);
                red.SetActive(true);
            }
            if (color > blueMin)
            {
                neutral.SetActive(false);
                blue.SetActive(true);
            }
            agent.speed = chaseSpeed;
            agent.areaMask = NavMesh.AllAreas;
        }
    }

    // So Far, Called when Hit by FLProjectile, Disables current Particles and Shows Death Animation
    public void Die()
    {
        neutral.SetActive(false);
        red.SetActive(false);
        blue.SetActive(false);
        sCollider.enabled = false;
        if (color < redMax)
        {
            redDeath.SetActive(true);
            if (!gameManager.gameOver)
            {
                scoreModifer = 1;
                gameManager.UpdateScore(scoreModifer);
            }
        }
        if(color > blueMin)
        {
            blueDeath.SetActive(true);
            if (!gameManager.gameOver)
            {
                scoreModifer = -1;
                gameManager.UpdateScore(scoreModifer);
            }
        }

        Destroy(gameObject, 2);
    }
}
