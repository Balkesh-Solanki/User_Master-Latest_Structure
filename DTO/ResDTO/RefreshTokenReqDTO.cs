using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.ResDTO
{
    public class RefreshTokenReqDTO
    {
        public string AccessToken { get; set; } // The expired or existing JWT token
        public string RefreshToken { get; set; } // The stored refresh token
    }
}
