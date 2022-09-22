using Godot;
using System;

public class Menu : Node
{
    [Export]
    private PackedScene Level;
    public override void _Ready()
    {

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("scene_continue"))
        {
            GetTree().Root.AddChild(Level.Instance());
            QueueFree();
        }
    }
}
