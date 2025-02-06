﻿using APIWithJWTAuthentication.Contracts;
using APIWithJWTAuthentication.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SharedLibrary.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static SharedLibrary.DTOs.ServiceResponse;

namespace APIWithJWTAuthentication.Repository
{
    public class AccountRepository(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration config) : IUserAccount
    {
        public async Task<GeneralResponse> CreateAccount(UserDTO userDTO)
        {
            try
            {
                if (userDTO == null) return new GeneralResponse(false, "Model is Empty");

                var newUser = new ApplicationUser()
                {
                    Name = userDTO.Name,
                    Email = userDTO.Email,
                    UserName = userDTO.Email  // Better to use email as username
                };

                var user = await userManager.FindByEmailAsync(newUser.Email);
                if (user is not null) return new GeneralResponse(false, "User already Registered");

                var createUser = await userManager.CreateAsync(newUser, userDTO.Password);
                if (!createUser.Succeeded)
                {
                    var errors = string.Join(", ", createUser.Errors.Select(x => x.Description));
                    return new GeneralResponse(false, $"Error Occurred: {errors}");
                }

                var checkAdmin = await roleManager.FindByNameAsync("Admin");
                if (checkAdmin is null)
                {
                    await roleManager.CreateAsync(new IdentityRole() { Name = "Admin" });
                    await userManager.AddToRoleAsync(newUser, "Admin");
                    return new GeneralResponse(true, "Admin Account Created");
                }
                else
                {
                    var checkUser = await roleManager.FindByNameAsync("User");
                    if (checkUser is null)
                    {
                        await roleManager.CreateAsync(new IdentityRole() { Name = "User" });
                    }
                    await userManager.AddToRoleAsync(newUser, "User");
                    return new GeneralResponse(true, "User Account Created");
                }
            }
            catch (Exception ex)
            {
                return new GeneralResponse(false, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<ServiceResponse.LoginResponse> LoginAccount(LoginDTO loginDTO)
        {
            if (loginDTO == null) return new LoginResponse(false, null!, "Login container is empty");
            var getUser=await userManager.FindByEmailAsync(loginDTO.Email);
            if(getUser is null) return new LoginResponse(false, null!, "User not found");
            bool checkPassword=await userManager.CheckPasswordAsync(getUser, loginDTO.Password);
            if (!checkPassword) return new LoginResponse(false, null!, "Invalid username/password");
            var getUserRole=await userManager.GetRolesAsync(getUser);
            var userSession = new UserSession(getUser.Id, getUser.Name, getUser.Email, getUserRole.First());
            string token = GenerateJWTToken(userSession);
            return new LoginResponse(true, token, "User login successfully");
           
        }

        private string GenerateJWTToken(UserSession userSession)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Key"]!));
            var credentials=new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userSession.Id!),
                new Claim(ClaimTypes.Name, userSession.Name!),
                new Claim(ClaimTypes.Email, userSession.Email!),
                new Claim(ClaimTypes.Role, userSession.Role !)

            };
            var token = new JwtSecurityToken(
                issuer: config["JWT:Issuer"],
                audience: config["JWT:Audience"],
                claims:userClaims,
                expires:DateTime.Now.AddDays(1),
                signingCredentials:credentials

                );
            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
