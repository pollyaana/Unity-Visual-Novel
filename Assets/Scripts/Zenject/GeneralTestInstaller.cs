using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GeneralTestInstaller : MonoInstaller
{
    [SerializeField] private TestInstaller testInstaller;

    public override void InstallBindings()
    {
        BindTestInstaller();
    }

    private void BindTestInstaller()
    {
        Container.Bind<TestInstaller>().FromInstance(testInstaller).AsSingle();
    }
}
