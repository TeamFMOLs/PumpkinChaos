using Godot;
using System;
using System.Collections.Generic;

public class GameManager : Node2D
{
    [Export]
    private NodePath GameOverNodePath, GamePauseNodePath, GameMusicNodepath;

    private Node2D GameOverNode, GamePausedNode;
    private bool _isPlayerDead = false, _isGamePaused = false, musicStopped = false;


    public override void _EnterTree()
    {
        StaticRefs.gameManager = this;
    }
    public override void _Ready()
    {

        GameOverNode = GetNode(GameOverNodePath) as Node2D;
        GamePausedNode = GetNode<Node2D>("CanvasLayer/UI/GameIsPaused");
        StaticRefs.inputManager.OnPause += PauseGame;
        StaticRefs.inputManager.ContinueScene += ContinueScene;
        StaticRefs.inputManager.OnStopMusic += StopMusic;
    }

    public void OnPlayerDie()
    {
        GetTree().CreateTimer(1f, false).Connect("timeout", this, nameof(PlayerDeathComplete));
        StaticRefs.CurrentPlayer.StopMovement = true;
        StaticRefs.EnemySpawner.StopAll();
        GetNode<AudioStreamPlayer>(GameMusicNodepath).Stop();
        _isPlayerDead = true;
    }

    private void StopMusic()
    {
        if (musicStopped)
            GetNode<AudioStreamPlayer>(GameMusicNodepath).Play();
        else
            GetNode<AudioStreamPlayer>(GameMusicNodepath).Stop();
        musicStopped = !musicStopped;
    }

    private void ContinueScene()
    {
        if (_isPlayerDead)
        {
            StaticRefs.menu.ResetScene();
        }
    }

    private void PauseGame()
    {
        _isGamePaused = !_isGamePaused;
        GamePausedNode.Visible = _isGamePaused;
        GetTree().Paused = _isGamePaused;
        GD.Print(GetTree().Paused);
    }

    private void PlayerDeathComplete()
    {
        GameOverNode.Visible = true;
    }



    public override void _Notification(int what)
    {
        /* if (what == MainLoop.NotificationWmQuitRequest)
        {
            OS.ShellOpen("https://docs.google.com/forms/d/e/1FAIpQLScQ_wkXp0oNaqo6iGzrKTPb-ihzMIMu1u8D3lU-VTwzxwgfTw/viewform");
            GetTree().Quit(); // default behavior
        }
 */
    }

}
