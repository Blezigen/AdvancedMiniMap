using System.Collections.Generic;
using System.Drawing;

namespace AdvancedMiniMap
{
    public class Database
    {
        public enum BuildingName
        {
            goodguys_tower1_top,
            goodguys_tower1_mid,
            goodguys_tower1_bot,
            goodguys_tower2_top,
            goodguys_tower2_mid,
            goodguys_tower2_bot,
            goodguys_tower3_top,
            goodguys_tower3_mid,
            goodguys_tower3_bot,
            goodguys_tower4,

            badguys_tower1_bot,
            badguys_tower1_top,
            badguys_tower1_mid,
            badguys_tower2_top,
            badguys_tower2_mid,
            badguys_tower2_bot,
            badguys_tower3_top,
            badguys_tower3_mid,
            badguys_tower3_bot,
            badguys_tower4,

            goodguys_melee_rax_top,
            goodguys_range_rax_top,
            goodguys_melee_rax_mid,
            goodguys_range_rax_mid,
            goodguys_melee_rax_bot,
            goodguys_range_rax_bot,
            badguys_melee_rax_top,
            badguys_range_rax_top,
            badguys_melee_rax_bot,
            badguys_range_rax_bot,
            badguys_range_rax_mid,
            badguys_melee_rax_mid,

            goodguys_healers,
            badguys_healers,

            badguys_fort,
            goodguys_fort,
        };

        public enum HudId
        {
            BuildingHealth,
            BuildingHealthGreen,
            BuildingHealthRed
        }
        public static Dictionary<HudId, Rectangle> HudSprites = new Dictionary<HudId, Rectangle>();

        public Database()
        {
            HudSprites[HudId.BuildingHealth] = new Rectangle(0, 0, 48, 48);
            HudSprites[HudId.BuildingHealthRed] = new Rectangle(48, 0, 48, 48);
            HudSprites[HudId.BuildingHealthGreen] = new Rectangle(96, 0, 48, 48);
        }

    }
}
