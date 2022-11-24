using Microsoft.Data.SqlClient;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", (IConfiguration configuration) =>
{
    try
    {
        using var connection = new SqlConnection(configuration["DBConnectionString"]);
        connection.Open();

        return "Connection succeeded";
    }
    catch (Exception e)
    {
        return ExtractExceptionMessage(e);
    }
});

app.MapGet("/select", (IConfiguration configuration) =>
{
    try
    {
        using var connection = new SqlConnection(configuration["DBConnectionString"]);
        connection.Open();

        SqlCommand command = new SqlCommand("SELECT 1", connection);
        command.ExecuteNonQuery();

        return "SELECT succeeded";
    }
    catch (Exception e)
    {
        return ExtractExceptionMessage(e);
    }
});

app.Run();

static string ExtractExceptionMessage(Exception e)
{
    var sb = new StringBuilder(e.Message + Environment.NewLine + e.StackTrace);

    while (e.InnerException is not null)
    {
        e = e.InnerException;
        sb.AppendLine(e.Message + Environment.NewLine + e.StackTrace);
    }

    return sb.ToString();
}