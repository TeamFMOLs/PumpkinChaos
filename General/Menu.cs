using Godot;
using System;

public class Menu : Node
{
    [Export]
    private PackedScene Level;
    private bool firstTime  = true;
    
    public override void _EnterTree()
    {
        StaticRefs.menu = this;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("scene_continue") && firstTime)
        {
            firstTime = false;
            ResetScene();
        }
    }
    public void ResetScene() {
        foreach (var item in GetChildren())
        {
            (item as Node).QueueFree();
        }
        AddChild(Level.Instance());
            
    }
}
