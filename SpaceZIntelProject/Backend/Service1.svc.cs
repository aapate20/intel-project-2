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
    
    /* Purpose to use concurrency mode multiple is to serve Multiple request at once while 
     * keeping single shared instance to shared data between clients.
     */
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single)]
    public class BackendServiceImpl : IBackendServices
    {
        private static readonly log4net.ILog log = LogHelper.GetLogger();
        private static string connStr = ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;
        private static readonly MongoClientSettings settings = MongoClientSettings.FromConnectionString(connStr);
        private static readonly MongoClient client = new MongoClient(settings);
        private Dictionary<string, Telemetry> telemetryDictionary = new Dictionary<string, Telemetry>();  
        private Dictionary<string, Weather> weatherDictionary = new Dictionary<string, Weather>();  
        private Dictionary<string, Spy> spyDictionary = new Dictionary<string, Spy>();  
        private Dictionary<string, Comm> commDictionary = new Dictionary<string, Comm>();  
        private Dictionary<string, IBackendCallback> allClients = new Dictionary<string, IBackendCallback>();  
        
        // Add Spacecraft in the database.
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

        /// <summary>
        /// Return List of all spacecraft from DB.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Check if Spacecraft exist in Database, Make sure doesn't contains duplicate spacecraft name and payload name.
        /// </summary>
        /// <param name="vehicleName"></param>
        /// <param name="payloadName"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Return List of all object of newly added spacecraft in system.
        /// </summary>
        /// <returns>List<Vehicle></returns>
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
        /// <summary>
        /// Update Spacecraft Status
        /// </summary>
        /// <param name="vehicleName"></param>
        /// <param name="dsnDashboardName"></param>
        public void LaunchSpacecraft(string vehicleName, string dsnDashboardName)
        {
            UpdateSpacecraft2(vehicleName, Constants.COLUMN_STATUS, Constants.STATUS_LAUNCH_INITIATED,
                            Constants.COLUMN_SPACECRAFT_STATUS, Constants.STATUS_ONLINE);
        }

        /// <summary>
        /// Update Spacecraft Status
        /// </summary>
        /// <param name="vehicleName"></param>
        /// <param name="column"></param>
        /// <param name="status"></param>
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

        /// <summary>
        /// Update Spacecraft Status
        /// </summary>
        /// <param name="vehicleName"></param>
        /// <param name="column1"></param>
        /// <param name="value1"></param>
        /// <param name="column2"></param>
        /// <param name="value2"></param>
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

        /// <summary>
        ///  Update Spacecraft Status
        /// </summary>
        /// <param name="vehicleName"></param>
        /// <param name="column1"></param>
        /// <param name="value1"></param>
        /// <param name="column2"></param>
        /// <param name="value2"></param>
        /// <param name="column3"></param>
        /// <param name="value3"></param>
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

        /// <summary>
        /// List of all Spacecraft object having Status online.
        /// </summary>
        /// <returns>List<Vehicle></returns>
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

        /// <summary>
        /// List of all Spacecraft object having payload status online.
        /// </summary>
        /// <returns>List<Vehicle></returns>
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

        /// <summary>
        /// Return Spacecraft data.
        /// </summary>
        /// <param name="vehicleName"></param>
        /// <returns>Spacecraft Object</returns>
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

        // Update Telemetry data by Payload and Spacecraft.
        public void UpdateTelemetryMap(string vehicleName, Telemetry telemetry)
        {
            this.telemetryDictionary[vehicleName] = telemetry;
        }

        // Send Telemetry data to DSN.
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

        // Update Scientific data by Payload.
        public void UpdateWeatherMap(string vehicleName, Weather weather)
        {
            this.weatherDictionary[vehicleName] = weather;
        }

        // Send Scientific data to DSN.
        public Weather GetWeatherDataOfVehicle(string vehicleName)
        {
            try
            {
                return this.weatherDictionary[vehicleName];
            }
            catch (Exception ex)
            {
                log.Error("Backend, GetWeatherDataOfVehicle(string vehicleName) Error for vehicle: " + vehicleName, ex);
                return new Weather();
            }
        }

        // Update Communication data by Payload.
        public void UpdateCommMap(string vehicleName, Comm comm)
        {
            this.commDictionary[vehicleName] = comm;
        }

        // Send Communication data to DSN.
        public Comm GetCommDataOfVehicle(string vehicleName)
        {
            try
            {
                return this.commDictionary[vehicleName];
            }
            catch (Exception ex)
            {
                log.Error("Backend, GetCommOfVehicle(string vehicleName) Error for vehicle: " + vehicleName, ex);
                return new Comm();
            }
        }

        // Update Spy data by Payload.
        public void UpdateSpyMap(string vehicleName, Spy spy)
        {
            this.spyDictionary[vehicleName] = spy;
        }

        // Send Spy data to DSN.
        public Spy GetSpyDataOfVehicle(string vehicleName)
        {
            try
            {
                return this.spyDictionary[vehicleName];
            }
            catch (Exception ex)
            {
                log.Error("Backend, GetSpyOfVehicle(string vehicleName) Error for vehicle: " + vehicleName, ex);
                return new Spy();
            }
        }

        // Send DSN command through callback to Spacecraft.
        public void SendCommandToVehicle(Vehicle vehicle, string command)
        {
            try
            {
                if (allClients.ContainsKey(vehicle.Name))
                {
                    log.Info("Sending Command to : " + vehicle.Name + " with command: " + command);
                    allClients[vehicle.Name].ReceiveCommand(command);
                }
                else
                {
                    log.Warn("Client not in Connection pool.");
                }
            }
            catch (Exception ex)
            {
                log.Error("Backend, SendCommandToVehicle(string vehicleName) Error for vehicle: " + vehicle.Name, ex);
            }
        }

        // Send DSN command through callback to Payload.
        public void SendCommandToPayloadVehicle(string vehicleName, string command)
        {
            try
            {
                if (allClients.ContainsKey(vehicleName))
                {
                    log.Info("Sending Command to : " + vehicleName + " with command: " + command);
                    allClients[vehicleName].ReceiveCommand(command);
                }
                else
                {
                    log.Warn("Client not in Connection pool.");
                }
            }
            catch (Exception ex)
            {
                log.Error("Backend, SendCommandToVehicle(string vehicleName) Error for vehicle: " + vehicleName, ex);
            }
        }

        // Connect client and add it in pool of connection.
        public void ConnectToBackend(string vehicleName)
        {
            try
            {
                if (!allClients.ContainsKey(vehicleName))
                {
                    var connection = OperationContext.Current.GetCallbackChannel<IBackendCallback>();
                    allClients[vehicleName] = connection;
                    log.Info("Client Connected: " + vehicleName);
                }
            }
            catch (Exception ex)
            {
                log.Error("Backend, ConnectToBackend(string vehicleName) Error for vehicle: " + vehicleName, ex);
            }
        }

        // Disconnect client and remove it from pool of connection.
        public void DisconnectFromBackend(string vehicleName)
        {
            try
            {
                if (allClients.ContainsKey(vehicleName))
                {
                    allClients.Remove(vehicleName);
                    log.Info("Client Disconnected: " + vehicleName);
                }
                else
                {
                    log.Info("Client not in Connection pool.");
                }
            }
            catch(Exception ex)
            {
                log.Error("Backend, DisconnectFromBackend(string vehicleName) Error for vehicle: " + vehicleName, ex);
            }
            
        }

        // Check if client is connected or not.
        public bool CheckVehicleConnectedToBackend(string vehicleName)
        {
            try
            {
                if (allClients.ContainsKey(vehicleName))
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                log.Error("Backend, CheckVehicleConnectedToBackend(string vehicleName) Error for vehicle: " + vehicleName, ex);
                return false;
            }
        }


    }


}
