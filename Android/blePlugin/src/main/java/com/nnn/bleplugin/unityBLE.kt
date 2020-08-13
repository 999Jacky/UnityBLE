package com.nnn.bleplugin

import android.bluetooth.*
import android.bluetooth.le.BluetoothLeScanner
import android.bluetooth.le.ScanCallback
import android.bluetooth.le.ScanResult
import android.os.Handler
import android.util.Log
import android.widget.Toast
import com.unity3d.player.UnityPlayer
import java.lang.Float
import java.nio.ByteBuffer
import java.nio.ByteOrder
import java.util.*
import kotlin.collections.ArrayList

interface gatt {
    fun Discovered(p1: BluetoothGatt) {}
}

open class mBluetoothGattCallback : BluetoothGattCallback(), gatt {

}


public class unityBLE() {

    private var mBluetoothLeScanner: BluetoothLeScanner? = null
    val scnList = ArrayList<String>()

    //    val viewList = ArrayList<View>()
    val charMap = mutableMapOf<BluetoothGatt, ArrayList<BluetoothGattCharacteristic>>()
    val gattList = ArrayList<BluetoothGatt>()
    lateinit var mBtAdapter: BluetoothAdapter

    companion object {
        val Battery_Service = UUID.fromString("0000180f-0000-1000-8000-00805f9b34fb")
        val Battery_Level = UUID.fromString("00002a19-0000-1000-8000-00805f9b34fb")
        val Gsensor_Service: UUID = UUID.fromString("90406bee-33fd-381a-8fd4-dfed9f7d5310")
        val Gsensor_XYZ = UUID.fromString("90406bef-33fd-381a-8fd4-dfed9f7d5310")
    }

    fun test(): Int {
        showMessage("5555")
        return 5
    }

    fun showMessage(text: String, time: Int = Toast.LENGTH_SHORT) {

        Log.d("AndroidBLE", text)
    }

    fun sendMsg2Unity(eventFunc: String, msg: String) {
        UnityPlayer.UnitySendMessage("BLE", eventFunc, msg)
    }

    public fun BLEInit(): Boolean {
        showMessage("BLEInit")
        mBtAdapter = BluetoothAdapter.getDefaultAdapter()
        if (mBtAdapter == null) {
            return false
        } else {
            mBtAdapter.enable()
        }
        mBluetoothLeScanner = mBtAdapter.bluetoothLeScanner
//        ScanBleDev()

        return true
    }

    public fun ScanBleDev() {
        mBluetoothLeScanner!!.stopScan(scanCallback)
        Handler().postDelayed({ mBluetoothLeScanner!!.stopScan(scanCallback) }, 5000)
        mBluetoothLeScanner!!.startScan(scanCallback)
    }

    public fun ConnectBLE(index: Int): Boolean {
        val addr = scnList[index].substring(scnList[index].lastIndexOf(',') + 1)
        val device = mBtAdapter.getRemoteDevice(addr)
        gattList.add(device.connectGatt(null, false, mGattCallback))
        return true
    }


    private val scanCallback: ScanCallback = object : ScanCallback() {
        override fun onScanResult(callbackType: Int, result: ScanResult) {
            showMessage("scan new Dev:" + result.device.name + "," + result.device.address)
            if (!scnList.contains(result.device.name + "," + result.device.address) && result.device.name != null) {
                scnList.add(result.device.name + "," + result.device.address)
                showMessage("found new dev" + result.device.name + "," + result.device.address)
                sendMsg2Unity(
                        "NewDevicesFunc",
                        "${result.device.name},${result.device.address}"
                )
            }
        }

        override fun onScanFailed(errorCode: Int) {
            showMessage("BLE scan FAIL:$errorCode")
        }
    }

