using System;
using System.Data.Common;
using System.Reflection;
using Npgsql;
using NpgsqlTypes;

namespace CoreCard.Tesla.Common
{
    public static class NpgsqlExtensions
    {
        public static object TryGetOrdinal(this NpgsqlDataReader dataReader, string columnsName)
        {
            try
            {
                return dataReader[columnsName.ToLower()];
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return "";
            }
        }

        
    }
}