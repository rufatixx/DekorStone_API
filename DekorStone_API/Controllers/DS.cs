using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DekorStone_API.Models.Db;
using DekorStone_API.Models.Structs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DekorStone_API.Controllers
{
    [Route("api/[controller]")]
    public class DS : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        public IConfiguration Configuration;
        //Communications communications;
        public DS(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            Configuration = configuration;

            _hostingEnvironment = hostingEnvironment;
            //communications = new Communications(Configuration, _hostingEnvironment);
        }
        
        [HttpPost]
        [Route("user/signIn")]
        public ActionResult<LogInStruct> LogIn(long phone,string pass)
        {
            DbSelect select = new DbSelect(Configuration, _hostingEnvironment);
            return select.LogIn(phone, pass);

        }
        [HttpPost]
        [Route("get/models")]
        public ActionResult<ResponseStruct<ProductModel>> GetModels(string userToken,string requestToken)
        {
            DbSelect select = new DbSelect(Configuration, _hostingEnvironment);
            return select.GetModels(userToken,requestToken);

        }
        [HttpPost]
        [Route("user/add/order")]
        public ActionResult<ResponseStruct<int>> AddOrder(string userToken, string requestToken,
            string clientFullName, long clientPhone, double kvm, int modelID, double price, double payment,string endDate, string note)
        {
            DbInsert insert = new DbInsert(Configuration, _hostingEnvironment);
            return insert.InsertOrder(userToken,requestToken,clientFullName,clientPhone,kvm
               ,modelID,price,payment,note,endDate);

        }
        [HttpPost]
        [Route("user/get/daily/sum")]
        public ActionResult<ResponseStruct<DailySumStruct>> GetDailySum(string userToken, string requestToken)
        {
            DbSelect select = new DbSelect(Configuration, _hostingEnvironment);
            return select.GetDailySum(userToken,requestToken);

        }

    }
}