    val mGattCallback: BluetoothGattCallback = object : mBluetoothGattCallback() {
        override fun Discovered(p1: BluetoothGatt) {
            super.Discovered(p1)
            Log.d("dis", p1.device.name)
            if (p1 != gattList.last()) {
                requestCharacteristics(gattList[gattList.indexOf(p1) + 1])
            } else {
                requestCharacteristics(gattList[0])
            }
        }

        fun requestCharacteristics(gatt: BluetoothGatt) {
            Log.d("req", gatt.device.name)
            gatt.readCharacteristic(charMap[gatt]!!.last())
        }

        override fun onConnectionStateChange(gatt: BluetoothGatt, status: Int, newState: Int) {
            when (newState) {
                BluetoothProfile.STATE_CONNECTED -> {
                    showMessage("連線成功")
                    gatt.discoverServices()
                }
                BluetoothProfile.STATE_DISCONNECTED -> {
                    showMessage("${gatt.device.name}連線失敗")
                    gattList.remove(gatt)

                }
            }

        }

        override fun onServicesDiscovered(gatt: BluetoothGatt, status: Int) {
            try {
                if (status == BluetoothGatt.GATT_SUCCESS) {
                    val GsensorService = gatt.getService(Gsensor_Service)
                    if (GsensorService == null) {
                        showMessage("Gsensor service not found!")
                        gatt.disconnect()
                        return
                    }
                    val GsensorXYZ = GsensorService.getCharacteristic(Gsensor_XYZ)
                    if (GsensorXYZ == null) {
                        showMessage("GsensorXYZ not found!")
                        gatt.disconnect()
                        return
                    }
                    if (!charMap.containsKey(gatt)) {
                        charMap[gatt] = arrayListOf()
                    }
                    charMap[gatt]!!.add(GsensorXYZ)
                    Discovered(gatt)
                } else {
                    Log.w("GATT", "onServicesDiscovered received: $status")
                }
            } catch (e: java.lang.Exception) {
                Log.e("e", e.toString())
            }

        }


        override fun onCharacteristicChanged(
                gatt: BluetoothGatt,
                characteristic: BluetoothGattCharacteristic
        ) {
            Log.d("gattOnChange", characteristic.uuid.toString())
            if (Battery_Level == characteristic.uuid) {
                showMessage(
                        "bty:" + characteristic.getIntValue(
                                BluetoothGattCharacteristic.FORMAT_UINT8,
                                0
                        ).toString()
                )
                Log.d("change", "bty")
            }
            if (Gsensor_XYZ == characteristic.uuid) {
                showMessage(
                        "x:" + ByteBuffer.wrap(characteristic.value)
                                .order(ByteOrder.LITTLE_ENDIAN).float.toString()
                )
                Log.d("change", "x")
            }
        }


        override fun onCharacteristicRead(
                gatt: BluetoothGatt,
                characteristic: BluetoothGattCharacteristic,
                status: Int
        ) {
            super.onCharacteristicRead(gatt, characteristic, status)
            if (status == 0) {
                when (characteristic.uuid) {
                    Battery_Level -> {//讀取電池電量
                        var btyLevel = "0"
                        btyLevel =
                                characteristic.getIntValue(BluetoothGattCharacteristic.FORMAT_UINT8, 0)
                                        .toString()//電量為int
                        showMessage("Show Bett:$btyLevel")
                    }
                    Gsensor_XYZ -> {//讀取角度
                        val angleArray = arrayOf(0f, 0f, 0f) //x,y,z
                        val buffer = characteristic.value     //每個角度以float拆成4個byte 共12個byte
                        for (n in angleArray.indices) {
                            val intBits: Int = (buffer[(n * 4) + 3].toUByte().toInt() shl 24) or
                                    (buffer[(n * 4) + 2].toUByte().toInt() shl 16) or
                                    (buffer[(n * 4) + 1].toUByte().toInt() shl 8) or
                                    (buffer[(n * 4) + 0].toUByte().toInt() and 0xFF)
                            angleArray[n] = Float.intBitsToFloat(intBits)
                        }
                        sendMsg2Unity(
                                "SensorXYZUpdate",
                                gatt.device.address + "@" + angleArray[0] + "," + angleArray[1] + "," + angleArray[2]
                        )
                    }
                }
                charMap[gatt]!!.remove(charMap[gatt]!!.last())
                if (charMap[gatt]!!.size > 0) {
                    requestCharacteristics(gatt)
                } else {
                    gatt.discoverServices()
                }
            }

        }

    }
}