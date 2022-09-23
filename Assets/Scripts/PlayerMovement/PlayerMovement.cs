
using System;
using UnityEngine;

[RequireComponent (typeof(PlayerInputs))]
public class PlayerMovement : MonoBehaviour
{
    private PlayerInputs _playerInputs;
    private Rigidbody2D _rigidbody2D;
    private CapsuleCollider2D _capsuleCollider2D;
    private bool _isGrounded;
    
    [SerializeField] private float speed;
    [SerializeField] private float jumpHeight;
    
    [Header("Bullet")]
    [SerializeField] private GameObject bullet;

    [SerializeField] private float bulletSpeed;

    private void Start()
    {
        _capsuleCollider2D = gameObject.GetComponent<CapsuleCollider2D>();
        _playerInputs = gameObject.GetComponent<PlayerInputs>();
        _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _isGrounded = Physics2D.BoxCast(_capsuleCollider2D.bounds.center, _capsuleCollider2D.bounds.size, 0f,
            Vector2.down, 0.1f, 64);

        if (_playerInputs.input.Player.Jump.WasPerformedThisFrame() && _isGrounded)
        {
            _rigidbody2D.AddForce(Vector2.up * jumpHeight);
        }
        
        _rigidbody2D.velocity = new Vector2(_playerInputs.input.Player.Move.ReadValue<float>() * speed, _rigidbody2D.velocity.y);

        if (_playerInputs.input.Player.Shoot.WasPerformedThisFrame())
        {
            Shoot();
        }
    }

    private void FixedUpdate()
    {
        
    }

    void Shoot()
    {
        GameObject b = Instantiate(bullet, transform.position, Quaternion.identity);
        b.GetComponent<Rigidbody2D>().velocity = Vector2.right * bulletSpeed;
        Destroy(b.gameObject, 2f);
    }
}
