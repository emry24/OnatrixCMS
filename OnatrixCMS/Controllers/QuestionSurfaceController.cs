using Microsoft.AspNetCore.Mvc;
using OnatrixCMS.Models;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;

namespace OnatrixCMS.Controllers
{
    public class QuestionSurfaceController : SurfaceController
    {
        public QuestionSurfaceController(IUmbracoContextAccessor umbracoContextAccessor, IUmbracoDatabaseFactory databaseFactory, ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider) : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
        }

        public IActionResult HandleSubmit(QuestionFormModel form)
        {
            if (!ModelState.IsValid)
            {
                ViewData["name"] = form.Name;
                ViewData["email"] = form.Email;
                ViewData["message"] = form.Message;

                ViewData["error_name"] = string.IsNullOrEmpty(form.Name);
                ViewData["error_email"] = string.IsNullOrEmpty(form.Email);
                ViewData["error_message"] = string.IsNullOrEmpty(form.Message);

                return CurrentUmbracoPage();
            }

            TempData["success"] = "Form submitted successfully.";
            return RedirectToCurrentUmbracoPage();
        }
    }
}
