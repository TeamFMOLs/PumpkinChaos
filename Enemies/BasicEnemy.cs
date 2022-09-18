using Godot;
using System;

public class BasicEnemy : CharacterController, IDestructible
{
    [Export]
    public float Speed = 100;
    [Export]
    protected Vector2 MinMaxAttackInterval;
    [Export]
    protected float SpreadAttackProb = 0.33f;
    [Export]
    protected int SpreadAttackNumber = 3;
    [Export]
    protected PackedScene EnemyProjectile;
    [Export]
    protected float ProjectileSpeed;
    protected RandomNumberGenerator rnd = new RandomNumberGenerator();
    protected EnemyNavigator _navigator;
    public HealthSystem healthSystem { get; private set; }
    protected Timer _attackTimer;
    public override void _Ready()
    {
        base._Ready();
        _navigator = GetNode<EnemyNavigator>("Navigator");
        _navigator.Controller = this;
        healthSystem = GetNode<HealthSystem>("HealthSystem");
        GetNode<SpriteAnimationController>("SpriteAnimationController").characterController = this;
        healthSystem.OnDeath += Death;
        healthSystem.OnTakeDamage += OnHurt;
        _attackTimer = GetNode("AttackTimer") as Timer;
        _attackTimer.Connect("timeout", this, nameof(InitiateAttack));
        rnd.Randomize();
        _attackTimer.Start(rnd.RandfRange(MinMaxAttackInterval.x, MinMaxAttackInterval.y));

    }
    public override void _Process(float delta)
    {
        base._Process(delta);
        _navigator.GoToLocation(StaticRefs.CurrentPlayer.GlobalPosition, Speed);
    }

    private void Death()
    {
        QueueFree();
    }

    private void OnHurt()
    {
        GetNode<AnimationPlayer>("AnimationPlayer").Play("get_hurt");
    }

    public virtual void AttackPos(Vector2 pos)
    {
        var blt = EnemyProjectile.Instance() as Bullet;
        GetTree().Root.AddChild(blt);
        var vec = (pos - GlobalPosition).Normalized();
        blt.GlobalPosition = GlobalPosition;
        blt.GlobalRotation = vec.Angle();
        blt.Velocity = ProjectileSpeed * vec;

    }
    public virtual void SpreadAttack(Vector2 pos, int n)
    {
        for (int i = 0; i < n; i++)
        {
            var blt = EnemyProjectile.Instance() as Bullet;
            GetTree().Root.AddChild(blt);
            var vec = (pos - GlobalPosition).Normalized();
            var angle = vec.Angle();
            angle += Mathf.Pi / 6 * i * Mathf.Pow(-1, i);
            vec = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            blt.GlobalPosition = GlobalPosition;
            blt.GlobalRotation = vec.Angle();
            blt.Velocity = ProjectileSpeed * vec;
        }
    }

    public virtual void InitiateAttack()
    {
        var choice = rnd.RandfRange(0, 1);
        if (choice > SpreadAttackProb)
            SpreadAttack(StaticRefs.CurrentPlayer.GlobalPosition, SpreadAttackNumber);
        else
            AttackPos(StaticRefs.CurrentPlayer.GlobalPosition);
        _attackTimer.Start(rnd.RandfRange(MinMaxAttackInterval.x, MinMaxAttackInterval.y));

    }
}
