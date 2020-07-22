using System.Linq;
using Verse;

namespace AllowTool.Context {
	public class MenuEntry_CancelSelected : RemoveDesignationBaseContextMenuEntry {
		protected override string SettingHandleSuffix => "cancelSelected";
		protected override string BaseTextKey => "Designator_context_cancel_selected";

		public override ActivationResult Activate(Designator designator, Map map) {
			// distinct designation defs on selected things
			var selectedObjects = Find.Selector.SelectedObjects.ToHashSet();
			// also include designations on cells of selected things
			var selectedTilePositions = selectedObjects.OfType<Thing>().Select(t => t.Position).ToHashSet();

			var affectedDesignations = map.designationManager.allDesignations
				.Where(des => des.target.HasThing ? selectedObjects.Contains(des.target.Thing) : selectedTilePositions.Contains(des.target.Cell))
				.ToArray();

			var selectedDesignationDefs = affectedDesignations
				.Select(des => des.def)
				.Distinct()
				.ToArray();

			RemoveDesignations(designator, map, selectedDesignationDefs);

			return affectedDesignations.Length > 0
				? ActivationResult.Success(BaseMessageKey, selectedDesignationDefs.Length, affectedDesignations.Length)
				: ActivationResult.Failure(BaseMessageKey);
		}
	}
}