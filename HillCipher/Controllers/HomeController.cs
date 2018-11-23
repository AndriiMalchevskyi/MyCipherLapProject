using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.IO;

namespace HillCipher.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult HillCipher()
        {
            return View();
        }

        public ActionResult XORCipher()
        {
            return View();
        }

        public ActionResult BBSGenerator()
        {
            return View();
        }

        public string EncryptHill(string text, string keyWord)
        {
            return HillCipherClass.Encrypt(text, keyWord);
        }

        public string DecryptHill(string text, string keyWord)
        {
            return HillCipherClass.Decrypt(text, keyWord);
        }


        public string EncryptXOR(string text, string keyWord)
        {
            return XORCipherClass.Encrypt(text, keyWord);
        }

        public string DecryptXOR(string text, string keyWord)
        {
            return XORCipherClass.Decrypt(text, keyWord);
        }

        public string EncryptBBS(string text)
        {
            return BBS.Encrypt(text);
        }

        public string DecryptBBS(string text, int n)
        {
            return BBS.Decrypt(text, n);
        }

        [HttpPost]
        public JsonResult UploadFile()
        {
            foreach (string file in Request.Files)
            {
                var upload = Request.Files[file];
                if (upload != null)
                {
                    string fileName = System.IO.Path.GetFileName(upload.FileName);
                    DirectoryInfo dirInfo = new DirectoryInfo(Server.MapPath("~/TempFiles/" + fileName));
                    dirInfo.Create();
                    string fullFileName = dirInfo.FullName + "\\" + fileName;
                    upload.SaveAs(fullFileName);
                }
            }
            return Json("Ok");
        }


        public FileResult UploadFileAndEncryptHill(string  fileName, string keyWord)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(Server.MapPath("~/TempFiles/" + fileName));
            string fullFileName = dirInfo.FullName + "\\" + fileName;
            string fileData = System.IO.File.ReadAllText(fullFileName);
            System.IO.File.WriteAllText(fullFileName, EncryptHill(fileData, keyWord));
            return File(fullFileName, ".txt", fileName);
        }

        public FileResult UploadFileAndDecryptHill(string fileName, string keyWord)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(Server.MapPath("~/TempFiles/" + fileName));
            string fullFileName = dirInfo.FullName + "\\" + fileName;
            string fileData = System.IO.File.ReadAllText(fullFileName);
            System.IO.File.WriteAllText(fullFileName, DecryptHill(fileData, keyWord));
            return File(fullFileName, ".txt", fileName);
        }

        public FileResult UploadFileAndEncryptXOR(string fileName, string keyWord)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(Server.MapPath("~/TempFiles/" + fileName));
            string fullFileName = dirInfo.FullName + "\\" + fileName;
            string fileData = System.IO.File.ReadAllText(fullFileName);
            System.IO.File.WriteAllText(fullFileName, EncryptXOR(fileData, keyWord));
            return File(fullFileName, ".txt", fileName);
        }

        public FileResult UploadFileAndDecryptXOR(string fileName, string keyWord)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(Server.MapPath("~/TempFiles/" + fileName));
            string fullFileName = dirInfo.FullName + "\\" + fileName;
            string fileData = System.IO.File.ReadAllText(fullFileName);
            System.IO.File.WriteAllText(fullFileName, DecryptXOR(fileData, keyWord));
            return File(fullFileName, ".txt", fileName);
        }

    }
}