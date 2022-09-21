using Godot;
using System;
using System.Collections.Generic;

public class UpgradeSystem : Node
{
    [Export]
    private float IncreasePerUpgrade = 0.2f ;
    [Export]
    private int LevelIncreaseDemandedScore = 1000;
    [Export]
    private NodePath UpgradeControlNodepath;
    [Export]
    private NodePath[] ButtonsPaths, LabelsPaths;
    private Button[] Buttons = new Button[3];
    private Label[] labels = new Label[3];
    private Control UpgradeMenu;
    private int CurrentLevelScore ;
    private int CurrentLevel = 0;
    
    private UpgradeOption[] currentOptions;

    [Export]
    UpgradeOption[] upgradeOptions;

   private AnimationPlayer animationPlayer;

    public override void _EnterTree()
    {
        StaticRefs.UpgradeSystem = this;
    }

    public override void _Ready()
    {
        CurrentLevelScore = LevelIncreaseDemandedScore;
        for (int i = 0; i < 3; i++)
        {
            Buttons[i] = GetNode<Button>(ButtonsPaths[i]);
            labels[i] = GetNode<Label>(LabelsPaths[i]);
        }
        animationPlayer = GetNode("AnimationPlayer") as AnimationPlayer;
        Buttons[0].Connect("pressed", this, nameof(OnButton1Pressed));
        Buttons[1].Connect("pressed", this, nameof(OnButton2Pressed));
        Buttons[2].Connect("pressed", this, nameof(OnButton3Pressed));
        
    }

    private UpgradeOption[] PickThreeOptions()
    {
        var rnd = new RandomNumberGenerator(); rnd.Randomize();
        var optionsList = new UpgradeOption[3];
        List<int> currentIndexes = new List<int>() { -1, -1, -1 };
        for (int i = 0; i < 3; i++)
        {
            var index = rnd.RandiRange(0, upgradeOptions.Length - 1);
            while (currentIndexes.Contains(index))
            {
                index = rnd.RandiRange(0, upgradeOptions.Length - 1);
            }
            currentIndexes[i] = index;
            optionsList[i] = upgradeOptions[index];
        }
        return optionsList;
    }

    private void InitializeOptions()
    {
        var options = PickThreeOptions();
        for (int i = 0; i < 3; i++)
        {
            Buttons[i].Icon = options[i].Texture;
            labels[i].Text = options[i].Text;
        }
        currentOptions = options;
        foreach (var item in currentOptions)
        {
            
                GD.Print(item.UpgradeOptionType);
            
        }
    }
    private void OnButton1Pressed() => OnButtonPressed(0);
    private void OnButton2Pressed() => OnButtonPressed(1);
    private void OnButton3Pressed() => OnButtonPressed(2);

    private void OnButtonPressed(int index)
    {
        GD.Print(index);
        GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();
        animationPlayer.Play("hide");
        GetTree().Paused = false;
        var result = currentOptions[index];
        GD.Print(result.UpgradeOptionType);
        switch (result.UpgradeOptionType)
        {
            case UpgradeOptionType.INCREASE_HEALTH:
                StaticRefs.CurrentPlayer.IncreaseHealth(IncreasePerUpgrade); break;
            case UpgradeOptionType.INCREASE_ATTACK_SPEED:
                StaticRefs.CurrentPlayer.IncreaseAttackSpeed(IncreasePerUpgrade); break;
            case UpgradeOptionType.INCREASE_CRIT_CHANCE:
                StaticRefs.CurrentPlayer.IncreaseCritChance(IncreasePerUpgrade); break;
            case UpgradeOptionType.INCREASE_DAMAGE:
                StaticRefs.CurrentPlayer.IncreaseDamage(IncreasePerUpgrade); break;
            case UpgradeOptionType.INCREASE_AMMO:
                StaticRefs.CurrentPlayer.IncreaseAmmoPickUp(1); break;
            default: break;
        }
    }

    public void OnLevelUp() {
        GetTree().Paused = true;
        animationPlayer.Play("show");
        InitializeOptions();
    }

    public void NotifyScore(int score) {
        if (score >= CurrentLevelScore)
        {
            CurrentLevel ++;
            StaticRefs.PlayerUi.UpdateLevel(CurrentLevel);
            CurrentLevelScore += (int)( (float)LevelIncreaseDemandedScore *(1+ 0.3f*(float) CurrentLevel));
            OnLevelUp();
        }
    }








}

