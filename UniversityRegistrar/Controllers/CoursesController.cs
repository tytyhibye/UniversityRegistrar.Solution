using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using UniversityRegistrar.Models;

namespace UniversityRegistrar.Controllers
{
  public class CoursesController : Controller{
    private readonly UniversityRegistrarContext _db;

    public CoursesController(UniversityRegistrarContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      List<Course> model = _db.Courses.ToList();
      return View(model);
    }

    public ActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public ActionResult Create(Course course)
    {
      _db.Courses.Add(course);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Details(int id)
    {
      var thisCourse = _db.Courses
      .Include(course => course.Students)
      .ThenInclude(join => join.Student)
      .FirstOrDefault(course => course.CourseId == id);
      return View(thisCourse);
    }

    public ActionResult Delete(int id)
    {
      var thisCourse = _db.Courses.FirstOrDefault(course => course.CourseId == id);
      return View(thisCourse);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      var thisCourse = _db.Courses.FirstOrDefault(course => course.CourseId == id);
      _db.Courses.Remove(thisCourse);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
    [HttpGet("/search")]
    public ActionResult Search(string search)
    {
      List<Course> model = _db.Courses.Include(courses => courses.Students).ToList();
      Course match = new Course();
      List<Course> matches = new List<Course>{};

      if (!string.IsNullOrEmpty(search))
      {
       foreach(Course course in model)
       {
         if (course.Name.ToLower().Contains(search))
         {
           matches.Add(course);
         }
       } 
      }
      return View(matches);
    }
  }
}