using Godot;
using System;

public class KogEnemy : BasicEnemy
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    private Vector2 AwayRadius;
    [Export]
    public bool _Shooting = false;
    [Export]
    protected PackedScene KogProjectile, TargetMark;
    protected Timer _AniTimer;
    private int KogShotDamage = 80;
    public override void _Ready()
    {
        base._Ready();
        MinMaxAttackInterval.x = 3f; MinMaxAttackInterval.y = 7;
        AwayRadius = new Vector2(200, 200);
        _AniTimer = GetNode("_AniTimer") as Timer;
        _AniTimer.WaitTime = 2.1f;
        _AniTimer.Connect("timeout", this, nameof(AttackPos));
        SpreadAttackProb = 0.5f;
        Speed = 70;
        _attackTimer.Start(rnd.RandfRange(MinMaxAttackInterval.x+2, MinMaxAttackInterval.y));
    }
    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (!_Shooting)
        {
            _navigator.GoToLocation(StaticRefs.CurrentPlayer.GlobalPosition - AwayRadius, Speed);

        }
        else
            Velocity = Vector2.Zero;
    }
    public void AttackPos()
    {
        var pos = StaticRefs.CurrentPlayer.GlobalPosition;
        var blt = EnemyProjectile.Instance() as KogBullet;
        blt.Damage = KogShotDamage;
        blt.Target = pos;
        GetTree().Root.AddChild(blt);

        var vec = (pos * Vector2.Up).Normalized();
        blt.GlobalPosition = pos + Vector2.Up * 400;
        blt.Velocity = (ProjectileSpeed) * Vector2.Down;

        var Mark = TargetMark.Instance() as Node2D;
        Mark.GlobalPosition = pos;
        GetTree().Root.AddChild(Mark);
        _Shooting = false;
    }
    public override void SpreadAttack(Vector2 pos, int n)
    {
        for (int i = 0; i < n; i++)
        {
            var blt = KogProjectile.Instance() as Bullet;
            blt.Damage = ProjectileDamage;
            GetTree().Root.AddChild(blt);
            var vec = (pos - GlobalPosition).Normalized();
            var angle = vec.Angle();
            angle += Mathf.Pi / 6 * i * Mathf.Pow(-1, i);
            vec = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            blt.GlobalPosition = GlobalPosition;
            blt.GlobalRotation = vec.Angle();
            blt.Velocity = ProjectileSpeed * 1.5f * vec;
        }
    }
    public override void InitiateAttack()
    {
        var choice = rnd.RandfRange(0, 1);
        if (!_Shooting)
        {
            if (choice >= SpreadAttackProb)
            {
                _AniTimer.Start();
                _Shooting = true;
                GetNode<AnimatedSprite>("SpriteParent/EnemySprite").Play("KogShot");
            }

            else
                SpreadAttack(StaticRefs.CurrentPlayer.GlobalPosition, 1);

            _attackTimer.Start(rnd.RandfRange(MinMaxAttackInterval.x, MinMaxAttackInterval.y));
        }

    }
}
