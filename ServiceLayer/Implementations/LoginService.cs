using DataLayer.DbScript;
using DataLayer.Entities;
using DTO.ReqDTO;
using DTO.ResDTO;
using Helper.CommonHelpers;
using Helper;
using ServiceLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Security.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace ServiceLayer.Implementations
{
    public class LoginService : ILoginService
    {
        private readonly ApplicationDbContext _context;
        private readonly AuthHelper _authHelper;

        public LoginService(ApplicationDbContext context, AuthHelper authHelper)
        {
            _context = context;
            _authHelper = authHelper;
        }
        public async Task<LoginResDTO> Login(LoginReqDTO req)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == req.Email);
                if (user == null || user.Password != req.Password)
                {
                    throw new AuthenticationException("Invalid credentials");
                }

                if (user == null)
                {
                    Console.WriteLine("User not found");
                    throw new AuthenticationException("Invalid credentials");
                }

                string accessToken = _authHelper.GenerateAccessToken(user.Id, user.Email);
                string refreshToken = _authHelper.GenerateRefreshToken();

                var tokenMst = new TokenMst
                {
                    UserId = user.Id,
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    ExpiryDate = DateTime.UtcNow.AddDays(7) 
                };

                _context.TokenMst.Add(tokenMst);
                await _context.SaveChangesAsync();

                return new LoginResDTO
                {
                    UserId = user.Id,
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    Email = user.Email,
                    Message = "Login Successful"
                };
            }
            catch (AuthenticationException ex)
            {
                throw new UnauthorizedAccessException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while processing your request", ex);
            }
        }

        public async Task<LoginResDTO> RefreshToken(string refreshToken)
        {
            var tokenEntry = await _context.TokenMst.FirstOrDefaultAsync(t => t.RefreshToken == refreshToken);

            if (tokenEntry == null || tokenEntry.ExpiryDate < DateTime.UtcNow)
                return null;

            if (!string.IsNullOrEmpty(tokenEntry.AccessToken) && !_authHelper.IsTokenExpired(tokenEntry.AccessToken))
            {
                return null;
            }

            var accesstokenEntry = await _context.TokenMst
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.RefreshToken == refreshToken);

            string newAccessToken = _authHelper.GenerateAccessToken(tokenEntry.UserId, tokenEntry.User.Email);
            string newRefreshToken = _authHelper.GenerateRefreshToken();

            tokenEntry.AccessToken = newAccessToken;
            tokenEntry.RefreshToken = newRefreshToken;
            tokenEntry.ExpiryDate = DateTime.UtcNow.AddDays(7);

            await _context.SaveChangesAsync();

            return new LoginResDTO
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }
    }
}
