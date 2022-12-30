using HeadPositionViewer.Models;
using Zenject;

namespace HeadPositionViewer.Installers
{
    public class HeadPositionPlayerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            this.Container.BindInterfacesAndSelfTo<HeadPositionController>().FromNewComponentOnNewGameObject().AsCached().NonLazy();
        }
    }
}
