using System.Collections.Generic;

public class PrincipalValue
{
    [Newtonsoft.Json.JsonProperty("auth_typ")]
    public string AuthType { get; set; }
    public List<ClaimItem> Claims { get; set; }
    [Newtonsoft.Json.JsonProperty("name_typ")]
    public string NameType { get; set; }
    [Newtonsoft.Json.JsonProperty("role_typ")]
    public string RoleType { get; set; }

    public bool IsUserInRole(string role)
    {
        return Claims != null && Claims.Exists(c => c.Type == "roles" && c.Value == role);
    }
}

public class ClaimItem
{
    [Newtonsoft.Json.JsonProperty("typ")]
    public string Type { get; set; }
    [Newtonsoft.Json.JsonProperty("val")]
    public string Value { get; set; }
}
