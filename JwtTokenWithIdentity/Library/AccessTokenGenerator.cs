using JwtTokenWithIdentity.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JwtTokenWithIdentity.Library
{
    public class AccessTokenGenerator
    {
        public APIDbContext _context { get; set; }
        public IConfiguration _config { get; set; }
        public ApplicationUser _applicationUser { get; set; }

        /// <summary>
        /// Class'ın oluşturulması.
        /// </summary>
        /// <param name="_context"></param>
        /// <param name="_config"></param>
        /// <param name="_applicationUser"></param>
        /// <returns></returns>
        public AccessTokenGenerator(APIDbContext context,
                                    IConfiguration config,
                                    ApplicationUser applicationUser)
        {
            _config = config;
            _context = context;
            _applicationUser = applicationUser;
        }

        /// <summary>
        /// Kullanıcı üzerinde tanımlı tokenı döner;Token yoksa oluşturur. Expire olmuşsa update eder.
        /// </summary>
        /// <returns></returns>
        public ApplicationUserTokens GetToken()
        {
            ApplicationUserTokens userTokens = null;
            TokenInfo tokenInfo = null;

            //Kullanıcıya ait önceden oluşturulmuş bir token var mı kontrol edilir.
            if (_context.ApplicationUserTokens.Count(x => x.UserId == _applicationUser.Id) > 0)
            {
                //İlgili token bilgileri bulunur.
                userTokens = _context.ApplicationUserTokens.FirstOrDefault(x => x.UserId == _applicationUser.Id);

                //Expire olmuş ise yeni token oluşturup günceller.
                if (userTokens.ExpireDate <= DateTime.Now)
                {
                    //Create new token
                    tokenInfo = GenerateToken();

                    userTokens.ExpireDate = tokenInfo.ExpireDate;
                    userTokens.Value = tokenInfo.Token;

                    _context.ApplicationUserTokens.Update(userTokens);
                }
            }
            else
            {
                //Create new token
                tokenInfo = GenerateToken();

                userTokens = new ApplicationUserTokens();

                userTokens.UserId = _applicationUser.Id;
                userTokens.LoginProvider = "SystemAPI";
                userTokens.Name = _applicationUser.FullName;
                userTokens.ExpireDate = tokenInfo.ExpireDate;
                userTokens.Value = tokenInfo.Token;

                _context.ApplicationUserTokens.Add(userTokens);
            }

            _context.SaveChangesAsync();

            return userTokens;
        }

        /// <summary>
        /// Kullanıcıya ait tokenı siler.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> DeleteToken()
        {
            bool ret = true;

            try
            {
                //Kullanıcıya ait önceden oluşturulmuş bir token var mı kontrol edilir.
                if (_context.ApplicationUserTokens.Count(x => x.UserId == _applicationUser.Id) > 0)
                {
                    ApplicationUserTokens userTokens = userTokens = _context.ApplicationUserTokens.FirstOrDefault(x => x.UserId == _applicationUser.Id);

                    _context.ApplicationUserTokens.Remove(userTokens);
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                ret = false;
            }

            return ret;
        }

        /// <summary>
        /// Yeni token oluşturur.
        /// </summary>
        /// <returns></returns>
        private TokenInfo GenerateToken()
        {
            DateTime expireDate = DateTime.Now.AddSeconds(50);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["Application:Secret"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _config["Application:Audience"],
                Issuer = _config["Application:Issuer"],
                Subject = new ClaimsIdentity(new Claim[]
                {
                    //Claim tanımları yapılır. Burada en önemlisi Id ve emaildir.
                    //Id üzerinden, aktif kullanıcıyı buluyor olacağız.
                    new Claim(ClaimTypes.NameIdentifier, _applicationUser.Id),
                    new Claim(ClaimTypes.Name, _applicationUser.FullName),
                    new Claim(ClaimTypes.Email, _applicationUser.Email)
                }),

                //ExpireDate
                Expires = expireDate,

                //Şifreleme türünü belirtiyoruz: HmacSha256Signature
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            TokenInfo tokenInfo = new TokenInfo();
            
            tokenInfo.Token = tokenString;
            tokenInfo.ExpireDate = expireDate;

            return tokenInfo;
        }
    }
}
