using Godot;
using System;
using System.Collections.Generic;

public class GameManager : Node2D
{
    [Export]
    private NodePath GameOverNodePath;
    private Node2D GameOverNode;
    private bool _isPlayerDead = false;
    
    public override void _EnterTree()
    {
        StaticRefs.gameManager = this;
    }
    public override void _Ready()
    {
        
        GameOverNode = GetNode(GameOverNodePath) as Node2D;
    }


    public void OnPlayerReachedGoal() {
        
    }
    public void OnPlayerDie() {
        GetTree().CreateTimer(1f).Connect("timeout" , this ,nameof(PlayerDeathComplete));
        StaticRefs.CurrentPlayer.StopMovement = true;
        _isPlayerDead = true;
    }

    /* private void ChangePlayerAgent() {
        if (!_isPlayerDead)
        {
           StaticRefs.ChangePlayer(); 
        }    
    } */

    private void ContinueScene() {
        if (_isPlayerDead)
        {
            GetTree().ReloadCurrentScene();
        }
    }

    private void PlayerDeathComplete() {
        /* StaticRefs.PlayerSoul.GetParent().RemoveChild(StaticRefs.PlayerSoul);
        GetTree().Root.AddChild(StaticRefs.PlayerSoul);
        StaticRefs.PlayerSoul.GlobalPosition = StaticRefs.CurrentPlayer.GlobalPosition;
        StaticRefs.CurrentPlayer.QueueFree();
        StaticRefs.CurrentPlayer = null;
        SetNotifierState(GameOverNode , true); */
    }

    void SetNotifierState(Node2D it , bool state) {
        it.GlobalPosition = StaticRefs.CurrentCamera.GlobalPosition;
        it.Visible = state;
    }
}
