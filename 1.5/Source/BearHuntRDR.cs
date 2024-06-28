using RimWorld;
using RimWorld.Planet;
using Verse;
using RimWorld.QuestGen;
using System.Collections.Generic;
using Verse.AI;
using RimRedRedemption;

namespace RimRedRedemption
{
    [DefOf]
    public static class RDR_DefOf
    {
        static RDR_DefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(RDR_DefOf));
        }

        public static PawnKindDef RDR_LegendBearPawn;
        public static ThingDef RDR_LegendBearDef;
        public static QuestScriptDef RDR_LegendaryBearHunt;
    }

    public class WorldComponent_LegendaryHunts : WorldComponent
    {
        public int tickCounter;
        public int ticksToNextHunt = 60000 * 55; // About 55 days

        public WorldComponent_LegendaryHunts(World world) : base(world) { }

        public override void FinalizeInit()
        {
            base.FinalizeInit();
        }

        public override void WorldComponentTick()
        {
            base.WorldComponentTick();

            if (tickCounter > ticksToNextHunt)
            {
                if (RDR_DefOf.RDR_LegendaryBearHunt == null)
                {
                    Log.Error("RDR_LegendaryBearHunt is not defined.");
                    return;
                }

                List<QuestScriptDef> questList = new List<QuestScriptDef>
                {
                    RDR_DefOf.RDR_LegendaryBearHunt
                };

                Slate slate = new Slate();
                Quest quest = QuestUtility.GenerateQuestAndMakeAvailable(questList.RandomElement(), slate);
                QuestUtility.SendLetterQuestAvailable(quest);

                ticksToNextHunt = 60000 * Rand.RangeInclusive(50, 70);
                tickCounter = 0;
            }
            tickCounter++;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref this.tickCounter, "tickCounter");
            Scribe_Values.Look(ref this.ticksToNextHunt, "ticksToNextHunt");
        }
    }
}
