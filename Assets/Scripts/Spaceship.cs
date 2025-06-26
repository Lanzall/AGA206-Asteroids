using Unity.VisualScripting;
using UnityEngine;

public class Spaceship : MonoBehaviour
{
    public float EnginePower = 10f;
    public float TurnPower = 10f;
    public int HealthMax = 3;
    public int HealthCurrent;

    private Rigidbody2D rb2D;
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        HealthCurrent = HealthMax;
    }
    
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        ApplyThrust(vertical);
        ApplyTorque(horizontal);

        if(Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(1);
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

        //If current health is zero, then Explode
        if(HealthCurrent < 1)
        {
            Explode();
        }
    }

    public void Explode()
    {
        //Destroy the ship, end the game
        Debug.Log("Game Over");
        Destroy(gameObject);
    }
}
