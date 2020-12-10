using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebTracNghiemOnline.Models;

namespace WebTracNghiemOnline.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        [HttpGet]
        public ActionResult Test(int? id)
        {
            using (var _context = new TRACNGHIEM_ONLINEEntities())
            {
                var lstQuestion = _context.Questions.ToList().Where(n => n.ID_Quiz == id);
                var lstAnswer = _context.Answers.ToList().Where(n => n.ID_Quiz == id);
                ViewBag.Answers = lstAnswer;
                ViewBag.Id = id;
                return View(lstQuestion);
            }

        }
        public ActionResult Check(IEnumerable<AnswerViewModel> a,int ID_Quiz)
        {
            List<Question> lstQuestion = new List<Question>();
            List<Answer> lstAnswer = new List<Answer>();
            List<AnswerViewModel> check = new List<AnswerViewModel>();
            check.AddRange(a);
            double score = 0;
            using (var _context = new TRACNGHIEM_ONLINEEntities())
            {
                lstQuestion.AddRange(_context.Questions.ToList().Where(n => n.ID_Quiz == ID_Quiz));
                lstAnswer.AddRange(_context.Answers.ToList().Where(n => n.ID_Quiz == ID_Quiz));
            }
            for (int i = 0; i < lstAnswer.Count(); i++)
            {
                if (lstAnswer[i].Correct_Answer == check[i].Correct_Answer && lstAnswer[i].Correct_Answer == true)
                {
                    score += (double)(lstQuestion.SingleOrDefault(n => n.ID_Question == lstAnswer[i].ID_Question)).Score;
                }
            }
            return RedirectToAction("Mesage","Mesage",new { Score=score});
        }
    }
}