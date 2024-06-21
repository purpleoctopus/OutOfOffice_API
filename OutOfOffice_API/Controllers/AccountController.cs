using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OutOfOffice_API.Models;
using OutOfOffice_API.Models.Domain;
using OutOfOffice_API.Repositories.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IEmployeeRepository repository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;

    public AccountController(IEmployeeRepository repository, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
    {
        this.repository = repository;
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        checkBaseAccounts();
    }
    [HttpPost("create")]
    
    public async Task<IActionResult> CreateAccount([FromBody] RegisterModel model)
    {
        Guid id = Guid.NewGuid();

        var user = new ApplicationUser
        {
            UserName = model.Username,
            Email = model.Email,
            EmployeeID = id
        };

        Guid guid; 
        Guid.TryParse(model.PeoplePartnerId, out guid);

        Employee employee = new Employee()
        {
            Id = id,
            FullName = model.FullName,
            Subdivizion = model.Subdivizion,
            Position = model.Position,
            Status = model.Status,
            PeoplePartnerId = guid,
            OOO_balance = model.OOO_balance,
            Photo = model.Photo
        };

        await repository.CreateAsync(employee);

        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            await repository.DeleteAsync(employee);
            return BadRequest(result.Errors);
        }
        foreach (var userRole in model.Roles)
        {
            await _userManager.AddToRoleAsync(user, userRole);
        }

        return Ok();
    }
    [HttpPost("register")]
    [Authorize(Policy = "RequireEmployeeRole")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        Guid id = Guid.NewGuid();

        var user = new ApplicationUser
        {
            UserName = model.Username,
            Email = model.Email,
            EmployeeID = id
        };
        Guid guid;
        Guid.TryParse(model.PeoplePartnerId, out guid);
        Employee employee = new Employee()
        {
            Id = id,
            FullName = model.FullName,
            Subdivizion = model.Subdivizion,
            Position = model.Position,
            Status = model.Status,
            PeoplePartnerId = guid,
            OOO_balance = model.OOO_balance,
            Photo = model.Photo
        };

        await repository.CreateAsync(employee);

        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            return BadRequest("Username alredy exists");
        }

        await _userManager.AddToRoleAsync(user, "employee");

        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var user = await _userManager.FindByNameAsync(model.Username);
        if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return Ok(new
            {
                user.UserName,
                roles = userRoles,
                employeeid = user.EmployeeID,
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }
        return Unauthorized();
    }
    private async void checkBaseAccounts()
    {
        var result = await _userManager.Users.FirstAsync(x => x.UserName == "admin");
        if (result == null)
        {
            RegisterModel registerModel = new RegisterModel()
            {
                Username = "admin",
                Password = "Junior123?",
                Email = "denysostapuk7@gmail.com",
                Roles = ["admin", "employee", "hr manager", "pm"],
                FullName = "Denys Ostapiuk",
                Subdivizion = Subdivizion.IT,
                Position  = Position.ProjectManager,
                Status = Status.Active,
                PeoplePartnerId = null,
                OOO_balance = 20,
                Photo = null
};
            await this.CreateAccount(registerModel);
        }
    }
}