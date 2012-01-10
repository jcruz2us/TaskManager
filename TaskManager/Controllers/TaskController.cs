using System;
using System.Web.Mvc;
using TaskManager.Data;
using TaskManager.Models;
using TaskManager.ViewModels;

namespace TaskManager.Controllers
{
    public class TaskController : Controller
    {
        private IRepository<Task> _repository;

        /// <summary>
        /// The default constructor that defaults the task repository to the FlatFileTaskRepository
        /// </summary>
        public TaskController() : this(new FlatFileTaskRepository()) { }

        /// <summary>
        /// Constructor that allows the task repository to be injected. Allows for easier testing
        /// </summary>
        /// <param name="repository"></param>
        public TaskController(IRepository<Task> repository)
        {
            _repository = repository;
        }

        public ViewResult Index()
        {
            var tasks = _repository.GetAll();
            var model = new TaskIndex(tasks);
            return View(model);
        }

        public PartialViewResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult Create(Task newTask)
        {
            if (ModelState.IsValid)
            {
                //give the task a new id
                newTask.Id = DateTime.Now.Ticks;
                _repository.Add(newTask);
                return RedirectToAction("Index", "Task"); //if the controller is ommited, it will be empty during testing
            }
            return View(newTask);
        }

        [HttpPost]
        public RedirectToRouteResult Delete(long id)
        {
            var task = _repository.Get(id);
            _repository.Remove(task);
            return RedirectToAction("Index", "Task");
        }

        public PartialViewResult Edit(long id)
        {
            var model = _repository.Get(id);
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult Edit(Task model)
        {
            if (ModelState.IsValid)
            {
                _repository.Update(model);
                return RedirectToAction("Index", "Task");
            }
            return View(model);
        }
    }
}
