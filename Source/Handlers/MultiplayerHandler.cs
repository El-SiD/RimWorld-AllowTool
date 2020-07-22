using Multiplayer.API;
using Verse;
using HarmonyLib;
using AllowTool.Context;
using System.Collections.Generic;
using System.Reflection;
using System;

namespace AllowTool
{
	[StaticConstructorOnStartup]
	public class MultiplayerHandler
	{
		static MultiplayerHandler() {
			if (!MP.enabled) return;

			// Context
			MP.RegisterSyncMethod(typeof(BaseContextMenuEntry), nameof(BaseContextMenuEntry.DesignateThings));
			MP.RegisterSyncMethod(typeof(RemoveDesignationBaseContextMenuEntry), nameof(RemoveDesignationBaseContextMenuEntry.RemoveDesignations));
			MP.RegisterSyncMethod(typeof(MenuEntry_MineConnected), nameof(MenuEntry_MineConnected.Activate)).SetContext(SyncContext.MapSelected);
			
			// Designators
			MP.RegisterSyncMethod(typeof(Designator_AllowAll), nameof(Designator_AllowAll.AllowAllTheThings));
			MP.RegisterSyncMethod(typeof(Designator_HaulUrgently), nameof(Designator_HaulUrgently.CheckForJobOverride));
			MP.RegisterSyncMethod(typeof(Designator_StripMine), nameof(Designator_StripMine.DesignateCells)).SetContext(SyncContext.CurrentMap);

			// PartyHunt Gizmo
			MP.RegisterSyncMethod(typeof(Command_PartyHunt), nameof(Command_PartyHunt.ToggleAction));
			MP.RegisterSyncMethod(typeof(Command_PartyHunt), nameof(Command_PartyHunt.SetAutoFinishOff));
			MP.RegisterSyncMethod(typeof(Command_PartyHunt), nameof(Command_PartyHunt.SetHuntDesignatedOnly));
			MP.RegisterSyncMethod(typeof(Command_PartyHunt), nameof(Command_PartyHunt.SetUnforbidDrops));

			// Worker for BaseContextMenuEntry serialization
			MP.RegisterSyncWorker<BaseContextMenuEntry>(SyncWorkerFor, typeof(BaseContextMenuEntry), true);
		}

		static void SyncWorkerFor(SyncWorker sw, ref BaseContextMenuEntry bcme) {
			if (sw.isWriting) {
				sw.Write(bcme.GetType());
			} else {
				bcme = (BaseContextMenuEntry) Activator.CreateInstance(sw.Read<Type>());
			}
		}
	}

	[HarmonyPatch]
	static class RandFix {
		static bool Prepare() {
			return MP.enabled;
		}

		static IEnumerable<MethodBase> TargetMethods() {
			yield return AccessTools.Method(typeof(JobDriver_FinishOff), nameof(JobDriver_FinishOff.TryMakeSkullMote));
		}

		static void Prefix() {
			Rand.PushState();
		}

		static void Postfix() {
			Rand.PopState();
		}
	}
}
