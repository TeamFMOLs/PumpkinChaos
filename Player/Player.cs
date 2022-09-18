using Godot;
using System;

public class Player : CharacterController, IDestructible
{
    [Export]
    private float speed = 200;
    [Export]
    NodePath WeaponNodePath;
    [Export]
    private int MaxHealth ;
    private InputManager _inputHandler;
    private Weapon _weapon;
    public HealthSystem healthSystem { get; set; }
    public bool StopMovement;
    private AnimationPlayer animationPlayer;

    public override void _EnterTree()
    {
        base._EnterTree();
        StaticRefs.CurrentPlayer = this;
    }
    public override void _Ready()
    {
        _inputHandler = StaticRefs.inputManager;
        _weapon = GetNode<Weapon>(WeaponNodePath);
        healthSystem = GetNode<HealthSystem>("HealthSystem");
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        _inputHandler.OnAttack += Attack;
        healthSystem.OnTakeDamage += OnTakeDamage ;
        healthSystem.MaxHealth = this.MaxHealth;
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        _weapon.WeaponPointAt(_inputHandler.MousePos);
    }

    public override void _PhysicsProcess(float delta)
    {
        Velocity = _inputHandler.MoventDir * speed;
        base._PhysicsProcess(delta);
    }

    public void Attack(Vector2 target) {
        if (_weapon.Attack(target))
        {
            // play animation
        }
    }

    private void OnTakeDamage() {
        animationPlayer.Play("get_hurt");
    }

}
