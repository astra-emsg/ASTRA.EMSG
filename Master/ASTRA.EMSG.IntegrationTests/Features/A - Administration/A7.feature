Funktionalität: A7 - Ausgewählte Systemparameter pflegen
	Als Applikationsadministrator,
	will ich Ausgewählte Systemparameter pflegen
	damit ich eine zentrale Steuerungsmöglichkeit wichtiger Basiseinstellungen habe 

Grundlage: 
Gegeben sei ich bin Applikationsadministrator
Und es gibt folgende Massnahmevorschläge im System:
| Massnahmenvorschlag     | Typ      |
| Deckbelagserneuerung    | Fahrbahn |
| Oberflächenverbesserung | Fahrbahn |
Und es gibt folgende Massnahmen im System:
| Massnahmenvorschlag     | Typ      |
| Deckbelagserneuerung    | Fahrbahn |
| Oberflächenverbesserung | Fahrbahn |

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Applikationsadministrator kann ausgewählte Systemparameter, die für alle Mandanten angewendet werden, pflegen
Wenn ich folgende Systemparameter je Belastungskategorie anpasse:
| Systemparameter                        | IA  | IB  | IC  | II  | III | IV  |
| Wiederbeschaffungswert Fläche Fahrbahn | 430 | 400 | 140 | 380 | 430 | 340 |
| Alterungsbeiwert I                     | 1,6 | 1,6 | 1,4 | 1,8 | 2,2 | 2,6 |
Und ich folgende Standard-Kosten-Werte für Massnahmenvorschläge je Belastungskategorie anpasse:
| Massnahmenvorschlag     | Typ      | IA | IB | IC | II | III | IV |
| Oberflächenverbesserung | Fahrbahn | 50 | 50 | 50 | 50 | 50  | 50 |
Und ich folgenden neuen Massnahmenvorschläge je Belastungskategorie erfasse:
| Massnahmenvorschlag    | Typ      | IA  | IB  | IC  | II  | III | IV  |
| Deckbelagsverbesserung | Fahrbahn | 100 | 100 | 100 | 100 | 100 | 100 |
Dann stehen die veränderten Parameter allen Mandanten zur Verfügung.

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Applikationsadministrator kann Massnahmenvorschläge löschen
Wenn ich folgende Massnahmenvorschläge lösche:
| Massnahmenvorschlag  | Typ      |
| Deckbelagserneuerung | Fahrbahn |
Dann stehen diese Massnahmenvorschläge allen Mandanten nicht mehr zur Verfügung.

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Der Applikationsadministrator kann Massnahmen löschen
Wenn ich folgende Massnahmen lösche:
| Massnahmenvorschlag  | Typ      |
| Deckbelagserneuerung | Fahrbahn |
Dann stehen diese Massnahmen allen Mandanten nicht mehr zur Verfügung.

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Das System wendet die definierten Systemparameter für alle Mandanten von EMSG an

#------------------------------------------------------------------------------------------------------------------------------------------------------

@Manuell
Szenario: Wenn Systemparameter verändert werden, so soll das keine Auswirkung auf die Auswertungen abgeschlossener Jahre haben
Wenn ich folgende Systemparameter je Belastungskategorie anpasse:
| Systemparameter                        | IA  | IB  | IC  | II  | III | IV  |
| Wiederbeschaffungswert Fläche Fahrbahn | 430 | 400 | 140 | 380 | 430 | 340 |
| Alterungsbeiwert I                     | 1,6 | 1,6 | 1,4 | 1,8 | 2,2 | 2,6 |
Und ich folgende Standard-Kosten-Werte für Massnahmenvorschläge je Belastungskategorie anpasse:
| Massnahmenvorschlag     | Typ      | IA | IB | IC | II | III | IV |
| Oberflächenverbesserung | Fahrbahn | 50 | 50 | 50 | 50 | 50  | 50 |
Dann sind alle Auswertungen abgeschlossener Jahre aller Mandanten unverändert.
