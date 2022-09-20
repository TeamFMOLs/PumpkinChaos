using Godot;
using System;

public class UpgradeOption : Resource
{
    [Export]
    public Texture Texture;
    [Export]
    public string Text;
    [Export]
    public UpgradeOptionType UpgradeOptionType;
}

public enum UpgradeOptionType
{
    INCREASE_HEALTH,
    INCREASE_ATTACK_SPEED,
    INCREASE_CRIT_CHANCE,
    INCREASE_DAMAGE,
    INCREASE_AMMO
}
