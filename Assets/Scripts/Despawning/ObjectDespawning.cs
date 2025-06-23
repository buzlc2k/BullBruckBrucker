using UnityEngine;

namespace BullBrukBruker
{
    public class ObjectDespawning : MonoBehaviour
    {
        public virtual void Despawn()
        {
            transform.parent.gameObject.SetActive(false);
        }
    }
}