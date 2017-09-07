using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using ASTRA.EMSG.Common.Enums;
using NLog;

namespace ASTRA.EMSG.Localization
{
    public partial class MobileLocalization
    {
        public const string MassnahmenvorschlagkKatalogPrefix = "MassnahmenvorschlagKatalog_";

        public MobileLocalization()
        {
            foreach (var propertyInfo in GetType().GetProperties())
            {
                var defaultValueAttribute = (DefaultValueAttribute)propertyInfo.GetCustomAttributes(false).SingleOrDefault(a => a is DefaultValueAttribute);

                if (defaultValueAttribute != null)
                    propertyInfo.SetValue(this, defaultValueAttribute.Value, new object[0]);
            }

            InitDefaultLookups();
        }

        //Titles
        [DefaultValue("EMSG Mobile")]
        public string MainWindowTitle { get; set; }
        [DefaultValue("EMSG Hilfe")]
        public string HelpWindowTitle { get; set; }
        [DefaultValue("Fahrbahn {0} {1} - {2}")]
        public string ZustandFahrbahnWindowTitle { get; set; }
        [DefaultValue("Fahrbahn {0}")]
        public string ZustandFahrbahnShortWindowTitle { get; set; }
        [DefaultValue("Trottoir {0} {1} - {2}")]
        public string ZustandTrottoirtWindowTitle { get; set; }
        [DefaultValue("Trottoir {0}")]
        public string ZustandTrottoirShortWindowTitle { get; set; }
        [DefaultValue("Zustand Trottoir links")]
        public string ZustandTrottoirLinksFormTitle { get; set; }
        [DefaultValue("Zustand Trottoir rechts")]
        public string ZustandTrottoirRechtsFormTitle { get; set; }
        [DefaultValue("Massnahmen Trottoir links")]
        public string MassnahmenTrottoirLinksFormTitle { get; set; }
        [DefaultValue("Massnahmen Trottoir rechts")]
        public string MassnahmenTrottoirRechtsFormTitle { get; set; }
        [DefaultValue("Schadenschwere")]
        public string Schadenschwere { get; set; }
        [DefaultValue("Schadenausmass")]
        public string Schadenausmass { get; set; }
        [DefaultValue("Schadensumme: ")]
        public string SchadensummeGridFooter { get; set; }
        [DefaultValue("Zustandsindex: ")]
        public string ZustandsindexGridFooter { get; set; }
        [DefaultValue("Matrix")]
        public string Matrix { get; set; }
        [DefaultValue("Gewicht")]
        public string Gewicht { get; set; }
        [DefaultValue("Bewertung")]
        public string Bewertung { get; set; }
        [DefaultValue("Inspektionsroute:")]
        public string InspektionsRoutenLabel { get; set; }
        [DefaultValue("Layer")]
        public string LayerLabel { get; set; }
        [DefaultValue("Legende")]
        public string Legend { get; set; }
        [DefaultValue("Abschnitt")]
        public string AbschnittTabTitle { get; set; }
        [DefaultValue("Fahrbahn")]
        public string FahrbahnTabTitle { get; set; }
        [DefaultValue("Trottoir")]
        public string TrottoirTabTitle { get; set; }


