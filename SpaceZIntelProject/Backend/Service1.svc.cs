using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Configuration;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Backend
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "BackendServiceImpl" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select BackendServiceImpl.svc or BackendServiceImpl.svc.cs at the Solution Explorer and start debugging.
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single)]
    public class BackendServiceImpl : IBackendServices
    {
        private static readonly log4net.ILog log = LogHelper.GetLogger();
        private static string connStr = ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;
        private static readonly MongoClientSettings settings = MongoClientSettings.FromConnectionString(connStr);
        private static readonly MongoClient client = new MongoClient(settings);
        private Dictionary<string, Telemetry> telemetryDictionary = new Dictionary<string, Telemetry>();  
        private Dictionary<string, IBackendCallback> allClients = new Dictionary<string, IBackendCallback>();  
        public string AddSpaceCraft(Vehicle vehicle)
        {
            try
            {
                var db = client.GetDatabase(Constants.DATABASE_SPACEZ);
                var collection = db.GetCollection<Vehicle>(Constants.COLLECTION_SPACECRAFT);
                collection.InsertOne(vehicle);
                log.Info("Spacecraft added successfully. " + vehicle.Name);
                return "Spacecraft added successfully.";
            }
            catch (Exception ex)
            {
                log.Error("Backend, AddSpacecraft(Vehicle vehicle) Error.", ex);
                return ex.ToString();
            }
            
        }

        public List<Vehicle> GetAllSpacecraft()
        {
            try
            {
                var db = client.GetDatabase(Constants.DATABASE_SPACEZ);
                var collection = db.GetCollection<Vehicle>(Constants.COLLECTION_SPACECRAFT);
                List<Vehicle> documents = new List<Vehicle>();
                documents = collection.Find(new BsonDocument()).ToList<Vehicle>();
                log.Info("GetAllSpacecraft(), Return successful with documents: " + documents);
                return documents;
            }
            catch(Exception ex)
            {
                log.Error("Backend, GetAllSpacecraft() Error.", ex);
                return new List<Vehicle>();
            }
        }

        public long CheckSpacecraftExists(string vehicleName, string payloadName)
        {
            try
            {
                var db = client.GetDatabase(Constants.DATABASE_SPACEZ);
                var collection = db.GetCollection<Vehicle>(Constants.COLLECTION_SPACECRAFT);
                var filterBuilder = Builders<Vehicle>.Filter;
                FilterDefinition<Vehicle> filter = filterBuilder.Eq(Constants.COLUMN_NAME, vehicleName) |
                    filterBuilder.Eq(Constants.COLUMN_PAYLOAD_NAME, payloadName);
                long count = collection.CountDocuments(filter);
                log.Info("CheckSpacecraftExists() return successfully with count: " + count + " for vehicle: " + vehicleName + " " + payloadName);
                return count;
            }
            catch (Exception ex)
            {
                log.Error("Backend, CheckSpacecraftExists(string vehicleName) Error.", ex);
                return long.MinValue;
            }
        }

        public List<Vehicle> GetAddedSpacecraft()
        {
            try
            {
                var db = client.GetDatabase(Constants.DATABASE_SPACEZ);
                var collection = db.GetCollection<Vehicle>(Constants.COLLECTION_SPACECRAFT);
                var filterBuilder = Builders<Vehicle>.Filter;
                FilterDefinition<Vehicle> filter = filterBuilder.Eq(Constants.COLUMN_STATUS, Constants.STATUS_ADDED);
                List<Vehicle> documents = new List<Vehicle>();
                var cursor = collection.Find(filter).ToCursor();
                foreach (var document in cursor.ToEnumerable())
                {
                    documents.Add(document);
                }
                log.Info("GetAddedSpacecraft() return successfully.");
                return documents;
            }
            catch(Exception ex)
            {
                log.Error("Backend, GetAddedSpacecraft() Error.", ex);
                return new List<Vehicle>();
            }
            
        }

        public void LaunchSpacecraft(string vehicleName, string dsnDashboardName)
        {
            UpdateSpacecraft2(vehicleName, Constants.COLUMN_STATUS, Constants.STATUS_LAUNCH_INITIATED,
                            Constants.COLUMN_SPACECRAFT_STATUS, Constants.STATUS_ONLINE);
            var connection = OperationContext.Current.GetCallbackChannel<IBackendCallback>();
            allClients[dsnDashboardName] = connection;
        }

        public void UpdateSpacecraft1(string vehicleName, string column, string status)
        {
            try
            {
                var db = client.GetDatabase(Constants.DATABASE_SPACEZ);
                var collection = db.GetCollection<Vehicle>(Constants.COLLECTION_SPACECRAFT);
                var filterBuilder = Builders<Vehicle>.Filter;
                FilterDefinition<Vehicle> filter = filterBuilder.Eq(Constants.COLUMN_NAME, vehicleName);
                var update = Builders<Vehicle>.Update.Set(column, status);
                collection.UpdateOne(filter, update);
                log.Info("UpdateSpacecraft() return successfully for vehicle: " + vehicleName + " " + column + " " + status);
            }
            catch (Exception ex)
            {
                log.Error("Backend, UpdateSpacecraft(string vehicleName) Error.", ex);
            }
            
        }

        public void UpdateSpacecraft2(string vehicleName, string column1, string value1, string column2, string value2)
        {
            try
            {
                var db = client.GetDatabase(Constants.DATABASE_SPACEZ);
                var collection = db.GetCollection<Vehicle>(Constants.COLLECTION_SPACECRAFT);
                var filterBuilder = Builders<Vehicle>.Filter;
                FilterDefinition<Vehicle> filter = filterBuilder.Eq(Constants.COLUMN_NAME, vehicleName);
                var update = Builders<Vehicle>.Update.Set(column1, value1).Set(column2, value2);
                collection.UpdateOne(filter, update);
                log.Info("UpdateSpacecraft() return successfully for vehicle: " + vehicleName + " " + column1 
                    + ":" + value1 + ", " + column2 + ":" + value2);
            }
            catch (Exception ex)
            {
                log.Error("Backend, UpdateSpacecraft(string vehicleName) Error.", ex);
            }

        }

        public void UpdateSpacecraft3(string vehicleName, string column1, string value1, string column2, string value2, string column3, string value3)
        {
            try
            {
                var db = client.GetDatabase(Constants.DATABASE_SPACEZ);
                var collection = db.GetCollection<Vehicle>(Constants.COLLECTION_SPACECRAFT);
                var filterBuilder = Builders<Vehicle>.Filter;
                FilterDefinition<Vehicle> filter = filterBuilder.Eq(Constants.COLUMN_NAME, vehicleName);
                var update = Builders<Vehicle>.Update.Set(column1, value1).Set(column2, value2).Set(column3, value3);
                collection.UpdateOne(filter, update);
                log.Info("UpdateSpacecraft() return successfully for vehicle: " + vehicleName + " " + column1 + ":" + 
                    value1 + ", " + column2 + ":" + value2 + ", " + column2 + ":" + value2);
            }
            catch (Exception ex)
            {
                log.Error("Backend, UpdateSpacecraft(string vehicleName) Error.", ex);
            }

        }

        public List<Vehicle> GetAllOnlineSpacecraft()
        {
            try
            {
                var db = client.GetDatabase(Constants.DATABASE_SPACEZ);
                var collection = db.GetCollection<Vehicle>(Constants.COLLECTION_SPACECRAFT);
                var filterBuilder = Builders<Vehicle>.Filter;
                FilterDefinition<Vehicle> filter = filterBuilder.Eq(Constants.COLUMN_SPACECRAFT_STATUS, Constants.STATUS_ONLINE);
                List<Vehicle> documents = new List<Vehicle>();
                var cursor = collection.Find(filter).ToCursor();
                foreach (var document in cursor.ToEnumerable())
                {
                    documents.Add(document);
                }
                log.Info("GetAllLaunchSequenceSpacecraft() return successfully.");
                return documents;
            }
            catch (Exception ex)
            {
                log.Error("Backend, GetAddedSpacecraft() Error.", ex);
                return new List<Vehicle>();
            }
        }

        public List<Vehicle> GetAllOnlinePayload()
        {
            try
            {
                var db = client.GetDatabase(Constants.DATABASE_SPACEZ);
                var collection = db.GetCollection<Vehicle>(Constants.COLLECTION_SPACECRAFT);
                var filterBuilder = Builders<Vehicle>.Filter;
                FilterDefinition<Vehicle> filter = filterBuilder.Eq(Constants.COLUMN_PAYLOAD_PAYLOAD_STATUS, Constants.STATUS_ONLINE);
                List<Vehicle> documents = new List<Vehicle>();
                var cursor = collection.Find(filter).ToCursor();
                foreach (var document in cursor.ToEnumerable())
                {
                    documents.Add(document);
                }
                log.Info("GetAllOnlinePayload() return successfully." + documents);
                return documents;
            }
            catch (Exception ex)
            {
                log.Error("Backend, GetAddedSpacecraft() Error.", ex);
                return new List<Vehicle>();
            }
        }

        public Vehicle GetSpacecraft(string vehicleName)
        {
            try
            {
                var db = client.GetDatabase(Constants.DATABASE_SPACEZ);
                var collection = db.GetCollection<Vehicle>(Constants.COLLECTION_SPACECRAFT);
                var filterBuilder = Builders<Vehicle>.Filter;
                FilterDefinition<Vehicle> filter = filterBuilder.Eq(Constants.COLUMN_NAME, vehicleName);
                Vehicle document = collection.Find(filter).First<Vehicle>();
                log.Info("GetSpacecraft() return successfully for vehicle: " + vehicleName);
                return document;
            }
            catch (Exception ex)
            {
                log.Error("Backend, UpdateSpacecraft(string vehicleName) Error.", ex);
                return null;
            }
        }

        public void RequestTelemetryOfVehicle(Vehicle vehicle)
        {

        }

        public void UpdateTelemetryMap(string vehicleName, Telemetry telemetry)
        {
            this.telemetryDictionary[vehicleName] = telemetry;
        }

        public Telemetry GetTelemetryOfVehicle(string vehicleName)
        {
            try
            {
                return this.telemetryDictionary[vehicleName];
            }
            catch(Exception ex)
            {
                log.Error("Backend, GetTelemetryOfVehicle(string vehicleName) Error for vehicle: " + vehicleName, ex);
                return new Telemetry();
            }
        }

        public void SendCommandToVehicle(Vehicle vehicle, string command)
        {
            allClients[vehicle.Name].ReceiveCommand(vehicle, command);
        }

        public void ConnectToBackend(string vehicleName)
        {
            var connection = OperationContext.Current.GetCallbackChannel<IBackendCallback>();
            allClients[vehicleName] = connection;
        }
    }


}
