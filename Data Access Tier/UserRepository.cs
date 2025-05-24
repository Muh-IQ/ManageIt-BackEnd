using Interface_Tier.DTOs.User_DTOs;
using Interface_Tier.Repository;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Data_Access_Tier
{
    public class UserRepository : IUserRepository
    {
        public async Task<int> AddUser(AddUserRepositoryDTO addUserDTO)
        {
            int userId = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionProvider.Instance.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_AddNewUser", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Email", addUserDTO.Email);
                        cmd.Parameters.AddWithValue("@UserName", addUserDTO.Username);
                        cmd.Parameters.AddWithValue("@Password", addUserDTO.Password);
                        cmd.Parameters.AddWithValue("@ImagePath", addUserDTO.ImagePath);

                        SqlParameter outputIdParam = new SqlParameter("@UserID", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputIdParam);

                        await conn.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();

                        userId = (int)outputIdParam.Value;
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

            return userId;
        }
        public async Task<bool> IsEmailVerifiedExists(string email)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionProvider.Instance.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_CheckEmailVerifiedExists", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Email", email);

                        await conn.OpenAsync();
                        var result = await cmd.ExecuteScalarAsync();

                        if (result != null && int.TryParse(result.ToString(), out int exists))
                        {
                            return exists == 1;
                        }

                        return false;
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }

        public async Task<int> Login(string email, string password)
        {
            int userId = -1; // Default value if authentication fails

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionProvider.Instance.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_Login", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Password", password);

                        await conn.OpenAsync();

                        object result = await cmd.ExecuteScalarAsync();

                        if (result != null && int.TryParse(result.ToString(), out int id))
                        {
                            userId = id; // Assign the retrieved ID
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Database Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Error: {ex.Message}");
            }

            return userId;
        }
    }
}
