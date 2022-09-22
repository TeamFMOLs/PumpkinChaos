using Godot;
using System;

public class ThirdEnemy : BasicEnemy
{
    [Export]
    private int ShotDamage = 50;
    public override void _Ready()
    {
        base._Ready();
        MinMaxAttackInterval.x = 2f; MinMaxAttackInterval.y = 9;
        SpreadAttackProb = 0.4f;
        Speed = 0;
        _attackTimer.Start(rnd.RandfRange(MinMaxAttackInterval.x+2, MinMaxAttackInterval.y));
    }
    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        Velocity = Vector2.Zero;
    }
    public override void AttackPos(Vector2 pos)
    {
        var blt = EnemyProjectile.Instance() as Bullet;
        StaticRefs.CurrentLevel.AddChild(blt);
        var vec = (pos - GlobalPosition).Normalized();
        blt.GlobalPosition = GlobalPosition + vec*60;
        blt.GlobalRotation = vec.Angle();
        blt.Velocity = ProjectileSpeed * vec * 1.5f;
        blt.Damage = ShotDamage*2;
    }

    public override void SpreadAttack(Vector2 pos, int n)
    {
        AttackPos(pos);
        for (int i = 1; i < n/2+1; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                var blt = EnemyProjectile.Instance() as Bullet;
                blt.Damage = ShotDamage;
                StaticRefs.CurrentLevel.AddChild(blt);
                var vec = (pos - GlobalPosition).Normalized();
                var angle = vec.Angle();
                angle += Mathf.Pi / 8 * i * Mathf.Pow(-1, j);
                vec = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                blt.GlobalPosition = GlobalPosition;
                blt.GlobalRotation = vec.Angle();
                blt.Velocity = ProjectileSpeed * vec;
            }
        }
    }
    public override void InitiateAttack()
    {
        var choice = rnd.RandfRange(0, 1);
        if (choice <= SpreadAttackProb)
            SpreadAttack(StaticRefs.CurrentPlayer.GlobalPosition, SpreadAttackNumber);
        //else
            //AttackPos(StaticRefs.CurrentPlayer.GlobalPosition);
        _attackTimer.Start(rnd.RandfRange(MinMaxAttackInterval.x, MinMaxAttackInterval.y));

    }
}
