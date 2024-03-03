using UnityEngine;

public class Player : MonoBehaviour
{
    private const int MaxHealth = 100;
    private int _health = MaxHealth;
    private PlayerWorld _world = PlayerWorld.Real;
    private PlayerType _playerType;

    public short JumpCount { get; set; }

    public PlayerType PlayerType
    {
        get => _playerType;
        private set => _playerType = value;
    }

    public PlayerWorld World
    {
        get => _world;
        private set => _world = value;
    }

    public JumpWay JumpWay
    {
        get => World == PlayerWorld.Real ? JumpWay.Up : JumpWay.Down;
    }

    public int Health
    {
        get => _health;
        private set => _health = Mathf.Clamp(value, 0, MaxHealth);
    }

    private void Awake()
    {
        PlayerType = gameObject.name == "PlayerTop" ? PlayerType.PlayerTop : PlayerType.PlayerBottom;
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
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
        World = World == PlayerWorld.Real ? PlayerWorld.Parallel : PlayerWorld.Real;
        GetComponent<Rigidbody2D>().gravityScale = World == PlayerWorld.Real ? 1 : -1;
    }

    public void Heal(int amount)
    {
        Health += amount;
    }

    public void ChangePlayerPosition(Vector2 newPosition)
    {
        transform.position = newPosition;
    }
}

public enum PlayerWorld
{
    Real,
    Parallel
}

public enum PlayerType
{
    PlayerTop,
    PlayerBottom
}

public enum JumpWay
{
    Up = 1,
    Down = -1
}