        //Buttons
        [DefaultValue("Bearbeiten")]
        public string Edit { get; set; }
        [DefaultValue("Erfassen")]
        public string Create { get; set; }
        [DefaultValue("Speichern und schliessen")]
        public string Save { get; set; }
        [DefaultValue("OK")]
        public string Ok { get; set; }
        [DefaultValue("Speichern")]
        public string Apply { get; set; }
        [DefaultValue("Aktualisieren")]
        public string Update { get; set; }
        [DefaultValue("Abbrechen")]
        public string Cancel { get; set; }
        [DefaultValue("L�schen")]
        public string Delete { get; set; }
        [DefaultValue("Hilfe f�r Karte")]
        public string MapHelp { get; set; }
        [DefaultValue("Hilfe f�r Formular")]
        public string FormHelp { get; set; }
        [DefaultValue("Hilfe")]
        public string Help { get; set; }
        [DefaultValue("Zustand Fahrbahn")]
        public string ZustandFahrbahn { get; set; }
        [DefaultValue("Zustand Trottoir")]
        public string ZustandTrottoir { get; set; }
        [DefaultValue("Zustandsabschnitt ausw�hlen")]
        public string selectZustandsabschnitt { get; set; }
        [DefaultValue("Zustandsabschnitt bearbeiten")]
        public string EditZustandsabschnitt { get; set; }
        [DefaultValue("Zustandsabschnitt anlegen")]
        public string CreateZustandsabschnitt { get; set; }
        [DefaultValue("Zoom In")]
        public string ZoomIn { get; set; }
        [DefaultValue("Zoom Out")]
        public string ZoomOut { get; set; }
        [DefaultValue("Zoom auf alle Inspektionsrouten")]
        public string FullExtent { get; set; }
        [DefaultValue("Pan")]
        public string Pan { get; set; }
        [DefaultValue("Legende")]
        public string ShowLegend { get; set; }
        [DefaultValue("Vorheriger Extent")]
        public string PreviousExtent { get; set; }
        [DefaultValue("N�chster Extent")]
        public string NextExtent { get; set; }
        [DefaultValue("Zoom auf aktives Element")]
        public string ZoomToActiveElement { get; set; }
        [DefaultValue("Fl�che messen")]
        public string MeasureAreaToolText { get; set; }
        [DefaultValue("Entfernung messen")]
        public string MeasureLineToolText { get; set; }
        [DefaultValue("X: ${lon}, Y: ${lat}")]
        public string MousePos_Format { get; set; }
        [DefaultValue("Zoom")]
        public string ZoomToolText { get; set; }


        //Notifications
        [DefaultValue("Import war erfolgreich!")]
        public string SuccessfullImport { get; set; }
        [DefaultValue("Export war erfolgreich!")]
        public string SuccessfullExport { get; set; }
        [DefaultValue("Fehler beim Import")]
        public string ImportError { get; set; }
        [DefaultValue("Fehler beim Export!")]
        public string ExportError { get; set; }
        [DefaultValue("Es war noch nichts importiert!")]
        public string NoPackageImportedError { get; set; }
        [DefaultValue("Hilfe steht nicht zur Verf�gung!")]
        public string HelpNotExistsWarning { get; set; }
        [DefaultValue("Wenn Sie Ihre Eingaben noch nicht gespeichert haben, gehen diese bei �nderung der Art der Zustandserfassung verloren. Sind Sie sicher, dass Sie die Art der Zustandserfassung �ndern m�chten?")]
        public string ErfassungsmodusChangedConfirmation { get; set; }
        [DefaultValue("Sind Sie sicher, dass Sie diesen Datensatz l�schen wollen?")]
        public string DeleteConfirmation { get; set; }
        [DefaultValue("Bitte warten Sie, EMSG-Mobile wird gestartet")]
        public string PleaseWait { get; set; }
        [DefaultValue("Sollen von der Anwendung lokal gespeicherte Daten gel�scht werden?")]
        public string DeleteLocalData { get; set; }		


        //Validation Errors
        [DefaultValue("Feld ist erforderlich!")]
        public string RequiredValidationError { get; set; }
        [DefaultValue("Der Wert darf nicht l�nger sein als {0}!")]
        public string LengthValidationError { get; set; }
        [DefaultValue("Der Wert muss zwischen {0} und {1} sein!")]
        public string RangeValidationError { get; set; }
        [DefaultValue("Es muss eine Geometrie ausgew�hlt werden!")]
        public string GeometryShouldBeNotNull { get; set; }
        [DefaultValue("Ung�ltiges Nummernformat!")]
        public string InvalidNumberValidationError { get; set; }
        [DefaultValue("Ung�ltige Anzahl der Nachkommastellen! Erlaubt: {0}")]
        public string InvalidDecimalPlacesValidationError { get; set; }
        [DefaultValue("Die Summe der L�ngen der Zustandsabschnitte darf nicht gr�sser sein als die L�nge des Strassenabschnitts ({0} m).")]
        public string StrassenabschnittZustandsabschnittLaengeError { get; set; }
        [DefaultValue("Es wurde noch kein Schaden erfasst!")]
        public string GrobFormIsNotinitialized { get; set; }
        [DefaultValue("Es wurde noch kein Schaden erfasst!")]
        public string DetailFormIsNotinitialized { get; set; }

