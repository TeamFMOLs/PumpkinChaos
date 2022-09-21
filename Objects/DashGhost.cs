using Godot;
using System;

public class DashGhost : Sprite
{
    private Tween tween;
    protected Timer timer;
    public override void _Ready()
    {
        tween = GetNode("Tween") as Tween;
        tween.InterpolateProperty(this, "modulate:a",1.0f, 0f, 2f,Tween.TransitionType.Quart , Tween.EaseType.Out);
        tween.Start();
        GD.Print("A7a");
        timer = GetNode("Timer") as Timer;
        timer.WaitTime = 2f;
        timer.Connect("timeout", this, nameof(DestroyGhost));
        timer.Start();
    }
    
    public void DestroyGhost(){
        GD.Print("A7ooooooooo");
        QueueFree();
    }
}
