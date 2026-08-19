using System;
using System.Collections.Generic;
using System.Text;

namespace ITSOLUTION.Application.DTOs.Auth
{
    public class LoginRequestDto
    {
        public string Correo { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}