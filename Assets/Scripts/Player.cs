using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int _health = 100;

    [SerializeField] private PlayerWorld _world;
    public PlayerWorld World => _world;
    public JumpWay jumpWay
    {
        get => _world == PlayerWorld.Real ? JumpWay.Up : JumpWay.Down;
    }

    public int Health => _health;

    public void TakeDamage(int damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player died");
    }

    public void SwitchWorld()
    {
        if (_world == PlayerWorld.Real)
        {
            _world = PlayerWorld.Parallel;
        }
        else
        {
            _world = PlayerWorld.Real;
        }
    }

    public void Heal(int amount)
    {
        _health += amount;
    }

    public void ChangePlayerPosition(Vector2 position)
    {
        transform.position = position;
    }

}

public enum PlayerWorld
{
    Real,
    Parallel
}

public enum JumpWay
{
    Up = 1,
    Down = -1
}
