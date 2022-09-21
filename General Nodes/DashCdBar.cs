using Godot;
using System;

public class DashCdBar : ProgressBar
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    protected Timer DashCdTimer;
    public override void _Ready()
    {
        DashCdTimer=StaticRefs.CurrentPlayer.GetNode<Timer>("DashCdTimer") as Timer;
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta) {
        Value=MaxValue-DashCdTimer.TimeLeft;
        if(Value==MaxValue){GetNode<Particles2D>("Particles2D").Emitting=false;}
        else{GetNode<Particles2D>("Particles2D").Emitting=true;}
    }
}
