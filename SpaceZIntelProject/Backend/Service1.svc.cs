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
    public class BackendServiceImpl : BackendServices
    {
        private static readonly log4net.ILog log = LogHelper.GetLogger();
        private static string connStr = ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;
        private static readonly MongoClientSettings settings = MongoClientSettings.FromConnectionString(connStr);
        private static readonly MongoClient client = new MongoClient(settings);
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

        public void UpdateSpacecraft(string vehicleName, string column, string status)
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
    }


}
