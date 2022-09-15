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
    private bool _isDead = false;
    public bool HasSoul
    {
        get { return _hasSoul; }
        set
        {
            _hasSoul = value;
            light.Enabled = value;
            animationPlayer.Play(value ? "Idle" : "RESET");

        }
    }
    public bool StopMovement
    {
        get => _stopMovement;
        set => _stopMovement = value;
    }
    private Timer MovementTimer;
    private bool IsMoving = false;
    [Export]
    private bool _hasSoul;
    private bool _stopMovement;
    private Light2D light;

    private Camera2D camera;
    // private SceneTreeTween MovementTween;
    private Vector2 lastPos;
    private AnimatedSprite _animatedSprite, _Light, _Dark;

    private int lastInput = 0;

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
        _Dark = GetNode<AnimatedSprite>("SpriteParent/AnimatedSprite");
        _Light = GetNode<AnimatedSprite>("SpriteParent/AnimatedSpriteL");
        animationPlayer = GetNode<AnimationPlayer>(AnimatorNodePath);
        MovementTimer = GetNode<Timer>(MovementTimerNodePath);
        stepsAudioPlayer = GetNode<AudioStreamPlayer2D>(StepsAudioPlayerPath);
        light = GetNode<Light2D>(SoulLightPath);
        MovementTimer.Connect("timeout", this, nameof(ResetMovement));
        MovementTimer.WaitTime = StepTime;
        light.Enabled = _hasSoul;
        if (_hasSoul)
        {
            GetNode<Soul>("Soul").SetCurrent();
            _animatedSprite = _Light;
        }
        else
        {
            GetNode<Soul>("Soul").QueueFree();
            _animatedSprite = _Dark;
        }
        camera = StaticRefs.CurrentCamera;

        _animatedSprite.Visible = true;

        ResetMovement();
    }

    public override void _Process(float delta)
    {

        if (_hasSoul && !StopMovement)
        {
            _animatedSprite = _Light;
            _Light.Visible = true;
            _Dark.Visible = false;

            var vel = Vector2.Zero;
            if (inputManager.MoveUp)
            {
                vel = Vector2.Up;
                switch (lastInput)
                {
                    case 2:
                        _animatedSprite.Play("Right_Back"); break;
                    case 3:
                        _animatedSprite.Play("Front_Back"); break;
                    case 4:
                        _animatedSprite.Play("Left_Back"); break;
                }
                lastInput = 1;
            }

            if (inputManager.MoveRight)
            {
                vel = Vector2.Right;
                switch (lastInput)
                {
                    case 1:
                        _animatedSprite.Play("Back_Right"); break;
                    case 3:
                        _animatedSprite.Play("Front_Right"); break;
                    case 4:
                        _animatedSprite.Play("Left_Right"); break;
                }
                lastInput = 2;
            }

            if (inputManager.MoveDown)
            {

                vel = Vector2.Down;
                switch (lastInput)
                {
                    case 1:
                        _animatedSprite.Play("Back_Front"); break;
                    case 2:
                        _animatedSprite.Play("Right_Front"); break;
                    case 4:
                        _animatedSprite.Play("Left_Front"); break;
                }
                lastInput = 3;

            }

            if (inputManager.MoveLeft)
            {
                vel = Vector2.Left;
                switch (lastInput)
                {
                    case 1:
                        _animatedSprite.Play("Back_Left"); break;
                    case 2:
                        _animatedSprite.Play("Right_Left"); break;
                    case 3:
                        _animatedSprite.Play("Front_Left"); break;
                }
                lastInput = 4;
            }

            if (vel != Vector2.Zero)
            {
                Move(vel.Normalized());
            }
        }
        else
        {
            _animatedSprite = _Dark;
            switch (lastInput)
            {
                case 1:
                    _animatedSprite.Play("Back_Front"); _animatedSprite.Frame = 0; break;
                case 2:
                    _animatedSprite.Play("Right_Front"); _animatedSprite.Frame = 0; break;
                case 3:
                    _animatedSprite.Play("Front_Back"); _animatedSprite.Frame = 0; break;
                case 4:
                    _animatedSprite.Play("Left_Front"); _animatedSprite.Frame = 0; break;
            }
            _Light.Visible = false;
            _Dark.Visible = true;
        }


    }

    void Move(Vector2 dir)
    {
        if (!IsMoving)
        {
            var newPos = GlobalPosition + dir * StepSize;
            if (!CheckCollision(newPos))
            {
                (camera as SceneCamera).ShakeForSeconds(0.3f, 5);
                lastPos = GlobalPosition;
                IsMoving = true;
                animationPlayer.Play("SlightJump");

                stepsAudioPlayer.Play();
                MoveToLoc(newPos, StepTime);
                MovementTimer.Start(StepTime);
            }


        }
    }

    public void ReachedGoal()
    {
        animationPlayer.Play("SlightJump");
        GD.Print(_animatedSprite==_Light);
        _animatedSprite.Play("Front_Back");
        _animatedSprite.Frame = 1;
         GD.Print(HasSoul);
    }

    void ResetMovement()
    {

        IsMoving = false;
    }

    public void Die()
    {
        if (!_isDead)
        {
            animationPlayer.Play("Die");
            GetNode<AudioStreamPlayer2D>("DeathSound").Play();
            _isDead = true;
            StaticRefs.gameManager.OnPlayerDie();
        }

    }

    public void MoveToLoc(Vector2 loc, float t)
    {
        var MovementTween = CreateTween();
        MovementTween.TweenProperty(this, "global_position", loc, t);
    }

    private bool CheckCollision(Vector2 dir)
    {
        var spaceState = GetWorld2d().DirectSpaceState;
        var result = spaceState.IntersectRay(GlobalPosition, dir);
        return result.Count != 0;
    }






}
