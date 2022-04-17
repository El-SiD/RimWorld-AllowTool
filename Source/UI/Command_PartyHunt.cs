﻿using System.Collections.Generic;
using AllowTool.Settings;
using RimWorld;
using UnityEngine;
using Verse;

namespace AllowTool {
	/// <summary>
	/// A toggle button with context options to switch party hunting mode for a pawn.
	/// </summary>
	public class Command_PartyHunt : Command_Toggle {
		private static readonly Vector2 overlayIconOffset = new Vector2(59f, 57f);

		private static PartyHuntSettings WorldSettings {
			get { return AllowToolController.Instance.WorldSettings.PartyHunt; }
		}

		private readonly Pawn pawn;

		public Command_PartyHunt(Pawn pawn) {
			this.pawn = pawn;
			icon = AllowToolDefOf.Textures.partyHunt;
			defaultLabel = "PartyHuntToggle_label".Translate();
			defaultDesc = "PartyHuntToggle_desc".Translate();
			isActive = () => WorldSettings.PawnIsPartyHunting(pawn);
			toggleAction = () => ToggleAction(pawn);
			hotKey = KeyBindingDefOf.Misc9;
			disabledReason = TryGetDisabledReason(pawn);
			disabled = disabledReason != null;
		}

		// Hook for Multiplayer
		internal static void ToggleAction(Pawn pawn) {
			WorldSettings.TogglePawnPartyHunting(pawn, !WorldSettings.PawnIsPartyHunting(pawn));
		}

		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms renderParams) {
			var result = base.GizmoOnGUI(topLeft, maxWidth, renderParams);
			if (Event.current.type == EventType.Repaint) {
				AllowToolUtility.DrawRightClickIcon(topLeft.x + overlayIconOffset.x, topLeft.y + overlayIconOffset.y);
			}
			return result;
		}

		public override bool InheritFloatMenuInteractionsFrom(Gizmo other) {
			// activate context menu items only for one selected pawn
			return false;
		}

		public override IEnumerable<FloatMenuOption> RightClickFloatMenuOptions {
			get {
				yield return AllowToolUtility.MakeCheckmarkOption("setting_partyHuntFinish_label", null, 
					() => WorldSettings.AutoFinishOff, SetAutoFinishOff);
				yield return AllowToolUtility.MakeCheckmarkOption("setting_partyHuntDesignated_label", null, 
					() => WorldSettings.HuntDesignatedOnly, SetHuntDesignatedOnly);
				yield return AllowToolUtility.MakeCheckmarkOption("setting_partyHuntUnforbid_label", null, 
					() => WorldSettings.UnforbidDrops, SetUnforbidDrops);
			}
		}

		// Hooks for Multiplayer
		internal static void SetAutoFinishOff(bool b) {
			WorldSettings.AutoFinishOff = b;
		}
		internal static void SetHuntDesignatedOnly(bool b) {
			WorldSettings.HuntDesignatedOnly = b;
		}
		internal static void SetUnforbidDrops(bool b) {
			WorldSettings.UnforbidDrops = b;
		}

		private string TryGetDisabledReason(Pawn forPawn) {
			return forPawn.WorkTagIsDisabled(WorkTags.Violent)
				? "IsIncapableOfViolenceShort".Translate().CapitalizeFirst()
				: null;
		}
	}
}