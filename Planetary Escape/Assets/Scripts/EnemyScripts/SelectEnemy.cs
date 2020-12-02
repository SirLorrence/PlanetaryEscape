using System;
using UnityEngine;

[Serializable]
public class SelectEnemy 
{
    public GameObject Weak;
    public GameObject Regular;
    public GameObject Strong;
    public GameObject SetEnemyType(int typeNum)
    {
      switch (typeNum) // returns the gameobject with all its stats and model
        {
            case (int)GameManager.EnemyTypes.Weak:
                return Weak;
            case (int)GameManager.EnemyTypes.Medium:
                return Regular;
            case (int)GameManager.EnemyTypes.Strong:
                return Strong;
        }
      throw new NotImplementedException();
  }
}