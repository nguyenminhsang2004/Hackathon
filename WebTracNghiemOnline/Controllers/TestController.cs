using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebTracNghiemOnline.Models;
using System.Web.UI.WebControls;
using System.Timers;

namespace WebTracNghiemOnline.Controllers
{
    public class TestController : Controller
    {
        List<bool> lstSubmit = new List<bool>();
        int id = 0;
        // GET: Test
        [HttpGet]
        public ActionResult Test(int? id)
        {
            using (var _context = new TRACNGHIEM_ONLINEEntities())
            {
                var lstQuestion = _context.Questions.Where(n => n.ID_Quiz == id).ToList();
                var lstAnswer = _context.Answers.Where(n => n.ID_Quiz == id).ToList();
                var title= _context.Quizs.SingleOrDefault(n => n.ID_Quiz == id).Title;
                ViewBag.Answers = lstAnswer;
                ViewBag.Id = id;
                ViewBag.TitleQuiz = title;
                ViewBag.Thoigianlambai = _context.Quizs.SingleOrDefault(n => n.ID_Quiz == id).Time.Value.TotalSeconds.ToString();
                return View(lstQuestion);
            }

        }
        public ActionResult Check(List<bool> a,int ID_Quiz)
        {
            //List<int> lstq = new List<int>();
            //List<int> lstques = new List<int>();
            //int count = 1;
            //int question = 1;
            //int flag = 1;
            //a.ForEach(x =>
            //{
            //if (x == true && flag == 1)
            //{
            //    flag = 0;
            //    lstq.Add(question);
            //}
            //if (count % 4 == 0)
            //{
            //    question++;
            //    flag = 1;
            //}
            //count++;
            //});

            ViewBag.ID_Quiz = ID_Quiz;
            return View(a);
        }
        public ActionResult Submit(List<bool> a, int ID_Quiz)
        {
            List<Question> lstQuestion = new List<Question>();
            List<Answer> lstAnswer = new List<Answer>();
            List<AnswerViewModel> check = new List<AnswerViewModel>();
            double score = 0;
            using (var _context = new TRACNGHIEM_ONLINEEntities())
            {
                lstQuestion.AddRange(_context.Questions.ToList().Where(n => n.ID_Quiz == ID_Quiz));
                lstAnswer.AddRange(_context.Answers.ToList().Where(n => n.ID_Quiz == ID_Quiz));
            }
            for (int i = 0; i < lstAnswer.Count(); i++)
            {
                if (lstAnswer[i].Correct_Answer == a[i] && lstAnswer[i].Correct_Answer == true)
                {
                    score += (double)(lstQuestion.SingleOrDefault(n => n.ID_Question == lstAnswer[i].ID_Question)).Score;
                }
            }
            return RedirectToAction("Mesage", "Mesage", new { Score = score });
        }
    }
}