using System;
using RimWorld.Planet;
using Verse;
using RimWorld;
using Verse.AI.Group;
using System.Collections.Generic;

namespace RimRedRedemption.MapGeneration.LegendaryHunts
{
    [DefOf]
    public static class RDR_DefOf
    {
        static RDR_DefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(RDR_DefOf));
        }

        public static PawnKindDef RDRPawnLegendBear;
        public static ThingDef RDRBaseLegendBear;
        public static QuestScriptDef RDR_LegendaryBearHunt;
    }

    public class GenStep_LegendaryBear : GenStep_Scatterer
    {
        public override int SeedPart => 931842770;

        protected override bool CanScatterAt(IntVec3 c, Map map)
        {
            return base.CanScatterAt(c, map) && c.Standable(map);
        }

        protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int count = 1)
        {
            if (RDR_DefOf.RDRPawnLegendBear == null)
            {
                Log.Error("RDRPawnLegendBear is not defined.");
                return;
            }

            Pawn bear = PawnGenerator.GeneratePawn(RDR_DefOf.RDRPawnLegendBear);
            if (bear == null)
            {
                Log.Error("Failed to generate RDRPawnLegendBear.");
                return;
            }

            List<Pawn> pawns = new List<Pawn> { bear };
            GenSpawn.Spawn(bear, loc, map, WipeMode.Vanish);

            bear.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.ManhunterPermanent);

            MapGenerator.rootsToUnfog.Add(loc);
            MapGenerator.SetVar("RectOfInterest", CellRect.CenteredOn(loc, 1, 1));
        }
    }
}