        //Enum Localizations
        [DefaultValue("Trocken")]
        public string WetterTypKeinRegen { get; set; }
        [DefaultValue("Nass")]
        public string WetterTypRegen { get; set; }
        [DefaultValue("Unbekannt")]
        public string ZustandsindexTrottoirTypUnbekannt { get; set; }
        [DefaultValue("0.0-0.9 - gut")]
        public string ZustandsindexTrottoirTypGut { get; set; }
        [DefaultValue("1.0-1.9 - mittel")]
        public string ZustandsindexTrottoirTypMittel { get; set; }
        [DefaultValue("2.0-2.9 - ausreichend")]
        public string ZustandsindexTrottoirTypAusreichend { get; set; }
        [DefaultValue("3.0-3.9 - kritisch")]
        public string ZustandsindexTrottoirTypKritisch { get; set; }
        [DefaultValue("4.0-5.0 - schlecht")]
        public string ZustandsindexTrottoirTypSchlecht { get; set; }
        [DefaultValue("Ohne Angabe")]
        public string DringlichkeitTypUnbekannt { get; set; }
        [DefaultValue("Kurzfristig")]
        public string DringlichkeitTypDringlich { get; set; }
        [DefaultValue("Mittelfristig")]
        public string DringlichkeitTypMittelfristig { get; set; }
        [DefaultValue("Langfristig")]
        public string DringlichkeitTypLangfristig { get; set; }
        [DefaultValue("Zustandsindex")]
        public string ZustandsErfassungsmodusManuel { get; set; }
        [DefaultValue("Grob")]
        public string ZustandsErfassungsmodusGrob { get; set; }
        [DefaultValue("Detail")]
        public string ZustandsErfassungsmodusDetail { get; set; }
        [DefaultValue("leicht")]
        public string SchadenschwereTypS1 { get; set; }
        [DefaultValue("mittel")]
        public string SchadenschwereTypS2 { get; set; }
        [DefaultValue("schwer")]
        public string SchadenschwereTypS3 { get; set; }
        [DefaultValue("0 %")]
        public string SchadenausmassTypA0 { get; set; }
        [DefaultValue("< 10 %")]
        public string SchadenausmassTypA1 { get; set; }
        [DefaultValue("10 - 50 %")]
        public string SchadenausmassTypA2 { get; set; }
        [DefaultValue("> 50 %")]
        public string SchadenausmassTypA3 { get; set; }
        [DefaultValue("Abbl�tterung")]
        public string SchadendetailTypAbblaetterung { get; set; }
        [DefaultValue("Abgedr�ckte R�der")]
        public string SchadendetailTypAbgedrueckteRaeder { get; set; }
        [DefaultValue("Abl�sungen")]
        public string SchadendetailTypAbloesungen { get; set; }
        [DefaultValue("Abplatzungen")]
        public string SchadendetailTypAbplatzungen { get; set; }
        [DefaultValue("Abrieb")]
        public string SchadendetailTypAbrieb { get; set; }
        [DefaultValue("Anrisse von Setzungen")]
        public string SchadendetailTypAnrisseVonSetzungen { get; set; }
        [DefaultValue("Aufw�lbungen")]
        public string SchadendetailTypAufwoelbungen { get; set; }
        [DefaultValue("Ausmagerung, Absanden")]
        public string SchadendetailTypAusmagerungAbsanden { get; set; }
        [DefaultValue("Belagsrandrisse")]
        public string SchadendetailTypBelagsrandrisse { get; set; }
        [DefaultValue("Blow-up")]
        public string SchadendetailTypBlowUp { get; set; }
        [DefaultValue("Fehlender oder spr�der Fugenverguss")]
        public string SchadendetailTypFehlenderOderSproederFugenverguss { get; set; }
        [DefaultValue("Flicke")]
        public string SchadendetailTypFlicke { get; set; }
        [DefaultValue("Frosthebungen")]
        public string SchadendetailTypFrosthebungen { get; set; }
        [DefaultValue("Kantensch�den, Absplitterung")]
        public string SchadendetailTypKantenschaedenAbsplitterung { get; set; }
        [DefaultValue("Kornausbr�che")]
        public string SchadendetailTypKornausbrueche { get; set; }
        [DefaultValue("L�ngsrisse")]
        public string SchadendetailTypLaengsrisse { get; set; }
        [DefaultValue("Netzrisse")]
        public string SchadendetailTypNetzrisse { get; set; }
        [DefaultValue("Offene N�hte")]
        public string SchadendetailTypOffeneNaehte { get; set; }
        [DefaultValue("Polieren")]
        public string SchadendetailTypPolieren { get; set; }
        [DefaultValue("Pumpen")]
        public string SchadendetailTypPumpen { get; set; }
        [DefaultValue("Querrisse")]
        public string SchadendetailTypQuerrisse { get; set; }
        [DefaultValue("Risse")]
        public string SchadendetailTypRisse { get; set; }
        [DefaultValue("Schlagl�cher")]
        public string SchadendetailTypSchlagloecher { get; set; }
        [DefaultValue("Schubverformungen")]
        public string SchadendetailTypSchubverformungen { get; set; }
        [DefaultValue("Schwitzen")]
        public string SchadendetailTypSchwitzen { get; set; }
        [DefaultValue("Setzungen, Einsenkungen")]
        public string SchadendetailTypSetzungenEinsenkungen { get; set; }
        [DefaultValue("Setzungen, Frosthebungen")]
        public string SchadendetailTypSetzungenFrosthebungen { get; set; }
        [DefaultValue("Spurrinnen")]
        public string SchadendetailTypSpurrinnen { get; set; }
        [DefaultValue("Stufenbildung")]
        public string SchadendetailTypStufenbildung { get; set; }
        [DefaultValue("Wellblechverformungen")]
        public string SchadendetailTypWellblechverformungen { get; set; }
        [DefaultValue("Wilde Risse")]
        public string SchadendetailTypWildeRisse { get; set; }
        [DefaultValue("Zerst�rte Platten")]
        public string SchadendetailTypZerstoertePlatten { get; set; }
        [DefaultValue("Belagssch�den")]
        public string SchadengruppeTypBelagschaeden { get; set; }
        [DefaultValue("Belagsverformungen")]
        public string SchadengruppeTypBelagsverformungen { get; set; }
        [DefaultValue("Flicke")]
        public string SchadengruppeTypFlicke { get; set; }
        [DefaultValue("Fugen und Kantensch�den")]
        public string SchadengruppeTypFugenUndKantenschaeden { get; set; }
        [DefaultValue("Materialverluste")]
        public string SchadengruppeTypMaterialverluste { get; set; }
        [DefaultValue("Oberfl�chengl�tte")]
        public string SchadengruppeTypOberflaechenglaette { get; set; }
        [DefaultValue("Risse, Br�che")]
        public string SchadengruppeTypRisseBrueche { get; set; }
        [DefaultValue("Strukturelle Sch�den")]
        public string SchadengruppeTypStrukturelleSchaeden { get; set; }
        [DefaultValue("Vertikalverschiebung")]
        public string SchadengruppeTypVertikalverschiebung { get; set; }

