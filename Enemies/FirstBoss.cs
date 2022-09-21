using Godot;
using System;
public class FirstBoss : BasicEnemy
{
    [Export]
    private ProgressBar HealthBar;
    public override void _Process(float delta)
    {
        
    }

    protected override void OnHurt()
    {
        base.OnHurt();
        HealthBar.Value =  (float)healthSystem.Health/(float)healthSystem.MaxHealth *100f;
    }
}
