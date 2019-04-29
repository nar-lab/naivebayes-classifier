using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NBayes.Models;

namespace NBayes.Controllers
{
    public class HomeController : Controller
    {
        NbContext ctx = new NbContext();
        public ActionResult Index()
        {

            return View();
        }

        public JsonResult GetAllProbabilities()
        {
            var trainingData = ctx.People.ToList();
            var probabilites = DataUtils.GetFeatureProb(trainingData);
            return Json(probabilites, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllPeople()
        {
            var trainingData = ctx.People.ToList();
            return Json(trainingData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult _TestModel()
        {
            //var trainingData = DataUtils.SeedTrainingData();
            var trainingData = ctx.People.ToList();
            var eduGroup = trainingData.GroupBy(x => x.Education).Select(grp => grp.First());
            ViewBag.ddlEducation = new SelectList(eduGroup,VariableNames.Education.ToString(),VariableNames.Education.ToString());

            //nihahaha, funny variable name ha?
            var groupSex = trainingData.GroupBy(x => x.Sex).Select(grp => grp.First());
            ViewBag.ddlSex = new SelectList(groupSex, VariableNames.Sex.ToString(),VariableNames.Sex.ToString());

            var ageGroup = trainingData.GroupBy(x => x.Age).Select(grp => grp.First());
            ViewBag.ddlAge = new SelectList(ageGroup, VariableNames.Age.ToString(),VariableNames.Age.ToString());

            var acceptenceGroup = trainingData.GroupBy(x => x.Acceptence).Select(grp => grp.First());
            ViewBag.ddlAcceptence = new SelectList(acceptenceGroup, VariableNames.Acceptence.ToString(), VariableNames.Acceptence.ToString());

            ViewBag.Result = TargetLevels.None.ToString();
            return PartialView();
        }

        [HttpPost]
        public JsonResult AddToModel(PersonVM person)
        {

            //var trainingData = DataUtils.SeedTrainingData();
            var trainingData = ctx.People.ToList();
            var probmatrix = DataUtils.GetFeatureProb(trainingData);
            
            var eduObj = probmatrix.FirstOrDefault(x => x.VarName == VariableNames.Education.ToString() && x.LevelName == person.EducationVM);
            var ageObj = probmatrix.FirstOrDefault(x => x.VarName == VariableNames.Age.ToString() && x.LevelName == person.AgeVM);
            var sexObj = probmatrix.FirstOrDefault(x => x.VarName == VariableNames.Sex.ToString() && x.LevelName == person.SexVM);

            var yProb = eduObj.LevelYAdjProb * ageObj.LevelYAdjProb * sexObj.LevelYAdjProb;
            var nProb = eduObj.LevelNAdjProb * ageObj.LevelNAdjProb * sexObj.LevelNAdjProb;

            var acceptence = yProb > nProb ? TargetLevels.Yes.ToString() : TargetLevels.No.ToString();

            if (person.PersistData)
            {
                //trainingData.Add(new Person() {Education = education,Age=age,Sex=sex,Acceptence = usePred ? acceptence : ViewBag.Result});
                var newOne = new Person()
                             {
                                 Education = person.EducationVM,
                                 Age = person.AgeVM,
                                 Sex = person.SexVM,
                                 Acceptence = person.UsePrediction ? acceptence : person.AcceptenceVM
                };
                ctx.People.Add(newOne);
                ctx.SaveChanges();
            }
            return Json(person.UsePrediction ? acceptence : person.AcceptenceVM);
        }
    }
}