        public string GetSchadengruppeBezeichnung(SchadengruppeTyp schadengruppeTyp)
        {
            var propertyInfo = typeof(MobileLocalization).GetProperties().SingleOrDefault(p => p.Name == "SchadengruppeTyp" + schadengruppeTyp.ToString());
            if (propertyInfo == null)
            {
                GetLogger().Warn("Localization not found for enum: " + schadengruppeTyp.ToString());
                return schadengruppeTyp.ToString();
            }

            return (string)propertyInfo.GetValue(this, new object[0]);
        }

        private static Logger GetLogger()
        {
            return LogManager.GetLogger("TechLogger");
        }

        public string GetSchadendetailBezeichnung(SchadendetailTyp schadendetailTyp)
        {
            var propertyInfo = typeof(MobileLocalization).GetProperties().SingleOrDefault(p => p.Name == "SchadendetailTyp" + schadendetailTyp.ToString());
            if (propertyInfo == null)
            {
                GetLogger().Warn("Localization not found for enum: " + schadendetailTyp.ToString());
                return schadendetailTyp.ToString();
            }

            return (string)propertyInfo.GetValue(this, new object[0]);
        }

        //Lookup Localization
        public Dictionary<string, string> LookupLocalizations { get; set; }

        private void InitDefaultLookups()
        {
            LookupLocalizations = new Dictionary<string, string>
                          {
                              {MassnahmenvorschlagkKatalogPrefix + "Oberflaechenverbesserung", "Oberfl�chenverbesserung"},
                              {MassnahmenvorschlagkKatalogPrefix + "Deckbelagserneuerung", "Deckbelagserneuerung"},
                              {MassnahmenvorschlagkKatalogPrefix + "Belagserneuerung", "Belagserneuerung"},
                              {MassnahmenvorschlagkKatalogPrefix + "ErneuerungOberbau", "Erneuerung Oberbau"},
                              {MassnahmenvorschlagkKatalogPrefix + "KeineErneuerung", "Keine Erneuerung"},
                              {MassnahmenvorschlagkKatalogPrefix + "Erneuerung", "Erneuerung"},
                          };
        }

