﻿using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class Usuario
{
    public long Id { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string Email { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string? Password { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<PreferenciaUsuario> PreferenciaUsuarios { get; set; } = new List<PreferenciaUsuario>();
}
