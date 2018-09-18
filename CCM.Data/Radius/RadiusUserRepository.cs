using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using CCM.Core.Entities;
using CCM.Core.Interfaces.Repositories;
using NLog;
using MySql.Data.MySqlClient;

namespace CCM.Data.Radius
{
    /// <summary>
    /// Account management. Connects to and updates data directly in the MySql database of the Radius server.
    /// </summary>
    public class RadiusUserRepository : IRadiusUserRepository
    {
        protected static readonly Logger log = LogManager.GetCurrentClassLogger();
        private static MySqlConnection GetMySqlConnection()
        {
            return new MySqlConnection(ConfigurationManager.ConnectionStrings["RadiusDbContext"].ConnectionString);
        }
        
        public List<RadiusUser> GetUsers()
        {
            log.Debug("RadiusUserRepository.GetUsers called.");

            var users = new List<RadiusUser>();

            using (var conn = GetMySqlConnection())
            {
                conn.Open();

                var cmd = conn.CreateCommand();
                cmd.CommandText = " select id, username from radcheck";

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var user = new RadiusUser
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        Username = reader["username"].ToString()
                    };

                    users.Add(user);
                }
            }

            log.Debug("Found {0} radius users.", users.Count);
            return users;
        }
        

        public List<UserInfo> GetSipAccountPasswords()
        {
            return DoGetUserPasswords("Cleartext-Password");
        }

        public List<UserInfo> GetUserPasswords()
        {
            return DoGetUserPasswords("SMD5-Password");
        }

        private List<UserInfo> DoGetUserPasswords(string pwdType)
        {

            try
            {
                using (var conn = GetMySqlConnection())
                {
                    conn.Open();
                    var cmd = conn.CreateCommand();
                    cmd.CommandText = $"select id, username, value from radcheck where attribute = '{pwdType}'";

                    MySqlDataReader reader = cmd.ExecuteReader();

                    var users = new List<UserInfo>();

                    while (reader.Read())
                    {
                        var user = new UserInfo()
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            UserName = reader["username"].ToString(),
                            Password = reader["value"].ToString()
                        };

                        users.Add(user);
                    }

                    return users;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex, "Fel då användare lästes från Radius");
                return null;
            }
        }

        public List<Dictionary<string, object>> GetUsersInfo()
        {
            using (var conn = GetMySqlConnection())
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = "select id, username, attribute, value from radcheck";
                var reader = cmd.ExecuteReader();
                return Serialize(reader);
            }
        }

        private List<Dictionary<string, object>> Serialize(MySqlDataReader reader)
        {
            var results = new List<Dictionary<string, object>>();

            var cols = new List<string>();
            for (var i = 0; i < reader.FieldCount; i++)
            {
                cols.Add(reader.GetName(i));
            }

            while (reader.Read())
            {
                var row = cols.ToDictionary(col => col, col => reader[col]);
                results.Add(row);
            }

            return results;
        }
    }

}