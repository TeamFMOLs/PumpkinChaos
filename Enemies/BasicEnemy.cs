using Godot;
using System;

public class BasicEnemy : CharacterController , IDestructible
{
    [Export] public float Speed = 100;
    private EnemyNavigator _navigator;
    public HealthSystem healthSystem {get; private set;}
    public override void _Ready()
    {
        base._Ready();
        _navigator = GetNode<EnemyNavigator>("Navigator");
        _navigator.Controller = this;
        healthSystem = GetNode<HealthSystem>("HealthSystem");
        healthSystem.OnDeath += Death;
        
    }
    public override void _Process(float delta)
    {
        base._Process(delta);
        _navigator.GoToLocation(StaticRefs.CurrentPlayer.GlobalPosition, Speed);
    }
    
    private void Death() {
        QueueFree();
    }
}
