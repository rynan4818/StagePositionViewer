using HeadPositionViewer.Models;
using HeadPositionViewer.Views;
using Zenject;

namespace HeadPositionViewer.Installers
{
    public class HeadPositionPlayerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            this.Container.BindInterfacesAndSelfTo<HeadPositionUI>().FromNewComponentAsViewController().AsCached().NonLazy();
            this.Container.BindInterfacesAndSelfTo<HeadPositionController>().FromNewComponentOnNewGameObject().AsCached().NonLazy();
        }
    }
}
