using System;
using System.Configuration;
using LouNexus.Core.Interfaces.Core;
using LouNexus.Core.Interfaces.Inventory;
using LouNexus.Core.Interfaces.Prod;
using LouNexus.Core.Interfaces.Quality;
using LouNexus.Data.DataBase;
using LouNexus.Data.Repositories.Core;
using LouNexus.Data.Repositories.Inventory;
using LouNexus.Data.Repositories.Prod;
using LouNexus.Data.Repositories.Quality;

namespace LouNexus.Client.Configuration
{
    public static class AppServices
    {
        private static readonly IDbConnectionProvider _dbConnectionProvider;

        static AppServices()
        {
            ConnectionStringSettings? settings = ConfigurationManager.ConnectionStrings["LouNexusDb"];

            if (settings == null || string.IsNullOrWhiteSpace(settings.ConnectionString))
            {
                throw new InvalidOperationException("Connection string 'LouNexusDb' is not defined in the configuration.");
            }

            _dbConnectionProvider = new DbConnectionProvider(settings.ConnectionString);
        }

        #region Core Repositories

        public static IFactoryRepository factoryRepository =>
            new FactoryRepository(_dbConnectionProvider);

        public static IPartMeasurementSpecRepository partMeasurementSpecRepository =>
            new PartMeasurementSpecRepository(_dbConnectionProvider);

        public static IPartRepository partRepository =>
            new PartRepository(_dbConnectionProvider);

        public static IPartTrackingAttributeRepository partTrackingAttributeRepository =>
            new PartTrackingAttributeRepository(_dbConnectionProvider);

        public static IPartWorkStationRequirementRepository partWorkStationRequirementRepository =>
            new PartWorkStationRequirementRepository(_dbConnectionProvider);

        public static IRejectCodeRepository rejectCodeRepository =>
            new RejectCodeRepository(_dbConnectionProvider);

        public static IWorkStationRepository workStationRepository =>
            new WorkStationRepository(_dbConnectionProvider);

        public static IWorkStationTypeRepository workStationTypeRepository =>
            new WorkStationTypeRepository(_dbConnectionProvider);

        #endregion

        #region Inventory Repositories

        public static IRawMaterialRepository rawMaterialRepository =>
            new RawMaterialRepository(_dbConnectionProvider);

        #endregion

        #region Prod Repositories

        public static IInspectionRepository inspectionRepository =>
            new InspectionRepository(_dbConnectionProvider);

        public static IStationEventRepository stationEventRepository =>
            new StationEventRepository(_dbConnectionProvider);

        public static IStationEventAttributeRepository stationEventAttributeRepository =>
            new StationEventAttributeRepository(_dbConnectionProvider);

        public static IStationEventRejectRepository stationEventRejectRepository =>
            new StationEventRejectRepository(_dbConnectionProvider);

        #endregion

        #region Quality Repositories

        public static IMeasurementSetRepository measurementSetRepository =>
            new MeasurementSetRepository(_dbConnectionProvider);

        public static IMeasurementValueRepository measurementValueRepository =>
            new MeasurementValueRepository(_dbConnectionProvider);

        #endregion

        #region Admin Repositories

        // future admin repositories go here

        #endregion
    }
}
