using Godot;
using System;
using static Godot.GD;

public class HealthSystem : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    [Export]
    public int MaxHealth { get => _maxHealth; set => _maxHealth = value; }

    public bool IsShielded { get => _isShielded; set => _isShielded = value; }
    public int Health { get => _health; set => _health = value; }
    [Export]
    private PackedScene DamageLabelNode;
    public event Action OnDeath;
    public event Action OnTakeDamage;
    private int _health;
    private int _maxHealth;
    private bool _isDead, _isShielded;
    private Label _damageLabel;



    public override void _Ready()
    {

        _health = _maxHealth;

    }

    void Die()
    {
        OnDeath?.Invoke();
    }

    public void ResetHealth(int h)
    {
        MaxHealth = h; Health = h;
    }


    public void TakeDamage(int damage)
    {
        if (_isDead || _isShielded)
            return;
        _health -= damage;
        DisplayDamage(damage);

        if (_health <= 0)
        {
            _health = 0;
            _isDead = true;

            Die();
        }
        else
            OnTakeDamage?.Invoke();



    }

    private void DisplayDamage(int d)
    {
        var newlabel = DamageLabelNode.Instance<DamageLabel>();
        GetTree().Root.AddChild(newlabel);
        newlabel.GlobalPosition = GlobalPosition;
        newlabel.Text = d.ToString();
    }
}
