﻿using Microsoft.Data.SqlClient;
using Mozaic.PasswordManager.DAL;
using Mozaic.PasswordManager.Entities.SearchFilters;
using Mozaic.PasswordManager.Entities;
using System.Data;




internal static class SystemUserProvider
{
    internal static async Task<TransactionResult> CreateUser(SystemUser user)
    {
        var transactionResult = new TransactionResult { Status = SystemTransactionStatus.Success };
        using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
        {
            using (SqlCommand createuser = new SqlCommand("dbo.CreateSystemUser", conn))
            {
                createuser.CommandType = CommandType.StoredProcedure;

                createuser.Parameters.AddWithValue("@UserName", user.UserName);
                createuser.Parameters.AddWithValue("@Password", user.password); // Ensure property name is correct (case-sensitive)
                createuser.Parameters.AddWithValue("@CreatedDate", user.CreationDate);
                createuser.Parameters.AddWithValue("@IsAdmin", user.IsAdmin); // Pass the IsAdmin property
                createuser.Parameters.AddWithValue("CreatedBy", user.CreatedBy);


                var resultParam = new Microsoft.Data.SqlClient.SqlParameter();
                resultParam.Direction = ParameterDirection.Output;
                resultParam.SqlDbType = SqlDbType.VarChar;
                resultParam.Size = 4;
                resultParam.ParameterName = "@Result";
                createuser.Parameters.Add(resultParam);


                conn.Open();
                await createuser.ExecuteNonQueryAsync();
                string? result = resultParam.Value.ToString();
                if (result != SystemTransactionStatus.Success)
                {
                    transactionResult.Status = result;
                    switch (transactionResult.Status)
                    {
                        case SystemTransactionStatus.ExistingRecord:
                            transactionResult.Message = "Username already exists. Please choose a different one";
                            break;
                    }
                }
            }
        }
        return transactionResult;
    }

    public static List<SystemUser> GetSystemUser(SystemUserSearchFilter filter)
    {
        var systemUsers = new List<SystemUser>();
        int totalRecords = 0;

        using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand("GetSystemUsers", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            sqlCommand.Parameters.AddWithValue("@UserName", string.IsNullOrEmpty(filter.UserName) ? (object)DBNull.Value : filter.UserName);
            sqlCommand.Parameters.AddWithValue("@UserId", filter.Id.HasValue ? (object)filter.Id.Value : DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@IsAdmin", filter.IsAdmin.HasValue ? (object)filter.IsAdmin.Value : DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@PageSize", filter.PageSize);
            sqlCommand.Parameters.AddWithValue("@PageNumber", filter.PageNumber);

            var totalNumberOResultParam = new SqlParameter();
            totalNumberOResultParam.Direction = ParameterDirection.Output;
            totalNumberOResultParam.SqlDbType = SqlDbType.Int;
            totalNumberOResultParam.ParameterName = "@TotalResult";
            sqlCommand.Parameters.Add(totalNumberOResultParam);

            conn.Open();
            using (SqlDataReader reader = sqlCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    systemUsers.Add(new SystemUser
                    {
                        Id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                        UserName = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                        IsAdmin = reader.IsDBNull(2) ? (bool?)null : reader.GetBoolean(2)
                    });
                }

                

            }
            totalRecords = (int)totalNumberOResultParam.Value;

        }

        filter.TotalRecords = totalRecords;

        return systemUsers;
    }




}
