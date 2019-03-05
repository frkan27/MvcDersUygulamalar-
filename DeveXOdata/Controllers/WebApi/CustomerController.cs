using DeveXOdata.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace DeveXOdata.Controllers.WebApi
{
    public class CustomerController : ApiController
    {
        // GET: Customer
       public IHttpActionResult GetAll()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return Ok(new
            {
                success = true,
                data = db.Customers.ToList()
            });
        }
    }
}