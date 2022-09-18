using Godot;
using System;

public class Bullet : Area2D
{
    [Export]
    public int Damage = 50;
    [Export]
    protected float LifeTime = 4f;
    private Vector2 _vel;
    public Vector2 Velocity { get => _vel; set => _vel = value; }

    public override void _Ready()
    {
        Monitoring = true;
    }
    
    public override void _PhysicsProcess(float delta)
    {
        GlobalPosition += Velocity*delta;
        var ressult = GetOverlappingBodies();
        if (ressult.Count != 0 )
        {
            var it = ressult[0] as IDestructible;
            if (it != null)
            {
                it.healthSystem.TakeDamage(Damage);
            }
            DestroyBullet();
        }
        CreateTween().TweenCallback(this,"queue_free").SetDelay(LifeTime);
    }

    public void DestroyBullet() {
        QueueFree();
    }
}
