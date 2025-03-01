using Items;
using Manager;
using SO;
using UnityEngine;

namespace Control.NPC
{
    public class Master : NPCController
    {
        protected override void Awake()
        {
            base.Awake();
            actions.Add("OpenSkillShop", () =>
            {
                UIManager.Instance.skillShopPanel.BuildPanel(goods);
                if (GameManager.Instance.player.professionConfig != GetComponent<Entity>().professionConfig)
                {
                    foreach (var skill in GameManager.Instance.player.professionConfig.skillTree)
                        skill.RemoveFromInventory();

                    GameManager.Instance.player.professionConfig =
                        Resources.LoadAsync("Config/Profession/ProfessionConfig_Warrior").asset as ProfessionConfig;
                }
            });
            dialogueConfig =
                Resources.LoadAsync("Config/Dialogue/DialogueConfig_NPC_WarriorMaster").asset as DialogueConfig;
        }
    }
}