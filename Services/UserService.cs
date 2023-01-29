using System;
using System.Collections.Generic;
// using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
// using Microsoft.IdentityModel.Tokens;
namespace User.Services
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Text.Json;
    using Microsoft.IdentityModel.Tokens;
    using User.Interfaces;
    using User.Models;
    public class UserService : IUserService
    {

        // private static SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SXkSqsKyNUyvGbnHs7ke2NCq8zQzNLW7mPmHbnZZ"));
        // private static string issuer = "";
        // public static SecurityToken GetToken(List<Claim> claims) =>
        //     new JwtSecurityToken(
        //         issuer,
        //         issuer,
        //         claims,
        //         expires: DateTime.Now.AddDays(30.0),
        //         signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        //     );


        List<User>? users { get; }

        // static int nextId = 3;
        private IWebHostEnvironment webHost;
        private string filePath;
        public UserService(IWebHostEnvironment webHost)
        {
            this.webHost = webHost;
            this.filePath = Path.Combine(webHost.ContentRootPath, "Data", "User.json");
            using (var jsonFile = File.OpenText(filePath))
            {
                users = JsonSerializer.Deserialize<List<User>>(jsonFile.ReadToEnd(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
        }
        private void saveToFile()
        {
            File.WriteAllText(filePath, JsonSerializer.Serialize(users));
        }

        public List<User>? GetAll() => users;

        public User? Get(int Id) => users?.FirstOrDefault(t => t.Id == Id);

        public void Post(User u)
        {
            u.Id = users.Count() + 1;
            users.Add(u);
            saveToFile();
        }

        public void Delete(int id)
        {
            var user = Get(id);
            if (user is null)
                return;
            users.Remove(user);
            saveToFile();
        }

        public int Count => users.Count();
    }

}
