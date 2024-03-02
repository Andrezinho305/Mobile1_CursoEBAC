using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EBAC.Core.Singleton;

public class ArtManager : Singleton<ArtManager>
{
    public enum ArtType
    {
        TYPE_01,
        TYPE_02,
        TYPE_03,
        TYPE_04
    }

    public List<ArtSettup> artSettup;

    public ArtSettup GetSetupByType(ArtType artType)
    {
        return artSettup.Find(i => i.artType == artType);
    }

}

[System.Serializable]
public class ArtSettup
{
    public ArtManager.ArtType artType;
    public GameObject gameObject;

}