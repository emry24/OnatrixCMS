using Microsoft.AspNetCore.Mvc;
using OnatrixCMS.Models;
using OnatrixCMS.Services;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;

namespace OnatrixCMS.Controllers
{
    public class ContactSurfaceController : SurfaceController
    {
        private readonly EmailService _emailService;
        public ContactSurfaceController(EmailService emailService, IUmbracoContextAccessor umbracoContextAccessor, IUmbracoDatabaseFactory databaseFactory, ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider) : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
            _emailService = emailService;
        }

        public async Task<IActionResult> HandleSubmit(ContactFormModel form)
        {
            if (!ModelState.IsValid)
            {
                ViewData["name"] = form.Name;
                ViewData["phone"] = form.Phone;
                ViewData["email"] = form.Email;
                ViewData["consulting"] = form.Consulting;

                ViewData["error_name"] = string.IsNullOrEmpty(form.Name);
                ViewData["error_phone"] = string.IsNullOrEmpty(form.Phone);
                ViewData["error_email"] = string.IsNullOrEmpty(form.Email);

                return CurrentUmbracoPage();
            }

            var result = await _emailService.SendEmailAsync(form.Email);

            if (result)
            {
                TempData["success"] = "Form submitted successfully";
            }
            else
            {
                TempData["error"] = "Failed to send email";
            }

            return RedirectToCurrentUmbracoPage();

            //TempData["success"] = "Form submitted successfully.";
            //return CurrentUmbracoPage();
        }
    }
}
