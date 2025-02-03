using Microsoft.EntityFrameworkCore;
using DataLayer.Entities;
using DTO.ReqDTO;
using DTO.ResDTO;
using DataLayer.DbScript;
using Helper;
using Microsoft.Extensions.Localization;

namespace BussinessLayer
{
    public class UserBLL
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<UserBLL> _localizer;

        public UserBLL(ApplicationDbContext context, IStringLocalizer<UserBLL> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        public async Task<List<UserResDTO>> GetUsers()
        {
            var translatedUserName = _localizer["UserName"];
            var translatedAddress = _localizer["Address"];
            Console.WriteLine($"Translation for UserName: {translatedUserName}");
            Console.WriteLine($"Translation for Address: {translatedAddress}");

            return await _context.Users
                .Select(u => new UserResDTO
                {
                    Id = u.Id,
                    Name = $"{_localizer["UserName"]}: {u.Name}",
                    DOB = u.DOB,
                    Address = $"{_localizer["Address"]}: {u.Address}",
                    //Name = u.Name,
                    //DOB = u.DOB,
                    //Address = u.Address,
                    Mobile = u.Mobile,
                    Email = u.Email
                }).ToListAsync();
        }

        public async Task<bool> AddUser(UserReqDTO req)
        {
            var user = new UserMst
            {
                Name = req.Name,
                DOB = req.DOB,
                Address = req.Address,
                Mobile = req.Mobile,
                Email = req.Email,
                Password = req.Password
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<UserResDTO> GetUserById(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return null; 

            return new UserResDTO
            {
                Id = user.Id,
                Name = user.Name,
                DOB = user.DOB,
                Address = user.Address,
                Mobile = user.Mobile,
                Email = user.Email
            };
        }

        public async Task<bool> UpdateUser(int userId, UserReqDTO req)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return false; 

            user.Name = req.Name;
            user.DOB = req.DOB;
            user.Address = req.Address;
            user.Mobile = req.Mobile;
            user.Email = req.Email;

            if (!string.IsNullOrEmpty(req.Password))
            {
                user.Password = req.Password;
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUser(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
