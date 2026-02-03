using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SecretKeysInjector {
    public readonly static List<(System.Type,Func<bool>)> injectors = new() {
        (typeof(VuforiaKeyInjector), VuforiaKeyInjector.Inject),
    };

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void InjectSecretKeys() {
        foreach (var (type, injector) in injectors) {
            if (!injector()) {
                Debug.LogError($"SecretKeysInjector: Key injection failed for {type.Name}.");
            }
        }
    }
}
