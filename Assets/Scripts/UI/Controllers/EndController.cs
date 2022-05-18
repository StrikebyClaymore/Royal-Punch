using System;
using System.Security.Claims;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndController : BaseController<EndView>
{
    private void Start()
    {
        ConnectActions();
    }

    private void Claim()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    private void ConnectActions()
    {
        ui.OnClaimPressed += Claim;
    }
}