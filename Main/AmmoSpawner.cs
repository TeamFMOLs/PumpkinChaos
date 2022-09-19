using Godot;
using System;

public class AmmoSpawner : Node2D
{
    [Export]
    private PackedScene AmmoScene;
    [Export]
    private Vector2 MinMaxTimeToSpawn;
    [Export]
    private float FallTime = 6f;
    private Node2D start, end, sky;
    public override void _Ready()
    {
        start = GetNode<Node2D>("SrartPos");
        end = GetNode<Node2D>("EndPos");
        sky = GetNode<Node2D>("SkyPos");
        GetTree().CreateTimer(1f).Connect("timeout", this, nameof(SpawnAmmo));
    }

    Vector2 PickRandomPoint()
    {
        var rnd = new RandomNumberGenerator();
        rnd.Randomize();
        var loc = new Vector2(
            rnd.RandfRange(start.GlobalPosition.x, end.GlobalPosition.x),
            rnd.RandfRange(start.GlobalPosition.y, end.GlobalPosition.y)
        );
        return loc;
    }

    void SpawnAmmo()
    {
        var rnd = new RandomNumberGenerator();
        rnd.Randomize();
        var startPos = new Vector2(
            rnd.RandfRange(start.GlobalPosition.x, end.GlobalPosition.x),
            sky.GlobalPosition.y
        );
        var endPos = PickRandomPoint();
        var it = AmmoScene.Instance() as Ammo;
        it.GlobalPosition = startPos;
        GetTree().Root.AddChild(it);
        it.StartTweenPos(endPos, FallTime);
        GetTree().CreateTimer(rnd.RandfRange(MinMaxTimeToSpawn.x, MinMaxTimeToSpawn.y)).Connect("timeout", this, nameof(SpawnAmmo));

    }
}
