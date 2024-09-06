using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GeneralPointsInstaller : MonoInstaller
{
    [SerializeField] private PointsInstaller pointsInstaller;

    public override void InstallBindings()
    {
        BindPointsInstaller();
    }

    private void BindPointsInstaller()
    {
        Container.Bind<PointsInstaller>().FromInstance(pointsInstaller).AsSingle();
    }
}
    