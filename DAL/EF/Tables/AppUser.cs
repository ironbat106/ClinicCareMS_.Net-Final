using System;
using System.Collections.Generic;

namespace DAL.EF.Tables;

public partial class AppUser
{
    public int UserId { get; set; }

    public string FullName { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string? Token { get; set; }

    public bool IsActive { get; set; }
}
