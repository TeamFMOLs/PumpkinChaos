using Godot;
using System;

public class KogEnemy : BasicEnemy
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
    }
    public override void _PhysicsProcess(float delta){
        base._PhysicsProcess(delta);
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(float delta)
  {
      base._Process(delta);
  }
}