        public string GetLocalizedMassnahmenvorschlag(string typ)
        {
            var key = MassnahmenvorschlagkKatalogPrefix + typ;
            if (LookupLocalizations.ContainsKey(key))
                return LookupLocalizations[key];
            return typ;
        }

        //Field names
        [DefaultValue("Belastungskategorie")]
        public string Belastungskategorie { get; set; }
        [DefaultValue("Strassenname")]
        public string Strassenname { get; set; }
        [DefaultValue("Bezeichnung von")]
        public string BezeichnungVon { get; set; }
        [DefaultValue("Bezeichnung bis")]
        public string BezeichnungBis { get; set; }
        [DefaultValue("L�nge [m]")]
        public string Laenge { get; set; }
        [DefaultValue("Aufnahmedatum")]
        public string Aufnahmedatum { get; set; }
        [DefaultValue("Aufnahmeteam")]
        public string Aufnahmeteam { get; set; }
        [DefaultValue("Abschnittsnummer")]
        public string Abschnittsnummer { get; set; }
        [DefaultValue("Wetter")]
        public string Wetter { get; set; }
        [DefaultValue("Bemerkung")]
        public string Bemerkung { get; set; }
        [DefaultValue("Fl�che Fahrbahn [m�]")]
        public string FlaecheFahrbahn { get; set; }
        [DefaultValue("Fl�che erfasstes Trottoir links [m�]")]
        public string FlaecheTrottoirLinks { get; set; }
        [DefaultValue("Fl�che erfasstes Trottoir rechts [m�]")]
        public string FlaecheTrottoirRechts { get; set; }
        [DefaultValue("Pflichtfeld")]
        public string Pflichtfeld { get; set; }
        [DefaultValue("Massnahmenvorschlag")]
        public string Massnahmenvorschlag { get; set; }
        [DefaultValue("Kosten [CHF/m�]")]
        public string Kosten { get; set; }
        [DefaultValue("Dringlichkeit")]
        public string Dringlichkeit { get; set; }
        [DefaultValue("Gesamtkosten [CHF]")]
        public string Gesamtkosten { get; set; }
        [DefaultValue("Zustandsindex")]
        public string Zustandsindex { get; set; }
        [DefaultValue("Zustandserfassung")]
        public string Zustandserfassung { get; set; }
        [DefaultValue("Inspektionsroute")]
        public string Inspektionsroute { get; set; }

        //Menu
        [DefaultValue("Datei")]
        public string File { get; set; }
        [DefaultValue("Importieren")]
        public string Import { get; set; }
        [DefaultValue("Ausgew�hlte Inspektionsroute exportieren")]
        public string Export { get; set; }
        [DefaultValue("Alle Inspektionsrouten exportieren")]
        public string ExportAll { get; set; }
        [DefaultValue("Zwischenspeichern")]
        public string SaveExportPackage { get; set; }
        [DefaultValue("Log exportieren")]
        public string ExportLog { get; set; }
        [DefaultValue("Schliessen")]
        public string Exit { get; set; }

