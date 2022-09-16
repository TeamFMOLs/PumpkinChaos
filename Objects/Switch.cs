using Godot;
using System;

public class Switch : Area2D
{
    [Export]
    private NodePath TargetObjectPath;
    private IRemoteControledObject TargetObject;
    [Export]
    private SwitchType switchType;
    private AudioStreamPlayer2D audioStream;
    private AnimationPlayer animationPlayer;
    private bool IsOn = false;
    public override void _Ready()
    {
        Monitoring = true;
        if (!(TargetObjectPath is null))
        {
            TargetObject = GetNode(TargetObjectPath) as IRemoteControledObject;
        }
        animationPlayer = GetNode("AnimationPlayer") as AnimationPlayer;
        audioStream = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
        Connect("body_entered", this, nameof(OnObjectEntered));
        Connect("body_exited", this, nameof(OnBodyLeft));
    }

    void OnObjectEntered(PhysicsBody2D it)
    {
        GD.Print(it);
        if (switchType == SwitchType.ONE_TIME_PRESS && IsOn)
        {
            return;
        }
        TargetObject?.OnSwitchStateChanged();
        animationPlayer.Play("switch_on");
        audioStream.Play();
    }

    void OnBodyLeft(PhysicsBody2D it)
    {
        if (switchType == SwitchType.CONSTANT_PRESS)
        {
            TargetObject?.OnSwitchStateChanged();
            animationPlayer.Play("switch_off");
            audioStream.Play();
        }
    }


}

public enum SwitchType
{
    ONE_TIME_PRESS,
    CONSTANT_PRESS
}
