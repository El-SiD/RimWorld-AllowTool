using System.Linq;
using RimWorld;
using Verse;

namespace AllowTool.Context {
	public class MenuEntry_CancelDesignations : RemoveDesignationBaseContextMenuEntry {
		protected override string SettingHandleSuffix => "cancelDesginations";
		protected override string BaseTextKey => "Designator_context_cancel_desig";

		protected virtual DesignationDef[] AllowedDefs => new DesignationDef[0];

		public override ActivationResult Activate(Designator designator, Map map) {
			int hitCountThings = 0;
			int hitCountTiles = 0;
			var manager = map.designationManager;

			var targets = manager.allDesignations
				// skip planning designation, as so does cancel
				.Where(des => des.def != null && des.def.designateCancelable && des.def != DesignationDefOf.Plan)
				.Select(des => des.target)
				.ToArray();

			foreach (var target in targets) {
				if (target.Thing != null) {
					hitCountThings++;
				} else {
					hitCountTiles++;
				}
			}

			RemoveDesignations(designator, map, AllowedDefs);

			return ActivationResult.SuccessMessage("Designator_context_cancel_desig_msg".Translate(hitCountThings, hitCountTiles));
		}
	}
}