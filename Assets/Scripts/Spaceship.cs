using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Spaceship : MonoBehaviour
{
    #region Variables
    [Header("Engine")]
    public float EnginePower = 10f;
    public float TurnPower = 10f;

    [Header("Health")]
    public int HealthMax = 3;
    public int HealthCurrent;

    [Header("Bullets")]
    public GameObject BulletPrefab;
    public float BulletSpeed = 100f;
    public float FiringRate = 0.33f;
    private float fireTimer = 0f;

    [Header("Player Explosion")]
    public GameObject PlayerExplosion;

    [Header("Game Scoring")]
    public int Score = 0;
    private int PreviousScore;

    [Header("Other")]
    public float deathDelay = 1;
    public float HUDRecoil = .5f;
    GameManager gameManager;
    Animator animator;
    Camera cam;

    [Header("Sound")]
    public SoundPlayer HitSounds;

    [Header("UI")]
    public ScreenFlash Flash;
    public CameraShake CameraShake;
    public float ShakeDuration = .15f;
    public GameOverUI GameOverUI;
    public CockpitHUD CockpitHUD;


    private Rigidbody2D rb2D;

    #endregion
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        HealthCurrent = HealthMax;
        gameManager = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();
        animator = GetComponentInChildren<Animator>();
        cam = Camera.main;

        PreviousScore = Score;
    }
    
    void Update()
    {
        if (gameManager.isPaused)
            return;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        ApplyThrust(vertical);
        ApplyTorque(horizontal);
        UpdateFiring();

        if (Input.GetKeyDown(KeyCode.Delete))
        {
            PlayerPrefs.DeleteAll();
        }

        //Damage Testers
        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(100);
        }

        if (Input.GetButtonDown("Fire2"))
        {
            TakeDamage(1);
        }

        //Checks to see if player scored any points
        if (Score != PreviousScore)
        {
            StartCoroutine(CockpitHUD.RoidDestroy());
            Debug.Log("Score changed from " + PreviousScore + " to " +  Score);
            //Updates the previous score to current Score
            PreviousScore = Score;
        }
    }

    public int GetHighScore()
    {
        return PlayerPrefs.GetInt("Hiscore", 0);
    }

    public void SetHighScore(int score)
    {
        PlayerPrefs.SetInt("Hiscore", score);
    }

    public void GameOver()
    {
        StartCoroutine(CockpitHUD.PlayerDeath());

        bool celebrateHiScore = false;
        if (Score > GetHighScore() && celebrateHiScore == false)
        {
            SetHighScore(Score);
            celebrateHiScore = true;
        }
        GameOverUI.Show(celebrateHiScore, Score, GetHighScore());
    }

    private void UpdateFiring()
    {
        bool isFiring = Input.GetButton("Fire1");

        fireTimer -= Time.deltaTime;    //Decrement timer

        if (isFiring == true && fireTimer <= 0)
        {
            FireBullet();
            fireTimer = FiringRate;
        }
    }

    private void ApplyThrust(float amount)
    {
        
        Vector2 thrust = transform.up * EnginePower * Time.deltaTime * amount;
        rb2D.AddForce(thrust);
    }

    private void ApplyTorque(float amount)
    {
        // Debug.Log("Torque amount is " + amount);
        float torque = amount * TurnPower * Time.deltaTime;
        rb2D.AddTorque(-torque);
    }

    public void TakeDamage(int damage)
    {
        //Reduce the current health
        HealthCurrent = HealthCurrent - damage;

        //HealthCurrent -= damage;  another way of writing the above

        HitSounds.PlayRandomSound();
        

        //If current health isn't Zero, shake the camera
        if (HealthCurrent > 0)
        {
            StartCoroutine(CockpitHUD.TakenDamage());
            Debug.Log("Shake");
            StartCoroutine(CameraShake.ShakeRoutine(ShakeDuration, .4f));
        }

        StartCoroutine(Flash.FlashRoutine());

        //If current health is zero, then Explode
        if (HealthCurrent <= 0)
        {
            StartCoroutine(DeathDelay());
        }
    }
    IEnumerator DeathDelay()
    {
        rb2D.bodyType = RigidbodyType2D.Static;
        cam.transform.position = new Vector3(transform.position.x, transform.position.y, -10f);
        cam.orthographicSize = 4;
        animator.SetTrigger("Death");
        yield return new WaitForSeconds(deathDelay);

        Explode();
    }

    public void Explode()
    {
        //Destroy the ship, end the game
        Debug.Log("Game Over");
        GameOver();
        Instantiate(PlayerExplosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void FireBullet()
    {
        //Create a new bullet at the spaceships position and rotation

        GameObject bullet = Instantiate(BulletPrefab, transform.position, transform.rotation);
        //Find the bullets rigidbody component
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        //Create a force to push the bullet 'up' from the spaceships facing direction
        Vector2 force = transform.up * BulletSpeed;
        rb.AddForce(force);
    }
}
