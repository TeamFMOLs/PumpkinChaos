using Godot;
using System;

public class PlayerMelee : Area2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Connect("body_entered", this, nameof(OnObjectEntered));
    }

    void OnObjectEntered(PhysicsBody2D other) {
        if (other is BasicEnemy)
        {
            var plyr = other as BasicEnemy;
            plyr.PushBack();
            GD.Print("Omae Wa Mou Shindeiru:Melee");
        }
    }
}
