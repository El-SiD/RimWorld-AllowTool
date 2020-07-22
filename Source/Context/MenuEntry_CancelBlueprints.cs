using Verse;

namespace AllowTool.Context {
	public class MenuEntry_CancelBlueprints : BaseContextMenuEntry {
		protected override string SettingHandleSuffix => "cancelBlueprints";
		protected override string BaseTextKey => "Designator_context_cancel_build";

		public override ActivationResult Activate(Designator designator, Map map) {
			var blueprints = map.listerThings.ThingsInGroup(ThingRequestGroup.Blueprint);

			DesignateThings(designator, map, blueprints);

			return ActivationResult.FromCount(blueprints.Count, BaseMessageKey);
		}

		protected override void DesignateThing(Designator designator, Map map, Thing blueprint) {
			blueprint.Destroy(DestroyMode.Cancel);
		}
	}
}