using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class DestinosPreferencia
{
    public long PreferenciasId { get; set; }

    public long DestinosId { get; set; }

    public virtual Destino Destinos { get; set; } = null!;

    public virtual Preferencia Preferencias { get; set; } = null!;
}
