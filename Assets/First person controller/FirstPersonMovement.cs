using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    //Sets the float speed and velocity.
    public float speed = 5;
    Vector2 velocity;

    //Updates the players movement based on input and is translated.
    void Update()
    {
        velocity.y = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        velocity.x = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        transform.Translate(velocity.x, 0, velocity.y);
    }
}
