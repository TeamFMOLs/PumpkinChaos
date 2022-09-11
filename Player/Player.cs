using Godot;
using System;

public class Player : KinematicBody2D
{
    [Export]
    NodePath AnimatorNodePath;
    [Export]
    NodePath MovementTimerNodePath, StepsAudioPlayerPath, SoulLightPath;
    private InputManager inputManager;
    private AnimationPlayer animationPlayer;
    private AudioStreamPlayer2D stepsAudioPlayer;
    [Export]
    private float StepSize = 80f;
    [Export]
    private float StepTime = 0.5f;
    public bool HasSoul { get { return _hasSoul; } set { _hasSoul = value; light.Enabled = value; camera.Current = value; } }
    private Timer MovementTimer;
    private bool IsMoving = false;
    [Export]
    private bool _hasSoul;
    private Light2D light;
    public Camera2D PlayerCamera { get { return camera; } }
    private Camera2D camera;
    // private SceneTreeTween MovementTween;
    private Vector2 lastPos;

    public override void _EnterTree()
    {
        StaticRefs.PlayerAgents.Add(this);
        if (_hasSoul)
        {
            StaticRefs.CurrentPlayer = this;
            StaticRefs.PlayerAgentIndex = StaticRefs.PlayerAgents.IndexOf(this);
        }
    }


    public override void _Ready()
    {
        inputManager = StaticRefs.inputManager;
        animationPlayer = GetNode<AnimationPlayer>(AnimatorNodePath);
        MovementTimer = GetNode<Timer>(MovementTimerNodePath);
        stepsAudioPlayer = GetNode<AudioStreamPlayer2D>(StepsAudioPlayerPath);
        light = GetNode<Light2D>(SoulLightPath);
        MovementTimer.Connect("timeout", this, nameof(ResetMovement));
        MovementTimer.WaitTime = StepTime;
        camera = GetNode<Camera2D>("Camera2D");
        camera.Current = _hasSoul;
        light.Enabled = _hasSoul;

        ResetMovement();
    }

    public override void _Process(float delta)
    {
        if (_hasSoul)
        {

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


    }

    void Move(Vector2 dir)
    {
        if (!IsMoving)
        {
            (camera as SceneCamera).ShakeForSeconds(0.3f, 5);
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
