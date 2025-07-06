using Interface_Tier.Repository;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Tier
{
    public class UserAuthorizationRepository : IUserAuthorizationRepository
    {
        public async Task<bool> IsUserAuthorizedForProject(int userId, int projectId)
        {
            bool isAuthorized = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionProvider.Instance.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_IsUserAuthorizedForProject", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserID", userId);
                        cmd.Parameters.AddWithValue("@ProjectID", projectId);

                        await conn.OpenAsync();

                        var result = await cmd.ExecuteScalarAsync();

                        if (result != null && int.TryParse(result.ToString(), out int value))
                        {
                            isAuthorized = value == 1;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return isAuthorized;
        }


        public async Task<bool> IsUserAuthorizedForProjectChat(int userId, int projectId)
        {
            bool isAuthorized = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionProvider.Instance.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_IsUserAuthorizedForProjectChat", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ProjectID", projectId);
                        cmd.Parameters.AddWithValue("@UserID", userId);

                        conn.Open();

                        var result = await cmd.ExecuteScalarAsync();
                        if (result != null && int.TryParse(result.ToString(), out int value))
                        {
                            isAuthorized = value == 1;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return isAuthorized;
        }
        public async Task<bool> IsUserAuthorizedForTaskChat(int userId, int taskId)
        {

            bool isAuthorized = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionProvider.Instance.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_IsUserAuthorizedForTaskChat", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@TaskID", taskId);
                        cmd.Parameters.AddWithValue("@UserID", userId);

                        conn.Open();

                        var result = await cmd.ExecuteScalarAsync();
                        if (result != null && int.TryParse(result.ToString(), out int value))
                        {
                            isAuthorized = value == 1;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return isAuthorized;
        }
    }
}
