using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public enum MuGameState
{
    Playing, End, EndUI
}

public class MuGameManager : MonoBehaviour
{
    public static List<GameObject> Targets = new List<GameObject>();
    public static MuGameState GameState = MuGameState.Playing;
    private void Start() 
    {
        Targets = GameObject.FindGameObjectsWithTag("Target").ToList();
    }

    


}
