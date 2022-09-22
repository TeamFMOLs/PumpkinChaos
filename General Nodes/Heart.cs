using Godot;
using System;

public class Heart : Loot
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    private int HpAmount=30;
    protected override void OnContactWithPlayer(PhysicsBody2D other) {
        base.OnContactWithPlayer(other);
        if (other is Player)
        {
            GiveHp();
        }
    }
    public void GiveHp() {
        StaticRefs.CurrentPlayer.GiveHp(HpAmount);
    }
}
