using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TimeFrameLib.Models;
using TimeFrameWebAPIClient;
using TimeFrameWebAPIClient.DTOs;

namespace WebSite.Controllers
{
    public class ProjectsController : Controller
    {
        ITimeFrameApiClient _client;

        public ProjectsController(ITimeFrameApiClient client)
        {
            _client = client;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var projects = await _client.GetProjectsByTeamIdAsync(209);
            dynamic model = new ExpandoObject();
            model.Projects = projects;
            return View(model);
        }

        public async Task<ActionResult> Details([FromRoute] int id)
        {
            try
            {
                var project = await _client.GetProjectByIdAsync(id);
                var workItems = await _client.GetWorkItemsByProjectAsync(id);
                foreach (var workItem in workItems)
                {
                    workItem.PeopleAssigned = (await _client.GetUsersByWorkItemAsync(workItem.Id)).ToList();
                }
                dynamic model = new ExpandoObject();
                model.Project = project;
                model.WorkItems = workItems;
                return View(model);
            }
            catch(Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }

        public ActionResult Create([FromRoute] int id)
        {
            ProjectDto projectDto = new ProjectDto { TeamID = id };
            return View(projectDto);
        }
        [ValidateAntiForgeryToken]
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Create([FromRoute] int id, [FromForm] ProjectDto newProject)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string userIdValue = claims.Where(c => c.Type == "user_id").FirstOrDefault()?.Value;

            //newProject.UserId = int.Parse(userIdValue);

            try
            {
                newProject.TeamID = id;
                int projectId = await _client.CreateProjectAsync(newProject);
                newProject.ID = projectId;
                TempData["Message"] = "The project has been created";
                return RedirectToAction("Details", new { newProject.ID });
            }

            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }

        public ActionResult Overview()
        {
            return View();
        }

        private int GetUserId()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string userIdValue = claims.Where(c => c.Type == "user_id").FirstOrDefault()?.Value;

            return int.Parse(userIdValue);
        }
    }
}
