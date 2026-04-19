using HarmonyLib;
using Verse;

namespace StkOutfitStand
{
	[StaticConstructorOnStartup]
	public static class Startup
	{
		static Startup()
		{
			var harmony = new Harmony("stk.outfitstandbeauty");
			harmony.PatchAll();
			//Log.Message("[StkOutfitStand] Harmony patches applied.");
		}
	}
}