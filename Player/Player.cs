using Godot;
using System;

public class Player : CharacterController
{
    [Export]
    NodePath AnimatorNodePath;
    [Export]
    NodePath MovementTimerNodePath;
    private InputManager inputManager;
    private AnimationPlayer animationPlayer;
    [Export]
    private float StepSize = 80f;
    [Export]
    private float StepTime = 0.5f;
    private Timer MovementTimer;
    private bool IsMoving = false;
    private Vector2 lastPos ;


    public override void _Ready()
    {
        inputManager = GetNode<InputManager>("InputManager");
        animationPlayer = GetNode<AnimationPlayer>(AnimatorNodePath);
        MovementTimer = GetNode<Timer>(MovementTimerNodePath);
        MovementTimer.Connect("timeout" , this , nameof(ResetMovement));
        MovementTimer.WaitTime = StepTime;
        ResetMovement();
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        var vel = Vector2.Zero;
        if (inputManager.MoveDown)
        {
            vel += Vector2.Down ;
        }
        if (inputManager.MoveRight)
        {
            vel += Vector2.Right ;
        }
        if (inputManager.MoveLeft)
        {
            vel += Vector2.Left ;
        }
        if (inputManager.MoveUp)
        {
            vel += Vector2.Up ;
        }
        if (vel != Vector2.Zero)
        {
            GD.Print("why") ;
            Move(vel.Normalized());
        }
        
    }

    void Move(Vector2 dir)
    {
        if (!IsMoving)
        {
            lastPos = GlobalPosition;
            IsMoving = true;
            animationPlayer.Play("SlightJump");
            MovementTimer.Start(StepTime);
            Velocity = StepSize/StepTime * dir;
        }
    }

    void ResetMovement() {
        GD.Print("a7a");
        Velocity = Vector2.Zero;
        IsMoving = false;
    }

    




}
