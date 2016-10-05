// Temporary script by https://gist.github.com/pandr
// https://gist.github.com/pandr/0372b849abee809f0bbe

using UnityEngine;
using System.Collections;

/*
 * Based on http://www.blenderfreak.com/blog/post/flythrough-camera-smooth-movement-unity3d/
 *
 * Controls:
 *   WASD + R/F for Up/Down
 *   Mouse wheel control movement speed
 *   Mouse pointer is used for joystick like control of orientation
 *
 */

public class Flycam : MonoBehaviour
{
    public float speed = 1.0f;
    public float sensitivity = 100f;
    public bool inverted = false;
    public float acceleration = 0.1f;

    private float actSpeed = 0.0f;
    private Vector3 lastDir = new Vector3();

    void Start()
    {
        Cursor.visible = false;
    }

    void Update()
    {
        // Mouse Look
        var mouse = Input.mousePosition;
        mouse.x /= Screen.width;
        mouse.y /= Screen.height;
        mouse -= new Vector3(0.5f, 0.5f, 0.0f);
        mouse.x *= Mathf.Sign(mouse.x) * mouse.x;
        mouse.y *= Mathf.Sign(mouse.y) * mouse.y;
        if (!inverted) mouse.y = -mouse.y;
        mouse *= sensitivity;
        mouse.y *= 0.5f;
        mouse = new Vector3(transform.eulerAngles.x + mouse.y, transform.eulerAngles.y + mouse.x, 0);
        var rot = transform.rotation;
        rot = Quaternion.Slerp(rot, Quaternion.Euler(mouse), 0.1f);
        transform.rotation = rot;

        // Speed
        speed += Input.GetAxis("Mouse ScrollWheel");

        // Direction
        Vector3 dir = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) dir.z += 1.0f;
        if (Input.GetKey(KeyCode.S)) dir.z -= 1.0f;
        if (Input.GetKey(KeyCode.D)) dir.x += 1.0f;
        if (Input.GetKey(KeyCode.A)) dir.x -= 1.0f;
        if (Input.GetKey(KeyCode.R)) dir.y += 0.5f;
        if (Input.GetKey(KeyCode.F)) dir.y -= 0.5f;
        dir.Normalize();

        // Update translation
        if (dir.magnitude > 0.001f)
        {
            if (actSpeed < 1)
                actSpeed += acceleration * Time.deltaTime * 40;
            else
                actSpeed = 1.0f;

            lastDir = dir;
        }
        else
        {
            if (actSpeed > 0)
                actSpeed -= acceleration * Time.deltaTime * 20;
            else
                actSpeed = 0.0f;
        }
        transform.Translate(lastDir * actSpeed * speed * Time.deltaTime);
    }
}