        //Esri license state
        [DefaultValue("You are not licensed to run this product.")]
        public string EsriLicenseNotLicensed { get; set; }
        [DefaultValue("The licenses needed are currently in use.")]
        public string EsriLicenseUnavailable { get; set; }
        [DefaultValue("The licenses unexpectedly failed.")]
        public string EsriLicenseFailure { get; set; }
        [DefaultValue("Already initialized (initialization can only occur once).")]
        public string EsriLicenseAlreadyInitialized { get; set; }
        [DefaultValue("License status no handled")]
        public string EsriLicenseFailureDefault { get; set; }

        //map notifications
        [DefaultValue("Kein Zustandsabschnitt selektiert!")]
        public string NoZustandsabschnittSelected { get; set; }
        [DefaultValue("Sie k�nnen keine leere Geometrie speichern")]
        public string EmptyGeometry { get; set; }
        [DefaultValue("Sie m�ssen erneut einen Zustandsabschnitt ausw�hlen!")]
        public string ReselectZustandsabschnitt { get; set; }
        [DefaultValue("Sie m�ssen erneut einen Strassenabschnitt ausw�hlen!")]
        public string ReselectStrassenabschnitt { get; set; }
        [DefaultValue("Die Geometrien d�rfen sich nicht �berlappen!")]
        public string GeometriesMustNotOverlap { get; set; }
        [DefaultValue("Es ist nicht m�glich einen Zustandsabschnitt zu erstellen!")]
        public string UnableToCreateZustandsabschnitt { get; set; }
        [DefaultValue("Etwaige �nderungen werden verworfen, wollen Sie fortfahren?")]
        public string DiscardChanges { get; set; }
        [DefaultValue("Etwaige �nderungen wurden noch nicht exportiert. Wenn Sie einen neuen Import durchf�hren, gehen alle �nderungen verloren. Sind Sie sicher, dass Sie einen neuen Import durchf�hren wollen?")]
        public string ImportWarning { get; set; }
        [DefaultValue("Sie k�nnen keinen Zustandsabschnitt auf einem anderen Strassenabschnitt anlegen!")]
        public string ForeignStrassenabschnitt { get; set; }
        [DefaultValue("Erstelle Karten-Cache")]
        public string CreateMapCache { get; set; }
        [DefaultValue("Starte Import")]
        public string StartImport { get; set; }
        [DefaultValue("Entpacke Paket")]
        public string UnzipPackage { get; set; }	


        //DisplayLayer names
        [DefaultValue("Grenzfl�chen")]
        public string DisplayLayerName_gg25_fill { get; set; }
        [DefaultValue("Karte (farbig)")]
        public string DisplayLayerName_Pixelmap_color { get; set; }
        [DefaultValue("Landeskarte SW")]
        public string DisplayLayerName_Pixelmap_gray { get; set; }
        [DefaultValue("Relief")]
        public string DisplayLayerName_rimini_relief { get; set; }
        [DefaultValue("Orthophoto")]
        public string DisplayLayerName_swissimage { get; set; }
        [DefaultValue("Geb�ude")]
        public string DisplayLayerName_v25_geb { get; set; }
        [DefaultValue("Strassen")]
        public string DisplayLayerName_v25_str { get; set; }
        [DefaultValue("Standard")]
        public string DisplayLayerName_v25_pri { get; set; }
        [DefaultValue("Achsen")]
        public string DisplayLayerName_MV_ACHSENSEGMENT { get; set; }
        [DefaultValue("Strassenabschnitte")]
        public string DisplayLayerName_STRASSENABSCHNITTGIS { get; set; }
        [DefaultValue("Zustandsabschnitte Fahrbahn")]
        public string DisplayLayerName_ZUSTANDSABSCHNITTGIS { get; set; }
        [DefaultValue("Zustandsabschnitte ohne Index")]
        public string DisplayLayerName_ZUSTANDSABSCHNITTGIS_IndexIsNull { get; set; }
        [DefaultValue("Zustandsabschnitte Trottoir")]
        public string DisplayLayerName_ZUSTANDTROTTOIRLEFT { get; set; }
        [DefaultValue("Grundwasservorkommen")]
        public string DisplayLayerName_grundwasservorkommen { get; set; }
        [DefaultValue("Grundwasservulnerabilit�t")]
        public string DisplayLayerName_grundwasservulnerabilitaet { get; set; }
        [DefaultValue("Kataster Belasteter Standorte")]
        public string DisplayLayerName_katasterBelasteterBtandorteOEv { get; set; }
        [DefaultValue("Hausnummern")]
        public string DisplayLayerName_gebaeudeWohnungsRegisterLabel { get; set; }
        [DefaultValue("Bahnl�rm bei Nacht")]
        public string DisplayLayerName_bahnlaermNacht { get; set; }
        [DefaultValue("Bahnl�rm bei Tag")]
        public string DisplayLayerName_bahnlaermTag { get; set; }
        [DefaultValue("Strassenl�rm bei Nacht")]
        public string DisplayLayerName_strassenlaermNacht { get; set; }
        [DefaultValue("Strassenl�rm bei Tag")]
        public string DisplayLayerName_strassenlaermTag { get; set; }
        [DefaultValue("AV Daten")]
        public string DisplayLayerName_compositeav { get; set; }


