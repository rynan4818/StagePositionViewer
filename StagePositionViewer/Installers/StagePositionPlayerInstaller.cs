using StagePositionViewer.Models;
using StagePositionViewer.Views;
using Zenject;

namespace StagePositionViewer.Installers
{
    public class StagePositionPlayerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            this.Container.BindInterfacesAndSelfTo<StagePositionUI>().FromNewComponentAsViewController().AsCached().NonLazy();
            this.Container.BindInterfacesAndSelfTo<StagePositionController>().FromNewComponentOnNewGameObject().AsCached().NonLazy();
        }
    }
}
