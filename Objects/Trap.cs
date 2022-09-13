using Godot;
using System;

public class Trap : Area2D
{
    private AnimatedSprite _animatedSprite;
    public override void _Ready()
    {
        Monitoring=true;
        _animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        Connect("body_entered", this, nameof(OnObjectEntered));
    }

    void OnObjectEntered(PhysicsBody2D other) {
        if (other is Player && _animatedSprite.GetFrame()>=7 && _animatedSprite.GetFrame()<=11)
        {
            // do st
            GD.Print("a7a");
        }
    }
}
