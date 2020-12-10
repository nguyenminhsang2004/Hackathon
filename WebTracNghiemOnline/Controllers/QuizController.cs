using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebTracNghiemOnline.Models;

namespace WebTracNghiemOnline.Controllers
{
    public class QuizController : Controller
    {
        // GET: AddQuiz_01_
        [HttpGet]
        public ActionResult AddQuiz()
        {
            Random rand = new Random();
            ViewBag.Key = rand.Next(100000, 999999);
            return View();
        }
        [HttpPost]
        public ActionResult AddQuiz(QuizViewModel quiz, IEnumerable<QuestionViewModel> modelQuestion, IEnumerable<AnswerViewModel> modelAnswer)
        {
            using (var _context = new TRACNGHIEM_ONLINEEntities())
            {
                Quiz q = new Quiz();
                List<Question> lstQuestion = new List<Question>();
                List<Answer> lstAnswer = new List<Answer>();
                var id_Quiz = _context.Quizs.ToList().Count();
                if (id_Quiz == 0)
                    q.ID_Quiz = 1;
                else
                    q.ID_Quiz = id_Quiz + 1;
                q.Title = quiz.Title;
                q.Keys = quiz.Keys;
                q.Time_Start = quiz.Time_Start;
                q.Time_Finish = quiz.Time_Finish;
                q.Time = quiz.Time;
                //q.Creator = quiz.Creator;
                q.Creator = 1;
                q.Allow_Attemp = 1;
                _context.Quizs.Add(q);
                _context.SaveChanges();
                //Question
                foreach (var item in modelQuestion)
                {
                    if (item.ID_Question != 0)
                    {
                        Question i = new Question();
                        i.ID_Quiz = q.ID_Quiz;
                        i.ID_Question = item.ID_Question;
                        i.Content_of_Question = item.Content_of_Question;
                        i.Type_of_Question = item.Type_of_Question;
                        i.Score = item.Score;
                        lstQuestion.Add(i);
                    }
                }
                _context.Questions.AddRange(lstQuestion);
                _context.SaveChanges();
                //Answer
                foreach (var item in modelAnswer) 
                {
                    Answer a = new Answer();
                    a.ID_Quiz = q.ID_Quiz;
                    a.ID_Question = item.ID;
                    a.Answer1 = item.Answer;
                    a.Correct_Answer = item.Correct_Answer == true ? true : false;
                    lstAnswer.Add(a);
                }
                _context.Answers.AddRange(lstAnswer);
                _context.SaveChanges();
            }
            return View();
        }
    }
}