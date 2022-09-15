using Godot;
using System;

public class Switch : Area2D
{
    [Export]
    private NodePath TargetObjectPath;
    private IRemoteControledObject TargetObject;
    [Export]
    private SwitchType switchType;
    bool IsOn = false;
    public override void _Ready()
    {
        Monitoring = true;
        TargetObject = GetNode(TargetObjectPath) as IRemoteControledObject;
        Connect("body_entered", this, nameof(OnObjectEntered));
        Connect("body_exited", this, nameof(OnBodyLeft));

    }

    void OnObjectEntered(PhysicsBody2D it)
    {
        if (switchType == SwitchType.ONE_TIME_PRESS && IsOn)
        {
            return;
        }
        TargetObject?.OnSwitchStateChanged();
    }

    void OnBodyLeft(PhysicsBody2D it)
    {
        TargetObject?.OnSwitchStateChanged();
    }


}

public enum SwitchType
{
    ONE_TIME_PRESS,
    CONSTANT_PRESS
}
