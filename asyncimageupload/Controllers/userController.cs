using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace asyncimageupload.Controllers
{
    public class userController : Controller
    {
        //
        // GET: /user/

        public ActionResult Index()
        {
            veeruEntities ve = new veeruEntities();
            ViewBag.UsersList = ve.users.ToList();
            return View();
        }
        

        public async Task<ActionResult> Upload(user user)
        {
            veeruEntities ve = new veeruEntities();
            if(ModelState.IsValid)
            {
                ve.users.Add(user);
                if (Request.Files.Count > 0)
                {
                    Stream fis = Request.Files[0].InputStream;
                    byte[] data = new Byte[Request.Files[0].ContentLength];
                    await fis.ReadAsync(data, 0, data.Length);
                    user.image = data;
                }
                await ve.SaveChangesAsync();
            }
            ViewBag.UsersList = ve.users.ToList();
            return View("Index", user);
        }

        [HttpGet]
        public FileStreamResult GetImage(int id)
        {
            veeruEntities ve = new veeruEntities();
            user founduser =ve.users.Find(id);
            if(founduser!=null)
            {
                byte[] data = founduser.image;
                System.IO.MemoryStream ms = new MemoryStream(data);
                FileStreamResult res = new FileStreamResult(ms, "image/jpg");
                return res;
            }
            return null;
        }

    }
}
