using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TimeFrameLib.Models;
using TimeFrameWebAPIClient;
using TimeFrameWebAPIClient.DTOs;


namespace TimeFrameWebSite.Controllers
{
    public class WorkItemsController : Controller
    {

        ITimeFrameApiClient _client;

        public WorkItemsController(ITimeFrameApiClient client)
        {
            _client = client;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            dynamic model = new ExpandoObject();
            model.WorkItems = await _client.GetWorkItemsByProjectAsync(79);
            model.Test = 202;
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> MyWorkItems()
        {
            dynamic model = new ExpandoObject();
            model.WorkItems = await _client.GetWorkItemsByUserIdAsync(GetUserId());
            return View(model);
        }

        [HttpGet("Search")]
        public async Task<IActionResult> MyWorkItems(string searchCriteria)
        {
            dynamic model = new ExpandoObject();
            ViewData["GetWorkItemDetails"] = searchCriteria;
            if (searchCriteria == null)
            {
                var workItems = await _client.GetWorkItemsByUserIdAsync(GetUserId());
                model.WorkItems = workItems;
            }
            else {
                var workItems = await _client.SearchUsersWorkItems(GetUserId(), searchCriteria);
                model.WorkItems = workItems;
            }
            return View(model);
        }

        public ActionResult Create([FromRoute] int id)
        {
            return View();
        }

        // POST: WorkItemsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Create([FromRoute] int id, WorkItemDto newWorkItem)
        {
            try
            {
                newWorkItem.ProjectId = id;
                newWorkItem.Version = "";
                newWorkItem.Id = await _client.CreateWorkItemAsync(newWorkItem);
                return RedirectToAction("Details", "Projects", new { Id = newWorkItem.ProjectId });
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }

        public async Task<ActionResult> Remove([FromRoute] int id)
        {
            try
            {
                var projectId = _client.GetWorkItemByIdAsync(id).Result.ProjectId;
                await _client.DeleteWorkItemAsync(id);
                return RedirectToAction("Details", "Projects", new { Id = projectId });
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            var workItem = await _client.GetWorkItemByIdAsync(id);
            return View(workItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(WorkItemDto newWorkItem)
        {
            if (await _client.UpdateWorkItemConcurrentlyAsync(newWorkItem))
            {
                return RedirectToAction("Details", "Projects", new { Id = newWorkItem.ProjectId });
            }
            else
            {
                ViewBag.ErrorMessage = "Error updating work item - refresh by clicking 'Refresh'";
                return View();
            }
        }

        public async Task<ActionResult> UnassignPerson([FromRoute] int id)
        {
            try
            {
                var projectId = _client.GetWorkItemByIdAsync(id).Result.ProjectId;
                await _client.DeleteWorkItemAsync(id);
                return RedirectToAction("Details", "Projects", new { Id = projectId });
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }

        public async Task<ActionResult> AssignPerson([FromRoute] int id, UserDto userToAssign)
        {
            try
            {   
                //UserDto user = new UserDto { Email = "viktor@orban.hu", Id = 588, FirstName = "Viktor", LastName = "Orbán" };
                await _client.AssignUserToWorkItemAsync(id, userToAssign);
                return RedirectToAction("Edit", "WorkItems", new { Id = id });
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }


        #region Helper methods
        //retrieves the logged in user's id using claims
        private int GetUserId()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string userIdValue = claims.Where(c => c.Type == "user_id").FirstOrDefault()?.Value;

            return int.Parse(userIdValue);
        }
        #endregion
    }
}
