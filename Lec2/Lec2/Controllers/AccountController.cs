using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Lec2.ViewModels;
using Lec2.Models;

namespace Lec2.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        public AccountController(UserManager<ApplicationUser> uManager,
            SignInManager<ApplicationUser> sManager,
            RoleManager<IdentityRole> roleManager)
        {
            userManager = uManager;
            signInManager = sManager;
            this.roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            RegisterViewModel vm = new RegisterViewModel();
            return View(vm);
        }
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Username,
                    Email = model.Email,
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    Gender=model.Gender
                };
                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("index", "home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("index", "home");
                }
                ModelState.AddModelError(string.Empty, "Invalid Username or Password");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult AddRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.RoleName,
                };
                IdentityResult result = await
                roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRole");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("",
                                  error.Description);
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult ListRole()
        {
            var roles = roleManager.Roles.ToList();
            var vm = new List<ListRoleViewModel>();
            roles.ForEach(item => vm.Add(
                new ListRoleViewModel()
                {
                    Id = item.Id,
                    RoleName = item.Name
                }));
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> AssignRole(string roleId)
        {
            ViewBag.roleId = roleId;
            var role = await roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with id: { roleId} not found";
                return View("NotFound");
            }
            var model = new List<RoleUserViewModel>();
            foreach (var user in userManager.Users)
            {
                var roleUserViewModel = new RoleUserViewModel
                {
                    UserId = user.Id,
                    Username = user.UserName
                };
                if (await userManager.IsInRoleAsync(user,
                                                role.Name))
                {
                    roleUserViewModel.IsSelected = true;
                }
                else
                {
                    roleUserViewModel.IsSelected = false;
                }
                model.Add(roleUserViewModel);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AssignRole(List<RoleUserViewModel> model, string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with id: {roleId} not found";
                return View("NotFound");
            }
            for (int i = 0; i < model.Count(); i++)
            {
                var user = await userManager.FindByIdAsync(model[i].UserId);
                IdentityResult result = null;
                if (model[i].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsSelected && (await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }
                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                    {
                        continue;
                    }
                    else
                    {
                        return RedirectToAction("index", "home");
                    }
                }
            }
            return RedirectToAction("index", "home");
        }



        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }

    }
}
