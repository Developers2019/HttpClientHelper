using SupportPowerTool.Services.API.Abstraction.DapperSupport.Model;
using System.Data;
using System.Reflection;

namespace SupportPowerTool.Services.API.Abstraction.DapperSupport;

public static class StoredProcedureParameterBuilder
{
    public static async Task<DynamicParameters> BuildDynamicParametersFromDtoAsync<T>(
         this T dto,
         string connectionString,
         string storedProcedureName)
         where T : class
    {
        var parameters = await GetParamsAsync(connectionString, storedProcedureName);

        var dynamicParams = new DynamicParameters();

        foreach (SqlParameterInfo param in parameters)
        {
            string paramName = param.ParameterName.TrimStart('@');
            var property = typeof(T).GetProperty(paramName,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            var dbType = MapToDbType(param.DataType);

            if (property != null)
            {
                var value = property.GetValue(dto);
                dynamicParams.Add(param.ParameterName, value, dbType,
                    param.IsOutput ? ParameterDirection.Output : ParameterDirection.Input);
            }
            else if (!param.IsOutput)
            {
                var defaultValue = dbType == DbType.Boolean ? (object)false : DBNull.Value;
                dynamicParams.Add(param.ParameterName, defaultValue, dbType, ParameterDirection.Input);
            }
        }

        return dynamicParams;
    }

    private static async Task<List<SqlParameterInfo>> GetParamsAsync(string connectionString, string procName)
    {
        using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();

        var parameters = (await connection.QueryAsync<SqlParameterInfo>(@"
				SELECT
					p.name AS ParameterName,
					TYPE_NAME(p.user_type_id) AS DataType,
					CAST(p.is_output AS bit) AS IsOutput
				FROM sys.parameters p
				WHERE object_id = OBJECT_ID(@ProcName)
				ORDER BY p.parameter_id
		",
        new { ProcName = procName })).ToList();

        return parameters;
    }

    private static DbType MapToDbType(string sqlType)
    {
        return sqlType.ToLower() switch
        {
            "int" => DbType.Int32,
            "smallint" => DbType.Int32,
            "tinyint" => DbType.Int32,
            "bigint" => DbType.Int64,
            "decimal" => DbType.Decimal,
            "numeric" => DbType.Decimal,
            "bit" => DbType.Boolean,
            "datetime" => DbType.DateTime,
            "nvarchar" => DbType.String,
            "varchar" => DbType.String,
            "uniqueidentifier" => DbType.Guid,
            _ => DbType.Object
        };
    }
}