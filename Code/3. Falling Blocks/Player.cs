using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 7;
    float halfScreenWidth;
    public event System.Action OnPlayerDeath;

    private void Start()
    {
        float halfPlayerWidth = transform.localScale.x / 2;
        halfScreenWidth = Camera.main.aspect * Camera.main.orthographicSize + halfPlayerWidth;
    }

    void Update()
    {
        MovePlayer();
        CheckForOOB();
    }

    void MovePlayer()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float velocity = inputX * speed;
        transform.Translate(Time.deltaTime * velocity * Vector2.right);
    }

    void CheckForOOB()
    {
        Vector2 playerPos = transform.position;

        if (playerPos.x > halfScreenWidth)
        {
            transform.position = new Vector2(-halfScreenWidth, playerPos.y);
        }
        else if (playerPos.x < -halfScreenWidth)
        {
            transform.position = new Vector2(halfScreenWidth, playerPos.y);
        }
    }

    void OnTriggerEnter2D(Collider2D triggerCollider)
    {
        if (triggerCollider.CompareTag("Falling Block"))
        {
            OnPlayerDeath?.Invoke();
            Destroy(gameObject);
        }
    }
}