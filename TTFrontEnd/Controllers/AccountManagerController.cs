using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TTBackEnd.Shared;
using TTFrontEnd.Classes;
using TTFrontEnd.Models;
using TTFrontEnd.Models.DataContext;
//using TTFrontEnd.Models.SqlDataContext;

namespace TTFrontEnd.Controllers
{
    public class AccountManagerController : BaseController
    {
        private readonly HttpClient _httpClient;

        //private readonly TTContext _context;

        public AccountManagerController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("LoginClient");
        }

        // GET: AccountManager
        public async Task<IActionResult> Index()
        {

            //var rs = Utils.GetApiAsync(_httpClient, $"api/AccountsApi/GetAccounts?" + $"pageSize={10}&pageIndex={1}");

            //return View(await _context.Users.ToListAsync());
            return View();
        }

        public async Task<IActionResult> LoadUser([FromForm] PaggerSetting pageSetting)
        {
            var kk = pageSetting;
            try
            {
                //var searchDate = Request.Form["DateRange"].FirstOrDefault();

                //TransactionFilter transactionFilter = new TransactionFilter()
                //{
                //    PayMethod = Request.Form["PayMethod"].FirstOrDefault(),
                //    SellerWalletId = Request.Form["SellerWalletId"].FirstOrDefault(),
                //    SenpayId = Request.Form["SenpayId"].FirstOrDefault(),
                //    SenpayTranId = Request.Form["SenpayTranId"].FirstOrDefault(),
                //    TranId = Request.Form["TranId"].FirstOrDefault(),
                //    TranStatus = Request.Form["TranStatus"].FirstOrDefault(),
                //    TranType = Request.Form["TranType"].FirstOrDefault()
                //};

                ////TranId=100&SenpayTranId=10&SellerWalletId=10&SenpayId=5&TranStatus=1&TranType=4&PayMethod=7&FromDate=2019-01-10&ToDate=2019-05-02%2023%3A59%3A59
                //DateRange dateRange = Helpers.Helper.ParseDateRange(searchDate, '~');
                //dateRange.ToDate = dateRange.ToDate.AddDays(1).AddSeconds(-1);
                var apiUrl = $"/api/GetAccounts/";
                apiUrl = $"/api/AccountsApi/GetAccounts?";// + $"pageSize={1}&pageIndex={1}";// $"?TranId={transactionFilter.TranId}&SenpayTranId={transactionFilter.SenpayTranId}&SellerWalletId={transactionFilter.SellerWalletId}&SenpayId={transactionFilter.SenpayId}&TranStatus={transactionFilter.TranStatus}&TranType={transactionFilter.TranType}&PayMethod={transactionFilter.PayMethod}&FromDate={dateRange.FromDate}&ToDate={dateRange.ToDate}";
                Pagger<Users> pagger = new Pagger<Users>();
                return await pagger.GetDataFromApi(_httpClient,pageSetting, apiUrl);
            }
            catch (Exception ex)
            {
                // TODO : Add Error Log
                throw;
            }
        }


        //// GET: AccountManager/Details/5
        //public async Task<IActionResult> Details(string id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var users = await _context.Users
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (users == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(users);
        //}

        // GET: AccountManager/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AccountManager/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Email,PhoneNumber,FullName,Birthday,Gender,Address,Avatar,Status")] CreateUserModel users)
        {
            if (ModelState.IsValid)
            {
             
                //_context.Add(users);
                //await _context.SaveChangesAsync();

                //return RedirectToAction(nameof(Index));
            }
            return View(users);
        }

        //// GET: AccountManager/Edit/5
        //public async Task<IActionResult> Edit(string id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var users = await _context.Users.FindAsync(id);
        //    if (users == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(users);
        //}

        //// POST: AccountManager/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(string id, [Bind("Id,Email,PasswordHash,PhoneNumber,FullName,CreatedDate,IsLocked,Birthday,Gender,Address,Role,UserLevel,MerchantLevel,Avatar,LastLogin,Status")] Users users)
        //{
        //    if (id != users.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(users);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!UsersExists(users.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(users);
        //}

        //// GET: AccountManager/Delete/5
        //public async Task<IActionResult> Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var users = await _context.Users
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (users == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(users);
        //}

        //// POST: AccountManager/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(string id)
        //{
        //    var users = await _context.Users.FindAsync(id);
        //    _context.Users.Remove(users);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool UsersExists(string id)
        //{
        //    return _context.Users.Any(e => e.Id == id);
        //}
    }
}
