using System.Linq;
using RimWorld;
using Verse;

namespace AllowTool.Context {
	public class MenuEntry_FinishOffAll : BaseContextMenuEntry {
		protected override string SettingHandleSuffix => "finishOffAll";
		protected override string BaseTextKey => "Designator_context_finish";
		protected override ThingRequestGroup DesignationRequestGroup => ThingRequestGroup.Pawn;

		public override ActivationResult Activate(Designator designator, Map map) {
			var things = map.listerThings.ThingsInGroup(DesignationRequestGroup)
				.Where(thing => ThingIsValidForDesignation(thing) && designator.CanDesignateThing(thing).Accepted)
				.ToList();

			DesignateThings(designator, map, things);

			int hitCount = things.Count;
			bool friendliesFound = things.Any(thing => AllowToolUtility.PawnIsFriendly(thing));

			if (hitCount>0 && friendliesFound) {
				Messages.Message("Designator_context_finish_allies".Translate(hitCount), MessageTypeDefOf.CautionInput);
			}
			return ActivationResult.FromCount(hitCount, BaseTextKey);
		}
	}
}