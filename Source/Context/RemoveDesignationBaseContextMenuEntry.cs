using System.Linq;
using RimWorld;
using Verse;

namespace AllowTool.Context
{
	public abstract class RemoveDesignationBaseContextMenuEntry : BaseContextMenuEntry
	{
		// Hook for Multiplayer
		internal void RemoveDesignations(Designator designator, Map map, DesignationDef[] selectedDesignationDefs)
		{
			if (selectedDesignationDefs.Length == 0) {
				selectedDesignationDefs = DefDatabase<DesignationDef>.AllDefs.Where(def => def.designateCancelable && def != DesignationDefOf.Plan).ToArray();
			}
			foreach(var def in selectedDesignationDefs) {
				map.designationManager.RemoveAllDesignationsOfDef(def);
			}
		}
	}
}
