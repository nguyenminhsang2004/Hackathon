using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebTracNghiemOnline.Models;

namespace WebTracNghiemOnline.Controllers
{
    public class QuanLyController : Controller
    {
        // GET: QuanLy
        public ActionResult Quiz()
        {
            IEnumerable<Quiz> lstQuiz;
            using (var _context = new TRACNGHIEM_ONLINEEntities()) 
            {
                lstQuiz = _context.Quizs.ToList();
            }
            return View(lstQuiz);
        }
        public ActionResult EditQuiz(int? id)
        {
            using (var _context = new TRACNGHIEM_ONLINEEntities())
            {
                var q = _context.Quizs.SingleOrDefault(n => n.ID_Quiz == id);
                return View(q);
            }            
        }
        public ActionResult UpdateQuiz(Quiz q)
        {
            Quiz update = new Quiz();
            using(var _context = new TRACNGHIEM_ONLINEEntities())
            {
                update = q;
                _context.Quizs.AddOrUpdate(update);
                _context.SaveChanges();
            }
            return RedirectToAction("Quiz");

        }
        public ActionResult DeleteQuiz(int? id)
        {
            using (var _context = new TRACNGHIEM_ONLINEEntities())
            {
                _context.Quizs.Remove(_context.Quizs.SingleOrDefault(n => n.ID_Quiz == id));
                _context.SaveChanges();
            }
            return RedirectToAction("Quiz");
        }
        public ActionResult Question(int? id)
        {
            IEnumerable<Question> lstQuestion;
            IEnumerable<Answer> lstAnswer;
            using (var _context = new TRACNGHIEM_ONLINEEntities())
            {
                lstQuestion = _context.Questions.ToList().Where(n => n.ID_Quiz == id);
                lstAnswer = _context.Answers.ToList().Where(n => n.ID_Quiz == id);
            }
            ViewBag.Answers = lstAnswer;
            return View(lstQuestion);
        }
        public ActionResult EditQuestion(int? id)
        {
            using (var _context = new TRACNGHIEM_ONLINEEntities())
            {
                var q = _context.Questions.SingleOrDefault(n => n.ID_Question == id);
                var a = _context.Answers.ToList().Where(n => n.ID_Question == id);
                ViewBag.Answers = a;
                ViewBag.Id = q.ID_Quiz;
                return View(q) ;
            }
        }
        public ActionResult UpdateQuestion(Question q, IEnumerable<AnswerViewModel> a)
        {
            Question qupdate = new Question();
            List<Answer> aupdate = new List<Answer>();
            foreach(var item in a)
            {
                Answer i = new Answer();
                i.ID_Quiz = q.ID_Quiz;
                i.ID_Question = item.ID;
                i.Answer1 = item.Answer;
                i.Correct_Answer = item.Correct_Answer == true ? true : false;
                aupdate.Add(i);
            }
            using (var _context = new TRACNGHIEM_ONLINEEntities())
            {
                qupdate = q;
                _context.Questions.AddOrUpdate(qupdate);
                _context.Answers.RemoveRange(_context.Answers.ToList().Where(n => n.ID_Quiz == q.ID_Quiz && n.ID_Question == q.ID_Question));
                _context.Answers.AddRange(aupdate);
                _context.SaveChanges();
            }
            return RedirectToAction("Question", new { id = q.ID_Quiz });
        }
        public ActionResult DeleteQuestion(int? id)
        {
            using (var _context = new TRACNGHIEM_ONLINEEntities())
            {
                _context.Questions.Remove(_context.Questions.SingleOrDefault(n => n.ID_Question == id));
                _context.SaveChanges();
            }
            return RedirectToAction("Quiz");
        }
    }
}