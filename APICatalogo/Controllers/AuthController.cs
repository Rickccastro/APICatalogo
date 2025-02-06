﻿using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace APICatalogo.Controllers;
[Route("[controller]")]
[ApiConventionType(typeof(DefaultApiConventions))]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthController> _logger;

    public AuthController(ITokenService tokenService, UserManager<ApplicationUser>
        userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, ILogger<AuthController> logger)
    {
        _tokenService = tokenService;
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _logger = logger;
    }

    [HttpPost]
    [Route("AddUserToRole")]
    public async Task<IActionResult> AddUserToRole(string email,string roleName)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null) 
            return BadRequest(new { error = "Unable to find user" });
        
        var result = await _userManager.AddToRoleAsync(user, roleName);

        if (!result.Succeeded)
        {
            _logger.LogInformation(1, $"Error:Unable to add user{user.Email} to the role {roleName}");
            return StatusCode(StatusCodes.Status400BadRequest, new ResponseDTO { Status = "Error", Message = $"Unable to add user {user.Email} to the role {roleName} " });
        }

        _logger.LogInformation(1, $"User{user.Email} added to the role {roleName} role");
        return StatusCode(StatusCodes.Status200OK, new ResponseDTO { Status = "Success", Message = $"User {user.Email} added to the role {roleName} " });
    }



    [HttpPost]
    [Route("CreateRole")]
    public async Task<IActionResult> CreateRole(string roleName)
    {
        var roleExist = await _roleManager.RoleExistsAsync(roleName);
        if (!roleExist)
        {
            var roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));

            if (!roleResult.Succeeded)
            {
                _logger.LogInformation(2, "Error");
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseDTO { Status = "Error", Message = $"Error adding the role {roleName}" });

            }
            _logger.LogInformation(1, "Roles Added");
            return StatusCode(StatusCodes.Status200OK, new ResponseDTO { Status = "Sucess", Message = $"Role {roleName} added sucessfully" });
        }

        return StatusCode(StatusCodes.Status400BadRequest, new ResponseDTO { Status = "Error", Message = $"Role {roleName} already exists" });
    }



    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginModelDTO loginModel)
    {
        var user = await _userManager.FindByNameAsync(loginModel.Name!);

        if (user is not null && await _userManager.CheckPasswordAsync(user, loginModel.Password!))
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.UserName!),
                new Claim(ClaimTypes.Email,user.Email!),
                new Claim("id",user.UserName!),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var token = _tokenService.GenerateAcessToken(authClaims, _configuration);

            var refreshToken = _tokenService.GenerateRefreshToken();

            _ = int.TryParse(_configuration["JWT:RefreshTokenValidtyInMinutes"], out int refreshTokenValidityInMinutes);

            user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(refreshTokenValidityInMinutes);
            user.RefreshToken = refreshToken;
            await _userManager.UpdateAsync(user);

            return Ok(new
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken,
                Expiration = token.ValidTo,

            });
        }

        return Unauthorized();
    }



    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModelDTO registerModel)
    {
        var userExists = await _userManager.FindByNameAsync(registerModel.Name!);

        if (userExists != null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDTO { Status = "Error", Message = "User already exists!" });
        }

        var user = new ApplicationUser
        {
            Email = registerModel.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = registerModel.Name,
        };

        var result = await _userManager.CreateAsync(user, registerModel.Password!);

        if (!result.Succeeded)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDTO { Status = "Error", Message = "User creation failed" });
        }

        return Ok(new ResponseDTO { Status = "Sucess", Message = "User created sucessfully" });
    }



    [HttpPost]
    [Route("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] TokenModelDTO tokenModel)
    {
        if (tokenModel == null)
        {
            return BadRequest("Invalid client request");
        }

        string? acessToken = tokenModel.AcessToken ?? throw new ArgumentNullException(nameof(tokenModel));
        string? refreshToken = tokenModel.RefreshToken ?? throw new ArgumentNullException(nameof(tokenModel));

        var principal = _tokenService.GetPrincipalFromExpiredToken(acessToken!, _configuration);

        if (principal == null)
        {
            return BadRequest("Invalid acess token/refresh token");
        }

        string username = principal.Identity!.Name!;

        var user = await _userManager.FindByNameAsync(username!);

        if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
        {
            return BadRequest("Invalid acess token/refresh token");
        }

        var newAcessToken = _tokenService.GenerateAcessToken(principal.Claims.ToList(), _configuration);

        var newRefreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;

        await _userManager.UpdateAsync(user);

        return new ObjectResult(new
        {
            acessToken = new JwtSecurityTokenHandler().WriteToken(newAcessToken),
            refreshToken = newRefreshToken,
        });
    }

    [Authorize]
    [HttpPost]
    [Route("revoke/{username}")]
    public async Task<IActionResult> RefreshToken(string username)
    {
        var user = await _userManager.FindByNameAsync(username);

        if (user is null)
        {
            return BadRequest("Invalid username");
        }

        user.RefreshToken = null;
        await _userManager.UpdateAsync(user);

        return NoContent();
    }

}
