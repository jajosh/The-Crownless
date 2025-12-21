using Microsoft.Data.Sqlite;
using Microsoft.VisualBasic;
using System;
using System.Reflection;

public class GridRepository : GameDataBase
{
	public GridRepository()
	{
	}
	public static GridObject Query(object criteria)
	{
        if (criteria == null) // Null check
        {
            BugHunter.LogException(new ArgumentNullException(nameof(criteria)));
            return null;
        }
        Type criteriaType = criteria.GetType();
        var whereClauses = new List<string>(); // What will be the sql query
        var parameters = new List<SqliteParameter>();

        foreach (PropertyInfo prop in criteriaType.GetProperties())
        {
            object value = prop.GetValue(criteria);
            if(value != null && !IsDefaultValue(value))
            {
                string columnName = prop.Name;
                whereClauses.Add($"{columnName} = @{columnName}");
                parameters.Add(new SqliteParameter($"@{columnName}", value));
            }
        }
        if (whereClauses.Count == 0)
        {
            BugHunter.LogException(new InvalidOperationException("No criteria provided for query"));
        }
        string whereClause =string.Join(" AND ", whereClause);
        string query = $@"
            SELECT *
            FROM Items
            LEFT JOIN DescirptionEntry on TypeID
            AND DecriptionEntry.DescriptionType = 'Grid'
            where {whereClause}
            LIMIT 1";
        using (var connection = MyConnection)
        {
            connection.Open();
            using (var command = new SqliteCommand(query, connection))
            {
                command.Parameters.AddRange(parameters.ToArray());
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return GameDataBaseReader.MapToGridObject(reader);
                    }
                }
            }
        }


    }
    private static bool IsDefaultValue(object value)
    {
        // Check if value is default for its type (e.g., 0 for int, null for string)
        if (value == null) return true;
        Type type = value.GetType();
        if (type.IsValueType)
        {
            return value.Equals(Activator.CreateInstance(type));
        }
        return false;
    }
}