        //Exceptions
        [DefaultValue("Das Paket ist unvollst�ndig")]
        public string IncompletePackageExceptionMessage { get; set; }
        [DefaultValue("Das Paket ist ung�ltig")]
        public string InvalidPackageExceptionMessage { get; set; }
        [DefaultValue("Die Version des Pakets wird nicht unterst�tzt")]
        public string InvalidPackageVersionExceptionMessage { get; set; }
        [DefaultValue("Es sind keine Daten importiert")]
        public string NoDataImportedPackageExceptionMessage { get; set; }
        [DefaultValue("Ung�ltiges oder inkompatibles Paket")]
        public string InvalidOrIncompatiblePackageExceptionMessage { get; set; }
        [DefaultValue("Paket ist nicht kompatibel!\r\n Aktualisierung von EMSG-Mobile erforderlich")]
        public string InvalidPackageVersionNewExceptionMessage { get; set; }
        [DefaultValue("Paket ist nicht kompatibel!\r\n Das Paket ist veraltet")]
        public string InvalidPackageVersionOldExceptionMessage { get; set; }

        //splashscreen
        [DefaultValue("/ASTRA.EMSG.Mobile;component/Images/EMSG-03.bmp")]
        public string SplashScreenPath { get; set; }

        //LayerSwitcher Labels
        [DefaultValue("Hintergundkarten")]
        public string LabelBaseLayer { get; set; }
        [DefaultValue("�berlagernde Informationen")]
        public string LabelOverlays { get; set; }
        [DefaultValue("Zusatzinformationen")]
        public string LabelAdditionalInformation { get; set; }

        //label
        [DefaultValue("0 - 0.99")]
        public string Label_ZeroToOne { get; set; }
        [DefaultValue("1 - 1.99")]
        public string Label_OneToTwo { get; set; }
        [DefaultValue("2 - 2.99")]
        public string Label_TwoToThree { get; set; }
        [DefaultValue("3 - 3.99")]
        public string Label_ThreeToFour { get; set; }
        [DefaultValue("4 -  5")]
        public string Label_FourToFive { get; set; }
        [DefaultValue("kein Index")]
        public string Label_UnknownIndex { get; set; }
        [DefaultValue("Achsen")]
        public string Label_Achsen { get; set; }
        [DefaultValue("Strassenabschnitte")]
        public string Label_Strassenabschnitte { get; set; }

        //Other Texts
        [DefaultValue("von")]
        public string From { get; set; }
        [DefaultValue("bis")]
        public string To { get; set; }

        public string GetBackgroundLayerBezeichnung(string layerName)
        {
            var propertyInfo = typeof(MobileLocalization).GetProperties().SingleOrDefault(p => p.Name == "DisplayLayerName_" + layerName);
            if (propertyInfo == null)
            {
                GetLogger().Warn("Localization not found for DisplayLayerName_: " + layerName);
                return layerName;
            }

            return (string)propertyInfo.GetValue(this, new object[0]);
        }


        public string LocalizeLegendLabel(string label)
        {
            var propertyInfo = typeof(MobileLocalization).GetProperties().SingleOrDefault(p => p.Name == "Label_" + label.Replace(" ", string.Empty));
            if (propertyInfo == null)
            {
                return label;
            }

            return (string)propertyInfo.GetValue(this, new object[0]);

        }
    }
}
