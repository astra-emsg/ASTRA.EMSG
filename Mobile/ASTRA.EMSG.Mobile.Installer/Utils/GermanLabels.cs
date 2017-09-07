namespace ASTRA.EMSG.Mobile.Installer.Utils
{
    public class GermanLabels
    {
        public virtual string InstallInProgress { get { return "Installieren..."; } }
        public virtual string InstallFinished { get { return "Installation abgeschlossen"; } }
        public virtual string French { get { return "Fran�ais";  } }
        public virtual string German { get { return "Deutsch"; } }
        public virtual string Install { get { return "Installieren"; } }
        public virtual string InstallerText { get { return @"EMSG Mobile"; } }
        public virtual string Italian { get { return "Italiano"; } }
        public virtual string LanguageChooser { get { return "W�hlen Sie eine Sprache.\nS�lectionnez une langue.\nScegli una lingua."; } }
        public virtual string WindowTitle { get { return "EMSG Mobile Installer"; } }
        public virtual string Close { get { return "Schliessen"; } }
    }

    public class FrenchLabels : GermanLabels
    {
        public override string InstallInProgress { get { return "Installare..."; } }
        public override string InstallFinished { get { return "Installation termin�e"; } }
        public override string Install { get { return "Installer"; } }
        public override string Close { get { return "Fermer"; } }
    }

    public class ItalianLabels : GermanLabels
    {
        public override string InstallInProgress { get { return "Installare..."; } }
        public override string InstallFinished { get { return "Installazione completata"; } }
        public override string Install { get { return "Installare"; } }
        public override string Close { get { return "Chiudere"; } }
    }
}