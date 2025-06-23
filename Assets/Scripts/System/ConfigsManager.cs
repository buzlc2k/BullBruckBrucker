using UnityEngine;

namespace BullBrukBruker
{
    [CreateAssetMenu(menuName = "ConfigsManager")]
    public class ConfigsManager : SingletonSO<ConfigsManager>
    {
        [field: SerializeField] public ScreenConfig ScreenConfig { get; private set; }
        [field: SerializeField] public LevelConfig LevelConfig { get; private set; }
        [field: SerializeField] public BlockConfig BlockConfig { get; private set; }
        [field: SerializeField] public ObjectMovingConfig ObjectMovingConfig { get; private set; }
        [field: SerializeField] public ObjectCollisionConfig ObjectCollisionConfig { get; private set; }
    }   
}