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
    private AnimatedSprite _animatedSprite,_Light,_Dark;
    private int lastInput=0;

    public override void _EnterTree()
    {
        StaticRefs.PlayerAgents.Add(this);
        if (_hasSoul)
        {
            StaticRefs.CurrentPlayer = this;
            _animatedSprite=_Light;
            StaticRefs.PlayerAgentIndex = StaticRefs.PlayerAgents.IndexOf(this);
        }
    }


    public override void _Ready()
    {
        inputManager = StaticRefs.inputManager;
        _Dark = GetNode<AnimatedSprite>("AnimatedSprite");
        _Light = GetNode<AnimatedSprite>("AnimatedSpriteL");
        animationPlayer = GetNode<AnimationPlayer>(AnimatorNodePath);
        MovementTimer = GetNode<Timer>(MovementTimerNodePath);
        stepsAudioPlayer = GetNode<AudioStreamPlayer2D>(StepsAudioPlayerPath);
        light = GetNode<Light2D>(SoulLightPath);
        MovementTimer.Connect("timeout", this, nameof(ResetMovement));
        MovementTimer.WaitTime = StepTime;
        camera = GetNode<Camera2D>("Camera2D");
        camera.Current = _hasSoul;
        light.Enabled = _hasSoul;
        if(_hasSoul)
            _animatedSprite=_Light;
        else
            _animatedSprite=_Dark;

        _animatedSprite.Visible=true;

        ResetMovement();
    }

    public override void _Process(float delta)
    {
        
        if (_hasSoul)
        {
            _animatedSprite=_Light;
            _Light.Visible=true;
            _Dark.Visible=false;
            
            var vel = Vector2.Zero;
            if (inputManager.MoveUp)
            {
                vel = Vector2.Up ;
                switch (lastInput){
                    case 2:
                            _animatedSprite.Play("Right_Back");break;
                    case 3:
                            _animatedSprite.Play("Front_Back");break;
                    case 4:
                            _animatedSprite.Play("Left_Back");break;
                }
                lastInput=1;
            }

            if (inputManager.MoveRight)
            {
                vel = Vector2.Right ;
                switch (lastInput){
                    case 1:
                            _animatedSprite.Play("Back_Right");break;
                    case 3:
                            _animatedSprite.Play("Front_Right");break;
                    case 4:
                            _animatedSprite.Play("Left_Right");break;
                }
                lastInput=2;
            }

            if (inputManager.MoveDown)
            {
                
                vel = Vector2.Down ;
                switch (lastInput){
                    case 1:
                            _animatedSprite.Play("Back_Front");break;
                    case 2:
                            _animatedSprite.Play("Right_Front");break;
                    case 4:
                            _animatedSprite.Play("Left_Front");break;
                }
                lastInput=3;

            }

            if (inputManager.MoveLeft)
            {
                vel = Vector2.Left ;
                switch (lastInput){
                    case 1:
                            _animatedSprite.Play("Back_Left");break;
                    case 2:
                            _animatedSprite.Play("Right_Left");break;
                    case 3:
                            _animatedSprite.Play("Front_Left");break;
                }
                lastInput=4;
            }

            if (vel != Vector2.Zero)
            {
                GD.Print("why");
                Move(vel.Normalized());
            }
        }
        else{
                _animatedSprite=_Dark;
                switch (lastInput){
                    case 1:
                            _animatedSprite.Play("Back_Front");_animatedSprite.Frame = 0;break;
                    case 2:
                            _animatedSprite.Play("Right_Front");_animatedSprite.Frame = 0;break;
                    case 3:
                            _animatedSprite.Play("Front_Back");_animatedSprite.Frame = 0;break;
                    case 4: 
                            _animatedSprite.Play("Left_Front");_animatedSprite.Frame = 0;break;      
                }
                _Light.Visible=false;
                _Dark.Visible=true;
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
