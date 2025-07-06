using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class WinTracker : MonoBehaviour
{
    public static WinTracker sInstance { get; private set; }

    List<EnemyTankBehavior> enemyTanks = new List<EnemyTankBehavior>();

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void Awake()
    {
        if (sInstance != null && sInstance != this)
        {
            Destroy(this);
        }
        else
        {
            sInstance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void AddTankToTracker(EnemyTankBehavior newTank)
    {
        if (!enemyTanks.Contains(newTank))
        {
            enemyTanks.Add(newTank);
        }
    }

    public void RemoveTankFromTracker(EnemyTankBehavior tankDying)
    {
        if (enemyTanks.Contains(tankDying))
        {
            enemyTanks.Remove(tankDying);
        }
        
        if (enemyTanks.Count <= 0)
        {
            PlayerController.sInstance.SetControlEnabled(false);
            transform.GetChild(0).gameObject.SetActive(true);
            SaveManager.sInstance.SetLevelsUnlocked(1);
        }
    }


}
