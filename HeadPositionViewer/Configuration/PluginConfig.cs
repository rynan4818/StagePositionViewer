using System.Runtime.CompilerServices;
using IPA.Config.Stores;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]
namespace HeadPositionViewer.Configuration
{
    internal class PluginConfig
    {
        public static PluginConfig Instance { get; set; }

        // BSIPAが値の変更を検出し、自動的に設定を保存したい場合は、'virtual'でなければなりません。
        public virtual bool Enable { get; set; } = true;
        public virtual bool LockPosition { get; set; } = false;
        public virtual float movementSensitivityThreshold { get; set; } = 0.01f;  //HMD移動検出の閾値
        public virtual float ScreenPosX { get; set; } = 0f;
        public virtual float ScreenPosY { get; set; } = 0.7f;
        public virtual float ScreenPosZ { get; set; } = -1.1f;
        public virtual float ScreenRotX { get; set; } = 0;
        public virtual float ScreenRotY { get; set; } = 0;
        public virtual float ScreenRotZ { get; set; } = 0;
        public virtual float ScreenSize { get; set; } = 40f;
        public virtual int ScreenLayer { get; set; } = 5;
        /// <summary>
        /// これは、BSIPAが設定ファイルを読み込むたびに（ファイルの変更が検出されたときを含めて）呼び出されます。
        /// </summary>
        public virtual void OnReload()
        {
            // 設定ファイルを読み込んだ後の処理を行う。
        }

        /// <summary>
        /// これを呼び出すと、BSIPAに設定ファイルの更新を強制します。 これは、ファイルが変更されたことをBSIPAが検出した場合にも呼び出されます。
        /// </summary>
        public virtual void Changed()
        {
            // 設定が変更されたときに何かをします。
        }

        /// <summary>
        /// これを呼び出して、BSIPAに値を<paramref name ="other"/>からこの構成にコピーさせます。
        /// </summary>
        public virtual void CopyFrom(PluginConfig other)
        {
            // このインスタンスのメンバーは他から移入されました
        }
    }
}
