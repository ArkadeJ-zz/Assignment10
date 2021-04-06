using Assignment10.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Assignment10.Models.ViewModels;

namespace Assignment10.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private BowlingLeagueContext context { get; set; }

        public HomeController(ILogger<HomeController> logger, BowlingLeagueContext ctx)
        {
            _logger = logger;
            context = ctx;
        }

        [HttpGet]
        public IActionResult Index(long? teamid, string teamname, int pageNum = 1)
        {
            //dictates number of line items per page
            int pageSize = 5;
            //gets the team name to pass into the header when you select a team
            ViewData["Header"] = teamname;
            return View(new IndexViewModel
            {
                //gets the team ID from the parameters, and only returns the players who's team ID match that team
                Bowlers = context.Bowlers
                .Where(t => t.TeamId == teamid || teamid == null)
                //orders team by last name, and limits 5 entries per page
                .OrderBy(t => t.BowlerLastName)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToList(),

                PageNumInfo = new PageNumInfo
                {
                    ItemsPerPage = pageSize,
                    CurrentPage = pageNum,
                    //if no team, get full count, otherwise only count number from selected team
                    TotalNumItems = (teamid == null ? context.Bowlers.Count() :
                        context.Bowlers.Where(x => x.TeamId == teamid).Count())
                },

                TeamName = teamname
            });
                
                
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
