using DTO.ReqDTO;
using DTO.ResDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interfaces
{
    public interface ILoginService
    {
        Task<LoginResDTO> Login(LoginReqDTO req);
        Task<LoginResDTO> RefreshToken(string refreshToken);
    }
}
