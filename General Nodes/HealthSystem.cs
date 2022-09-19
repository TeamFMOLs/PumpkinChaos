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


    public event Action OnDeath;
    public event Action OnTakeDamage;
    private int _health;
    private int _maxHealth;
    private bool _isDead;
    private Label _damageLabel;
    [Export]
    private NodePath AnimationNodePath;
    private AnimationPlayer _animationPlayer;

    public override void _Ready()
    {
        
        _health = _maxHealth;
        //_damageLabel = GetNode<Label>("Label");
        //_damageLabel.Text = "";
        _animationPlayer = GetNode<AnimationPlayer>(AnimationNodePath);
    }

    void Die()
    {
        OnDeath?.Invoke();
    }

    public void TakeDamage(int damage)
    {
        if (_isDead)
            return;
        _health -= damage;
        //_damageLabel.Text = damage.ToString();
        //_animationPlayer.Play("FadeInFadeOut");
        OnTakeDamage?.Invoke();
        if (_health <= 0)
        {
            _health = 0;
            _isDead = true;
            Die();
        }
    }
}
