using Godot;
using System;

public class KogBullet : Bullet
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public Vector2 Target;
    public override void _Ready()
    {
        Monitoring = false;
        CreateTween().TweenCallback(this, "queue_free").SetDelay(LifeTime);

    }

    public override void _PhysicsProcess(float delta)
    {
        GlobalPosition += Velocity * delta;
        if(GlobalPosition.y+10<Target.y){Monitoring = true;}
        if(GlobalPosition.y>Target.y){DestroyBullet();}
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

}
