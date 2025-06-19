using UnityEngine;

public class BlockPusher : MonoBehaviour
{
    public float Speed = 1f;
    void Start()
    {
        
    }

    void Update()
    {
        //Capture the users Input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        //Create a new Vector of the players movement
        Vector3 move = new Vector3(horizontal, 0, vertical) * Time.deltaTime * Speed;


        //Move to the new position
        transform.position = transform.position + move;      

    }
}
