using Godot;
using System;

public class ScareCrow : Area2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Monitoring = true;
        Connect("body_entered", this, nameof(OnBodyEntered));
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    void OnBodyEntered(PhysicsBody2D body2D) {
        if (body2D is Player)
        {
            var newNode = GetNode("HeadPosition") as Node2D;
            var player = body2D as Player;
            var tween = CreateTween();
            tween.TweenProperty(player,"global_position" ,newNode.GlobalPosition,0.8f);
            tween.TweenProperty(player,"global_rotation" ,newNode.GlobalRotation,0.8f);
            player.ReachedGoal();
        }
        
    }
}
