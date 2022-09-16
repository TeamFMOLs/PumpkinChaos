using Godot;
using System;

public class Trap : Area2D
{
    [Export]
    private TrapType trapType;
    [Export]
    private float TimeToStart = 1f, SpikesUpTime = 1f, SpikesDownTime = 1f;
    [Export]
    private bool StartSpikesUp = false;
    private bool _isSpikesUp;
    private AnimatedSprite _animatedSprite;
    private AudioStreamPlayer2D _SpikesSound;
    private Timer _timer;
    public override void _Ready()
    {
        Monitoring = true;
        _animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        _timer = GetNode("Timer") as Timer;
        _SpikesSound = GetNode("SpikesSound") as AudioStreamPlayer2D;
        Connect("body_entered", this, nameof(OnObjectEntered));
        switch (trapType)
        {
            case TrapType.SWITCH_CONTROLED:
                if(StartSpikesUp) 
                    TrapsUp();
                break;
            case TrapType.TIME_BASED:
                _timer.Connect("timeout", this, nameof(StartTrapT));
                _timer.Start(TimeToStart);
                break;
            default: break;
        }
    }

    void OnObjectEntered(PhysicsBody2D other)
    {
        if (other is Player && _animatedSprite.GetFrame() >= 7 && _animatedSprite.GetFrame() <= 11)
        {
            // do st
            GD.Print("liol");
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        var it = GetOverlappingBodies();
        if (_isSpikesUp && it.Count != 0 && it.Contains(StaticRefs.CurrentPlayer))
        {
            StaticRefs.CurrentPlayer.Die();
        }


    }

    private void StartTrapT() {
        
        TrapsUp();
        _timer.Disconnect("timeout" , this , nameof(StartTrapT));
        _timer.Connect("timeout" , this , nameof(StopTrapT));
        _timer.Start(SpikesUpTime);
    }
    private void StopTrapT() {
        TrapsDown();
        
        _timer.Disconnect("timeout" , this , nameof(StopTrapT));
        _timer.Connect("timeout" , this , nameof(StartTrapT));
        _timer.Start(SpikesDownTime);
    }


    public void TrapsUp()
    {
        _SpikesSound.Play();
        _animatedSprite.Play("spikes_up");
        _isSpikesUp = true;
    }

    public void TrapsDown()
    {
        _SpikesSound.Play();
        _animatedSprite.Play("spikes_down");
        _isSpikesUp = false;
    }


}

public enum TrapType
{
    SWITCH_CONTROLED,
    TIME_BASED
}
