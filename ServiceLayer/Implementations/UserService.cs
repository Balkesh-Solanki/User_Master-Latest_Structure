using BussinessLayer;
using DTO.ReqDTO;
using DTO.ResDTO;
using ServiceLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Implementations
{
    public class UserService : IUserService
    {
        private readonly UserBLL _userBLL;

        public UserService(UserBLL userBLL)
        {
            _userBLL = userBLL;
        }

        public async Task<List<UserResDTO>> GetUsers()
        {
            return await _userBLL.GetUsers();
        }

        public async Task<UserResDTO> GetUserById(int userId)
        {
            return await _userBLL.GetUserById(userId);
        }

        public async Task<bool> AddUser(UserReqDTO req)
        {
            return await _userBLL.AddUser(req);
        }

        public async Task<bool> UpdateUser(int userId, UserReqDTO req)
        {
            return await _userBLL.UpdateUser(userId, req);
        }

        public async Task<bool> DeleteUser(int userId)
        {
            return await _userBLL.DeleteUser(userId);
        }
    }
}
