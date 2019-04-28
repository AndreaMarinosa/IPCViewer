﻿namespace IPCViewer.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using Common.Models;
    using Helpers;
    using IPCViewer.Api.Models;
    using IPCViewer.Common.Helpers;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;

    [Route("api/[Controller]")]
    public class AccountController : Controller
    {
        private readonly IUserHelper userHelper;
        private readonly ICityRepository cityRepository;
        private readonly IMailHelper mailHelper;
        private readonly IConfiguration configuration;

        public AccountController(
            IUserHelper userHelper,
            ICityRepository cityRepository,
            IMailHelper mailHelper,
            IConfiguration configuration)
        {
            this.userHelper = userHelper;
            this.cityRepository = cityRepository;
            this.mailHelper = mailHelper;
            this.configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> PostCreateToken([FromBody] LoginViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = await this.userHelper.GetUserByEmailAsync(model.Username);
                if (user != null)
                {
                    var result = await this.userHelper.ValidatePasswordAsync(
                        user,
                        model.Password);

                    if (result.Succeeded)
                    {
                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                        };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration["Tokens:Key"]));
                        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            this.configuration["Tokens:Issuer"],
                            this.configuration["Tokens:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddDays(15),
                            signingCredentials: credentials);
                        var results = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        };

                        return this.Created(string.Empty, results);
                    }
                }
            }
            return this.BadRequest();
        }


        [HttpGet]
        public async Task<List<Models.User>> GetUser()
        {
            return await userHelper.GetAllUsersAsync();
        }

        [HttpPost]
        public async Task<IActionResult> PostUser([FromBody] NewUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "Bad request"
                });
            }

            var user = await this.userHelper.GetUserByEmailAsync(request.Email);
            if (user != null)
            {
                return this.BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "This email is already registered."
                });
            }

            var city = await cityRepository.GetByIdAsync(request.City.Id);
            if (city == null)
            {
                return this.BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "City don't exists."
                });
            }

            user = new Models.User // Api/Models
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.Email,
                City = city
            };

            var result = await this.userHelper.AddUserAsync(user, request.Password);
            if (result != IdentityResult.Success)
            {
                return this.BadRequest(result.Errors.FirstOrDefault().Description);
            }

            var myToken = await this.userHelper.GenerateEmailConfirmationTokenAsync(user);
            var tokenLink = this.Url.Action("ConfirmEmail", "Account", new
            {
                userid = user.Id,
                token = myToken
            }, protocol: HttpContext.Request.Scheme);

            this.mailHelper.SendMail(request.Email, "Email confirmation", $"<h1>Email Confirmation</h1>" +
                $"To allow the user, " +
                $"plase click in this link:</br></br><a href = \"{tokenLink}\">Confirm Email</a>");

            return Ok(new Response
            {
                IsSuccess = true,
                Message = "A Confirmation email was sent. Plese confirm your account and log into the App."
            });
        }

        //    [HttpPost]
        //    [Route("RecoverPassword")]
        //    public async Task<IActionResult> RecoverPassword([FromBody] RecoverPasswordRequest request)
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return this.BadRequest(new Response
        //            {
        //                IsSuccess = false,
        //                Message = "Bad request"
        //            });
        //        }

        //        var user = await this.userHelper.GetUserByEmailAsync(request.Email);
        //        if (user == null)
        //        {
        //            return this.BadRequest(new Response
        //            {
        //                IsSuccess = false,
        //                Message = "This email is not assigned to any user."
        //            });
        //        }

        //        var myToken = await this.userHelper.GeneratePasswordResetTokenAsync(user);
        //        var link = this.Url.Action("ResetPassword", "Account", new { token = myToken }, protocol: HttpContext.Request.Scheme);
        //        this.mailHelper.SendMail(request.Email, "Password Reset", $"<h1>Recover Password</h1>" +
        //            $"To reset the password click in this link:</br></br>" +
        //            $"<a href = \"{link}\">Reset Password</a>");

        //        return Ok(new Response
        //        {
        //            IsSuccess = true,
        //            Message = "An email with instructions to change the password was sent."
        //        });
        //    }

        //    [HttpPost]
        //    [Route("GetUserByEmail")]
        //    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //    public async Task<IActionResult> GetUserByEmail([FromBody] RecoverPasswordRequest request)
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return this.BadRequest(new Response
        //            {
        //                IsSuccess = false,
        //                Message = "Bad request"
        //            });
        //        }

        //        var user = await this.userHelper.GetUserByEmailAsync(request.Email);
        //        if (user == null)
        //        {
        //            return this.BadRequest(new Response
        //            {
        //                IsSuccess = false,
        //                Message = "User don't exists."
        //            });
        //        }

        //        return Ok(user);
        //    }

        //    [HttpPut]
        //    public async Task<IActionResult> PutUser([FromBody] User user)
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return this.BadRequest(ModelState);
        //        }

        //        var userEntity = await this.userHelper.GetUserByEmailAsync(user.Email);
        //        if (userEntity == null)
        //        {
        //            return this.BadRequest("User not found.");
        //        }

        //        var city = await this.cityRepository.GetCityAsync(user.CityId);
        //        if (city != null)
        //        {
        //            userEntity.City = city;
        //        }

        //        userEntity.FirstName = user.FirstName;
        //        userEntity.LastName = user.LastName;
        //        userEntity.CityId = user.CityId;
        //        userEntity.Address = user.Address;
        //        userEntity.PhoneNumber = user.PhoneNumber;

        //        var respose = await this.userHelper.UpdateUserAsync(userEntity);
        //        if (!respose.Succeeded)
        //        {
        //            return this.BadRequest(respose.Errors.FirstOrDefault().Description);
        //        }

        //        var updatedUser = await this.userHelper.GetUserByEmailAsync(user.Email);
        //        return Ok(updatedUser);
        //    }

        //    [HttpPost]
        //    [Route("ChangePassword")]
        //    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return this.BadRequest(new Response
        //            {
        //                IsSuccess = false,
        //                Message = "Bad request"
        //            });
        //        }

        //        var user = await this.userHelper.GetUserByEmailAsync(request.Email);
        //        if (user == null)
        //        {
        //            return this.BadRequest(new Response
        //            {
        //                IsSuccess = false,
        //                Message = "This email is not assigned to any user."
        //            });
        //        }

        //        var result = await this.userHelper.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
        //        if (!result.Succeeded)
        //        {
        //            return this.BadRequest(new Response
        //            {
        //                IsSuccess = false,
        //                Message = result.Errors.FirstOrDefault().Description
        //            });
        //        }

        //        return this.Ok(new Response
        //        {
        //            IsSuccess = true,
        //            Message = "The password was changed succesfully!"
        //        });
        //    }

    }
}