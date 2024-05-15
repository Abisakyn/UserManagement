using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Authetication;
using UserManagement.DTO;
using UserManagement.ServiceContract;

namespace UserManagement.Controllers
{
    [AllowAnonymous]
    //[ApiVersion("1.0")]
    public class AccountController : CustomControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IJwtService _jwtService;


        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager, IJwtService jwtService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _jwtService = jwtService;
        }
        [HttpPost]

        public async Task<ActionResult<AppUser>>PostRegister(RegisterDTO registerDTO)
        {
            //validation
            if(ModelState.IsValid == false) 
            {
               string errrorMessage= string.Join("",ModelState.Values.SelectMany(v=>v.Errors).Select(e=>e.ErrorMessage));
                return Problem(errrorMessage);
            }

            //create user
            AppUser user = new AppUser()
            {
                Email = registerDTO.Email,
                Name = registerDTO.Name,
                PhoneNumber = registerDTO.PhoneNumber,
                IdNumber = registerDTO.IdNumber,
                UserName =registerDTO.Email
            };
            IdentityResult result =await _userManager.CreateAsync(user,registerDTO.Password!);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user,isPersistent: false);

               var authenticationResponse= _jwtService.CreateJwtToken(user);

                return Ok(authenticationResponse);
            }
            else
            {
                string errorMessage =string.Join(" ",result.Errors.Select(e=>e.Description));
                return Problem(errorMessage);
            }
        }

        [HttpGet]
        public async Task<ActionResult>IsEmailAlreadyRegister(string emaail)
        {
            AppUser? user =await _userManager.FindByEmailAsync(emaail);

            if (user == null)
            {
                return Ok(true);
            }
            else
            {
                return Ok(false);
            }
        }

        [HttpPost("Login")]
        public async Task<ActionResult<AppUser>>PostLogin(LoginDTO loginDTO)
        {
            if (ModelState.IsValid == false)
            {
                string errrorMessage = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return Problem(errrorMessage);
            }

            var result=await _signInManager.PasswordSignInAsync(loginDTO.Email!, loginDTO.Password!, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                AppUser? user = await
                    _userManager.FindByEmailAsync(loginDTO.Email);
                if (user == null)
                {
                    return NoContent();
                }
                await _signInManager.SignInAsync(user, isPersistent: false);

                var authenticationResponse = _jwtService.CreateJwtToken(user);

                return Ok(authenticationResponse);

            }
            else
            {
                return Problem("Invalid email or Password");
            }
        }

    }
}
