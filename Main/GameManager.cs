using Godot;
using System;
using System.Collections.Generic;

public class GameManager : Node2D
{
    [Export]
    private NodePath GameOverNodePath ,GamePauseNodePath;
    private Node2D GameOverNode;
    private bool _isPlayerDead = false, _isGamePaused = false;

    public override void _EnterTree()
    {
        StaticRefs.gameManager = this;
    }
    public override void _Ready()
    {

        GameOverNode = GetNode(GameOverNodePath) as Node2D;
        StaticRefs.inputManager.OnPause += PauseGame;
    }

    public void OnPlayerDie()
    {
        GetTree().CreateTimer(1f, false).Connect("timeout", this, nameof(PlayerDeathComplete));
        StaticRefs.CurrentPlayer.StopMovement = true;
        _isPlayerDead = true;
    }

    private void ContinueScene()
    {
        if (_isPlayerDead)
        {
            GetTree().ReloadCurrentScene();
        }
    }

    private void PauseGame()
    {
        _isGamePaused = !_isGamePaused;
        GetNode<Node2D>(GamePauseNodePath).Visible = _isGamePaused;
        GetTree().Paused = _isGamePaused;
        GD.Print( GetTree().Paused);
    }

    private void PlayerDeathComplete()
    {
        GameOverNode.Visible = true;
    }


}
