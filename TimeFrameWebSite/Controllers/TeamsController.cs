using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TimeFrameWebAPIClient;
using TimeFrameWebAPIClient.DTOs;

namespace TimeFrameWebSite.Controllers
{
    public class TeamsController : Controller
    {
        ITimeFrameApiClient _client;

        public TeamsController(ITimeFrameApiClient client)
        {
            _client = client;
        }

        // GET: TeamsController
        public async Task<ActionResult> Index()
        {
            int userId = GetUserId();
            try
            {
                var teams = await _client.GetTeamsFromUserIdAsync(userId);
                dynamic model = new ExpandoObject();
                model.Teams = teams;
                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }

        }

        // GET: TeamsController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var projects = await _client.GetProjectsByTeamIdAsync(id);
                var team = await _client.GetTeamByIdAsync(id);
                dynamic model = new ExpandoObject();
                model.Projects = projects;
                model.Team = team;
                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }

        [HttpPost("{id}")]
        public async Task<ActionResult> AddMember(string email, [FromRoute] int id)
        {

            int currentUserId = GetUserId();

            try
            {
                var user = await _client.GetUserByEmailAsync(email);
                var team = await _client.GetTeamByIdAsync(id);
                if (user != null && user.Id != currentUserId)
                {
                    await _client.AddUserToTeamAsync(user, team);
                    return RedirectToAction(nameof(Details), new { Id = team.Id });
                } else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }


        [HttpPost("{id}/remove")]
        public async Task<ActionResult> RemoveMember(string email, [FromRoute] int id)
        {

            int currentUserId = GetUserId();

            try
            {
                var user = await _client.GetUserByEmailAsync(email);
                var team = await _client.GetTeamByIdAsync(id);
                if (user != null && user.Id != currentUserId)
                {
                    await _client.RemoveUserFromTeamAsync(user, team);
                    return RedirectToAction(nameof(Details), new { Id = team.Id });
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }

        // GET: TeamsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TeamsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Create(TeamDto newTeam)
        {
            UserDto user = await _client.GetUserByIdAsync(GetUserId());

            newTeam.TeamMembers = new List<UserDto>();
            newTeam.TeamMembers.Add(user);

            try
            {
                newTeam.Id = await _client.CreateTeamAsync(newTeam);
                TempData["Message"] = "Team created";
                return RedirectToAction(nameof(Details), new { Id = newTeam.Id });
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }


        // GET: TeamsController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                var team = await _client.GetTeamByIdAsync(id);
                dynamic model = new ExpandoObject();
                model.Team = team;
                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }

        // POST: TeamsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        // POST: TeamsController/Delete/5
        public async Task<ActionResult> Delete1([FromRoute] int id)
        {
            try
            {
                await _client.DeleteTeamAsync(id);
                TempData["Message"] = "Team deleted";
                return RedirectToAction(nameof(Index));
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
