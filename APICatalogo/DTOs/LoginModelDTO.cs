﻿using System.ComponentModel.DataAnnotations;

namespace APICatalogo.DTOs;

public class LoginModelDTO
{
    [Required(ErrorMessage ="Username is required.")]
    public string? Name { get; set; } 

    [Required(ErrorMessage ="Password is required.")]
    public string? Password { get; set; }
}
