using Godot;
using System;

public class DamageLabel : Node2D
{
    public string Text {set{label.Text = value;}}
    private Label label;
    public override void _Ready()
    {
        label = GetNode("Parent/LabelParent/Label") as Label;
        
        GD.Print(label.Text);
       // GetNode<AnimationPlayer>("AnimationPlayer").Play("start");
        CreateTween().TweenCallback(this,"queue_free").SetDelay(0.8f);
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
