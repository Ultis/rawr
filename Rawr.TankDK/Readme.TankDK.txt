Tracking doc for status of Rawr.Tank Module.

As of Sept 9,2009, has support for 3.2 needs alot of validation.

2.2.20:
Integration for 3.2.2 changes
Fix Defect 14234: Mitigation wasn't properly taking in DamageReductionModifer.
No Defect: White damage wasn't properly calculated for Off-hands because it was using main hand speed only.
Also going through and re-evaluating the math for all things in CombatTable.CS.  There seems to be a mix of DPS and Damage values being returned.
I do need to at some point, completely re-factor the whole thing, but I'm not there yet.


Current TODOs:
* Fix defects
* Fix shot rotation/threat modeling.
* Add graphs for mapping Burst & Reaction Time.
* Integrate a Paperdoll vs. Effective Stats model.