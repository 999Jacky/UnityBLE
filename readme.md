# UnityBLE
將adafruit clue開發版上九軸傳感器，用NXP Sensor Fusion融合後，透過BLE傳送歐拉角到Unity，並用人偶展示。

- 只支援Android
- 開發版：adafuit clue
- 測試手機：Asus ROG Phone 2


Android專案build完成後需將**build\outputs\aar\blePlugin-debug.aar**拖進Unity專案裡


---
以實作功能：
- [X] 搜尋BLE裝置
- [X] 連線配對
- [X] 取得資料
- [ ] 斷線重連
- 人偶動作
  - [X] 左肩膀(L1)
  - [X] 左手臂(L2)
  - [X] 右手臂(R1)
  - [X] 右手臂(R2)
  - [ ] 胸口(C)

