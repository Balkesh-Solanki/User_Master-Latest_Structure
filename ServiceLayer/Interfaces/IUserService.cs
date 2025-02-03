using DTO.ReqDTO;
using DTO.ResDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interfaces
{
    public interface IUserService
    {
        Task<List<UserResDTO>> GetUsers();
        Task<UserResDTO> GetUserById(int userId);
        Task<bool> AddUser(UserReqDTO req);
        Task<bool> UpdateUser(int userId, UserReqDTO req);
        Task<bool> DeleteUser(int userId);
    }
}
