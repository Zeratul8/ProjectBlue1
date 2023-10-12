using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using JetBrains.Annotations;
using AllIn1VfxToolkit.Demo.Scripts;

[Serializable]
public class EffectsDictionary : SerializableDictionary<EffectManager.EffectType, All1VfxDemoEffect> { }