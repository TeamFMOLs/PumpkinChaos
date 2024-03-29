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
    private RandomNumberGenerator rnd = new RandomNumberGenerator();
    public override void _Ready()
    {
        start = GetNode<Node2D>("SrartPos");
        end = GetNode<Node2D>("EndPos");
        sky = GetNode<Node2D>("SkyPos");
        GetTree().CreateTimer(1f, false).Connect("timeout", this, nameof(SpawnAmmo));
        rnd.Randomize();
    }

    Vector2 PickRandomPoint()
    {
        var loc = new Vector2(
            rnd.RandfRange(start.GlobalPosition.x, end.GlobalPosition.x),
            rnd.RandfRange(start.GlobalPosition.y, end.GlobalPosition.y)
        );
        return loc;
    }

    void SpawnAmmo()
    {
        var startPos = new Vector2(
            rnd.RandfRange(start.GlobalPosition.x, end.GlobalPosition.x),
            sky.GlobalPosition.y
        );
        var endPos = PickRandomPoint();
        var it = AmmoScene.Instance() as Ammo;
        it.GlobalPosition = startPos;
        StaticRefs.CurrentLevel.AddChild(it);
        it.StartTweenPos(endPos, FallTime);
        GetTree().CreateTimer(rnd.RandfRange(MinMaxTimeToSpawn.x, MinMaxTimeToSpawn.y), false).Connect("timeout", this, nameof(SpawnAmmo));

    }



}
