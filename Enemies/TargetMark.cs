using Godot;
using System;

public class TargetMark : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    protected Timer MarkTimer;
    public float MarkTime=2f;
    public override void _Ready()
    {
        MarkTimer = GetNode("Timer") as Timer;
        MarkTimer.WaitTime = MarkTime;
        MarkTimer.Connect("timeout", this, nameof(Destroy));
        MarkTimer.Start();
    }

    public void Destroy(){
        GD.Print("TargetMark:bye bye T-T");
        QueueFree();
    }
}
