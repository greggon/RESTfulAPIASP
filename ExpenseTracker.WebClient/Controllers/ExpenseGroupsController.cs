using ExpenseTracker.DTO;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using ExpenseTracker.WebClient.Helpers;
using ExpenseTracker.WebClient.Models;
using Newtonsoft.Json;

namespace ExpenseTracker.WebClient.Controllers
{
    public class ExpenseGroupsController : Controller
    {

        public async Task<ActionResult> Index()
        {

            var client = ExpenseTrackerHttpClient.GetClient();

            var model = new ExpenseGroupsViewModel();

            var egsResponse = await client.GetAsync("api/expensegroupstatusses");

            if (egsResponse.IsSuccessStatusCode)
            {
                string egsContent = await egsResponse.Content.ReadAsStringAsync();

                model.ExpenseGroupStatusses = JsonConvert.DeserializeObject<IEnumerable<ExpenseGroupStatus>>(egsContent);
            }
            else
            {
                return Content("An error occurred.");
            }

            HttpResponseMessage response = await client.GetAsync("api/expensegroups");

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var expenseGroups = JsonConvert.DeserializeObject<IEnumerable<ExpenseGroup>>(content);

                model.ExpenseGroups = expenseGroups;

            }
            else
            {
                return Content("An error occured.");
            }

            return View(model);

        }

 
        // GET: ExpenseGroups/Details/5
        public ActionResult Details(int id)
        {
            return Content("Not implemented yet.");
        }

        // GET: ExpenseGroups/Create
 
        public ActionResult Create()
        {
            return View();
        }

        // POST: ExpenseGroups/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ExpenseGroup expenseGroup)
        {
            return View();
        }

        // GET: ExpenseGroups/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ExpenseGroups/Edit/5   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ExpenseGroup expenseGroup)
        {
            return View();
        }
         

        // POST: ExpenseGroups/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
    }
}
