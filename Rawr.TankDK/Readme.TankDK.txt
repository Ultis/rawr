Tracking doc for status of Rawr.Tank Module.

Currently targeting: 
* Wow version 3.3.0

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