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
    public Weapon Weapon { get => _weapon; set => _weapon = value; }

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
        Weapon = GetNode<Weapon>(WeaponNodePath);
        healthSystem = GetNode<HealthSystem>("HealthSystem");
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        _inputHandler.OnAttack += Attack;
        healthSystem.OnTakeDamage += OnTakeDamage ;
        healthSystem.MaxHealth = this.MaxHealth;
        StaticRefs.PlayerUi.UpdateAmmoNumber(Weapon.Ammo);
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        Weapon.WeaponPointAt(_inputHandler.MousePos);
    }

    public override void _PhysicsProcess(float delta)
    {
        Velocity = _inputHandler.MoventDir * speed;
        base._PhysicsProcess(delta);
    }

    public void Attack(Vector2 target) {
        if (Weapon.Attack(target))
        {
            // play animation
        }
    }
    public void AddAmmo(Ammo it) {
        Weapon.Ammo ++;
        StaticRefs.PlayerUi.UpdateAmmoNumber(Weapon.Ammo) ;
    }

    private void OnTakeDamage() {
        animationPlayer.Play("get_hurt");
    }

}
