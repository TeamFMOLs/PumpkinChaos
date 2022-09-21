using Godot;
using System;
public class FirstBoss : BasicEnemy
{
    [Export]
    private NodePath HealthBar;
    public override void _Ready()
    {
        base._Ready();
        GetNode<ProgressBar>(HealthBar).Value =  (float)healthSystem.Health/(float)healthSystem.MaxHealth *100f;
    }

    protected override void OnHurt()
    {
        base.OnHurt();
        GetNode<ProgressBar>(HealthBar).Value =  (float)healthSystem.Health/(float)healthSystem.MaxHealth *100f;
    }
}
