using UnityEngine;

namespace SO
{
    public enum WeaponType
    {
        NONE,
        SWORD,
        AXE,
        ARROW,
        STAFF,
        PROJECTILE
    }

    [CreateAssetMenu(fileName = "WeaponConfig_", menuName = "Unity RPG Project/WeaponConfig", order = 2)]
    public class WeaponConfig : ItemConfig
    {
        public WeaponType weaponType = WeaponType.NONE;
        public float atk = 10f;
        public RuntimeAnimatorController animatorController = null;
    }
}