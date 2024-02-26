using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBase : CollectableBase
{
    [Header("Power Up Duration")]
    public float duration;


    protected override void OnCollect() // override = sobreescreve a fun��o orgiinal que est� sendo herdada, adicionando comandos
    { 
        base.OnCollect(); 
        StartPowerUp(); 
    }

    protected virtual void StartPowerUp() 
    { 
        Debug.Log("Start Power Up");
        Invoke(nameof(EndPowerUp), duration); 
    }

    protected virtual void EndPowerUp() 
    { 
        Debug.Log("End Power Up"); 
    }






}
