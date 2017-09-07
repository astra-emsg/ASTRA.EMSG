using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using ASTRA.EMSG.Common.Master.Enums;
using ASTRA.EMSG.Entities;
using ASTRA.EMSG.Entities.Common;
using ASTRA.EMSG.Models;
using ASTRA.EMSG.Repositories;
using ASTRA.EMSG.Web.Infrastructure;
using FluentNHibernate.Testing.Values;
using NHibernate;
using NHibernate.Dialect;
using NHibernate.Tool.hbm2ddl;
using Oracle.DataAccess.Client;
using System.Linq;
using NHibernate.Linq;
using Configuration = NHibernate.Cfg.Configuration;
using Environment = System.Environment;

namespace ASTRA.EMSG.Tests.Utils
{
    public class DbHandlerUtils
    {
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
            using (var connection = new OracleConnection(currentConnectionString))
            {
                connection.Open();
                var sqlCommands = CreateCreateScript().Split(';').Where(sc => !sc.Trim().StartsWith("--") && !string.IsNullOrEmpty(sc.Trim()));
                foreach (var sqlCommand in sqlCommands)
                    RunSqlCommand(connection, sqlCommand);
            }
        }

        private static void RunSqlCommand(OracleConnection connection, string sqlCommand)
        {
            try
            {
                var oracleCommand = new OracleCommand(sqlCommand, connection);
                oracleCommand.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
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
            using (var connection = new OracleConnection(connectionString))
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
            RunSQLFromResource(connectionString, "ASTRA.EMSG.Common.Master.Scripts.ChangeScriptPostWork.txt");
        }

        public void ReGenerateStammDatenPostWorks(string connectionString)
        {
            RunScriptFromResource(connectionString, "ASTRA.EMSG.Common.Master.Scripts.InsertStammdaten.txt");
        }

        private static void RunSQLFromResource(string connectionString, string scriptFile)
        {
            using (var connection = new OracleConnection(connectionString))
            {
                connection.Open();

                Stream manifestResourceStream = Assembly.Load("ASTRA.EMSG.Common.Master").GetManifestResourceStream(scriptFile);

                using (var sr = new StreamReader(manifestResourceStream))
                {
                    string sqlFile = sr.ReadToEnd();
                    RunSqlCommand(connection, sqlFile);
                }
            }
        }

        private static void RunScriptFromResource(string connectionString, string scriptFile)
        {
          using (var connection = new OracleConnection(connectionString))
          {
            connection.Open();

            Stream manifestResourceStream = Assembly.Load("ASTRA.EMSG.Common.Master").GetManifestResourceStream(scriptFile);

            using (var sr = new StreamReader(manifestResourceStream))
            {
              string sqlFile = sr.ReadToEnd();
              var sqlCommands = sqlFile.Split(';').Where(sc => !sc.Trim().StartsWith("--") && !string.IsNullOrEmpty(sc.Trim()));
              foreach (var sqlCommand in sqlCommands)
                RunSqlCommand(connection, sqlCommand);
            }
          }
        }

        /// <summary>
        /// Clear all data in DB and generate StammDaten!
        /// Session should be Committed from outside!
        /// </summary>
        /// <param name="session">Session</param>
        public void ReGenerateStammDaten(ISession session)
        {
            var netzSummarischen = session.Query<NetzSummarisch>().ToList();
            var netzSummarischDetails = session.Query<NetzSummarischDetail>().ToList();
            var belastungskategorien = session.Query<Belastungskategorie>().ToList();
            var strassenabschnitten = session.Query<Strassenabschnitt>().ToList();
            var strassenabschnittenGIS = session.Query<StrassenabschnittGIS>().ToList();
            var zustandsabschnitten = session.Query<Zustandsabschnitt>().ToList();
            var mandanten = session.Query<Mandant>().ToList();
            var testUserInfos = session.Query<TestUserInfo>().ToList();
            var erfassungsPerioden = session.Query<ErfassungsPeriod>().ToList();

            DeleteAll(session, belastungskategorien);
            DeleteAll(session, netzSummarischen);
            DeleteAll(session, netzSummarischDetails);
            DeleteAll(session, strassenabschnitten);
            DeleteAll(session, strassenabschnittenGIS);
            DeleteAll(session, zustandsabschnitten);
            DeleteAll(session, testUserInfos);
            DeleteAll(session, mandanten);
            DeleteAll(session, erfassungsPerioden);
            

            //Lookups
            session.Save(new Belastungskategorie { Typ = "IA", DefaultBreiteFahrbahn = 4.5m });
            session.Save(new Belastungskategorie { Typ = "IB", DefaultBreiteFahrbahn = 5.5m });
            session.Save(new Belastungskategorie { Typ = "IC", DefaultBreiteFahrbahn = 5.75m });
            session.Save(new Belastungskategorie { Typ = "II", DefaultBreiteFahrbahn = 7 });
            session.Save(new Belastungskategorie { Typ = "III", DefaultBreiteFahrbahn = 7 });
            session.Save(new Belastungskategorie { Typ = "IV", DefaultBreiteFahrbahn = 14 });

            session.Save(new MassnahmenvorschlagKatalog { Typ = "Oberflaechenverbesserung", DefaultKosten = 50 });
            session.Save(new MassnahmenvorschlagKatalog { Typ = "Deckbelagserneuerung", DefaultKosten = 100 });
            session.Save(new MassnahmenvorschlagKatalog { Typ = "Belagserneuerung", DefaultKosten = 200 });
            session.Save(new MassnahmenvorschlagKatalog { Typ = "ErneuerungOberbau", DefaultKosten = 300 });
        }

        public void CreateDevelopmentTestDaten(ISession session, INetzSummarischRepository repository)
        {
            //TestUserInfo und Test Mandanten
            var allMandanten = new List<Mandant>
                                   {
                                       CreateMandant(session, "EMSG1", MengeTyp.StrassenFlaeche), 
                                       CreateMandant(session, "EMSG2", MengeTyp.StrassenflaecheUndTrottoirs),
                                       CreateMandant(session, "EMSG2", MengeTyp.StrassenLaenge)
                                   };

            var allErfassungsPeriod = new List<ErfassungsPeriod>
                                          {
                                              CreateErfassungsPeriod(session, allMandanten[0], NetzErfassungsmodus.Summarisch),
                                              CreateErfassungsPeriod(session, allMandanten[1], NetzErfassungsmodus.Tabellarisch),
                                              CreateErfassungsPeriod(session, allMandanten[2], NetzErfassungsmodus.Gis)
                                          };

            //BaseData
            foreach (ErfassungsPeriod erfassungsPeriod in allErfassungsPeriod)
                repository.CreateNetzSummarischFor(erfassungsPeriod);           

            //Test Users
            CreateTestUser(session, "Test", allMandanten, AllRollen);
        }

        public void CreateSpecFlowTestDaten(ISession session, INetzSummarischRepository repository)
        {
            //TestUserInfo und Test Mandanten
            Mandant mandant = CreateMandant(session, "Mandant", MengeTyp.StrassenFlaeche);
            ErfassungsPeriod erfassungsPeriod = CreateErfassungsPeriod(session, mandant, NetzErfassungsmodus.Summarisch);

            repository.CreateNetzSummarischFor(erfassungsPeriod);

            //Test Users
            CreateTestUser(session, "Test", new List<Mandant>{mandant}, AllRollen);
        }

        private static List<Rolle> AllRollen
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
        
        private ErfassungsPeriod CreateErfassungsPeriod(ISession session, Mandant mandant, NetzErfassungsmodus netzErfassungsmodus)
        {
            var erfassungsPeriod = new ErfassungsPeriod
                                       {
                                           IsClosed = false,
                                           Mandant = mandant,
                                           Name = mandant.MandantName + " InitialErfassungsPeriod",
                                           Erfassungsjahr = new DateTime(2000, 1, 1),
                                           NetzErfassungsmodus = netzErfassungsmodus
                                       };
            session.Save(erfassungsPeriod);

            return erfassungsPeriod;
        }

        private Mandant CreateMandant(ISession session, string mandantName, MengeTyp summarischerModusMengeTyp)
        {
            var mandant = new Mandant { MandantName = mandantName, SummarischerModusMengeTyp = summarischerModusMengeTyp };
            session.Save(mandant);
            return mandant;
        }

        private void CreateTestUser(ISession session, string userName, IEnumerable<Mandant> mandanten, List<Rolle> rollen)
        {
            foreach (Mandant mandant in mandanten)
            {
                foreach (Rolle rolle in rollen)
                {
                    session.Save(new TestUserInfo
                                     {
                                         UserName = userName,
                                         MandantName = mandant.MandantName,
                                         Rolle = rolle
                                     });
                }
            }
        }

        private void DeleteAll<TEntity>(ISession session, IEnumerable<TEntity> entities) where TEntity : Entity
        {
            foreach (TEntity entity in entities)
                session.Delete(entity);
        }
    }
}
