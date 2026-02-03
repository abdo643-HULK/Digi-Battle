using UnityEngine;
using Vuforia;

public static class VuforiaKeyInjector {
    public static bool Inject() {
        // 1. Check if the configuration instance exists
        if (VuforiaConfiguration.Instance != null) {
            // 2. Inject the key from your ignored file
            VuforiaConfiguration.Instance.Vuforia.LicenseKey = SecretKeys.VUFORIA_LICENSE_KEY;
            Debug.Log("Vuforia License Key injected successfully.");
            return true;
        } else {
            Debug.LogError("Could not find Vuforia Configuration to inject key.");
            return false;
        }
    }
}