using Godot;
using System;
using System.Collections.Generic;

public static class StaticRefs 
{
    public static InputManager inputManager;
    public static Player CurrentPlayer;
    
    public static List<Player> PlayerAgents = new List<Player>();
    public static int PlayerAgentIndex = 0; 

    public static void ChangePlayer() {
        CurrentPlayer.HasSoul = false;
        PlayerAgentIndex ++;
        if (PlayerAgentIndex >= PlayerAgents.Count)
        {
            PlayerAgentIndex = 0;
        }
        PlayerAgents[PlayerAgentIndex].HasSoul = true;
        CurrentPlayer =  PlayerAgents[PlayerAgentIndex];
    }
}
