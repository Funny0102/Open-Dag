
using System;
using System.Linq;
using UnityEngine;

[RequireComponent (typeof(PlayerInputs))]
public class PlayerMovement : MonoBehaviour
{
    private PlayerInputs _playerInputs;
    private Rigidbody2D _rigidbody2D;
    private CapsuleCollider2D _capsuleCollider2D;

    private Vector2 _aim;
    private bool _isGrounded;
    
    [Header("Ground")]
    [SerializeField] private float speed;
    
    [Header("Jump")]
    [SerializeField] private float jumpHeight;

    [SerializeField] private float pushDown;
    
    [Header("Bullet")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private float bulletSpeed;

    private void Start()
    {
        _capsuleCollider2D = gameObject.GetComponent<CapsuleCollider2D>();
        _playerInputs = gameObject.GetComponent<PlayerInputs>();
        _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        _aim = Vector2.right;
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

        if (_playerInputs.input.Player.Aim.ReadValue<Vector2>() != Vector2.zero)
        {
            _aim = _playerInputs.input.Player.Aim.ReadValue<Vector2>();
        }
    }

    private void FixedUpdate()
    {
        if (_rigidbody2D.velocity.y >= 0.3f && !_playerInputs.input.Player.Jump.IsPressed())
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _rigidbody2D.velocity.y * 0.5f);
        }

        if (_rigidbody2D.velocity.y <= -0.3f)
        {
            _rigidbody2D.AddForce(Vector2.down * pushDown);
        }
    }

    void Shoot()
    {
        GameObject b = Instantiate(bullet, transform.position, Quaternion.identity);
        b.GetComponent<Rigidbody2D>().velocity = _aim * bulletSpeed;
        Destroy(b.gameObject, 2f);
    }
}
