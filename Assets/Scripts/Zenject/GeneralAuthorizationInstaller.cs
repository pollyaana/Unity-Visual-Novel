using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GeneralAuthorizationInstaller : MonoInstaller
{
    [SerializeField] private AuthorizationInstaller authorizationInstaller;

    public override void InstallBindings()
    {
        BindAuthInstaller();
    }

    private void BindAuthInstaller()
    {
        Container.Bind<AuthorizationInstaller>().FromInstance(authorizationInstaller).AsSingle();
    }
}
