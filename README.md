# StagePositionViewer

このBeatSaberプラグインは、ステージ上のプレイヤー(HMD)の位置をグラフィカルに表示します。

部屋の環境に合わせて前後左右の限界ラインを設定して、限界に近づきすぎると青色から黄色、赤色にマークの色が変わります。

中央の壁抜け判断用に、Ｘ軸が±0.011mの範囲になると白色にマークの色が変わります。

ポーズ中に表示位置を好きな場所に移動できます。また、枠やマークのサイズ等も設定から自由に変更可能です。

![image](https://user-images.githubusercontent.com/14249877/210227621-3b5585ab-e77a-4067-850e-004ba6ac95be.png)

# インストール方法

1. [リリースページ](https://github.com/rynan4818/StagePositionViewer/releases)から最新のStagePositionViewerのリリースをダウンロードします。
2. ダウンロードしたzipファイルを`Beat Saber`フォルダに解凍して、`Plugin`フォルダに`StagePositionViewer.dll`ファイルをコピーします。

# 使い方

1. まず適当な譜面をスタートして、すぐにポーズして下さい。そうすると下記画面の様に座標が表示されます。

    ![image](https://user-images.githubusercontent.com/14249877/210227555-a92f7f15-91c3-41a3-8200-3c66be3b7101.png)

2. その状態で部屋の前後左右に移動して、これ以上行けない場所の座標を覚えるかメモします。前後はZで、左右はXです。

    例えば、前方ギリギリの場所がZ:0.7、後方ギリギリの場所がZ:-0.5、右側ギリギリの場所がX:1.2、左側ギリギリの場所がX:-1.2だとします。
3. メニューに戻って、左のMOD設定画面から`STAGE POSITION VIEWER`の設定画面を開きます。
4. 設定の下の方にある`Front, Back, Right, Left Limit Line [m]`を、それぞれ先程調べた座標値に設定します。マイナスは無視してプラス値で入れて下さい。

    ![image](https://user-images.githubusercontent.com/14249877/210227968-4b49f862-1976-4e73-bb9a-78da7ef07802.png)

5. その下にある`Yellow, Red Warning`は、Limit Lineに何%近づくとマークの色が変わるかの設定です。好みで調整して下さい。
6. 再度、適当な譜面をプレイしてポーズして、限界ラインが変わったか確認します。
7. 次に表示位置です、ポーズ中は白いハンドルが上部に表示されるので、それをコントローラでつかんで移動できます。
8. 奥行き方向の移動はやりにくいので、一旦先程の設定画面に戻って`Screen Z Position`で調整して下さい。値を小さくするほど近づきます。

    ![image](https://user-images.githubusercontent.com/14249877/210228203-072e3c83-aaee-4b72-a46d-16803c088773.png)

9. 画面の中央にしたい場合は`RESET CENTER`ボタンを押して下さい。また傾きをなくして真正面に向けたい場合は`RESET ROTATION`を押して下さい。初期位置に戻したい場合は`DEFAULT POSITION`です。
10. 奥行方向を移動させる時は`Screen Size`も調整して下さい。値を大きくすると表示が大きくなります。

    ![image](https://user-images.githubusercontent.com/14249877/210228363-71a12044-2480-4f46-a355-561c0c40ea3f.png)

11. 位置が決まったら`Lock Screen Position`をONにすると、ポーズしても移動できなくなります。
12. `First Person / HMD Only View`をONにすると、HMDと一人称画面のみに表示して、三人称カメラからは表示が消えます。ダンスの録画などにどうぞ。

    ![image](https://user-images.githubusercontent.com/14249877/210228516-1488a5a0-9988-4a2a-bb93-25500f30881d.png)

13. `X Center Signal`をOFFにすると、X(横方向)が壁抜け出来る中心部分でマークが白色にならなくなります。ボッ立ちプレイしない人、白色変化が煩わしい人はOFFにして下さい。

# 設定について

## MOD設定画面
| 項目 | デフォルト値 | 説明 |
|------|--------------|------|
| Enable | ON | modの機能を有効にします |
| Lock Screen Position | OFF | ポーズ中の移動をできなくします |
| Screen Z Position | 20 | 表示位置のZ位置 |
| RESET CENTER | - | 表示位置のX位置を0にします |
| RESET ROTATION | - | 表示位置のX,Y,Z角度を0にします |
| DEFAULT POSITION | - | 表示位置を初期値にします |
| First Person / HMD Only View | OFF | 一人称及びHMDのみ表示して、三人称カメラから表示を消します |
| Position Value View | OFF | プレイ中も座標を表示します |
| X Center Signal | ON | プレイヤーのX位置が壁抜け出来る範囲の時に白色マークにします |
| Screen Size | 40 | 表示画面の大きさ |
| Front Limit Line [m] | 0.6 | 前方の限界ライン |
| Back Limit Line [m] | 0.6 | 後方の限界ライン |
| Right Limit Line [m] | 1.3 | 右側の限界ライン |
| Left Limit Line [m] | 1.3 | 左側の限界ライン |
| Yellow Warning | 40% | 限界ラインに近づいた時にマークが黄色になる位置 |
| Red Warning | 80% | 限界ラインに近づいた時にマークが赤色になる位置 |
| X Center Limit [m] | 0.011 | 壁抜けX位置の左右(±)の限界位置 |
| Stage Line Width (Screen x) | 0.05 | ステージ表示ライン太さの、スクリーンサイズに対する割合 |
| Player Mark Size (Screen x) | 0.3 | プレイヤーマークの大きさの、スクリーンサイズに対する割合 |

## 以下は設定ファイル`StagePositionViewer.json`でのみ設定可能
| 項目 | デフォルト値 | 説明 |
|------|--------------|------|
| MovementSensitivityThreshold | 0.01 | 通常時のHMD移動検出の閾値 |
| CenterMovementSensitivityThreshold | 0.001 | 中央部分でのHMD移動検出の閾値 |
| CenterMovementSensitivityDistance | 0.02 | 中央部分の判断位置 |
| StageWidth | 3.0 | ステージ横サイズ |
| StageHight | 2.0 | ステージ縦サイズ |
| CanvasMargin | 1.25 | キャンバスサイズのステージの枠に対する割合 |
| ScreenLayer | 5 | 通常表示時のUnityレイヤー (5:UI) |
| FirstPersonLayer | 6 | FirstPersonOnly有効時のUnityレイヤー (6:FirstPerson) ※メインカメラ(HMD)のカリングマスクを設定します |

# mod製作の参考
HMDの位置取得の参考にデンパ時計さんの[HeadDistanceTravelled](https://github.com/denpadokei/HeadDistanceTravelled)、ポジション表示の参考に[BeatmapInformation](https://github.com/denpadokei/BeatmapInformation/blob/master/BeatmapInformation)のコードを流用・参考にしています。
* [HeadDistanceTravelledライセンス](https://github.com/denpadokei/HeadDistanceTravelled/blob/main/LICENSE)
* [BeatmapInformationライセンス](https://github.com/denpadokei/BeatmapInformation/blob/master/LICENSE)
