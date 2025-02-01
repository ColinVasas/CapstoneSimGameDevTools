using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class States
{
    public string State;
    public UnityEvent OnStateSet;
    public UnityEvent OnStateUnset;
}

public class GameState : MonoBehaviour
{
    [HideInInspector] public States CurrentState;

    public States[] GameStates;
    
    private static GameState instance;
    
    void Awake()
    {
        if (instance) Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    void Start()
    {
        CurrentState = GameStates[0];
    }

    public void SetState(string newState)
    {
        foreach (States state in GameStates)
        {
            if (state.State == newState)
            {
                CurrentState.OnStateUnset.Invoke();
                
                CurrentState = state;
                CurrentState.OnStateSet.Invoke();
                
                Debug.LogWarning($"Game state set to {CurrentState.State}");
                
                return;
            }
        }

        Debug.LogError("GameState.SetState called with undefined state name.");
    }
}