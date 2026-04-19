using HarmonyLib;
using Verse;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace StkOutfitStand
{
	[HarmonyPatch(typeof(Building_OutfitStand), nameof(Building_OutfitStand.BeautyOffset), MethodType.Getter)]
	public static class Patch_BeautyOffset
	{
		[HarmonyPrefix]
		public static bool Prefix(Building_OutfitStand __instance, ref float __result)
		{

			// If no items are held, set beauty to 0
			if (__instance.HeldItems == null || !__instance.HeldItems.Any())
			{
				__result = 0f;
				return false;
			}

			bool outdoors = __instance.GetRoom(RegionType.Set_All).PsychologicallyOutdoors;

			float totalBeauty = 0f;
			foreach (Thing item in __instance.HeldItems)
			{
				// Check for the bad apparel
				if (item is Apparel apparel)
				{
					// Check if apparel is tainted or tattered (less than 50% HP)
					bool isTainted = apparel.WornByCorpse;
					bool isTattered = apparel.HitPoints < apparel.MaxHitPoints * 0.5f;
					if (isTainted)
					{
						totalBeauty -= 12f;
						continue;
					}
					else if (isTattered)
					{
						totalBeauty -= 6f;
						continue;
					}
				}

				// For the rest of items that has quality Market/400*quality (normal/no quality = 2)
				float tempBeauty;
				int qualityInt = item.TryGetQuality(out QualityCategory quality) ? (int)quality : 2;
				tempBeauty = item.MarketValue / 400f * qualityInt;

				// Compare with vanilla beauty
				if (item.GetBeauty(outdoors) > tempBeauty )
				{
					tempBeauty = item.GetBeauty(outdoors);
				}

				totalBeauty += tempBeauty;
			}
			__result = Mathf.Round(totalBeauty) - 0.001f;

			// Skip the original method
			return false;
		}
	}
}