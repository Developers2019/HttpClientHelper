namespace SupportPowerTool.Services.API.Abstraction.DapperSupport.Model;

public class SqlParameterInfo
{
    public string ParameterName { get; set; } = string.Empty;
    public string DataType { get; set; } = string.Empty;
    public bool IsOutput { get; set; } = false;
}