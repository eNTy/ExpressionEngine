using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Linq.Expressions;
using System.Linq;
using System.Reflection;

using log4net;
using log4net.Config;

using Security;
using DataLayer;
using ExpressionEngine;
using System.Data;

namespace Security.Console
{
    class Program
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));

        static void Main(string[] args)
        {
            // read log4net config
            XmlConfigurator.Configure();

            // start logging
            log.Info("Starting Security");
            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();

            int errors = 0;

            string connString = System.Configuration.ConfigurationManager.AppSettings["ConnString"];
            DataLayer.ConnectionStringProvider.GetInstance().ConnectionString = connString;

            string securityPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RowLevelSecurity.xml");
            string securityEvalPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RowLevelSecurityEval.xml");

            // get Expressions from definition
            ExpressionCollection rowLevelUsed = ExpressionFactory.FromXml(System.IO.File.ReadAllText(securityPath));
            ExpressionCollection rowLevelEvaluation = ExpressionFactory.FromXml(System.IO.File.ReadAllText(securityEvalPath));


            Employee user = new Employee(6);
            Proposal proposal = new Proposal();

            var result = Evaluate.EvaluateCollection(proposal, user, rowLevelUsed, rowLevelEvaluation);

            // stop logging
            stopWatch.Stop();
            log.InfoFormat("Security finished in {0} seconds", stopWatch.Elapsed.TotalSeconds.ToString());

            if (errors > 0)
            {
                log.WarnFormat("{0} errors occured during synchronization, check log for more information!", errors);
                System.Environment.Exit(1); // return non-zero exit code to indicate trouble
            }
        }

        
    }
}
