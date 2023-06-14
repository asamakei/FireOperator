using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
public class TestDeviceAdder : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        List<string> testDevices = new List<string>();
        testDevices.Add(SystemInfo.deviceUniqueIdentifier);

        RequestConfiguration requestConfiguration = new RequestConfiguration.Builder().SetTestDeviceIds(testDevices).build();
        MobileAds.SetRequestConfiguration(requestConfiguration);

    }
}
