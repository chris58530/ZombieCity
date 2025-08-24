using UnityEngine;

public class SelectPlayerProxy : IProxy
{
    public int[] selectedPlayers;

    public void AddSelectPlayer(int playerId)
    {
        if (selectedPlayers == null)
        {
            selectedPlayers = new int[0];
        }

        int[] newSelectedPlayers = new int[selectedPlayers.Length + 1];
        for (int i = 0; i < selectedPlayers.Length; i++)
        {
            newSelectedPlayers[i] = selectedPlayers[i];
        }
        newSelectedPlayers[newSelectedPlayers.Length - 1] = playerId;
        selectedPlayers = newSelectedPlayers;
    }
    public void RemoveSelectPlayer(int playerId)
    {
        if (selectedPlayers == null || selectedPlayers.Length == 0) return;

        int index = System.Array.IndexOf(selectedPlayers, playerId);
        if (index < 0) return;

        int[] newSelectedPlayers = new int[selectedPlayers.Length - 1];
        for (int i = 0, j = 0; i < selectedPlayers.Length; i++)
        {
            if (i == index) continue;
            newSelectedPlayers[j++] = selectedPlayers[i];
        }
        selectedPlayers = newSelectedPlayers;
    }
    public void RemoveAllSelectPlayers()
    {
        selectedPlayers = new int[0];
    }
}
