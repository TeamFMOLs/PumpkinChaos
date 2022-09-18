using Godot;
using System;

public class Weapon : Node2D
{
    [Export]
    protected float AttackSpeed = 1.5f, BulletSpeed = 200f;
    [Export]
    protected PackedScene BulletScene;
    [Export]
    private int ammo = 20;
    protected Timer FireTimer;
    protected bool _canFire = true;
    private Node2D WeaponHead;

    public int Ammo { get => ammo; set => ammo = value; }

    public override void _Ready()
    {
        FireTimer = GetNode("Timer") as Timer;
        WeaponHead = GetNode("GunParent/GunHead") as Node2D;
        FireTimer.WaitTime = 1 / AttackSpeed;
        FireTimer.Connect("timeout", this, nameof(ReEnableFire));
    }
    public void WeaponPointAt(Vector2 target)
    {
        var angle = (target - GlobalPosition).Angle();
        if (angle < Mathf.Pi/2 && angle > -Mathf.Pi/2)
        {
            angle = angle - Mathf.Pi;
            Scale = new Vector2(-1, -1);
        }
        else
            Scale = new Vector2(1,- 1);
        GlobalRotation = angle;
        
    }

    public bool Attack(Vector2 target)
    {
        if (_canFire && ammo > 0)
        {
            Fire(target);
            _canFire = false;
            FireTimer.Start();
            return true;
        }
        return false;
    }

    public void ReEnableFire()
    {
        _canFire = true;
    }


    protected void Fire(Vector2 taget)
    {
        ammo--;
        var bullet = BulletScene.Instance() as Bullet;
        GetTree().Root.AddChild(bullet);
        GetNode<AnimationPlayer>("AnimationPlayer").Play("Fire");
        StaticRefs.PlayerUi.UpdateAmmoNumber(ammo);
        bullet.GlobalPosition = WeaponHead.GlobalPosition;
        bullet.GlobalRotation = WeaponHead.GlobalRotation;
        bullet.Velocity = BulletSpeed * (taget - GlobalPosition).Normalized();
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.

}
