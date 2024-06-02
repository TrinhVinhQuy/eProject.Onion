using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Owl.reCAPTCHA;
using Owl.reCAPTCHA.v2;
using System.Security.Claims;
using WebASPMvcCore.Application.Abstracts;
using WebASPMvcCore.Application.DTOs.ViewModels;
using WebASPMvcCore.Domain.Entities;
using WebASPMvcCore.UI.Models;

namespace WebASPMvcCore.UI.Controllers
{
    public class LoginController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IreCAPTCHASiteVerifyV2 _siteVerify;
        private readonly IUserStore<ApplicationUser> _userStore;
        public LoginController(IAuthenticationService authenticationService,
            SignInManager<ApplicationUser> signInManager,
            IUserStore<ApplicationUser> userStore,
            UserManager<ApplicationUser> userManager,
            IreCAPTCHASiteVerifyV2 siteVerify)
        {
            _authenticationService = authenticationService;
            _signInManager = signInManager;
            _userStore = userStore;
            _userManager = userManager;
            _siteVerify = siteVerify;
        }
        public IActionResult Index(string returnUrl = null)
        {
            var model = new LoginModel { ReturnUrl = returnUrl };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Index(LoginModel loginModel)
        {
            if (string.IsNullOrEmpty(loginModel.Captcha))
            {
                ModelState.AddModelError("error", "Invalid captcha");
                return View(loginModel);
            }

            var response = await _siteVerify.Verify(new reCAPTCHASiteVerifyRequest
            {
                Response = loginModel.Captcha,
                RemoteIp = HttpContext.Connection.RemoteIpAddress.ToString()
            });

            if (!response.Success)
            {
                ModelState.AddModelError("error", "Invalid captcha");
                return View(loginModel);
            }

            if (ModelState.IsValid)
            {
                var result = await _authenticationService.CheckLogin(loginModel.Username, loginModel.Password, hasRemmeber: false);

                if (result.Status)
                {
                    if (!string.IsNullOrEmpty(loginModel.ReturnUrl) && Url.IsLocalUrl(loginModel.ReturnUrl))
                    {
                        return Redirect(loginModel.ReturnUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("error", result.Message);
                }
            }

            return View(loginModel);
        }
        public IActionResult External(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = $"/login/callbackexternal?handler=Callback&returnUrl={returnUrl}&remoteError=";
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        [HttpGet]
        public async Task<IActionResult> CallbackExternal(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (remoteError != null)
            {
                //ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                //ErrorMessage = "Error loading external login information.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }
            if (result.IsLockedOut)
            {
                return RedirectToPage("./Lockout");
            }
            else
            {
                string email = string.Empty;

                if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
                {
                    email = info.Principal.FindFirstValue(ClaimTypes.Email);
                }

                var md = new ExternalLoginModel
                {
                    Provider = info.ProviderDisplayName,
                    Email = email
                };

                return View(md);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmationExternal(ExternalLoginModel externalLoginModel)
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToPage("./Login", new { ReturnUrl = externalLoginModel.ReturnUrl });
            }

            if (ModelState.IsValid)
            {
                var user = CreateUser();
                user.IsActive = true;
                user.Fullname = info.Principal.Identities.First().Name;
                user.Email = externalLoginModel.Email;
                user.EmailConfirmed = true;
                user.NormalizedEmail = externalLoginModel.Email.ToUpper();

                await _userStore.SetUserNameAsync(user, externalLoginModel.Email, CancellationToken.None);

                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false, info.LoginProvider);
                        return RedirectToAction("", "Home");
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return RedirectToAction("", "Home");
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the external login page in /Areas/Identity/Pages/Account/ExternalLogin.cshtml");
            }
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Login");
        }
    }
}
