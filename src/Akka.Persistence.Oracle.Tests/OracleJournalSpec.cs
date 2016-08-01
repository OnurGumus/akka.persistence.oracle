﻿using System.Configuration;
using Akka.Configuration;
using Akka.Persistence.TestKit.Journal;
using Xunit;
using Xunit.Abstractions;

namespace Akka.Persistence.Oracle.Tests
{
    [Collection("OracleSpec")]
    public class OracleJournalSpec : JournalSpec
    {
        private static readonly Config SpecConfig;

        static OracleJournalSpec()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["TestDb"].ConnectionString;

            SpecConfig = ConfigurationFactory.ParseString(@"
                akka.persistence {
                    publish-plugin-commands = on
                    journal {
                        plugin = ""akka.persistence.journal.oracle""
                        oracle {
                            class = ""Akka.Persistence.Oracle.Journal.OracleJournal, Akka.Persistence.Oracle""                            
                            plugin-dispatcher = ""akka.actor.default-dispatcher""
                            table-name = EVENTJOURNAL
                            schema-name = AKKA_PERSISTENCE_TEST
                            auto-initialize = on
                            connection-string = """ + connectionString + @"""
                        }
                    }
                }");
        }

        public OracleJournalSpec(ITestOutputHelper output)
            : base(SpecConfig, "OracleJournalSpec", output)
        {
            OraclePersistence.Get(Sys);
            Initialize();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            DbUtils.Clean();
        }
    }
}
