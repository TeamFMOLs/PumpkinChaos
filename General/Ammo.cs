using Godot;
using System;

public class Ammo : PickableObject
{
    
    public override void _Ready()
    {
        base._Ready(); 
    }

    protected override void OnContactWithPlayer(PhysicsBody2D other) {
        base.OnContactWithPlayer(other);
        (other as Player)?.AddAmmo(this);
    }

    

    

    

    
}
