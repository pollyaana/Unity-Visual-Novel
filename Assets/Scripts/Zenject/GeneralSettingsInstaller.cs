using UnityEngine;
using Zenject;

public class GeneralSettingsInstaller : MonoInstaller
{
    [SerializeField] private SettingsInstaller settingsInstaller;

    public override void InstallBindings()
    {
        BindSettingsInstaller();
    }

    private void BindSettingsInstaller()
    {
        Container.Bind<SettingsInstaller>().FromInstance(settingsInstaller).AsSingle();
    }
}
