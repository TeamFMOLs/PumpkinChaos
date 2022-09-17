using Godot;
using System;

public class Portal : Area2D
{
    [Export]
    private NodePath TargetObjectPath;
    private Portal OtherPortal;
    public bool IsDisabled = false;
    
    public override void _Ready(){
        Monitoring = true;
        OtherPortal = GetNode(TargetObjectPath) as Portal;
        Connect("body_entered", this, nameof(OnObjectEntered));
        Connect("body_entered", this, nameof(OnObjectExit));
    }

    void OnObjectEntered(PhysicsBody2D other)
    {
        if (other is Player && !IsDisabled)
        {
            if(!StaticRefs.CurrentPlayer._Teleport){
                OtherPortal.IsDisabled = true;
                var portalpos=OtherPortal.GlobalPosition;
                StaticRefs.CurrentPlayer.Teleport(portalpos);
            }
        }
    }
    void OnObjectExit(PhysicsBody2D other)
    {
        if (other is Player && IsDisabled)
        {
            if(StaticRefs.CurrentPlayer._Teleport){
                OtherPortal.IsDisabled = false;
                StaticRefs.CurrentPlayer._Teleport=false;
                IsDisabled=false;
            }
        }
    }
}