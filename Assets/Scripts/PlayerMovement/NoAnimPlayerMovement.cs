
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent (typeof(PlayerInputs))]
public class NoAnimPlayerMovement : MonoBehaviour
{
    
    private PlayerInputs _playerInputs;
    private Rigidbody2D _rigidbody2D;
    private CapsuleCollider2D _capsuleCollider2D;
    private SpriteRenderer _spriteRenderer;

    private Vector2 _aim;
    private bool _isGrounded;
    private float _currentCt;
    private float _currentShootCD;

    [SerializeField] private float maxFall;
    
    [Header("Ground")]
    [SerializeField] private float speed;

    [Header("Jump")] 
    [SerializeField] private float cayoteTime;
    [SerializeField] private float jumpHeight;

    [SerializeField] private float pushDown;
    
    [Header("Bullet")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float shootCD;

    private void Awake()
    {
    }

    private void Start()
    {
        _capsuleCollider2D = gameObject.GetComponent<CapsuleCollider2D>();
        _playerInputs = gameObject.GetComponent<PlayerInputs>();
        _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            _aim = Vector2.right;
        _currentShootCD = 0;
    }

    private void Update()
    {
        if (_playerInputs.input.Player.Move.ReadValue<float>() >= 0.1f)
        {
            _spriteRenderer.flipX = false;
        }
        else if (_playerInputs.input.Player.Move.ReadValue<float>() <= -0.1f)
        {
            _spriteRenderer.flipX = true;
        }
        
        _isGrounded = Physics2D.BoxCast(_capsuleCollider2D.bounds.center, _capsuleCollider2D.bounds.size, 0f,
            Vector2.down, 0.1f, 64);
        
        
        _rigidbody2D.velocity = new Vector2(_playerInputs.input.Player.Move.ReadValue<float>() * speed, _rigidbody2D.velocity.y);

        if (_playerInputs.input.Player.Shoot.IsPressed() && Time.time >= _currentShootCD)
        {
            Shoot();
        }

        if (_playerInputs.input.Player.Aim.ReadValue<Vector2>() != Vector2.zero)
        {
            _aim = _playerInputs.input.Player.Aim.ReadValue<Vector2>();
        }

        if (_rigidbody2D.position.y < maxFall)
        {
            Death();
        }

        if (_isGrounded && _playerInputs.input.Player.Move.ReadValue<float>() == 0)
        {

        }
        else if (_isGrounded && _playerInputs.input.Player.Move.ReadValue<float>() != 0)
        {

        }
        else if(!_isGrounded)
        {

        }
        
        if (_playerInputs.input.Player.Jump.WasPerformedThisFrame() && (_isGrounded || _currentCt < cayoteTime))
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0);
            _rigidbody2D.AddForce(Vector2.up * jumpHeight);
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

        if (!_isGrounded)
        {
            _currentCt++;
        }
        else
        {
            _currentCt = 0;
        }
    }

    void Shoot()
    {
        _currentShootCD = shootCD + Time.time;
        GameObject b = Instantiate(bullet, transform.position, Quaternion.identity);
        b.GetComponent<Rigidbody2D>().velocity = _aim * bulletSpeed;
        Destroy(b.gameObject, 2f);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            Death();
        }
    }

    void Death()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
