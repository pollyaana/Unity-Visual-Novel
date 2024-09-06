using UnityEngine;
using Zenject;

public class GeneralPupilInstaller : MonoInstaller
{
    [SerializeField] private PupilInstaller pupilInstaller;
    public override void InstallBindings()
    {
        BindDialogueInstaller();
    }
    private void BindDialogueInstaller()
    {
        Container.Bind<PupilInstaller>().FromInstance(pupilInstaller).AsSingle();
    }
}
