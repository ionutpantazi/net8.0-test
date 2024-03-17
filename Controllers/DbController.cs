using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using mvc.Models;
using MySqlConnector;
using System.Collections;
using System;
using System.Data;
using System.Data.Common;
using System.Reflection.PortableExecutable;
using System.Text.RegularExpressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.Json;
using System.Collections.ObjectModel;



namespace mvc.Controllers
{
    [Route("api/events")]
    public class DbController : Controller
    {
        [HttpPost()]
        public async Task<ActionResult<EventDbModel>> GetAllEvents([FromBody] EventPostDataModel post_data)
        {
            string status = "OK";
            int currentPage = (int)post_data.current_page - 1;
            int perPage = (int)post_data.per_page;
            int start = currentPage * perPage;
            if (start < 0) { start = 0; }
            int end = start + perPage;
            int count = MYSQLEvents.Count("SELECT COUNT(*) FROM events", []);
            EventDbDataModel data = MYSQLEvents.Query("SELECT * FROM events", [], [], count, start, end);

            var model = new EventDbModel
            {
                data = data,
                count = count,
                status = status,
            };

            return model;
        }

        [Route("add")]

        [HttpPost("add")]

        public async Task<ActionResult<EventDbModel>> AddNewEvent([FromBody] EventPostDataModel post_data)
        {
            if (post_data == null)
            {
                return BadRequest("Invalid data received");
            }

            string data2 = MYSQLEvents.Add(post_data);

            var model = new EventDbModel
            {
                response = data2,
            };

            return model;
        }

        [Route("count")]

        [HttpGet("count")]

        public async Task<ActionResult<EventDbModel>> GetCountEvents()
        {
            int count = MYSQLEvents.Count("SELECT COUNT(*) FROM events", []);

            var model = new EventDbModel
            {
                count = count
            };

            return model;
        }

        [Route("id/{event_id:}")]

        [HttpPost("id/{event_id}")]
        public async Task<ActionResult<EventDbModel>> GetEventbyId(string event_id)
        {
            int count = 1;
            EventDbDataModel data = MYSQLEvents.Query("SELECT * FROM events WHERE id=@event_id", ["event_id"], [event_id], count);

            var model = new EventDbModel
            {
                data = data,
                count = count,
                status = "OK"
            };

            return model;
        }

        [Route("delete/{event_id:}")]

        [HttpPost("delete/{event_id}")]
        public async Task<ActionResult<EventDbModel>> DeleteEventbyId(string event_id)
        {
            MYSQLEvents.Delete(event_id);

            var model = new EventDbModel
            {
                status = "OK"
            };

            return model;
        }

        [Route("name/{event_name:}")]

        [HttpGet("name/{event_name}")]
        public async Task<ActionResult<EventDbModel>> GetEventbyName(string event_name)
        {
            int count = 1;
            string data = Events.Query("SELECT * FROM events WHERE event_name=@event_name", ["event_name"], [event_name], count);

            var model = new EventDbModel
            {
                response = data,
                count = count
            };

            if (data.Length == 0) model.count = 0;

            return model;
        }
    }

    public class Events
    {
        public static string Query(string query, string[] param_key, string[] param_value, int count)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

            string connectionString = configuration.GetConnectionString("Default");
            string data = "";

            var conn = new SqlConnection(connectionString);
            var com = new SqlCommand(query, conn);

            for (int i = 0; i < param_key.Length; i++)
            {
                com.Parameters.AddWithValue("@" + param_key[i], param_value[i]);
            }

            SqlDataReader reader = null;

