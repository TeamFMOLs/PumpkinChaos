using Godot;
using System;

public class Player : CharacterController, IDestructible
{
    [Export]
    private float speed = 200,speedCopy,dashSpeed=1200;
    [Export]
    NodePath WeaponNodePath;
    [Export]
    private int MaxHealth ;
    [Export]
    private float PushDistance = 64 ,PushTime = 0.6f , ShieldOnTime = 1f,DashCd=2f, DashingTime=0.15f;
    private int _score = 0 , AmmoAdditionalpickUp = 0;
    
    private InputManager _inputHandler;
    private Weapon _weapon;
    public HealthSystem healthSystem { get; set; }
    public Weapon Weapon { get => _weapon; set => _weapon = value; }
    public int Score { get => _score; set {_score = value; StaticRefs.UpgradeSystem.NotifyScore(_score);} }

    public bool StopMovement;
    private AnimationPlayer animationPlayer;
    private SceneTreeTween MovementTween;
    protected Timer DashTimer,DashCdTimer;
    protected bool _Dashing=false, _CanDash=true;

    public override void _EnterTree()
    {
        base._EnterTree();
        StaticRefs.CurrentPlayer = this;
    }
    public override void _Ready()
    {
        _inputHandler = StaticRefs.inputManager;
        speedCopy=speed;

        DashTimer = GetNode("DashingTimer") as Timer;
        DashTimer.WaitTime = DashingTime;
        DashTimer.Connect("timeout", this, nameof(StopDash));

        DashCdTimer = GetNode("DashCdTimer") as Timer;
        DashCdTimer.WaitTime = DashCd;
        DashCdTimer.Connect("timeout", this, nameof(ReEnableDash));

        Weapon = GetNode<Weapon>(WeaponNodePath);
        healthSystem = GetNode<HealthSystem>("HealthSystem");
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

        _inputHandler.OnAttack += Attack;
        _inputHandler.OnMelee += Melee;
        _inputHandler.OnDash += Dash;

        healthSystem.OnTakeDamage += OnTakeDamage ;
        healthSystem.ResetHealth(MaxHealth);
        StaticRefs.PlayerUi.UpdateAmmoNumber(Weapon.Ammo);
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        Weapon.WeaponPointAt(_inputHandler.MousePos);
    }

    public override void _PhysicsProcess(float delta)
    {
        Velocity = _inputHandler.MoventDir * speed;
        base._PhysicsProcess(delta);
        var it = GetLastSlideCollision();
        if (it != null  )
        {
            if (it.Collider is BasicEnemy)
            {
                var enemy =    it.Collider as BasicEnemy;
                if(!_Dashing){
                    GetPushed(GlobalPosition-enemy.GlobalPosition,enemy.OnHitDamage);
                }
            }
            
        }
    }

    public void Attack(Vector2 target) {
        if (Weapon.Attack(target))
        {
            StaticRefs.CurrentCamera.ShakeForSeconds(0.2f,6f);
        }
    }
    public void Melee() {
        Weapon.Melee();
    }
    public void Dash() {
        if(_Dashing||!_CanDash){return;}
        DashTimer.Start();
        DashCdTimer.Start();
        GD.Print("Iam SPEEEED!:Dash");
        speed=dashSpeed;
        _Dashing=true;
        _CanDash=false;
        animationPlayer.Play("Dash");
    }
    public void ReEnableDash()
    {
        _CanDash = true;
        GD.Print("You Can Dash Again ******!:DashReEnable");
    }
    public void StopDash()
    {
        _Dashing = false;
        GD.Print("Iam SPEEEED Demorgen-ed!:DashStopeeee");
        speed=speedCopy;
    }
    public void AddAmmo(Ammo it) {
        Weapon.Ammo += 1+AmmoAdditionalpickUp;
        StaticRefs.PlayerUi.UpdateAmmoNumber(Weapon.Ammo) ;
    }

    private void OnTakeDamage() {
        if(!_Dashing){
            animationPlayer.Play("get_hurt");
            StaticRefs.CurrentCamera.ShakeForSeconds(0.35f,10f);
            GD.Print(healthSystem.Health);
            GD.Print(healthSystem.MaxHealth);
            StaticRefs.PlayerUi.UpdateHp(((float)healthSystem.Health)/((float)healthSystem.MaxHealth)*100f);
        }
    }

    public void IncreaseScore(int amount) {
        Score+= amount;
        StaticRefs.PlayerUi.UpdateScore(Score);
    }

    public void GetPushed(Vector2 dir , int damage) {
        healthSystem.TakeDamage(damage);
        healthSystem.IsShielded = true;
        MovementTween = CreateTween();
        var pos = GlobalPosition + dir.Normalized()*PushDistance;
        MovementTween.TweenProperty(this,"global_position",pos,PushTime);
        CreateTween().TweenCallback(this,nameof(DisableShield)).SetDelay(ShieldOnTime);
        
    }

    private void DisableShield() {
        healthSystem.IsShielded = false;
        MovementTween = null;
    }

    public void IncreaseHealth(float p) {
        healthSystem.MaxHealth += (int)(healthSystem.MaxHealth*p);
        healthSystem.Health += (int)(healthSystem.Health*p);
    }

    public void IncreaseAttackSpeed(float p) {
        Weapon.AttackSpeed *= 1f +p;
    }

    public void IncreaseDamage(float p) {
        Weapon.BulletDamage += (int) (Weapon.BulletDamage *p);
    }

    public void IncreaseAmmoPickUp(int n) {
        GD.Print(n);
        AmmoAdditionalpickUp += n;
    }

    public void IncreaseCritChance(float p) {
        _weapon.CritChance +=p;
    }

}
