using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float hp;
    private SpriteRenderer _spriteRenderer;
    private float currentHP;
    
    
    private void Start()
    {
        currentHP = hp;
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    public void Hurt(int dmg)
    {
        currentHP -= dmg;
        StartCoroutine(blink());
        Debug.Log(currentHP);

        if (currentHP <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Hurt(1);
    }

    IEnumerator blink()
    {
        Color baseColor = _spriteRenderer.color;
        baseColor.a = 1;

        Color inv = baseColor;
        inv.a = 0;

        for (int i = 0; i < 10; i++)
        {
            _spriteRenderer.color = inv;
            yield return new WaitForSeconds(0.05f);
            _spriteRenderer.color = baseColor;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
