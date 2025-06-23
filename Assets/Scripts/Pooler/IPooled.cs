using System;
using UnityEngine;

namespace BullBrukBruker
{
    public interface IPooled
    {
        Action<GameObject> ReleaseCallback { get; set; }
    }
}
