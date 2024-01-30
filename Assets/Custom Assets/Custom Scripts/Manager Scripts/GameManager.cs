using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static bool isDestoryed = false;
    public static bool IsDestoryed
    {
        get { return isDestoryed; }
        private set { isDestoryed = value; }
    }
    private void OnDestroy ()
    {
        IsDestoryed = false;
    }

    private static GameManager instance;
    public static GameManager GetInstance ()
    {
        if(GameManager.instance == null && IsDestoryed != true)
        {
            if(GameManager.instance == null)
            {
                GameObject container = new("GameManager");
                GameManager.instance = container.AddComponent<GameManager>();
                Object.DontDestroyOnLoad(container);
            }
        }
        return GameManager.instance;
    }

    public GameObject shipComponent;
    private bool isGenerated = false;

    public void HandleGenerateClick ()
    {
        if (isGenerated) return;
        Object.Instantiate(shipComponent);
        isGenerated = true;
    }

    public void HandleClearClick ()
    {
        if (!isGenerated) return;
        Object.Destroy(shipComponent);
        isGenerated = false;
    }
}