            using (conn)
            {
                try
                {
                    conn.Open();
                    reader = com.ExecuteReader();
                    int i = 0;
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(reader.GetOrdinal("ID"));
                        string event_name = reader.GetString(reader.GetOrdinal("event_name"));
                        if (i == 0)
                        {
                            data += "[{" + $"ID: {id}, EVENT_NAME: '{event_name}'" + "}";
                        }
                        else if (i < count)
                        {
                            data += ",{" + $"ID: {id}, EVENT_NAME: '{event_name}'" + "}";
                        }
                        if (i == count - 1)
                        {
                            data += "]";
                        }
                        i++;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return data;
        }

        public static string Add(EventPostDataModel post_data)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

            string connectionString = configuration.GetConnectionString("Default");
            var query = "INSERT INTO events (event_name, description, content_body) VALUES (@event_name, @description, @content_body)";
            string data = "";

            var conn = new SqlConnection(connectionString);
            var com = new SqlCommand(query, conn);

            if (post_data.event_name == null) post_data.event_name = "";
            if (post_data.date == null) post_data.date = DateTime.Now;
            if (post_data.description == null) post_data.description = "";
            if (post_data.content_body == null) post_data.content_body = "";

            com.Parameters.AddWithValue("@event_name", post_data.event_name);
            com.Parameters.AddWithValue("@date", post_data.date);
            com.Parameters.AddWithValue("@description", post_data.description);
            com.Parameters.AddWithValue("@content_body", post_data.content_body);

            SqlDataReader reader = null;

            using (conn)
            {
                try
                {
                    conn.Open();
                    reader = com.ExecuteReader();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return data;
        }

        public static int Count(string query, string[] param)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();
            string connectionString = configuration.GetConnectionString("Default");
            int count = 0;

            var conn = new SqlConnection(connectionString);
            var com = new SqlCommand(query, conn);

            using (conn)
            {
                try
                {
                    conn.Open();
                    SqlCommand count_command = new SqlCommand(query, conn);
                    count = (Int32)count_command.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return count;
        }
    }

    public class MYSQLEvents
    {
        public static EventDbDataModel Query(string query, string[] param_key, string[] param_value, int count = 0, int start = 0, int end = 100)
        {
            string connectionString = "Server=localhost;Database=demo;User=root;Password=;AllowUserVariables=True;";
            List<Dictionary<string, object>> resultList = new List<Dictionary<string, object>>();

            using MySqlConnection connection = new MySqlConnection(connectionString);
            try
            {
                query = query + " LIMIT @start,@end";
                connection.Open();
                using MySqlCommand command = new MySqlCommand(query, connection);
                for (int j = 0; j < param_key.Length; j++)
                {
                    command.Parameters.AddWithValue("@" + param_key[j], param_value[j]);
                }
                command.Parameters.AddWithValue("@start", start);
                command.Parameters.AddWithValue("@end", end);
                using MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var rowDict = new Dictionary<string, object>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        var id = reader.GetValue("id");
                        var event_name = reader.GetValue("event_name");
                        var date = reader.GetValue("date");
                        var description = reader.GetValue("description");
                        var content_body = reader.GetValue("content_body");
                        rowDict["id"] = id;
                        rowDict["event_name"] = event_name;
                        rowDict["date"] = date;
                        rowDict["description"] = description;
                        rowDict["content_body"] = content_body;
                    }
                    resultList.Add(rowDict);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            string jsonResult = JsonSerializer.Serialize(resultList);
            var model = new EventDbDataModel
            {
                jsonResult = jsonResult,
                objResult = resultList,
            };
            return model;
        }

        public static string Add(EventPostDataModel post_data)
        {
            string data = "success";
            string connectionString = "Server=localhost;Database=demo;User=root;Password=;AllowUserVariables=True;";

            var query = "INSERT INTO events (event_name, date, description, content_body) VALUES (@event_name, @date, @description, @content_body)";

            using MySqlConnection connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
                using MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@event_name", post_data.event_name);
                command.Parameters.AddWithValue("@date", post_data.date);
                command.Parameters.AddWithValue("@description", post_data.description);
                command.Parameters.AddWithValue("@content_body", post_data.content_body);
                using MySqlDataReader reader = command.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return data;
        }

        public static string Delete(string event_id)
        {
            string data = "success";
            string connectionString = "Server=localhost;Database=demo;User=root;Password=;AllowUserVariables=True;";

            var query = "DELETE FROM events WHERE id=@event_id";

            using MySqlConnection connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
                using MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@event_id", event_id);
                using MySqlDataReader reader = command.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return data;
        }

        public static int Count(string query, string[] param)
        {
            string connectionString = "Server=localhost;Database=demo;User=root;Password=;";
            int count = 0;

            using MySqlConnection connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
                using MySqlCommand command = new MySqlCommand(query, connection);
                count = Convert.ToInt32(command.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return count;
        }
    }
}
