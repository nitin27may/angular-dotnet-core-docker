﻿using System;

namespace Domain.Common;

public class BaseEntity
{
    public int Id { get; set; }
    public string CreatedBy { get; set; }
    public DateTime Created { get; set; }
    public string ModifiedBy { get; set; }
    public DateTime? Modified { get; set; }
}
