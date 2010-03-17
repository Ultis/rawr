Tracking doc for status of Rawr.Tank Module.

Currently targeting: 
* Wow version 3.3.2

2.3.13:
* Defect 16792: Crash when trying to save - Fixed by Jothay.
* Defect 16745: T9_2Piece not properly evalutated.

2.3.12:
* Defect 16597: Imp Icy Talons stacks /w Icy Talons
* Defect 16012: IIT was double stacking if it was selected in the Buffs Pane.
* No Defect: Experimental code expanded for shot rotation, and changes to BossHandler features.

2.3.11:
* No Defect: Adjust the defaults to be more appropriate for T9ish levels rather than Heroics.

2.3.10:
* No Defect: Actually implement the T10 set bonuses. Some how these didn't get in.

2.3.9:
* No Defect: Fixed some tool tips
* Defect 15732: Finally rooted out the expertise problem in experimental code.
* Defect 16144: Removing legacy mitigation code and adding the Mitigation multiplier to help with balancing issues until we can fine-tune it.
* No Defect: New buffs for DK tanks to Frost Presence and IBF.

2.3.8:
2.3.7:
RSVs still go hay-wire when using certain blood talents.
* Defect 15829: Armor miscalculated - based on the comments, moving the frost presence application to the end of my accumulate work.
* Defect 14569: Adjusting Survival by providing a more focused means of inputing the kinds of damage seen.  This is related to the BossHandler work.
* Defect 15844: Provide a user-checkbox to enable/disable Parry Haste.
* No Defect: Updating some of the tool tips to help with clarification.
* No Defect: Adding some abilities to future migration to the sim model.  Updating the calcs to ensure that we're pulling the right data.

2.3.6:
* No Defect: Factor in the DAMAGE procs, not just the healing procs of items.  This probably needs to be refactored.  Also the specials proc rate for OnMeleeHit was wrong.  
* Defect 15682: Add in a Hit Rating option to the Optimizer list.
* No Defect: I had the math all wrong for DW weapon speeds. It was increasing w/ 2 weapons rather than decreasing considering the white hits would occur more frequently rather than less in DW situations than 2H.
* Defect 15212: Refactored ManageRP to just limit based on RP.  More work needs to be done to better reveal what kind of values we should be seeing when bringing in Avoidance #s and weapon speed. 
* Defect 15671: I had Improved Icy Talons stacking with Normal IcyTalons for the tank's own haste.  It works out to only expand the effect from just the tank to the whole raid.  This suggests that we still need to find some way to include raid Utility into the value of effects/talents.
* Defect 14553: Attempt to better provide rotations that match the active talents.  Only a partial change as I need to do more testing.
* No Defect: Overvalue of things that had stacks & had an ability trigger.  The trigger would be infinity if it triggered off of things that were not in the rotation.  So now we just make sure to exclude infinite triggers.
* Defect 14553: Change the talent based rotation determination to use HS rather than Bloodgorged to build a blood rotation.
* Defect 15212: the code in the Combat table for RS was totally horked.  so I'm ripping it out since I didn't get any satisfactory results at this time.  I need to better segment any work that represents an attempt to do optimization for a given ability.  That's still a ways off.  So for now, RS is purely based on what's in the UI, even if the user enters in more RSs then they could possibly get given the combat situation available.
* Defect 15272: Changed where I'm doing the health calc for the healing talents (Rune Tap & Vampiric Blood) This seems to have settled the RSVs somewhat, but not completely.
* Defect 15732: Experimental mitigation now has expertise involved, but the results are totally weird.  I'm not done in there.
* No Defect: Fixed a bug in the way I was handling IBF & the Glyph of IBF
* No Defect: Fixed a bug in Blood Gorged that was not properly giving the bonusdamage part of it's effect.
* No Defect: Pulling out the value of Hysteria.  I still want a way to provide value for the utility talents, but for now it will have to wait.

2.3.5:
* Exposed Experimental switch via options panel in Rawr2.
* Fix for Defect 15640: Display option values were not all persisting.
* Fix for Defect 14840: Fixed parser & internal evaluation of multi-tiered special effects.
* No Defect: Bryntroll, the Bone Arbiter not properly modeled. - Healing not modeled as part of the life stealing.
* No Defect: Implementing HealthRestore & HP5 effects in the model.  It's not great, but really I need to figure out what the time frame really looks like for any given value.

2.3.4:
* Adding in the experimental work in prep for Bosshandler.

2.3.1:
* No Defect: Actually remembered to check in the Rune of Nerubian Carapace change.

2.3.0:
* Update for 3.3.
* Check ins for Ability base class and some implmented classes - not utilized yet
* Defect 14568: Burst & Reaction time charts now update the labels properly.
* Defect 13990: Should be the last of the plugging in of the Rawr3 UI to values in the model. It's a hack, no doubt, but it's better than nothing.
* Defect 14841: Ebon Plaguebringer wasn't putting the crit bonus in the right field.
* Defect 14527: VB & Glyph of VB changes along w/ some general Talent code clean up. 
* No Defect: Tweaked bloodworms a bit.
* Cleaned up commented out code.
* Defect 14525: Mark of Blood fixed.
* No Defect: +HealingRecieved has some value now, even though there's no inbound healing value yet.
* No Defect: Imp Blood Presence fixed.
* No Defect: Heart Strike adjusted for 2ndary target threat.
* No Defect: Improved some documenting comments.
* Defect 14751: Some rotations had RP deficits that would cause the RS count to drop negative.  So I put in a floor incase it does go negative, just over-ride and use what's already entered into the rotation dialog.  This is a patch until I can re-work the abilities and have RS take precedence over all other RP based abilities.

Current TODOs:
(kind of in order)
* Additional Rawr3 work.
* Integrate the BossHandler class.
* Fix shot rotation/threat modeling through re-work of ability usage.
* Fix/finish talent evaluation so that optimizing for talents produces a result that makes sense.