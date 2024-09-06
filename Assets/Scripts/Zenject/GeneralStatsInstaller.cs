using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GeneralStatsInstaller : MonoInstaller
{
    [SerializeField] private StatsInstaller statsInstaller;
    public override void InstallBindings()
    {
        BindStatsInstaller();
    }

    private void BindStatsInstaller()
    {
        Container.Bind<StatsInstaller>().FromInstance(statsInstaller).AsSingle();
    }
}
