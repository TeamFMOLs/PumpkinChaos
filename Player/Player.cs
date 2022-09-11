using Godot;
using System;

public class Player : KinematicBody2D
{
    [Export]
    NodePath AnimatorNodePath;
    [Export]
    NodePath MovementTimerNodePath ,StepsAudioPlayerPath;
    private InputManager inputManager;
    private AnimationPlayer animationPlayer;
    private AudioStreamPlayer2D stepsAudioPlayer;
    [Export]
    private float StepSize = 80f;
    [Export]
    private float StepTime = 0.5f;
    private Timer MovementTimer;
    private bool IsMoving = false;
    // private SceneTreeTween MovementTween;
    private Vector2 lastPos;


    public override void _Ready()
    {
        inputManager = GetNode<InputManager>("InputManager");
        animationPlayer = GetNode<AnimationPlayer>(AnimatorNodePath);
        MovementTimer = GetNode<Timer>(MovementTimerNodePath);
        stepsAudioPlayer = GetNode<AudioStreamPlayer2D>(StepsAudioPlayerPath);
        MovementTimer.Connect("timeout", this, nameof(ResetMovement));
        MovementTimer.WaitTime = StepTime;

        ResetMovement();
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        var vel = Vector2.Zero;
        if (inputManager.MoveDown)
        {
            vel = Vector2.Down;
        }
        else if (inputManager.MoveRight)
        {
            vel = Vector2.Right;
        }
        else if (inputManager.MoveLeft)
        {
            vel = Vector2.Left;
        }
        else if (inputManager.MoveUp)
        {
            vel = Vector2.Up;
        }

        if (vel != Vector2.Zero)
        {
            GD.Print("why");
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
            var newPos = GlobalPosition + dir * StepSize;
            stepsAudioPlayer.Play();
            MoveToLoc(newPos, StepTime);
            MovementTimer.Start(StepTime);

        }
    }

    void ResetMovement()
    {
        GD.Print("a7a");
        IsMoving = false;
    }

    void MoveToLoc(Vector2 loc, float t)
    {
        var MovementTween = CreateTween();
        MovementTween.TweenProperty(this, "global_position", loc, t);
    }






}
