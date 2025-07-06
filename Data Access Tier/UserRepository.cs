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

        public async Task<SimpleUserDataDTO> GetSimpleUserData(int userID)
        {
            SimpleUserDataDTO userData = null;

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionProvider.Instance.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_GetSimpleUserData", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserID", userID);

                        await conn.OpenAsync();
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                userData = new SimpleUserDataDTO
                                {
                                    Username = reader["UserName"].ToString(),
                                    ImagePath = reader["ImagePath"].ToString()
                                };
                            }
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

            return userData;
        }

        public async Task<UserDTO> GetUser(int UserID)
        {
            UserDTO userData = null;

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionProvider.Instance.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_GetUser", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserID", UserID);

                        await conn.OpenAsync();
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                userData = new UserDTO
                                {
                                    Username = reader["UserName"].ToString(),
                                    ImagePath = reader["ImagePath"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    
                                };
                            }
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

            return userData;
        }

        public async Task<int> GetUserIDByEmail(string email)
        {
            int userID = -1; 

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionProvider.Instance.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_GetUserIDByEmail", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Email", email);

                        await conn.OpenAsync();
                        object result = await cmd.ExecuteScalarAsync();

                        if (result != null && int.TryParse(result.ToString(), out int id))
                        {
                            userID = id;
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

            return userID;
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

        public async Task<bool> IsPasswordMatched(int UserID, string Password)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionProvider.Instance.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_IsPasswordMatched", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserID", UserID);
                        cmd.Parameters.AddWithValue("@Password", Password);

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

        public async Task<bool> UpdatePassword(int userID, string password)
        {
            bool isUpdated = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionProvider.Instance.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_UpdatePassword", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@UserID", userID);
                        cmd.Parameters.AddWithValue("@Password", password);

                        await conn.OpenAsync();

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();
                        isUpdated = rowsAffected > 0;
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

            return isUpdated;
        }
        public async Task<bool> UpdateUser(UpdateUserRepositoryDTO dto)
        {
            bool isUpdated = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionProvider.Instance.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_UpdateUser", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@UserID", dto.UserID);
                        cmd.Parameters.AddWithValue("@Email", dto.Email);
                        cmd.Parameters.AddWithValue("@UserName", dto.Username);

                        if (string.IsNullOrEmpty(dto.ImagePath))
                            cmd.Parameters.Add("@ImagePath", SqlDbType.NVarChar, 350).Value = DBNull.Value;
                        else
                            cmd.Parameters.AddWithValue("@ImagePath", dto.ImagePath);

                        await conn.OpenAsync();

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();
                        isUpdated = rowsAffected > 0;
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

            return isUpdated;
        }

    }
}
