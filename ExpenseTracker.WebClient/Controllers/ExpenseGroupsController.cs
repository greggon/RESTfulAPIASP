﻿using System;
using ExpenseTracker.DTO;
using System.Collections.Generic;
using System.IdentityModel.Protocols.WSTrust;
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

            HttpResponseMessage response = await client.GetAsync("api/expensegroups?sort=expensegroupstatusid,title");

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
        public async Task<ActionResult> Create(ExpenseGroup expenseGroup)
        {
            try
            {
                var client = ExpenseTrackerHttpClient.GetClient();

                //an expensegroup is created with status "Open", for the current user.
                expenseGroup.ExpenseGroupStatusId = 1;
                expenseGroup.UserId = @"https://expensetrackeridsrv3/embedded_1";

                var serializedItemToCreate = JsonConvert.SerializeObject(expenseGroup);

                var response = await client.PostAsync("api/expensegroups",
                    new StringContent(serializedItemToCreate,
                        System.Text.Encoding.Unicode,
                        "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                    return Content("An error occurred.");

            }
            catch 
            {
                return Content("An error occurred.");
            }
            return View();
        }

        // GET: ExpenseGroups/Edit/5
 
        public async Task<ActionResult> Edit(int id)
        {
            var client = ExpenseTrackerHttpClient.GetClient();
            HttpResponseMessage response = await client.GetAsync("api/expensegroups/" + id);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<ExpenseGroup>(content);
                return View(model);
            }
            return Content("An error occurred.");
        }

        // POST: ExpenseGroups/Edit/5   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, ExpenseGroup expenseGroup)
        {
            try
            {
                var client = ExpenseTrackerHttpClient.GetClient();

                //serialize & PUT
                var serializeditemToUpdate = JsonConvert.SerializeObject(expenseGroup);

                var response = await client.PutAsync("api/expensegroups/" + id,
                    new StringContent(serializeditemToUpdate,
                        System.Text.Encoding.Unicode, "application/json"));
                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Index");
                else
                    return Content("An error has occurred.");

            }
            catch
            {
                return Content("An error has occurred.");
            }
            return View();
        }
         

        // POST: ExpenseGroups/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var client = ExpenseTrackerHttpClient.GetClient();
                var response = await client.DeleteAsync("api/expensegroups/" + id);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return Content("An error has occurred.");
                }
            }
            catch
            {
                return Content("An error has occurred.");
            }
            return View();
        }
    }
}
