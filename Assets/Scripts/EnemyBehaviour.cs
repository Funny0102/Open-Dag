using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer;
    private int movedir;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float speed;

    [SerializeField] float rayRange;

    private void Start()
    {
        movedir = 1;
        _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _rigidbody2D.velocity = new Vector2( speed * movedir, _rigidbody2D.velocity.y);

        if (Physics2D.Raycast(transform.position, Vector2.right * movedir, rayRange, 64))
        {
            
            movedir *= -1;
        }
    }
}
