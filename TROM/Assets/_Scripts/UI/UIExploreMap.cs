using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIExploreMap : UIPanel
{
    public void ToCampScene()
    {
        SceneLoader.Singleton.LoadScene("CampScene");
    }
    public void ToDepartmentStoreScene()
    {
        SceneLoader.Singleton.LoadScene("DepartmentStoreScene");
    }
}
