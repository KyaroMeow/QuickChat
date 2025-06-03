using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using WebApplication2.Models;
using WebApplication2.DTO;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly QuickChatBaseContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly ITokenService _tokenService;

    public AuthController(QuickChatBaseContext context, IPasswordHasher<User> passwordHasher, ITokenService tokenService)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
    {
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Login == loginDto.Login);
        if (user == null)
            return Unauthorized("Invalid login or password.");

        var result = _passwordHasher.VerifyHashedPassword(null, user.PasswordHash, loginDto.Password);
        if (result == PasswordVerificationResult.Failed)
            return Unauthorized("Invalid login or password.");

        var token = _tokenService.GenerateToken(user);
        return Ok(new { Token = token });
    }
}


