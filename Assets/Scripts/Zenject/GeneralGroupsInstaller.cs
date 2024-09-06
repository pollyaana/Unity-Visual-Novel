using UnityEngine;
using Zenject;

public class GeneralGroupsInstaller : MonoInstaller
{
    [SerializeField] private GroupsInstaller groupsInstaller;

    public override void InstallBindings()
    {
        BindNameInstaller();
    }
    private void BindNameInstaller()
    {
        Container.Bind<GroupsInstaller>().FromInstance(groupsInstaller).AsSingle();
    }
}
