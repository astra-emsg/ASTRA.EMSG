using System;
using System.Collections.Generic;
using System.IO;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Entities.Katalogs;
using ASTRA.EMSG.Business.Entities.Summarisch;
using ASTRA.EMSG.Business.Services.EntityServices.Summarisch;
using ASTRA.EMSG.Common.Enums;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Tool.hbm2ddl;
using System.Linq;
using NHibernate.Linq;
using Environment = System.Environment;
using ASTRA.EMSG.Common.Utils;
using System.Data.SqlClient;
using ASTRA.EMSG.Business.Entities.GIS;
using System.Text;
using ASTRA.EMSG.Business.Models.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ASTRA.EMSG.Tests.Common.Utils
{
    public class DbHandlerUtils
    {
        public const string ApplicationName = "EMSG";
        public const string IntegrationTestUserName = "Test";

        private readonly Configuration configuration;
        private readonly bool saveScriptsInFilesystem;

        public DbHandlerUtils(Configuration configuration, bool saveScriptsInFilesystem = false)
        {
            this.configuration = configuration;
            this.saveScriptsInFilesystem = saveScriptsInFilesystem;

            if (saveScriptsInFilesystem)
            {
                if (!Directory.Exists("Scripts"))
                    Directory.CreateDirectory("Scripts");
            }
        }

        public void ReCreateDbSchema()
        {
            var schemaExport = new SchemaExport(configuration);
            schemaExport.Drop(false, true);
            schemaExport.Create(false, true);
            DeleteAllUsers(configuration.Properties["connection.connection_string"]);
        }

        /// <summary>
        /// Generates Create Script
        /// </summary>
        /// <returns>Create Script</returns>
        public string CreateCreateScript()
        {
            string createScript = string.Empty;

            if (saveScriptsInFilesystem)
            {
                if (File.Exists("Scripts\\CreateCreate.sql"))
                    File.Delete("Scripts\\CreateCreate.sql");
            }

            new SchemaExport(configuration).Create(line =>
            {
                if (saveScriptsInFilesystem)
                {
                    using (var sw = new StreamWriter("Scripts\\CreateScript.sql", true))
                        sw.WriteLine(line);
                }
                createScript += string.Format("{0};", line);
            }, false);
            
            return createScript;
        }

        public void RunCreateScript(string currentConnectionString)
        {
            using (var connection = new SqlConnection(currentConnectionString))
            {
                connection.Open();
                var sqlCommands = CreateCreateScript().Split(';').Where(sc => !sc.Trim().StartsWith("--") && !string.IsNullOrEmpty(sc.Trim()));
                foreach (var sqlCommand in sqlCommands)
                    RunSqlCommand(connection, sqlCommand);
            }
        }


        /// <summary>
        /// Generates Change Script
        /// </summary>
        /// <param name="connectionString">ConnectionString</param>
        /// <param name="execute">Run SQL script on the DB</param>
        /// <returns>Change Script</returns>
        public string CreateChangeScript(string connectionString, bool execute = false)
        {
            string changeScript = string.Empty;
            List<string> sqlCommands = new List<string>();

            if (saveScriptsInFilesystem)
            {
                if (File.Exists("Scripts\\ChangeScript.sql"))
                    File.Delete("Scripts\\ChangeScript.sql");
            }

            var dialect = Dialect.GetDialect(configuration.Properties);
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var schemaUpdateScript = configuration.GenerateSchemaUpdateScript(dialect, new DatabaseMetadata(connection, dialect));

                if (saveScriptsInFilesystem)
                {
                    using (var sw = new StreamWriter(string.Format("Scripts\\ChangeScript.sql"), true))
                    {
                        if (!schemaUpdateScript.Any())
                        {
                            var noChange = "-- No change";
                            sw.WriteLine(noChange);
                        }
                        else
                        {
                            foreach (var line in schemaUpdateScript)
                            {
                                sw.WriteLine("{0};", line);
                                sqlCommands.Add(line);
                            }
                        }
                    }
                }

                if (!schemaUpdateScript.Any())
                {
                    var noChange = "-- No change";
                    changeScript += noChange + Environment.NewLine;
                }
                else
                {
                    foreach (var line in schemaUpdateScript)
                        changeScript += string.Format("{0};{1}", line, Environment.NewLine);
                }

                if (schemaUpdateScript.Any() && execute)
                {
                    foreach (string sqlCommand in sqlCommands)
                        RunSqlCommand(connection, sqlCommand);
                }
            }

            return changeScript;
        }

        public void ChangeScriptPostWorks(string connectionString)
        {
            RunSQL(connectionString, "Scripts\\ChangeScriptPostWork.sql");
        }

        public void ClearDb(string connectionString)
        {
            RunSQL(connectionString, "Scripts\\CleanDb.sql");
        }

        private static void RunSQL(string connectionString, string scriptFile)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var sr = new StreamReader(scriptFile))
                {
                    string sqlFile = sr.ReadToEnd();
                    RunSqlCommand(connection, sqlFile);
                }
            }
        }
        private static void RunSqlCommand(SqlConnection connection, string sqlCommand)
        {
            try
            {
                var command = new SqlCommand(sqlCommand, connection);
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                if (sqlCommand.Contains("drop"))
                    return;

                throw new Exception(sqlCommand, e);
            }
        }

        public void GenerateStammDaten(ISession session)
        {
            //Configurations
            var temp = session.Query<KopieAchsenSegment>().ToList();
            session.Save(new BenchmarkingGruppenConfiguration { Id = new Guid("b29de818-e2eb-444d-9b63-b2e7472f9917"), EigenschaftTyp = BenchmarkingGruppenTyp.NetzGroesse.ToDataBaseEigenschaftTyp(), Grenzwert = 50m });
            session.Save(new BenchmarkingGruppenConfiguration { Id = new Guid("1f9efdfd-0851-4c2d-a481-3f7502e0d789"), EigenschaftTyp = BenchmarkingGruppenTyp.NetzGroesse.ToDataBaseEigenschaftTyp(), Grenzwert = 75m });
            session.Save(new BenchmarkingGruppenConfiguration { Id = new Guid("b8d9732b-80fb-47ce-b786-b25f00517081"), EigenschaftTyp = BenchmarkingGruppenTyp.NetzGroesse.ToDataBaseEigenschaftTyp(), Grenzwert = 100m });
            session.Save(new BenchmarkingGruppenConfiguration { Id = new Guid("948d17d2-1702-4543-b9dc-2cfaf5c663c5"), EigenschaftTyp = BenchmarkingGruppenTyp.NetzGroesse.ToDataBaseEigenschaftTyp(), Grenzwert = 150m });

            session.Save(new BenchmarkingGruppenConfiguration { Id = new Guid("fe584a54-5361-46ca-9e81-b4f981a5848a"), EigenschaftTyp = BenchmarkingGruppenTyp.EinwohnerGroesse.ToDataBaseEigenschaftTyp(), Grenzwert = 1000m });
            session.Save(new BenchmarkingGruppenConfiguration { Id = new Guid("0b5ff486-31b7-40d7-85d3-c0a1ec8c9e00"), EigenschaftTyp = BenchmarkingGruppenTyp.EinwohnerGroesse.ToDataBaseEigenschaftTyp(), Grenzwert = 3000m });
            session.Save(new BenchmarkingGruppenConfiguration { Id = new Guid("61b40a25-843a-4050-b4a2-383f5f0a0dbf"), EigenschaftTyp = BenchmarkingGruppenTyp.EinwohnerGroesse.ToDataBaseEigenschaftTyp(), Grenzwert = 5000m });
            session.Save(new BenchmarkingGruppenConfiguration { Id = new Guid("3fb3a7de-9500-4115-bcba-1f0421692ba4"), EigenschaftTyp = BenchmarkingGruppenTyp.EinwohnerGroesse.ToDataBaseEigenschaftTyp(), Grenzwert = 10000m });
            session.Save(new BenchmarkingGruppenConfiguration { Id = new Guid("d15c9963-d788-466a-b98c-f500ce8e3429"), EigenschaftTyp = BenchmarkingGruppenTyp.EinwohnerGroesse.ToDataBaseEigenschaftTyp(), Grenzwert = 50000m });

            session.Save(new BenchmarkingGruppenConfiguration { Id = new Guid("71c88d19-1937-4480-94c5-882255ed0a28"), EigenschaftTyp = BenchmarkingGruppenTyp.MittlereHoehenlageSiedlungsgebieteGroesse.ToDataBaseEigenschaftTyp(), Grenzwert = 500m });
            session.Save(new BenchmarkingGruppenConfiguration { Id = new Guid("3135d9f7-9900-4776-83cc-812f30f926d2"), EigenschaftTyp = BenchmarkingGruppenTyp.MittlereHoehenlageSiedlungsgebieteGroesse.ToDataBaseEigenschaftTyp(), Grenzwert = 800m });

            //Lookups
            var belastungskategorieIA = new Belastungskategorie { Id = new Guid("178b3184-9d6d-4f41-acc0-c0cdaafedd30"), Typ = "IA", Reihenfolge = 1, DefaultBreiteFahrbahn = 4.5m, DefaultBreiteTrottoirRechts = 1.5m, DefaultBreiteTrottoirLinks = 1.5m, ColorCode = "#b7e9b1", AllowedBelagList = { BelagsTyp.Asphalt, BelagsTyp.Beton, BelagsTyp.Chaussierung } };
            var belastungskategorieIB = new Belastungskategorie { Id = new Guid("1c04104f-59c4-4b77-8863-d0ebf4b89374"), Typ = "IB", Reihenfolge = 2, DefaultBreiteFahrbahn = 5.5m, DefaultBreiteTrottoirRechts = 1.5m, DefaultBreiteTrottoirLinks = 1.5m, ColorCode = "#90d188", AllowedBelagList = { BelagsTyp.Asphalt, BelagsTyp.Beton, BelagsTyp.Chaussierung } };
            var belastungskategorieIC = new Belastungskategorie { Id = new Guid("514c5d7e-204d-4bf5-8f25-0bd35b928f1b"), Typ = "IC", Reihenfolge = 3, DefaultBreiteFahrbahn = 5.75m, DefaultBreiteTrottoirRechts = 1.5m, DefaultBreiteTrottoirLinks = 1.5m, ColorCode = "#60ab57", AllowedBelagList = { BelagsTyp.Asphalt, BelagsTyp.Beton, BelagsTyp.Chaussierung } };
            var belastungskategorieII = new Belastungskategorie { Id = new Guid("e73f807c-5986-4c41-9dc0-b077ccff9423"), Typ = "II", Reihenfolge = 4, DefaultBreiteFahrbahn = 7, DefaultBreiteTrottoirRechts = 2.0m, DefaultBreiteTrottoirLinks = 2.0m, ColorCode = "#38aabb", AllowedBelagList = { BelagsTyp.Asphalt, BelagsTyp.Beton, BelagsTyp.Chaussierung } };
            var belastungskategorieIII = new Belastungskategorie { Id = new Guid("fe685f5c-5380-46be-a3a8-4ae0a711110b"), Typ = "III", Reihenfolge = 5, DefaultBreiteFahrbahn = 7, DefaultBreiteTrottoirRechts = 2.5m, DefaultBreiteTrottoirLinks = 2.5m, ColorCode = "#3882bb", AllowedBelagList = { BelagsTyp.Asphalt, BelagsTyp.Beton, BelagsTyp.Chaussierung } };
            var belastungskategorieIV = new Belastungskategorie { Id = new Guid("c4400520-a2e0-4479-9814-2dcdb70e3bd1"), Typ = "IV", Reihenfolge = 6, DefaultBreiteFahrbahn = 14, DefaultBreiteTrottoirRechts = 2.5m, DefaultBreiteTrottoirLinks = 2.5m, ColorCode = "#194e95", AllowedBelagList = { BelagsTyp.Asphalt, BelagsTyp.Beton, BelagsTyp.Chaussierung } };
            var belastungskategoriePflaesterung = new Belastungskategorie { Id = new Guid("685ac75a-9906-4b48-9fa8-dceaf6e9d0d6"), Typ = "Pflaesterung", Reihenfolge = 7, DefaultBreiteFahrbahn = 4.5m, DefaultBreiteTrottoirRechts = 1.5m, DefaultBreiteTrottoirLinks = 1.5m, ColorCode = "#EEDFCC", AllowedBelagList = { BelagsTyp.Pflaesterung } };
            var belastungskategorieChaussierung = new Belastungskategorie { Id = new Guid("e7ffd19b-e82e-4cfd-8275-42ce74f21c3b"), Typ = "Chaussierung", Reihenfolge = 8, DefaultBreiteFahrbahn = 2.5m, DefaultBreiteTrottoirRechts = null, DefaultBreiteTrottoirLinks = null, ColorCode = "#CDC0B0", AllowedBelagList = { BelagsTyp.Chaussierung } };
            var belastungskategorieBenutzerdefiniert1 = new Belastungskategorie { Id = new Guid("062faaa1-90ba-4a5e-845f-fda9916bdb0a"), Typ = "Benutzerdefiniert1", Reihenfolge = 9, DefaultBreiteFahrbahn = null, DefaultBreiteTrottoirRechts = null, DefaultBreiteTrottoirLinks = null, ColorCode = "#8B8378", AllowedBelagList = { BelagsTyp.Asphalt, BelagsTyp.Beton, BelagsTyp.Chaussierung, BelagsTyp.Pflaesterung } };
            var belastungskategorieBenutzerdefiniert2 = new Belastungskategorie { Id = new Guid("40143851-612e-4cac-b828-92fcc01f05e0"), Typ = "Benutzerdefiniert2", Reihenfolge = 10, DefaultBreiteFahrbahn = null, DefaultBreiteTrottoirRechts = null, DefaultBreiteTrottoirLinks = null, ColorCode = "#CDAF95", AllowedBelagList = { BelagsTyp.Asphalt, BelagsTyp.Beton, BelagsTyp.Chaussierung, BelagsTyp.Pflaesterung } };
            var belastungskategorieBenutzerdefiniert3 = new Belastungskategorie { Id = new Guid("cdab134e-b963-4083-b639-72de5b55542d"), Typ = "Benutzerdefiniert3", Reihenfolge = 11, DefaultBreiteFahrbahn = null, DefaultBreiteTrottoirRechts = null, DefaultBreiteTrottoirLinks = null, ColorCode = "#8B7765", AllowedBelagList = { BelagsTyp.Asphalt, BelagsTyp.Beton, BelagsTyp.Chaussierung, BelagsTyp.Pflaesterung } };

            session.Save(belastungskategorieIA);
            session.Save(belastungskategorieIB);
            session.Save(belastungskategorieIC);
            session.Save(belastungskategorieII);
            session.Save(belastungskategorieIII);
            session.Save(belastungskategorieIV);
            session.Save(belastungskategoriePflaesterung);
            session.Save(belastungskategorieChaussierung);
            session.Save(belastungskategorieBenutzerdefiniert1);
            session.Save(belastungskategorieBenutzerdefiniert2);
            session.Save(belastungskategorieBenutzerdefiniert3);

            session.Save(new OeffentlicheVerkehrsmittelKatalog { Id = new Guid("6f23a412-b0cd-4b4d-9d83-34552f320b17"), Typ = "Vorhanden" });
            session.Save(new OeffentlicheVerkehrsmittelKatalog { Id = new Guid("e05e6cf4-87f5-44e7-acbd-0271fa1e9d8b"), Typ = "NichtVorhanden" });

            session.Save(new GemeindeKatalog { Id = new Guid("5f4cb921-81a7-4e00-b2ff-95604345a54d"), Typ = "Zentrum" });
            session.Save(new GemeindeKatalog { Id = new Guid("7f4884c8-094d-4f50-aab0-76bedd653803"), Typ = "Periurbane" });
            session.Save(new GemeindeKatalog { Id = new Guid("22507369-b88c-43d8-adfa-7e5f7000fefa"), Typ = "Suburbane" });
            session.Save(new GemeindeKatalog { Id = new Guid("2f3a861c-e9e9-4fec-987f-9f131bf3d9eb"), Typ = "Industrielle" });
            session.Save(new GemeindeKatalog { Id = new Guid("dc8e5429-28c4-46ec-adf9-68c800c0a5c3"), Typ = "Laendliche" });
            session.Save(new GemeindeKatalog { Id = new Guid("db5f3c28-72a8-4246-951c-6e46a550df21"), Typ = "Agrargemischte" });
            session.Save(new GemeindeKatalog { Id = new Guid("495885af-16b2-4834-9c9f-a61886495e55"), Typ = "Einkommensstarke" });
            session.Save(new GemeindeKatalog { Id = new Guid("ebb5f0a4-15c1-4e69-899d-405b4c923592"), Typ = "Touristisch" });
            session.Save(new GemeindeKatalog { Id = new Guid("43bdf3a0-37d7-44d5-b89e-90fbb5b5b3b2"), Typ = "Agrarische" });


            var massnahmentypKatalog1 = new MassnahmentypKatalog { Id = Guid.NewGuid(), Typ = "Oberflaechenverbesserung", KatalogTyp = MassnahmenvorschlagKatalogTyp.Fahrbahn, LegendNumber = 2 };
            session.Save(massnahmentypKatalog1);
            var massnahmentypKatalog2 = new MassnahmentypKatalog { Id = Guid.NewGuid(), Typ = "Deckbelagserneuerung", KatalogTyp = MassnahmenvorschlagKatalogTyp.Fahrbahn, LegendNumber = 5 };
            session.Save(massnahmentypKatalog2);
            var massnahmentypKatalog3 = new MassnahmentypKatalog { Id = Guid.NewGuid(), Typ = "Belagserneuerung", KatalogTyp = MassnahmenvorschlagKatalogTyp.Fahrbahn, LegendNumber = 3 };
            session.Save(massnahmentypKatalog3);
            var massnahmentypKatalog4 = new MassnahmentypKatalog { Id = Guid.NewGuid(), Typ = "ErneuerungOberbau", KatalogTyp = MassnahmenvorschlagKatalogTyp.Fahrbahn, LegendNumber = 1 };
            session.Save(massnahmentypKatalog4);
            var massnahmentypKatalog5 = new MassnahmentypKatalog { Id = Guid.NewGuid(), Typ = "Erneuerung", KatalogTyp = MassnahmenvorschlagKatalogTyp.Trottoir, LegendNumber = 4 };
            session.Save(massnahmentypKatalog5);

            foreach (var belastungskategorie in session.Query<Belastungskategorie>())
            {
                session.Save(new GlobalMassnahmenvorschlagKatalog { Id = Guid.NewGuid(), Parent = massnahmentypKatalog1, DefaultKosten = 50, Belastungskategorie = belastungskategorie });
                session.Save(new GlobalMassnahmenvorschlagKatalog { Id = Guid.NewGuid(), Parent = massnahmentypKatalog2, DefaultKosten = 100, Belastungskategorie = belastungskategorie });
                session.Save(new GlobalMassnahmenvorschlagKatalog { Id = Guid.NewGuid(), Parent = massnahmentypKatalog3, DefaultKosten = 200, Belastungskategorie = belastungskategorie });
                session.Save(new GlobalMassnahmenvorschlagKatalog { Id = Guid.NewGuid(), Parent = massnahmentypKatalog4, DefaultKosten = 300, Belastungskategorie = belastungskategorie });
                session.Save(new GlobalMassnahmenvorschlagKatalog { Id = Guid.NewGuid(), Parent = massnahmentypKatalog5, DefaultKosten = 125, Belastungskategorie = belastungskategorie });
            }

            session.Save(new GlobalWiederbeschaffungswertKatalog { Id = new Guid("ed251feb-1037-42c1-8b52-cb19b8d939b5"), Belastungskategorie = belastungskategorieIA, GesamtflaecheFahrbahn = 430, FlaecheFahrbahn = 380, FlaecheTrottoir = 150, AlterungsbeiwertI = 1.6m, AlterungsbeiwertII = 1.3m });
            session.Save(new GlobalWiederbeschaffungswertKatalog { Id = new Guid("e29c34ec-f653-4b46-895f-4267abaaa84c"), Belastungskategorie = belastungskategorieIB, GesamtflaecheFahrbahn = 400, FlaecheFahrbahn = 320, FlaecheTrottoir = 150, AlterungsbeiwertI = 1.6m, AlterungsbeiwertII = 1.3m });
            session.Save(new GlobalWiederbeschaffungswertKatalog { Id = new Guid("41ce71e5-3370-4919-8ea6-82bcb0927b7c"), Belastungskategorie = belastungskategorieIC, GesamtflaecheFahrbahn = 140, FlaecheFahrbahn = 140, FlaecheTrottoir = 150, AlterungsbeiwertI = 1.4m, AlterungsbeiwertII = 0.9m });
            session.Save(new GlobalWiederbeschaffungswertKatalog { Id = new Guid("8d42420b-923b-4ef7-b346-f69fb7d3700b"), Belastungskategorie = belastungskategorieII, GesamtflaecheFahrbahn = 380, FlaecheFahrbahn = 310, FlaecheTrottoir = 125, AlterungsbeiwertI = 1.8m, AlterungsbeiwertII = 1.4m });
            session.Save(new GlobalWiederbeschaffungswertKatalog { Id = new Guid("c2ec0420-7cdd-4202-a0da-8d6f3afebf5e"), Belastungskategorie = belastungskategorieIII, GesamtflaecheFahrbahn = 430, FlaecheFahrbahn = 340, FlaecheTrottoir = 125, AlterungsbeiwertI = 2.2m, AlterungsbeiwertII = 1.9m });
            session.Save(new GlobalWiederbeschaffungswertKatalog { Id = new Guid("3afac0cd-126a-4cc2-9abd-8acbbd20fe37"), Belastungskategorie = belastungskategorieIV, GesamtflaecheFahrbahn = 340, FlaecheFahrbahn = 300, FlaecheTrottoir = 115, AlterungsbeiwertI = 2.6m, AlterungsbeiwertII = 2.1m });
            session.Save(new GlobalWiederbeschaffungswertKatalog { Id = new Guid("44d60c35-ca9c-4861-82ac-887b13b4f7ff"), Belastungskategorie = belastungskategoriePflaesterung, GesamtflaecheFahrbahn = 0, FlaecheFahrbahn = 0, FlaecheTrottoir = 0, AlterungsbeiwertI = 0, AlterungsbeiwertII = 0 });
            session.Save(new GlobalWiederbeschaffungswertKatalog { Id = new Guid("43c8347e-0209-4351-86d5-8efb62c55de8"), Belastungskategorie = belastungskategorieChaussierung, GesamtflaecheFahrbahn = 0, FlaecheFahrbahn = 0, FlaecheTrottoir = 0, AlterungsbeiwertI = 0, AlterungsbeiwertII = 0 });
            session.Save(new GlobalWiederbeschaffungswertKatalog { Id = new Guid("033964e7-4e54-404e-a5c5-30f90f9016e1"), Belastungskategorie = belastungskategorieBenutzerdefiniert1, GesamtflaecheFahrbahn = 0, FlaecheFahrbahn = 0, FlaecheTrottoir = 0, AlterungsbeiwertI = 0, AlterungsbeiwertII = 0 });
            session.Save(new GlobalWiederbeschaffungswertKatalog { Id = new Guid("e8675b61-3ce5-4528-9c7f-faf004643bd5"), Belastungskategorie = belastungskategorieBenutzerdefiniert2, GesamtflaecheFahrbahn = 0, FlaecheFahrbahn = 0, FlaecheTrottoir = 0, AlterungsbeiwertI = 0, AlterungsbeiwertII = 0 });
            session.Save(new GlobalWiederbeschaffungswertKatalog { Id = new Guid("f8a4e4cd-b4e8-4b0e-9696-4a1d56a7dc13"), Belastungskategorie = belastungskategorieBenutzerdefiniert3, GesamtflaecheFahrbahn = 0, FlaecheFahrbahn = 0, FlaecheTrottoir = 0, AlterungsbeiwertI = 0, AlterungsbeiwertII = 0 });

            AddScriptLogEntries(session);
        }

        private void AddScriptLogEntries(ISession session)
        {
            //ScriptLog Table
            session.Save(new ScriptLog { ScriptName = "001_CreateTables.sql", Version = 0, ExecutionDateTime = DateTime.Now });
            session.Save(new ScriptLog { ScriptName = "002_ASPIdentityTables.sql", Version = 0, ExecutionDateTime = DateTime.Now });
            session.Save(new ScriptLog { ScriptName = "003_ChangeScriptPostWork.sql", Version = 0, ExecutionDateTime = DateTime.Now });
            session.Save(new ScriptLog { ScriptName = "004_ForeignKeyIndexes.sql", Version = 0, ExecutionDateTime = DateTime.Now });
        }

        public void CreateDevelopmentTestDaten(ISession session, INetzSummarischService netzSummarischService)
        {
            //Test User und Test Mandanten
            var allMandanten = new List<Mandant>
                                   {
                                       CreateMandant(session, "0353", "0353"),
                                       CreateMandant(session, "0663", "0663", NetzErfassungsmodus.Tabellarisch),
                                       CreateMandant(session, "0358", "0358", NetzErfassungsmodus.Gis)
                                   };

            for (int i = 0; i < 5; i++)
                allMandanten.Add(CreateMandant(session, "EMSG" + (i + 1).ToString("00"), "0353"));
           

            // TestUsers and Mandants
            for (int i = 1; i < 16; i++)
            {
                CreateTestUser(session, "test" + i, new List<Mandant> { CreateMandant(session, "testmandant" + i, "0353") }, AllRollen);
            }

            CreateTestUserWithOwnMandant(session, "test", allMandanten, AllRollen);
        }

        private void CreateTestUserWithOwnMandant(ISession session, string userName, IEnumerable<Mandant> mandanten, List<Rolle> rollen)
        {
            var mandantList = new List<Mandant> { CreateMandant(session, userName + "Mandant", "0353") };
            mandantList.AddRange(mandanten);

            CreateTestUser(session, userName, mandantList, rollen);
        }

        public static List<Rolle> AllRollen
        {
            get
            {
                return new List<Rolle>
                           {
                               Rolle.DataManager,
                               Rolle.DataReader,
                               Rolle.Benutzeradministrator,
                               Rolle.Benchmarkteilnehmer,
                               Rolle.Applikationsadministrator,
                               Rolle.Applikationssupporter
                           };
            }
        }

        public static ErfassungsPeriod CreateErfassungsPeriod(ISession session, Mandant mandant, NetzErfassungsmodus netzErfassungsmodus)
        {
            var erfassungsPeriod = new ErfassungsPeriod
            {
                IsClosed = false,
                Mandant = mandant,
                Name = mandant.MandantName + " InitialErfassungsPeriod",
                Erfassungsjahr = new DateTime(2000, 1, 1),
                NetzErfassungsmodus = netzErfassungsmodus,
            };
            session.Save(erfassungsPeriod);

            return erfassungsPeriod;
        }

        public static Mandant CreateMandant(ISession session, string mandantName, string ownerId, NetzErfassungsmodus netzErfassungsmodus = NetzErfassungsmodus.Summarisch, MandantDetails mandantDetails = null, string mandantBezeichnung = null)
        {
            var mandant = new Mandant
            {
                MandantBezeichnung = mandantBezeichnung ?? mandantName,
                MandantName = mandantName,
                OwnerId = ownerId
            };
            session.Save(mandant);
            var erfassungsPeriod = CreateErfassungsPeriod(session, mandant, netzErfassungsmodus);

            var netzSummarisch = new NetzSummarisch
            {
                ErfassungsPeriod = erfassungsPeriod,
                MittleresErhebungsJahr = null,
                Mandant = erfassungsPeriod.Mandant
            };

            session.Save(netzSummarisch);

            foreach (Belastungskategorie belastungskategorie in session.Query<Belastungskategorie>())
            {
                session.Save(new NetzSummarischDetail
                {
                    NetzSummarisch = netzSummarisch,
                    Belastungskategorie = belastungskategorie
                });
            }

            foreach (GlobalWiederbeschaffungswertKatalog globalWiederbeschaffungswertKatalog in session.Query<GlobalWiederbeschaffungswertKatalog>())
            {
                session.Save(new WiederbeschaffungswertKatalog
                {
                    AlterungsbeiwertI = globalWiederbeschaffungswertKatalog.AlterungsbeiwertI,
                    AlterungsbeiwertII = globalWiederbeschaffungswertKatalog.AlterungsbeiwertII,
                    GesamtflaecheFahrbahn = globalWiederbeschaffungswertKatalog.GesamtflaecheFahrbahn,
                    FlaecheTrottoir = globalWiederbeschaffungswertKatalog.FlaecheTrottoir,
                    FlaecheFahrbahn = globalWiederbeschaffungswertKatalog.FlaecheFahrbahn,

                    IsCustomized = false,

                    Belastungskategorie = globalWiederbeschaffungswertKatalog.Belastungskategorie,

                    ErfassungsPeriod = erfassungsPeriod,
                    Mandant = mandant,
                });
            }


            foreach (string globalMassnahmenvorschlagKatalogtyp in session.Query<GlobalMassnahmenvorschlagKatalog>().Select(gmvk => gmvk.Parent.Typ).Distinct())
            {
                foreach (GlobalMassnahmenvorschlagKatalog globalMassnahmenvorschlagKatalog in session.Query<GlobalMassnahmenvorschlagKatalog>().Where(gmvk => gmvk.Parent.Typ == globalMassnahmenvorschlagKatalogtyp))
                {

                    session.Save(new MassnahmenvorschlagKatalog
                    {
                        DefaultKosten = globalMassnahmenvorschlagKatalog.DefaultKosten,
                        IsCustomized = false,
                        Belastungskategorie = globalMassnahmenvorschlagKatalog.Belastungskategorie,
                        Parent = globalMassnahmenvorschlagKatalog.Parent,
                        ErfassungsPeriod = erfassungsPeriod,
                        Mandant = mandant,
                    });

                }
            }

            if (mandantDetails == null)
            {
                session.Save(new MandantDetails
                {
                    DifferenzHoehenlageSiedlungsgebiete = 10,
                    Einwohner = 2000,
                    Gemeindeflaeche = 30000,
                    Gemeindetyp = session.Query<GemeindeKatalog>().FirstOrDefault(),
                    MittlereHoehenlageSiedlungsgebiete = 400,
                    OeffentlicheVerkehrsmittel = session.Query<OeffentlicheVerkehrsmittelKatalog>().FirstOrDefault(),
                    Siedlungsflaeche = 500000,
                    Steuerertrag = 7,
                    NetzLaenge = 0,

                    IsCompleted = true,

                    Mandant = mandant,
                    ErfassungsPeriod = erfassungsPeriod
                });
            }
            else
            {
                mandantDetails.Mandant = mandant;
                mandantDetails.ErfassungsPeriod = erfassungsPeriod;
                session.Save(mandantDetails);
            }

            return mandant;

        }

        public static void DeleteAllUsers(string connectionString)
        {
            var dbCtx = new ApplicationDbContext(connectionString);
            foreach (var mandantRole in dbCtx.MandantRoles)
            {
                dbCtx.MandantRoles.Remove(mandantRole);
            }
            foreach (var applicationUser in dbCtx.Users)
            {
                dbCtx.Users.Remove(applicationUser);
            }
            dbCtx.SaveChanges();
        }

        public static void SetupTestUserRole(ISession session, string userName, Rolle rolle)
        {
            var dbCtx = new ApplicationDbContext(session.Connection.ConnectionString);
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(dbCtx));

            var user = userManager.FindByName(userName);
            var mandantName = user.MandantRoles.First().MandantName;

            if (rolle != Rolle.Applikationsadministrator && rolle != Rolle.Applikationssupporter)
            {
                foreach (var mandantRole in user.MandantRoles.ToList())
                {
                    dbCtx.MandantRoles.Remove(mandantRole);
                }
                user.MandantRoles.Add(new MandantRole { MandantName = mandantName, RoleName = rolle.ToString() });
            }

            userManager.Update(user);
        }

        public static void CreateTestUser(ISession session, string userName, IEnumerable<Mandant> mandanten, List<Rolle> rollen)
        {
            var connectionString = session.Connection.ConnectionString;
            var dbCtx = new ApplicationDbContext(connectionString);
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(dbCtx));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(dbCtx));

            var newUser = new ApplicationUser
            {
                UserName = userName
            };
            userManager.Create(newUser, "Password");

            foreach (Rolle value in Enum.GetValues(typeof(Rolle)).OfType<Rolle>()
                .Where(r => r == Rolle.Applikationsadministrator || r == Rolle.Applikationssupporter))
            {
                string roleName = value.ToString();
                if (!roleManager.RoleExists(roleName))
                {
                    var role = new IdentityRole
                    {
                        Name = roleName
                    };
                    roleManager.Create(role);
                }
            }

            var user = userManager.FindByName(userName);
            var userId = user.Id;
            //Add globalrole
            foreach (var globalRole in rollen.Where(r => r == Rolle.Applikationsadministrator || r == Rolle.Applikationssupporter))
            {
                userManager.AddToRole(userId, globalRole.ToString());
            }
            //Add mandantrole
            foreach(var mandant in mandanten)
            {
                foreach (var role in
                        rollen.Where(r => r != Rolle.Applikationsadministrator && r != Rolle.Applikationssupporter))
                {

                    user.MandantRoles.Add(new MandantRole {MandantName = mandant.MandantName, RoleName = role.ToString()});
                }
            }
            userManager.Update(user);
        }


    }
}
