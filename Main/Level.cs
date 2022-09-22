using Godot;
using System;

public class Level : Node2D
{
    public override void _EnterTree()
    {
        StaticRefs.CurrentLevel = this;
    }
}
