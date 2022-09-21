using Godot;
using System;

public class Bullet : Area2D
{
    [Export]
    public int Damage = 50;
    [Export]
    protected float LifeTime = 4f;
    [Export]
    protected PackedScene HitEffect;
    private Vector2 _vel;
    public Vector2 Velocity { get => _vel; set => _vel = value; }
    

    public override void _Ready()
    {
        Monitoring = true;
        CreateTween().TweenCallback(this, "queue_free").SetDelay(LifeTime);

    }

    public override void _PhysicsProcess(float delta)
    {
        GlobalPosition += Velocity * delta;
        var ressult = GetOverlappingBodies();
        if (ressult.Count != 0)
        {
            var it = ressult[0] as IDestructible;
            if (it != null)
            {
                it.healthSystem.TakeDamage(Damage);
            }
            DestroyBullet();
        }
    }

    public void DestroyBullet()
    {
        if (HitEffect != null)
        {
            var it = HitEffect.Instance() as Node2D;
            GetTree().Root.AddChild(it);
            it.GlobalPosition = GlobalPosition;
        }
        QueueFree();
    }
}
