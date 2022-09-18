using Godot;
using System;

public class Bullet : CharacterController
{
    [Export]
    public int Damage = 50;
    public override void _Ready()
    {
        
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        if (GetLastSlideCollision() != null )
        {
            var it = GetLastSlideCollision() as IDestructible;
            if (it != null)
            {
                it.healthSystem.TakeDamage(Damage);
            }
            DestroyBullet();
        }
    }

    public void DestroyBullet() {
        QueueFree();
    }
}
