namespace Apllication.DTOs;

public class LogedUserInfoDto
{
  public bool Valid { get; set; }
  public CredentialsDto Credentials { get; set; } = new();
}

public class CredentialsDto
{
  public string DisplayName { get; set; } = "";
  public string Token { get; set; } = "";
  public string Image { get; set; } = "";
  public string Id { get; set; } = "";
  public string? RoleName { get; set; }
}
