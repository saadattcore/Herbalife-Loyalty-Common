using HL.Loyalty.Models;
using HL.Loyalty.RulesEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace HL.Loyalty.RulesEngine.Providers
{
    public class TierConfigurationProvider : ITierConfigurationProvider
    {
        private string _connectionString;

        public TierConfigurationProvider(string connectionString)
        {
            _connectionString = connectionString;

            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                throw new ArgumentException("TierConfigurationProvider Load Invalid ConnectionString");
            }
        }

        public IList<TierConfiguration> Load(PointsCategoryType category, string countryCode)
        {
            if (string.IsNullOrWhiteSpace(countryCode))
            {
                throw new ArgumentException("TierConfigurationProvider Load Invalid CountryCode");
            }

            List<TierConfiguration> tiers = new List<TierConfiguration>();
            string connString = _connectionString;
            using (var conn = new SqlConnection(connString))
            {
                conn.Open();
                var command = new SqlCommand("usp_GetTier", conn) { CommandType = CommandType.StoredProcedure };
                command.Parameters.Add(new SqlParameter("@CountryCodeISO", countryCode));
                command.Parameters.Add(new SqlParameter("@CategoryCode", category.ToString()));

                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var tierConfiguration = new TierConfiguration();
                            //tierConfiguration.PointsType = Convert.IsDBNull(reader["TierID"])? null: (string)reader["TierID"];
                            tierConfiguration.Tier = new Tier
                            {
                                Id = Convert.IsDBNull(reader["TierID"]) ? Guid.Empty : (Guid)reader["TierID"],
                                Level = Convert.IsDBNull(reader["Tier"]) ? 0 : (int)reader["Tier"],
                            };
                            tierConfiguration.PointsType = (Convert.IsDBNull(reader["PK_Category"]) ? true : reader["PK_Category"].ToString() == "1") ? PointsCategoryType.Activity : PointsCategoryType.Product;
                            tierConfiguration.CountryCode = Convert.IsDBNull(reader["CountryCodeISO"]) ? null : (string)reader["CountryCodeISO"];
                            tierConfiguration.ValidFrom = Convert.IsDBNull(reader["ValidFrom"]) ? DateTime.MinValue : (DateTime)reader["ValidFrom"];
                            tierConfiguration.ValidTo = Convert.IsDBNull(reader["ValidTo"]) ? DateTime.MinValue : (DateTime)reader["ValidTo"];
                            tierConfiguration.GoalPoints = Convert.IsDBNull(reader["Points"]) ? 0 : (float)Convert.ToDecimal(reader["Points"]);
                            tierConfiguration.AchievementsGoal = Convert.IsDBNull(reader["AchievementsGoal"]) ? 1 : Convert.ToInt32(reader["AchievementsGoal"]);
                            tiers.Add(tierConfiguration);
                        }

                    }
                }
            }

            return tiers;
        }
    }
}
