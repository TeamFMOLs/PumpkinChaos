using Godot;
using System;

public class BasicEnemy : CharacterController, IDestructible , IScoreObject
{
    [Export]
    public float Speed = 100;
    [Export]
    public int Score {get; set;}    
    [Export]
    public int OnHitDamage;
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
    [Export]
    protected PackedScene[] PrizesToDrop;
    [Export]
    protected int NumberOfPrizes = 3;
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

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        var it = GetLastSlideCollision();
        if (it != null && it.Collider is Player)
        {
            var plyr = it.Collider as Player;
            plyr.GetPushed((plyr.GlobalPosition-GlobalPosition),OnHitDamage);
        }
    }

    protected void Death()
    {
        GetNode<CollisionShape2D>("CollisionShape2D").Disabled = true;
        DropPrizes();
        GiveScore();
        GetNode<AnimationPlayer>("AnimationPlayer").Play("die");
        CreateTween().TweenCallback(this,"queue_free").SetDelay(0.4f);
    }

    protected void OnHurt()
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

    public  void GiveScore() {
        StaticRefs.CurrentPlayer.IncreaseScore(Score);
    }

    protected void DropPrizes() {
        for (int i = 0; i < NumberOfPrizes; i++)
        {
            var index = rnd.RandiRange(0,PrizesToDrop.Length-1);
            var pos = Vector2.Right*rnd.RandfRange(-64,64) + Vector2.Up*rnd.RandfRange(-64,64)  + GlobalPosition;
            var prize = PrizesToDrop[index].Instance<Loot>();
            GetTree().Root.AddChild(prize);
            prize.GlobalPosition = GlobalPosition;
            prize.StartTweenPos(pos , 0.6f); 
        }
    }
}
