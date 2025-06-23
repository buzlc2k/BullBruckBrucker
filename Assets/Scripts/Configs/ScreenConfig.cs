using UnityEngine;
using System;
using System.Collections.Generic;

namespace BullBrukBruker
{
    [CreateAssetMenu(menuName = "ScreenConfig")]
    public class ScreenConfig : ScriptableObject
    {
        [field: SerializeField] public float MinWidth { get; private set; }
        [field: SerializeField] public float MinHeight { get; private set; }
    }   
}